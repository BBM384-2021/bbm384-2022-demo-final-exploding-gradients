namespace LinkedHU_CENG.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //public IFormFile? Resume { get; set; }
        //public IFormFile? Certificates { get; set; }
    }
}
