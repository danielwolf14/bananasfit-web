using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class CadastroServicoViewModel
    {
        [Required]
        [MaxLength(15)]
        public virtual string Nome { get; set; }
        public virtual HttpPostedFileBase Imagem { get; set; }
        public virtual bool IsHabilitado { get; set; }
    }

    public class AtualizarServicoViewModel
    {
        public virtual int Chave { get; set; }
        [Required]
        [MaxLength(15)]
        public virtual string Nome { get; set; }
        public virtual bool IsHabilitado { get; set; }
    }
}
