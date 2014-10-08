using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Contato()
        {
            return View();
        }

        public ActionResult Termos()
        {
            return View();
        }

        public ActionResult Sobre()
        {
            return View();
        }

        public ActionResult Privacidade()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Contato(ContatoViewModel _objModelMail)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress("bananasfit@gmail.com"));
            mail.From = new MailAddress(_objModelMail.De);
            mail.Subject = "Assunto: " + _objModelMail.Assunto;
            string Body = "Email: " + (_objModelMail.De) + "<br/><br/>" + "Mensagem: " + _objModelMail.Mensagem;
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
                return RedirectToAction("Contato", "Home");
            }
            catch (NegocioException ex)
            {
                TratarMensagemException(ex);
                ExibirMensagemErro("Erro ao enviar email.");
                return RedirectToAction("Contato", "Home");
            }

            //ExibirMensagemSucesso("Email enviado com sucesso.");
            //return View("Index", _objModelMail);

        }
        
    }
}
