using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Cart_Account.Models
{
    public class RegisterViewModel
    {
        [Required, MaxLength(50), DataType(DataType.EmailAddress), RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")]
        public string Username { get; set; }

        [Required, DataType(DataType.Password), MaxLength(20), MinLength(5)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password)), MaxLength(20), MinLength(5)]
        public string ConfirmPassword { get; set; }
    }
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Xin vui lòng nhập Email !"), MaxLength(50), Display(Name = "Email"), DataType(DataType.EmailAddress), RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Xin vui lòng nhập mật khẩu !"), DataType(DataType.Password), MaxLength(20, ErrorMessage = "Mật khẩu phải ít hơn 20 kí tự"), MinLength(5, ErrorMessage = "Mật khẩu phải nhiều hơn 4 kí tự")]
        public string Password { get; set; }

        [Display(Name = "Nhớ mật khẩu")]
        public bool Remember { get; set; }
    }
}
