using System.ComponentModel.DataAnnotations;

namespace SunRose_Test.Models
{

    public class Message
    {
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = System.DateTime.Now;

        public int UserId { get; set; }

    }
}