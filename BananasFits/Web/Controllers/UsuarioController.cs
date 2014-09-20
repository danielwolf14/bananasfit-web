using Map = AutoMapper;
using Web.ViewModels;
using Processo;
using Processo.Database;
using Processo.Entidades;
using Processo.Negocio;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class UsuarioController : BaseController
    {
        #region Listar
        public ActionResult ListarPessoaFisica(string currentFilter, string searchString, int? page)
        {
            if (Session["usuario"] != null && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var usuarios = unityOfWork.PessoaFisicaNegocio.Consultar(e => e.IsHabilitado);

                if (!String.IsNullOrEmpty(searchString))
                {
                    usuarios = usuarios.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
                }

                usuarios = usuarios.OrderBy(e => e.Nome);

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(usuarios.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ListarPessoaJuridica(string currentFilter, string searchString, int? page)
        {
            if (Session["usuario"] != null && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var usuarios = unityOfWork.PessoaJuridicaNegocio.Consultar(e => e.IsHabilitado);

                if (!String.IsNullOrEmpty(searchString))
                {
                    usuarios = usuarios.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
                }

                usuarios = usuarios.OrderBy(e => e.Nome);

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(usuarios.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
        
        #region Login
        public ActionResult Login()
        {
            if (Session["usuario"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        public ActionResult DetalhesPessoaJuridica()
        {
            return View();
        }

        public ActionResult BuscarPessoaJuridica(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var usuarios = unityOfWork.PessoaJuridicaNegocio.Consultar(e => e.IsHabilitado);

            if (!String.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
            }

            usuarios = usuarios.OrderBy(e => e.Nome);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(usuarios.ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid && Session["usuario"] == null)
            {
                Usuario usuario = unityOfWork.PessoaFisicaNegocio.BuscarUsuarioPorEmail(model.Email);
                usuario = usuario != null ? usuario :
                    unityOfWork.PessoaJuridicaNegocio.BuscarUsuarioPorEmail(model.Email);

                if (usuario != null && (usuario.Password == model.Password) && usuario.IsHabilitado)
                {
                    var usuarioLogado = Map.Mapper.DynamicMap<UsuarioLogadoModel>(usuario);
                    usuarioLogado.IsPessoaFisica = usuario is PessoaFisica;
                    Session["usuario"] = usuarioLogado;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ExibirMensagemErro("Usuário inexistente ou senha inválida.");
                }
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session["usuario"] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Cadastrar Pessoa Fisica e Juridica

        public ActionResult CadastrarPessoaJuridica()
        {
            if (Session["usuario"] != null && !((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                return RedirectToAction("Index", "Home");

            ViewBag.Estado = MontarViewBagEstado();
            return View();
        }

        public ActionResult CadastrarPessoaFisica()
        {
            if (Session["usuario"] != null && !((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                return RedirectToAction("Index", "Home");

            ViewBag.Estado = MontarViewBagEstado();
            return View();
        }

        [HttpPost]
        public ActionResult CadastrarPessoaJuridica(CadastroPessoaJuridicaViewModel model, HttpPostedFileBase img)
        {
            ViewBag.Estado = MontarViewBagEstado();
            if (ModelState.IsValid)
            {
                var endereco = Map.Mapper.DynamicMap<Endereco>(model.Endereco);
                var usuario = Map.Mapper.DynamicMap<PessoaJuridica>(model);
                usuario.IsHabilitado = true;
                try
                {
                    unityOfWork.PessoaJuridicaNegocio.Cadastrar(usuario);
                    unityOfWork.Commit();
                    usuario.Imagem = SalvarImagem(img, usuario.Chave, usuario.Nome);
                    unityOfWork.PessoaJuridicaNegocio.Atualizar(usuario);
                    unityOfWork.Commit();
                    ExibirMensagemSucesso("Usuário cadastrado com sucesso.");
                    if (Session["usuario"] != null && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    {
                        return RedirectToAction("ListarPessoaJuridica", "Usuario");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                catch (NegocioException ex)
                {
                    TratarMensagemException(ex);
                }
            }

            return View(model);
        }

        public string SalvarImagem(HttpPostedFileBase imagem, int idPessoaJuridica, string nomePessoaJuridica)
        {
            string[] strName = imagem.FileName.Split('.');
            string extensao = strName[strName.Count() - 1];
            string caminhoSalvo = String.Format("{0}{1}.{2}", Server.MapPath("~/images/ImagemPessoaJuridica/"), nomePessoaJuridica + idPessoaJuridica, extensao);
            string caminhoFinal = String.Format("/images/ImagemPessoaJuridica/{0}.{1}", nomePessoaJuridica + idPessoaJuridica, extensao);
            imagem.SaveAs(caminhoSalvo);
            return caminhoFinal;
        }

        [HttpPost]
        public ActionResult CadastrarPessoaFisica(CadastroPessoaFisicaViewModel model)
        {
            ViewBag.Estado = MontarViewBagEstado();
            if (ModelState.IsValid)
            {
                var endereco = Map.Mapper.DynamicMap<Endereco>(model.Endereco);
                var usuario = Map.Mapper.DynamicMap<PessoaFisica>(model);
                usuario.IsHabilitado = true;
                try
                {
                    unityOfWork.PessoaFisicaNegocio.Cadastrar(usuario);
                    unityOfWork.Commit();
                    ExibirMensagemSucesso("Usuário cadastrado com sucesso.");
                    if (Session["usuario"] != null && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    {
                        return RedirectToAction("ListarPessoaFisica", "Usuario");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }


                }
                catch (NegocioException ex)
                {
                    TratarMensagemException(ex);
                }
            }

            return View();
        }
        #endregion

        #region Detalhar, Atualizar e Inativar Pessoa física e juridica
        public ActionResult MinhaConta()
        {
            var usuarioLogado = (UsuarioLogadoModel)Session["usuario"];
            var view = "";
            if (usuarioLogado.IsPessoaFisica)
                view = "MinhaContaPessoaFisica";
            else
                view = "MinhaContaPessoaJuridica";

            return RedirectToAction(view);
        }

        public ActionResult MinhaContaPessoaJuridica()
        {
            var estados = MontarViewBagEstado();
            var usuarioLogado = (UsuarioLogadoModel)Session["usuario"];
            if (usuarioLogado != null && !usuarioLogado.IsPessoaFisica)
            {
                var usuario = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(usuarioLogado.Chave);
                var model = Map.Mapper.DynamicMap<DetalharPessoaJuridicaViewModel>(usuario);

                estados.FirstOrDefault(e => e.Text == model.Endereco.Estado).Selected = true;
                ViewBag.Estado = estados;

                return View(model);
            }
            return RedirectToAction("Index", "Home");

        }

        public ActionResult MinhaContaPessoaFisica()
        {
            var estados = MontarViewBagEstado();
            var usuarioLogado = (UsuarioLogadoModel)Session["usuario"];
            if (usuarioLogado != null && usuarioLogado.IsPessoaFisica)
            {
                var usuario = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(usuarioLogado.Chave);
                var model = Map.Mapper.DynamicMap<DetalharPessoaFisicaViewModel>(usuario);
                estados.FirstOrDefault(e => e.Text == model.Endereco.Estado).Selected = true;
                ViewBag.Estado = estados;

                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult MinhaContaPessoaJuridica(DetalharPessoaJuridicaViewModel model)
        {
            ViewBag.Estado = MontarViewBagEstado();
            if (ModelState.IsValid && (UsuarioLogadoModel)Session["usuario"] != null
                && ((UsuarioLogadoModel)Session["usuario"]).Chave == model.Chave)
            {
                var usuarioAtualizado = Map.Mapper.DynamicMap<PessoaJuridica>(model);
                var usuarioParaAtualizar = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(usuarioAtualizado.Chave);
                try
                {
                    AtualizarPessoaJuridica(usuarioParaAtualizar, usuarioAtualizado);
                    unityOfWork.PessoaJuridicaNegocio.Atualizar(usuarioParaAtualizar);

                    unityOfWork.Commit();
                    ExibirMensagemSucesso("Usuário atualizado com sucesso.");
                    return View(model);
                }
                catch (NegocioException ex)
                {
                    TratarMensagemException(ex);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult MinhaContaPessoaFisica(DetalharPessoaFisicaViewModel model)
        {
            ViewBag.Estado = MontarViewBagEstado();
            if (ModelState.IsValid && (UsuarioLogadoModel)Session["usuario"] != null
                && ((UsuarioLogadoModel)Session["usuario"]).Chave == model.Chave)
            {
                var usuarioAtualizado = Map.Mapper.DynamicMap<PessoaFisica>(model);
                var usuarioParaAtualizar = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(usuarioAtualizado.Chave);
                try
                {
                    AtualizarPessoaFisica(usuarioParaAtualizar, usuarioAtualizado);
                    unityOfWork.PessoaFisicaNegocio.Atualizar(usuarioParaAtualizar);

                    unityOfWork.Commit();
                    ExibirMensagemSucesso("Usuário atualizado com sucesso.");

                    return View(model);
                }
                catch (NegocioException ex)
                {
                    TratarMensagemException(ex);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Inativar(int chave)
        {
            if ((UsuarioLogadoModel)Session["usuario"] != null)
                if (((UsuarioLogadoModel)Session["usuario"]).IsAdministrador
                || ((UsuarioLogadoModel)Session["usuario"]).Chave == chave)
                {
                    if (((UsuarioLogadoModel)Session["usuario"]).IsPessoaFisica && !((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    {
                        var usuario = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(chave);
                        usuario.IsHabilitado = false;
                        unityOfWork.PessoaFisicaNegocio.Atualizar(usuario);
                        unityOfWork.Commit();
                    }
                    if (!((UsuarioLogadoModel)Session["usuario"]).IsPessoaFisica && !((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    {
                        var usuario = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(chave);
                        usuario.IsHabilitado = false;
                        unityOfWork.PessoaJuridicaNegocio.Atualizar(usuario);
                        unityOfWork.Commit();
                    }
                    if (!(((UsuarioLogadoModel)Session["usuario"]).IsPessoaFisica) && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    {
                        var usuario = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(chave);
                        usuario.IsHabilitado = false;
                        unityOfWork.PessoaFisicaNegocio.Atualizar(usuario);
                        unityOfWork.Commit();
                    }
                    else if (((UsuarioLogadoModel)Session["usuario"]).IsPessoaFisica && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    {
                        var usuario = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(chave);
                        usuario.IsHabilitado = false;
                        unityOfWork.PessoaJuridicaNegocio.Atualizar(usuario);
                        unityOfWork.Commit();
                    }

                }
            if (((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
            {
                if (((UsuarioLogadoModel)Session["usuario"]).IsPessoaFisica && !((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    return RedirectToAction("ListarPessoaFisica");
                else
                    return RedirectToAction("ListarPessoaJuridica");
            }
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        private void AtualizarPessoaJuridica(PessoaJuridica usuarioParaAtualizar, PessoaJuridica usuarioAtualizado)
        {
            usuarioParaAtualizar.Celular = usuarioAtualizado.Celular;
            usuarioParaAtualizar.CNPJ = usuarioAtualizado.CNPJ;
            usuarioParaAtualizar.Descricao = usuarioAtualizado.Descricao;
            usuarioParaAtualizar.Email = usuarioAtualizado.Email;
            usuarioParaAtualizar.Endereco = usuarioAtualizado.Endereco;
            usuarioParaAtualizar.Password = usuarioAtualizado.Password;
            usuarioParaAtualizar.RazaoSocial = usuarioAtualizado.RazaoSocial;
            usuarioParaAtualizar.Telefone = usuarioAtualizado.Telefone;
            usuarioParaAtualizar.Nome = usuarioAtualizado.Nome;
            usuarioParaAtualizar.Imagem = usuarioAtualizado.Imagem;

        }

        private void AtualizarPessoaFisica(PessoaFisica usuarioParaAtualizar, PessoaFisica usuarioAtualizado)
        {
            usuarioParaAtualizar.Celular = usuarioAtualizado.Celular;
            usuarioParaAtualizar.CPF = usuarioAtualizado.CPF;
            usuarioParaAtualizar.Email = usuarioAtualizado.Email;
            usuarioParaAtualizar.Endereco = usuarioAtualizado.Endereco;
            usuarioParaAtualizar.Password = usuarioAtualizado.Password;
            usuarioParaAtualizar.Telefone = usuarioAtualizado.Telefone;
            usuarioParaAtualizar.Nome = usuarioAtualizado.Nome;
        }
        #endregion

        #region Util
        public List<SelectListItem> MontarViewBagEstado()
        {
            return new List<SelectListItem> 
            {
               new SelectListItem{ Text=EstadoEnum.TO.ToString(), Value = ((int)EstadoEnum.TO).ToString()},
                new SelectListItem{ Text=EstadoEnum.SE.ToString(), Value = ((int)EstadoEnum.SE).ToString()},
                new SelectListItem{ Text=EstadoEnum.SP.ToString(), Value = ((int)EstadoEnum.SP).ToString()},
                new SelectListItem{ Text=EstadoEnum.SC.ToString(), Value = ((int)EstadoEnum.SC).ToString()},
                new SelectListItem{ Text=EstadoEnum.RS.ToString(), Value = ((int)EstadoEnum.RS).ToString()},
                new SelectListItem{ Text=EstadoEnum.RN.ToString(), Value = ((int)EstadoEnum.RN).ToString()},
                new SelectListItem{ Text=EstadoEnum.RJ.ToString(), Value = ((int)EstadoEnum.RJ).ToString()},
                new SelectListItem{ Text=EstadoEnum.RO.ToString(), Value = ((int)EstadoEnum.RO).ToString()},
                new SelectListItem{ Text=EstadoEnum.RR.ToString(), Value = ((int)EstadoEnum.RR).ToString()},
                new SelectListItem{ Text=EstadoEnum.PI.ToString(), Value = ((int)EstadoEnum.PI).ToString()},
                new SelectListItem{ Text=EstadoEnum.PE.ToString(), Value = ((int)EstadoEnum.PE).ToString()},
                new SelectListItem{ Text=EstadoEnum.PR.ToString(), Value = ((int)EstadoEnum.PR).ToString()},
                new SelectListItem{ Text=EstadoEnum.PB.ToString(), Value = ((int)EstadoEnum.PB).ToString()},
                new SelectListItem{ Text=EstadoEnum.PA.ToString(), Value = ((int)EstadoEnum.PA).ToString()},
                new SelectListItem{ Text=EstadoEnum.MG.ToString(), Value = ((int)EstadoEnum.MG).ToString()},
                new SelectListItem{ Text=EstadoEnum.MS.ToString(), Value = ((int)EstadoEnum.MS).ToString()},
                new SelectListItem{ Text=EstadoEnum.MT.ToString(), Value = ((int)EstadoEnum.MT).ToString()},
                new SelectListItem{ Text=EstadoEnum.MA.ToString(), Value = ((int)EstadoEnum.MA).ToString()},
                new SelectListItem{ Text=EstadoEnum.GO.ToString(), Value = ((int)EstadoEnum.GO).ToString()},
                new SelectListItem{ Text=EstadoEnum.ES.ToString(), Value = ((int)EstadoEnum.ES).ToString()},
                new SelectListItem{ Text=EstadoEnum.DF.ToString(), Value = ((int)EstadoEnum.DF).ToString()},
                new SelectListItem{ Text=EstadoEnum.CE.ToString(), Value = ((int)EstadoEnum.CE).ToString()},
                new SelectListItem{ Text=EstadoEnum.BA.ToString(), Value = ((int)EstadoEnum.BA).ToString()},
                new SelectListItem{ Text=EstadoEnum.AM.ToString(), Value = ((int)EstadoEnum.AM).ToString()},
                new SelectListItem{ Text=EstadoEnum.AP.ToString(), Value = ((int)EstadoEnum.AP).ToString()},
                new SelectListItem{ Text=EstadoEnum.AL.ToString(), Value = ((int)EstadoEnum.AL).ToString()},
                new SelectListItem{ Text=EstadoEnum.AC.ToString(), Value = ((int)EstadoEnum.AC).ToString()}
            };
        }
        #endregion
    }
}