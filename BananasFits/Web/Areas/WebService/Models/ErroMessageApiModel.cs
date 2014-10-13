using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.WebService.Models
{
    public class ErroMessageApiModel
    {
        public virtual string Mensagem { get; set; }
        public virtual IList<string> ListaMensagem { get; set; }

    }
}