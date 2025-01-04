using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Data;
using RentalCar.Models;

namespace RentalCar.Controllers
{
    public class YoneticiController : Controller
    {
        private readonly VeriTabaniContext _veriTabani;

        public YoneticiController(VeriTabaniContext veriTabani)
        {
            _veriTabani = veriTabani;
        }

        public IActionResult Giris()
        {
            if (HttpContext.Session.GetString("YoneticiId") != null)
            {
                return RedirectToAction(nameof(Panel));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(string kullaniciAdi, string sifre)
        {
            var yonetici = await _veriTabani.Yoneticiler.FirstOrDefaultAsync(a => a.KullaniciAdi == kullaniciAdi && a.Sifre == sifre);
            if (yonetici != null)
            {
                HttpContext.Session.SetString("YoneticiId", yonetici.Id.ToString());
                HttpContext.Session.SetString("YoneticiAdi", yonetici.AdSoyad);
                return RedirectToAction(nameof(Panel));
            }

            TempData["Error"] = "Geçersiz kullanıcı adı veya şifre!";
            return View();
        }

        public async Task<IActionResult> Panel()
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            var kiralamaTalepleri = await _veriTabani.KiralamaTalepleri
                .Include(r => r.Arac)
                .OrderByDescending(r => r.TalepTarihi)
                .ToListAsync();

            return View(kiralamaTalepleri);
        }

        public IActionResult AracYonetimi()
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            var araclar = _veriTabani.Araclar.ToList();
            return View(araclar);
        }

        [HttpPost]
        public async Task<IActionResult> AracEkle(Arac arac)
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            try
            {
                arac.Musait = true;
                _veriTabani.Araclar.Add(arac);
                await _veriTabani.SaveChangesAsync();
                TempData["Success"] = "Vehicle added successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while adding the vehicle: " + ex.Message;
            }

            return RedirectToAction(nameof(AracYonetimi));
        }

        [HttpPost]
        public async Task<IActionResult> AracDuzenle(Arac arac)
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            try
            {
                _veriTabani.Araclar.Update(arac);
                await _veriTabani.SaveChangesAsync();
                TempData["Success"] = "Araç başarıyla güncellendi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Araç güncellenirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(AracYonetimi));
        }

        [HttpGet]
        [Route("Yonetici/AracSil/{id}")]
        public async Task<IActionResult> AracSil(int id)
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            try
            {
                var arac = await _veriTabani.Araclar.FindAsync(id);
                if (arac != null)
                {
                    _veriTabani.Araclar.Remove(arac);
                    await _veriTabani.SaveChangesAsync();
                    TempData["Success"] = "Vehicle deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the vehicle: " + ex.Message;
            }

            return RedirectToAction(nameof(AracYonetimi));
        }

        [HttpPost]
        public async Task<IActionResult> KiralamaDurumuGuncelle(int id, string durum)
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            try
            {
                var talep = await _veriTabani.KiralamaTalepleri
                    .Include(r => r.Arac)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (talep != null)
                {
                    talep.Durum = durum;
                    
                    if (durum == "Onaylandı")
                    {
                        if (talep.Arac != null)
                        {
                            talep.Arac.Musait = false;
                        }
                    }
                    else if (durum == "Reddedildi" && talep.Arac != null)
                    {
                        talep.Arac.Musait = true;
                    }

                    await _veriTabani.SaveChangesAsync();
                    TempData["Success"] = "Kiralama durumu güncellendi.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Kiralama durumu güncellenirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Panel));
        }

        [HttpPost]
        public async Task<IActionResult> KiralamaTalebiSil(int id)
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            try
            {
                var talep = await _veriTabani.KiralamaTalepleri.FindAsync(id);
                if (talep != null)
                {
                    _veriTabani.KiralamaTalepleri.Remove(talep);
                    await _veriTabani.SaveChangesAsync();
                    TempData["Success"] = "Kiralama talebi başarıyla silindi.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Kiralama talebi silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Panel));
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Giris));
        }
    }
} 