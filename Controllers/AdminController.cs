using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Data;
using RentalCar.Models;

namespace RentalCar.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("AdminId") != null)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
            if (admin != null)
            {
                HttpContext.Session.SetString("AdminId", admin.Id.ToString());
                HttpContext.Session.SetString("AdminName", admin.FullName);
                return RedirectToAction(nameof(Dashboard));
            }

            TempData["Error"] = "Geçersiz kullanıcı adı veya şifre!";
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var rentalRequests = await _context.RentalRequests
                .Include(r => r.Car)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            return View(rentalRequests);
        }

        public IActionResult ManageCars()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var cars = _context.Cars.ToList();
            return View(cars);
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(Car car)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                car.IsAvailable = true;
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Araç başarıyla eklendi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Araç eklenirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(ManageCars));
        }

        [HttpPost]
        public async Task<IActionResult> EditCar(Car car)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                _context.Cars.Update(car);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Araç başarıyla güncellendi.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Araç güncellenirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(ManageCars));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                var car = await _context.Cars.FindAsync(id);
                if (car != null)
                {
                    _context.Cars.Remove(car);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Araç başarıyla silindi.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Araç silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(ManageCars));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRentalStatus(int id, string status)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                var request = await _context.RentalRequests
                    .Include(r => r.Car)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (request != null)
                {
                    request.Status = status;
                    
                    if (status == "Approved")
                    {
                        if (request.Car != null)
                        {
                            request.Car.IsAvailable = false;
                        }
                    }
                    else if (status == "Rejected" && request.Car != null)
                    {
                        request.Car.IsAvailable = true;
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Kiralama durumu güncellendi.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Kiralama durumu güncellenirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRentalRequest(int id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                var request = await _context.RentalRequests.FindAsync(id);
                if (request != null)
                {
                    _context.RentalRequests.Remove(request);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Kiralama talebi başarıyla silindi.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Kiralama talebi silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Dashboard));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
} 