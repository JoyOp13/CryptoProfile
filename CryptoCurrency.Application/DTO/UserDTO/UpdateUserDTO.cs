using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.UserDTO
{
    public class UpdateUserDTO
    {

        [DefaultValue("")]
        public string UserName { get; set; }


        [DefaultValue("")]
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$",
        ErrorMessage = "Phone number must be exactly 10 digits")]
        public string PhoneNumber { get; set; }
    }
}
