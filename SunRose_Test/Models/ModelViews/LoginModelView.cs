using System.ComponentModel.DataAnnotations;

namespace SunRose_Test.Models.ModelViews
{
    public class LoginModelView
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
