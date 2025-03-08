namespace PayrollSystem.ApiService.Models;

public class Payroll : BaseModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Total { get; set; }

    public List<Paysheet> Paysheets { get; set; }
}
