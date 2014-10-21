using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Web.Areas.WebService.Controllers
{
    public class ServicoApiController : BaseApiController
    {
        public object QRCode(string qrCode)
        {

            return
                new
                {
                    ChaveAcademia = "Z",
                    NomeServico = "X",
                    NomeAcademia = "Y",
                    ValorServico = 32
                };
        }

        [HttpGet]
        [Route("api/servicoapi/buscarservicos")]
        public HttpResponseMessage BuscarTodos()
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                unityOfWork.ServicoNegocio
                .Consultar(e => e.IsHabilitado)
                .Select(e => new { Nome = e.Nome, Chave = e.Chave }).
                ToList());
        }

    }
}