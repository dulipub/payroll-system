namespace PayrollSystem.ApiService.Models;

public class PaysheetRecord : BaseModel
{
    public double Amount { get; set; }
    public string? Description { get; set; }
    public int PaysheetId { get; set; }
    public int? EmployeePayAdjustment { get; set; }
}

public class TaxRecord : BaseModel
{
    public double Amount { get; set; }
    public string? Description { get; set; }
    public double StartAmount { get; set; }
    public double EndAmount { get; set; }
    public double Percentage { get; set; }

    public int TaxId { get; set; }
    public int PaysheetId { get; set; }
}