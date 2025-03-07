using System.ComponentModel;

namespace PayrollSystem.ApiService.Requests;

public class ListRequest
{
    [DefaultValue(null)]
    public string? SearchTerm { get; set; }

    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;

    [DefaultValue(1)]
    public int PageNumber { get; set; } = 1;
}