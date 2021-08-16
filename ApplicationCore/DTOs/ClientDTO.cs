using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.ApplicationCore.DTOs
{
    public class ClientDTO
    {
        public ClientDTO() { }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ClientInfoQuery { get; set; }

        public string Token { get; set; }

        public string FullName { get; set; }
    }
}
