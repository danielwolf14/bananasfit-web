using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Web.ApiModel;

namespace Web.Controllers
{
    public class AvaliacaoApiController : BaseApiController
    {
        public HttpResponseMessage Post([FromBody]InserirAvaliacaoApiModel model)
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
    }
}