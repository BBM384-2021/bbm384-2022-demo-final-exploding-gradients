using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LinkedHU_CENG.Models
{
    public class Chat
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? SenderUserName { get; set; }  

        public string? ReceiverUserName { get; set; }
        public int? SenderId { get; set; }   

        public int? RecieverId { get; set; }

        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        [Required(ErrorMessage = "Field cannot be left blank")]
        [StringLength(240, ErrorMessage = "The value entered must be between {2} characters and {1} characters", MinimumLength = 1)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        public string? SenderProfilePicture { get; set; }
        public string? ReceiverProfilePicture { get; set; }

    }
}
