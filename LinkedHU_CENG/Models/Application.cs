using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkedHU_CENG.Models
{
    public class Application
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }
        public int AdvertisementId { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string? AdvertisementTitle { get; set; }
        [Required]
        [Display(Name = "Company")]
        public string? Company { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //public IFormFile? Resume { get; set; }
        //public IFormFile? Certificates { get; set; }
    }
}
