namespace PayrollSystem.ApiService.Models;

public class Holiday
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public bool IsApproved { get; set; }
}

public class PayAdjustment
{
    public int Id { get; set; }
    public string? Details { get; set; }
    public DateTime Date { get; set; }
    public string? Type { get; set; } 
    public bool IsActive { get; set; }
    public bool Recurring { get; set; }
}

public class Tax
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public decimal StartAmount { get; set; }
    public decimal Bracket { get; set; }
    public decimal Percentage { get; set; }

}

public class Report
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Details { get; set; }
    public string? File { get; set; }
    public int CreatedUser { get; set; }
}
