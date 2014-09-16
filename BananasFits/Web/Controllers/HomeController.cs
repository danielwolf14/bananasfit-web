using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}
