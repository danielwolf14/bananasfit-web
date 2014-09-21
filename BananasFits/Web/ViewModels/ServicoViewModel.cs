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
        //[ValidateFile]
        //[FileExtensions(Extensions = "png,jpg,jpeg,bmp", ErrorMessage = "Formato incorreto! Favor informar um formato correto (.png / .jpg / .jpeg / .bmp")]
        public virtual HttpPostedFileBase Imagem { get; set; }
        public virtual bool IsHabilitado { get; set; }


        //public class ValidateFileAttribute : ValidationAttribute
        //{
        //    public override bool IsValid(object value)
        //    {
        //        int MaxContentLength = 1024 * 1024 * 1; //3 MB
        //        string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };

        //        var file = value as HttpPostedFileBase;

        //        if (file == null)
        //            return false;
        //        else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
        //        {
        //            ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", AllowedFileExtensions);
        //            return false;
        //        }
        //        else if (file.ContentLength > MaxContentLength)
        //        {
        //            ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "MB";
        //            return false;
        //        }
        //        else
        //            return true;
        //    }
        //}



    }

    public class AtualizarServicoViewModel
    {
        public virtual int Chave { get; set; }
        [Required]
        [MaxLength(15)]
        public virtual string Nome { get; set; }
        //public virtual HttpPostedFileBase Imagem { get; set; }
        public virtual bool IsHabilitado { get; set; }
    }
}
