using System.ComponentModel.DataAnnotations;

namespace SunRose_Test.Models
{
    public class User
    {
        public User(long id, string name)
        {
            Id = id;
            Username = name;
            Feed = new Queue<Message>();
        }
        public User()
        {
            Id = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        [Required]
        public long Id { get; set; }

        [Required]
        public string Username { get; set; }

        public Queue<Message>? Feed { get; set; }
    }
}
