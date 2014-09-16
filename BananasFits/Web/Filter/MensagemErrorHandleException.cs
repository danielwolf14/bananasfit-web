using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Web.Filter
{
    public class MensagemErrorHandleException : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //filterContext.Controller.TempData["mensagemException"] = filterContext.Exception.

            var ex = filterContext.Exception;
            if (ex is NegocioException)
            {
                if (filterContext.Controller.TempData["mensagemErro"] == null)
                    filterContext.Controller.TempData["mensagemErro"] = ((NegocioException)ex).Mensagens;
                else
                    ((IList<string>)filterContext.Controller.TempData["mensagemErro"]).Concat(((NegocioException)ex).Mensagens);
            }
            else
            {
                if (filterContext.Controller.TempData["mensagemErro"] == null)
                    filterContext.Controller.TempData["mensagemErro"] = new List<string> { "Houve um erro inesperado. Por favor, entre em contato com o administrador." };
                else
                    ((IList<string>)filterContext.Controller.TempData["mensagemErro"]).Add("Houve um erro inesperado. Por favor, entre em contato com o administrador.");
            }


            var actionName = (string)filterContext.RouteData.Values["action"];
            var view = ViewEngines.Engines.FindView(filterContext.Controller.ControllerContext, actionName, null).View
                         ?? ViewEngines.Engines.FindView(filterContext.Controller.ControllerContext, View, null).View;

            filterContext.Result = new ViewResult
            {
                View = view,
                ViewData = filterContext.Controller.ViewData,

            };


            base.OnException(filterContext);
        }
    }
}