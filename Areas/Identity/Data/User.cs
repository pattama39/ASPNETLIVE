using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ASPNETLIVE.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    [Required(ErrorMessage = "ชื่อสกุล ห้ามว่าง")]
    public string FullName { get; set; } = null!;

    public string Photo { get; set; } = "nopic.png";
}

