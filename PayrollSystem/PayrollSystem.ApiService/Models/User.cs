using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.ApiService.Models;

public sealed class User : IdentityUser, IBaseModel
{
    public int Key { get; set; }
    public bool IsActive { get; set; }
}