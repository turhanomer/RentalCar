using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Data;
using RentalCar.Models;

namespace RentalCar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> Cars()
        {
            var cars = await _context.Cars.Where(c => c.IsAvailable).ToListAsync();
            return View(cars);
        }

        public async Task<IActionResult> CarDetails(int id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRentalRequest(int carId, string customerName, string customerEmail, 
            string customerPhone, DateTime startDate, DateTime endDate)
        {
            try
            {
                var car = await _context.Cars.FindAsync(carId);
                if (car == null)
                {
                    TempData["Error"] = "Araç bulunamadı.";
                    return RedirectToAction(nameof(Cars));
                }

                if (!car.IsAvailable)
                {
                    TempData["Error"] = "Bu araç kiralamaya uygun değil.";
                    return RedirectToAction(nameof(CarDetails), new { id = carId });
                }

                var rentalRequest = new RentalRequest
                {
                    CarId = carId,
                    CustomerName = customerName,
                    CustomerEmail = customerEmail,
                    CustomerPhone = customerPhone,
                    StartDate = startDate,
                    EndDate = endDate,
                    RequestDate = DateTime.Now,
                    Status = "Pending"
                };

                _context.RentalRequests.Add(rentalRequest);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Kiralama talebiniz başarıyla alınmıştır.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kiralama talebi gönderilirken hata oluştu.");
                TempData["Error"] = "Kiralama talebi gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
                return RedirectToAction(nameof(CarDetails), new { id = carId });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
