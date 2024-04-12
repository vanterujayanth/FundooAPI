using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class UserUpdateModel
    {
        [Required(ErrorMessage = "FIRST NAME CANNOT BE EMPTY....")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LAST NAME CANNOT BE EMPTY....")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EMAIL CANNOT BE EMPTY....")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
