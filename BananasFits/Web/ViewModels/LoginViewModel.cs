using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
    }
}