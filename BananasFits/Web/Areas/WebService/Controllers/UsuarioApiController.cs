﻿using AutoMapper;
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
using Processo.Negocio;
using Web.ApiModel;
using Web.Areas.WebService.Models;

namespace Web.Controllers
{
    public class UsuarioApiController : BaseApiController
    {
        [HttpPost]
        [Route("api/usuarioapi/CadastrarPessoaFisica")]
        public HttpResponseMessage CadastrarPessoaFisica([FromBody]PessoaFisica model)
        {
            //TODO: Retornar mensagem de sucesso com usuário cadastrado com sucesso
            if (model != null)
            {
                try
                {
                    var usuario = Mapper.DynamicMap<PessoaFisica>(model);
                    unityOfWork.PessoaFisicaNegocio.Cadastrar(usuario);
                    unityOfWork.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, usuario);
                }
                catch (NegocioException ex)
                {
                    var erroMensagemApiModel = new ErroMessageApiModel
                    {
                        Mensagem = ex.Message,
                        ListaMensagem = ex.Mensagens
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, erroMensagemApiModel);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Parâmetro inválido.");
            }
        }

        [HttpPost]
        [Route("api/usuarioapi/efetuarlogin")]
        public HttpResponseMessage EfetuarLogin([FromBody]EfetuarLoginApiModel model)
        {
            if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
            {
                Usuario usuario = unityOfWork.PessoaFisicaNegocio.BuscarUsuarioPorEmail(model.Email);
                usuario = usuario != null ? usuario :
                    unityOfWork.PessoaJuridicaNegocio.BuscarUsuarioPorEmail(model.Email);

                if (usuario != null && (usuario.Password == model.Password))
                {
                    var usuarioLogado = Mapper.DynamicMap<UsuarioLogadoModel>(usuario);
                    usuarioLogado.IsPessoaFisica = usuario is PessoaFisica;

                    return Request.CreateResponse(HttpStatusCode.OK, usuarioLogado);
                }
                else
                {
                    var erroMensagemApiModel = new ErroMessageApiModel
                    {
                        Mensagem = "Login ou senha incorretos."
                    };
                    return Request.CreateResponse(HttpStatusCode.NotFound, erroMensagemApiModel);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("api/usuarioapi/listarpessoajuridica")]
        public HttpResponseMessage ListarPessoaJuridica(int top = 0)
        {
            var lista = unityOfWork.PessoaJuridicaNegocio.ListarTodos();
            if (top != 0)
                lista = lista.Take(top);

            return Request.CreateResponse(HttpStatusCode.OK, lista.ToList());
        }

        [HttpGet]
        [Route("api/usuarioapi/detalharpessoajuridica")]
        public HttpResponseMessage DetalharPessoaJuridica(int chave)
        {
            var usuario = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(chave);
            if (usuario.IsHabilitado)
                return Request.CreateResponse(HttpStatusCode.OK, usuario);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}