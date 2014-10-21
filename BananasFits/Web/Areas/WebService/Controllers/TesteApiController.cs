using AutoMapper;
using Web.ViewModels;
using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Web.Areas.WebService.Controllers
{
    public class TesteApiController : BaseApiController
    {
        //TODO: Como testar post?
        public PessoaFisica Get()
        {
            return new PessoaFisica
            {
                CPF = "teste",
                Endereco = new Endereco{CEP = "teste", Cidade = "teste"},
                IsAdministrador = true,
                Nome = "teste",
                QuantidadeMoedas = 100,
                Telefone = "Teste",
                Email = "Teste@teste.com"
            };
        }

        [HttpGet]
        [Route("api/testeapi/getmessage")]
        public HttpResponseMessage GetMessage()
        {
            var teste = new PessoaFisica
            {
                CPF = "teste",
                Endereco = new Endereco { CEP = "teste", Cidade = "teste" },
                IsAdministrador = true,
                Nome = "teste",
                QuantidadeMoedas = 100,
                Telefone = "Teste",
                Email = "Teste@teste.com"
            };
            return Request.CreateResponse(HttpStatusCode.OK, teste);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]PessoaFisica teste)
        {
            return Request.CreateResponse(HttpStatusCode.OK, teste);
        }

        [HttpPost]
        [Route("api/testeapi/semparametros")]
        public HttpResponseMessage PostSemParametros()
        {
            var teste = new PessoaFisica
            {
                CPF = "teste",
                Endereco = new Endereco{CEP = "teste", Cidade = "teste"},
                IsAdministrador = true,
                Nome = "teste",
                QuantidadeMoedas = 100,
                Telefone = "Teste",
                Email = "Teste@teste.com"
            };
            return Request.CreateResponse(HttpStatusCode.OK, teste);
        }

    }
}