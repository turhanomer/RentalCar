using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Data;
using RentalCar.Models;

namespace RentalCar.Controllers
{
    public class AnasayfaController : Controller
    {
        private readonly ILogger<AnasayfaController> _logger;
        private readonly VeriTabaniContext _veriTabani;

        public AnasayfaController(ILogger<AnasayfaController> logger, VeriTabaniContext veriTabani)
        {
            _logger = logger;
            _veriTabani = veriTabani;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Iletisim()
        {
            return View();
        }

        public async Task<IActionResult> Araclar()
        {
            var araclar = await _veriTabani.Araclar.Where(c => c.Musait).ToListAsync();
            return View(araclar);
        }

        public async Task<IActionResult> AracDetay(int id)
        {
            var arac = await _veriTabani.Araclar.FirstOrDefaultAsync(c => c.Id == id);
            if (arac == null)
            {
                return NotFound();
            }
            return View(arac);
        }

        [HttpPost]
        public async Task<IActionResult> KiralamaTalebiGonder(int aracId, string musteriAdi, string musteriEmail, 
            string musteriTelefon, DateTime baslangicTarihi, DateTime bitisTarihi)
        {
            try
            {
                var arac = await _veriTabani.Araclar.FindAsync(aracId);
                if (arac == null)
                {
                    TempData["Error"] = "Araç bulunamadı.";
                    return RedirectToAction(nameof(Araclar));
                }

                if (!arac.Musait)
                {
                    TempData["Error"] = "Bu araç kiralamaya uygun değil.";
                    return RedirectToAction(nameof(AracDetay), new { id = aracId });
                }

                var kiralamaTalebi = new KiralamaTalebi
                {
                    AracId = aracId,
                    MusteriAdi = musteriAdi,
                    MusteriEmail = musteriEmail,
                    MusteriTelefon = musteriTelefon,
                    BaslangicTarihi = baslangicTarihi,
                    BitisTarihi = bitisTarihi,
                    TalepTarihi = DateTime.Now,
                    Durum = "Beklemede"
                };

                _veriTabani.KiralamaTalepleri.Add(kiralamaTalebi);
                await _veriTabani.SaveChangesAsync();

                TempData["Success"] = "Kiralama talebiniz başarıyla alınmıştır.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kiralama talebi gönderilirken hata oluştu.");
                TempData["Error"] = "Kiralama talebi gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
                return RedirectToAction(nameof(AracDetay), new { id = aracId });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new HataViewModel { TalepId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
