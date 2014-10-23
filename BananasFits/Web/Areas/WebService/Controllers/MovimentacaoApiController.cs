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
            if (string.IsNullOrEmpty(model.QrCode) ||  model.IdPessoaFisica == 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
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
                    return Request.CreateResponse(HttpStatusCode.OK, ex.Mensagens);                    
                }
                
            }
        }

    }
}