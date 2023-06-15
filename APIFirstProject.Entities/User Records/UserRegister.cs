using System.ComponentModel.DataAnnotations;

namespace APIFirstProject.Entities.User_Records
{
    public class UserRegister
    {
        [Key]

        public string UserName { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
    }
}
