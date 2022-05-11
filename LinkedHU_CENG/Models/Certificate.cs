using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkedHU_CENG.Models

{
    public class Certificate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CertificateId { get; set; }
        public int ApplicationId { get; set; }
        public int? UserId { get; set; }
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        [Display(Name = "Certificate")]
        [NotMapped]
        public IFormFile? File { get; set; }
        public string? FilePath { get; set; }
    }
}
