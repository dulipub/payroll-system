namespace PayrollSystem.ApiService.Responses;

public class ListResponse<T>
{
    public List<T>? Results { get; set; }
    public int Count { get; set; }
    public int CurrentPage { get; set; }
    public int TotalRecords { get; set; }
}