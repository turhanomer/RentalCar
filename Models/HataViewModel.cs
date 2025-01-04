namespace RentalCar.Models;

public class HataViewModel
{
    public string? TalepId { get; set; }

    public bool TalepIdGoster => !string.IsNullOrEmpty(TalepId);
}
