using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class LoginResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public DateTime? Createat { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Token { get; set; }
    }
}
