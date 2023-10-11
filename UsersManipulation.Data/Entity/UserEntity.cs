using System.ComponentModel.DataAnnotations;

namespace UsersManipulation.Data.Entity
{
    public class UserEntity : BaseEntity
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsBlocked { get; set; }
    }
}
