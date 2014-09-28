using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ApiModel
{
    public class EfetuarLoginApiModel
    {
        public virtual string Email { get; set; }

        public virtual string Password { get; set; }
    }
}