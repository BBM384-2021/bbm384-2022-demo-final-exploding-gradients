using System.ComponentModel.DataAnnotations;

namespace LinkedHU_CENG.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]  
        public string Password { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "PhoneNum")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is not valid")]
        public string PhoneNum { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "BirthDate")]
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }
    }

    public enum Role
    {
        Academician,
        Graduate,
        Student
    }
}
