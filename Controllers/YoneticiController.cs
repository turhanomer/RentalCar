using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Data;
using RentalCar.Models;
using System.IO;

namespace RentalCar.Controllers
{
    public class YoneticiController : Controller
    {
        private readonly VeriTabaniContext _veriTabani;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string[] _izinliResimTurleri = { ".jpg", ".jpeg", ".png", ".gif" };
        private const int _maksimumResimBoyutu = 5 * 1024 * 1024; // 5MB

        public YoneticiController(VeriTabaniContext veriTabani, IWebHostEnvironment hostingEnvironment)
        {
            _veriTabani = veriTabani;
            _hostingEnvironment = hostingEnvironment;
        }

        private bool ResimGecerliMi(IFormFile file)
        {
            if (file == null || file.Length == 0) return true; // Resim zorunlu değil

            if (file.Length > _maksimumResimBoyutu)
            {
                ModelState.AddModelError("ResimDosyasi", "Resim boyutu 5MB'dan küçük olmalıdır.");
                return false;
            }

            var uzanti = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_izinliResimTurleri.Contains(uzanti))
            {
                ModelState.AddModelError("ResimDosyasi", "Sadece .jpg, .jpeg, .png ve .gif uzantılı dosyalar yüklenebilir.");
                return false;
            }

            return true;
        }

        private async Task<string?> ResimKaydet(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/uploads/" + uniqueFileName;
        }

        private void ResimSil(string? resimUrl)
        {
            if (string.IsNullOrEmpty(resimUrl)) return;

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, resimUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
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
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                TempData["Error"] = "Kullanıcı adı ve şifre zorunludur!";
                return View();
            }

            var yonetici = await _veriTabani.Yoneticiler
                .FirstOrDefaultAsync(a => a.KullaniciAdi == kullaniciAdi && a.Sifre == sifre);

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

        public async Task<IActionResult> AracYonetimi()
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            var araclar = await _veriTabani.Araclar
                .OrderBy(a => a.Marka)
                .ThenBy(a => a.Model)
                .ToListAsync();

            return View(araclar);
        }

        [HttpPost]
        public async Task<IActionResult> AracEkle(Arac arac)
        {
            if (HttpContext.Session.GetString("YoneticiId") == null)
            {
                return RedirectToAction(nameof(Giris));
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen tüm zorunlu alanları doldurun.";
                return RedirectToAction(nameof(AracYonetimi));
            }

            try
            {
                if (arac.ResimDosyasi != null && !ResimGecerliMi(arac.ResimDosyasi))
                {
                    return RedirectToAction(nameof(AracYonetimi));
                }

                arac.ResimUrl = await ResimKaydet(arac.ResimDosyasi);
                arac.Musait = true;

                _veriTabani.Araclar.Add(arac);
                await _veriTabani.SaveChangesAsync();

                TempData["Success"] = "Araç başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Araç eklenirken bir hata oluştu: " + ex.Message;
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
                var mevcutArac = await _veriTabani.Araclar
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == arac.Id);

                if (mevcutArac == null)
                {
                    TempData["Error"] = "Araç bulunamadı.";
                    return RedirectToAction(nameof(AracYonetimi));
                }

                if (arac.ResimDosyasi != null && !ResimGecerliMi(arac.ResimDosyasi))
                {
                    return RedirectToAction(nameof(AracYonetimi));
                }

                // Mevcut resmi koru eğer yeni resim yüklenmediyse
                if (arac.ResimDosyasi != null)
                {
                    ResimSil(mevcutArac.ResimUrl);
                    arac.ResimUrl = await ResimKaydet(arac.ResimDosyasi);
                }
                else if (string.IsNullOrEmpty(arac.ResimUrl))
                {
                    arac.ResimUrl = mevcutArac.ResimUrl;
                }

                _veriTabani.Entry(arac).State = EntityState.Modified;
                await _veriTabani.SaveChangesAsync();

                TempData["Success"] = "Araç başarıyla güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Araç güncellenirken bir hata oluştu: " + ex.Message;
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
                if (arac == null)
                {
                    return NotFound();
                }

                // Kiralama talepleri varsa silmeyi engelle
                var kiralamaTalebiVar = await _veriTabani.KiralamaTalepleri
                    .AnyAsync(kt => kt.AracId == id && kt.Durum == "Onaylandı");

                if (kiralamaTalebiVar)
                {
                    return Json(new { success = false, message = "Bu araç şu anda kirada olduğu için silinemez." });
                }

                ResimSil(arac.ResimUrl);
                _veriTabani.Araclar.Remove(arac);
                await _veriTabani.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Araç silinirken bir hata oluştu: " + ex.Message });
            }
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

                if (talep == null)
                {
                    TempData["Error"] = "Kiralama talebi bulunamadı.";
                    return RedirectToAction(nameof(Panel));
                }

                // Araç başka birine kiralanmışsa reddet
                if (durum == "Onaylandı" && talep.Arac != null && !talep.Arac.Musait)
                {
                    TempData["Error"] = "Bu araç başka bir müşteriye kiralanmış durumda.";
                    return RedirectToAction(nameof(Panel));
                }

                talep.Durum = durum;
                
                if (talep.Arac != null)
                {
                    talep.Arac.Musait = durum != "Onaylandı";
                }

                await _veriTabani.SaveChangesAsync();
                TempData["Success"] = "Kiralama durumu güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kiralama durumu güncellenirken bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Panel));
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Giris));
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
                if (talep == null)
                {
                    TempData["Error"] = "Kiralama talebi bulunamadı.";
                    return RedirectToAction(nameof(Panel));
                }

                _veriTabani.KiralamaTalepleri.Remove(talep);
                await _veriTabani.SaveChangesAsync();

                TempData["Success"] = "Kiralama talebi başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kiralama talebi silinirken bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Panel));
        }
    }
} 