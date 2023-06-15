using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFirstProject.Entities.User_Records
{
    public class RefreshTokenRequestModel
    {
        public string RefreshToken { get; set; }
        public string Username { get; set; }
    }
}
