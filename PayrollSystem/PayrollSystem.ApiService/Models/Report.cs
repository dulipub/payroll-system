namespace PayrollSystem.ApiService.Models;

public class Report
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Details { get; set; }
    public string? File { get; set; }
    public int CreatedUser { get; set; }
}
