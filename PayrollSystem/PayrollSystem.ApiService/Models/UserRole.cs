using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.ApiService.Models;

public class UserRole : IdentityRole, IBaseModel
{
    public int Key { get; set; }
    public bool IsActive { get; set; }
}