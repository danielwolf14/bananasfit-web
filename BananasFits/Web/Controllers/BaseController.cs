using Processo;
using Processo.Database;
using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {

        protected UnityOfWork unityOfWork;

        public BaseController()
        {
            unityOfWork = UnityOfWork.GetInstancia();
        }
        
        public void ExibirMensagemSucesso(string mensagemSucesso = "Operação realizada com sucesso.")
        {
            TempData["mensagemSucesso"] = mensagemSucesso;
        }

        public void ExibirMensagemErro(string mensagemErro = "Houve um erro conhecido. Por favor, contate o administrador do sistema.")
        {
            if (TempData["mensagemErro"] == null)
                TempData["mensagemErro"] = new List<string>();
            ((List<string>)TempData["mensagemErro"]).Add(mensagemErro);
        }
        
        public void ExibirMensagemAtencao(string mensagemAtencao)
        {
            TempData["mensagemAtencao"] = mensagemAtencao;
        }

        public void TratarMensagemException(NegocioException ex)
        {
            ex.Mensagens.ToList().ForEach(e =>
            {
                ExibirMensagemErro(e);
            });

            //ex.Mensagens.ToList().ForEach(e =>
            //{
            //    ModelState.AddModelError("",e);
            //});
        }
    }
}