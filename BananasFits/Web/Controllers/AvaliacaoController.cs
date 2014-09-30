using Processo.Entidades;
using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AvaliacaoController : BaseController
    {
        //
        // GET: /Avaliacao/
        public ActionResult Avaliar(int pontuacao, int chavePessoaJuridica)
        {
            var chaveUsuario = ((UsuarioLogadoModel)Session["usuario"]).Chave;
            var pessoaJuridica = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(chavePessoaJuridica);
            var pessoaFisica = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(chaveUsuario);
            try
            {
                unityOfWork.AvaliacaoNegocio.Avaliar(new Avaliacao
                {
                    PessoaFisica = pessoaFisica,
                    PessoaJuridica = pessoaJuridica,
                    Pontuacao = pontuacao
                });
                unityOfWork.Commit();
            }
            catch (NegocioException ex)
            {
                TratarMensagemException(ex);
            }

            return RedirectToAction("DetalharPessoaJuridica", "Usuario", new { chave = chavePessoaJuridica });
        }


        public ActionResult Index()
        {
            return View();
        }
    }
}