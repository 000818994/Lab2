using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Fall_2022_Lab_1_000818994.Models
{
    public class ApplicationUser:IdentityUser
    {

        [Required, Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Birth Date")]
        public string? BirthDate { get; set; }
    }
}
