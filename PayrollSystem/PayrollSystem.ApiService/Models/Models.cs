namespace PayrollSystem.ApiService.Models;

public class TimeSheet
{
    public int Id { get; set; }
    public int EmpId { get; set; }
    public DateTime Date { get; set; }
    public string? Detail { get; set; }
    public decimal HoursWorked { get; set; }
    public bool IsApproved { get; set; }
}

public class Leave
{
    public int Id { get; set; }
    public int EmpId { get; set; }
    public DateTime Date { get; set; }
    public bool IsApproved { get; set; }
    public int ApprovedUserId { get; set; }
}

public class Holiday
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public bool IsApproved { get; set; }
}

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int DepId { get; set; } 
}

public class Department
{
    public int Id { get; set; }
    public string? Location { get; set; }
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
