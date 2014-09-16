using Map = AutoMapper;
using Web.ViewModels;
using Processo;
using Processo.Database;
using Processo.Entidades;
using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace Web.Controllers
{
    public class ContatoController : BaseController
    {


        [HttpPost]
        public ActionResult Contato(ContatoViewModel _objModelMail)
        {
          
                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress("bananasfit@gmail.com"));
                mail.From = new MailAddress(_objModelMail.De);
                mail.Subject = "Assunto: "+ _objModelMail.Assunto;
                string Body = "Email: " + (_objModelMail.De)+"<br/><br/>"+"Mensagem: "+ _objModelMail.Mensagem;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("bananasfit@gmail.com", "bananas@123");// Enter seders User name and password
                smtp.EnableSsl = true;
               

                try
                {
                    smtp.Send(mail);
                    ExibirMensagemSucesso("Email enviado com sucesso.");
                    return RedirectToAction("Contato","Home");
                }
                catch (NegocioException ex)
                {
                    TratarMensagemException(ex);
                    ExibirMensagemErro("Erro ao enviar email.");
                    return RedirectToAction("Contato","Home");
                }

                //ExibirMensagemSucesso("Email enviado com sucesso.");
                //return View("Index", _objModelMail);
           
        }

    }
}