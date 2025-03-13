namespace PayrollSystem.ApiService.Models;

public class Tax : BaseModel
{
    public string? Type { get; set; }
    public double StartAmount { get; set; }
    public double EndAmount { get; set; }
    public double Percentage { get; set; }

}
