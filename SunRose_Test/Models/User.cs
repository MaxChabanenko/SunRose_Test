using System.ComponentModel.DataAnnotations;

namespace SunRose_Test.Models
{
    public class User
    {

        public User(int id, string name)
        {
            Id = id;
            Username = name;
            Feed = new Queue<Message>();
        }
        public User()
        {
            //or autoincrement and stаtic field can be here, but json serialization will produce unnecessary increments
        }
        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public Queue<Message>? Feed { get; set; }
    }
}
