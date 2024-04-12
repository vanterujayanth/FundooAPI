using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class TokenModel
    {

       public long userID { get; set; }
        public string FirstName { get; set; }
       
        public string LastName { get; set; }

        public string Email { get; set; }
      
        public string Password { get; set; }
        public string Token { get; set; }
        }
}

