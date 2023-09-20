using System.ComponentModel.DataAnnotations;

namespace SunRose_Test.Models
{
    public class Message
    {
        //public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = System.DateTime.Now;

        public long UserId { get; set; }

    }
}