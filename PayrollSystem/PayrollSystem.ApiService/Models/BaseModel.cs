using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.ApiService.Models;

public interface IBaseModel
{
    public int Id { get; set; }

    public bool IsActive { get; set; }
}

public abstract class BaseModel : IBaseModel
{
    [Key]
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}