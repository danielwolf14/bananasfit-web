using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Web.ApiModel;

namespace Web.Areas.WebService.Controllers
{
    public class AvaliacaoApiController : BaseApiController
    {
        [HttpPost]
        [Route("api/avaliacaoapi/avaliar")]
        public HttpResponseMessage Avaliar([FromBody]InserirAvaliacaoApiModel model)
        {
            if (model.Pontuacao != 0 && model.PessoaFisica != 0 && model.PessoaJuridica != 0)
            {
                var avaliacao = new Avaliacao
                {
                    Pontuacao = model.Pontuacao,
                    PessoaFisica = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(model.PessoaFisica),
                    PessoaJuridica = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(model.PessoaJuridica)
                };

                unityOfWork.AvaliacaoNegocio.Avaliar(avaliacao);
                unityOfWork.Commit();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("api/avaliacaoapi/avaliacaopessoajuridica")]
        public HttpResponseMessage AvaliacaoPessoaJuridica(int chavePessoaJuridica, int chavePessoaFisica)
        {
            var usuarioAvaliado = unityOfWork.AvaliacaoNegocio.Consultar(e => e.PessoaFisica.Chave == chavePessoaFisica && e.PessoaJuridica.Chave == chavePessoaJuridica).First().Pontuacao;
            var totalAvaliacao = unityOfWork.AvaliacaoNegocio.Consultar(e => e.PessoaJuridica.Chave == chavePessoaJuridica).Count();
            var mediaAvaliacao = totalAvaliacao / totalAvaliacao;
           // var teste = unityOfWork.AvaliacaoNegocio.ConsultarTodos();
            var json = new AvaliacaoApiModel { 
            Pontuacao = usuarioAvaliado,
            MediaDeAvaliacoes = mediaAvaliacao,
            TotalDeAvaliacoes = totalAvaliacao
            };

            if (totalAvaliacao != 0)
                return Request.CreateResponse(HttpStatusCode.OK, json);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);

        }

    }
}