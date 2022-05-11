namespace LinkedHU_CENG.Models.ViewModels
{
    public class AdvertisementViewModel
    {
        public Advertisement Advertisement { get; set; }
        public Application Application { get; set; }
        public List<Application> Applications { get; set; }
        public List<Certificate>? Certificates { get; set; }
        public int AdvertisementId { get; set; }
    }
}
