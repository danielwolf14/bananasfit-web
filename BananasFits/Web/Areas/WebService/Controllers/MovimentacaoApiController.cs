using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Web.Areas.WebService.Models;

namespace Web.Areas.WebService.Controllers
{
    public class MovimentacaoApiController : BaseApiController
    {

        [HttpPost]
        [Route("api/movimentacaoapi/comprarservicos")]
        public HttpResponseMessage Comprar([FromBody]MovimentacaoApiModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.QrCode) || model.IdPessoaFisica == 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            if (!model.FinalizarCompra)
            {
                var servico = unityOfWork.ServicoPessoaJuridicaNegocio.Consultar(e => e.QRCode == model.QrCode).SingleOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    NomeServico = servico.Servico.Nome,
                    PessoaJuridica = servico.PessoaJuridica.Nome,
                    Valor = servico.Valor
                });
            }
            else
            {
                try
                {
                    unityOfWork.ServicoPessoaJuridicaNegocio.Comprar(model.QrCode, model.IdPessoaFisica);
                    unityOfWork.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (NegocioException ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ErroMessageApiModel { Mensagem = ex.Mensagens.FirstOrDefault() });
                }

            }
        }

        [HttpPost]
        [Route("api/movimentacaoapi/comprarfits")]
        public HttpResponseMessage ComprarFits([FromBody]ComprarFitsModel model)
        {
            if (model.QuantidadeFits != 0 && model.ChavePessoaFisica != 0)
            {

                try
                {
                    var usuario = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(model.ChavePessoaFisica);
                    usuario.QuantidadeMoedas += model.QuantidadeFits;
                    unityOfWork.PessoaFisicaNegocio.Atualizar(usuario);
                    unityOfWork.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK);

                }
                catch (NegocioException ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ex.Mensagens);
                }

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }
    }
}