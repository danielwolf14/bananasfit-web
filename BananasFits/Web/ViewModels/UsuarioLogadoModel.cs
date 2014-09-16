using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class UsuarioLogadoModel
    {
        public virtual long Chave { get; set; }
        public virtual string Email { get; set; }
        public virtual string Nome { get; set; }
        public virtual bool IsAdministrador { get; set; }
        public virtual bool IsPessoaFisica { get; set; }
    }
}