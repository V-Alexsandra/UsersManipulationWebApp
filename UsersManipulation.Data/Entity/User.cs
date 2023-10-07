using System.ComponentModel.DataAnnotations;

namespace UsersManipulation.Data.Entity
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Date of Registration")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime LastLoginDate { get; set; }

        public bool IsBlocked { get; set; }
    }
}
