using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Fall_2022_Lab_1_000818994.Models
{
    public class TeamEntity
    {
        [Key]
        [Required, Display(Name = "Id")]
        public int Id { get; set; }

        [Required, Display(Name = "Team Name")]
        public string? TeamName { get; set; }

        [Display(Name = "Established Date")]
        public DateTime EstablishedDate { get; set; }

        [Required, Display(Name = "Email")]
        public string? Email { get; set; }
    }
}
