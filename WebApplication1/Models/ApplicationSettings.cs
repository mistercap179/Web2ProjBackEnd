using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ApplicationSettings
    {
        public string JWT_Secret { get; set; }
        public string Client_URL { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string EmailHost { get; set; }
        public string EmailSenderName { get; set; }
    }
}
