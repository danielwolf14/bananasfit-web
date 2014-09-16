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

namespace Web.Controllers
{
    public class UsuarioApiController : BaseApiController
    {
        //TODO: Como testar post?

        [HttpPost]
        [Route("api/usuario/CadastrarPessoaFisica")]
        public HttpResponseMessage CadastrarPessoaFisica([FromBody]PessoaFisica model)
        {
            //TODO: Retornar mensagem de sucesso com usuário cadastrado com sucesso
            HttpResponseMessage response;
            if (model != null)
            {
                var usuario = Mapper.DynamicMap<PessoaFisica>(model);
                unityOfWork.PessoaFisicaNegocio.Cadastrar(usuario);
                unityOfWork.Commit();
                response = Request.CreateResponse(HttpStatusCode.OK, usuario);
                //TODO:Retornar mensagem de sucesso ou negócio exception
            }
            else
            {
                //TODO:Retornar mensagem de erro de model invalido
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        [HttpPost]
        [Route("api/usuario/efetuarlogin")]
        public HttpResponseMessage EfetuarLogin([FromBody]string email, [FromBody]string senha)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha))
            {
                Usuario usuario = unityOfWork.PessoaFisicaNegocio.BuscarUsuarioPorEmail(email);
                usuario = usuario != null ? usuario :
                    unityOfWork.PessoaJuridicaNegocio.BuscarUsuarioPorEmail(email);

                if (usuario != null && (usuario.Password == senha))
                {
                    var usuarioLogado = Mapper.DynamicMap<UsuarioLogadoModel>(usuario);
                    usuarioLogado.IsPessoaFisica = usuario is PessoaFisica;

                    return Request.CreateResponse(HttpStatusCode.OK, usuarioLogado);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
               
    }
}