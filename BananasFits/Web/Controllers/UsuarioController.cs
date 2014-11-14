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
        

        #region Listar e buscar
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

        public ActionResult BuscarPessoaJuridica(string nome, int? valor,
            int? page, int? pontuacao, int? servico, string estado)
        {

            ViewBag.Nome = nome;
            ViewBag.Valor = valor;
            ViewBag.Pontuacao = pontuacao;
            ViewBag.Servico = servico;
            ViewBag.Estado = estado;

            ViewBag.ListaServico = unityOfWork.ServicoNegocio.Consultar(e => e.IsHabilitado);
            var pessoasJuridicas = unityOfWork.PessoaJuridicaNegocio.Consultar(e => e.IsHabilitado);

            if (valor != null)
            {
                pessoasJuridicas = pessoasJuridicas.ToList().Where(e => e.Servicos.Any(s => s.Valor <= valor));
            }
            if (pontuacao != null)
            {
                pessoasJuridicas = pessoasJuridicas.ToList().Where(e => e.Avaliacoes.Count != 0 &&
                    e.Avaliacoes.Average(f => f.Pontuacao) == pontuacao);
            }
            if (servico != null)
            {
                pessoasJuridicas = pessoasJuridicas.ToList().Where(x => x.Servicos.Any(z => z.Servico.Chave == servico));
            }
            if (!string.IsNullOrEmpty(nome))
            {
                pessoasJuridicas = pessoasJuridicas.Where(s => s.Nome.ToUpper().Contains(nome.ToString().ToUpper()));
            }

            if (estado != null)
            {
                pessoasJuridicas = pessoasJuridicas.ToList().Where(e => e.Endereco.Estado.ToUpper().Contains(estado.ToString().ToUpper()));
            }
            if (pessoasJuridicas == null || pessoasJuridicas.Count() == 0)
            {
                ExibirMensagemErro("Nenhuma academia encontrada!");
            }

            pessoasJuridicas = pessoasJuridicas.OrderBy(e => e.Nome);

            ViewBag.Ranking = unityOfWork.PessoaJuridicaNegocio
                .Consultar(e => e.Avaliacoes.Count > 0)
                .OrderByDescending(e => e.Avaliacoes.Average(f => f.Pontuacao))
                .Take(5)
                .Select(e => new RankingViewModel { Nome = e.Nome, Pontuacao = (int)e.Avaliacoes.Average(f => f.Pontuacao) }); ;


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(pessoasJuridicas.ToPagedList(pageNumber, pageSize));

        }
        #endregion

        #region Detalhar Busca
        public ActionResult DetalharPessoaJuridica(int chave)
        {
            var usuario = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(chave);
            var model = Map.Mapper.DynamicMap<DetalharBuscaPessoaJuridicaViewModel>(usuario);
            model.QuantidadeAvaliacao = unityOfWork.AvaliacaoNegocio.Consultar(e => e.PessoaJuridica.Chave == chave).Count();
            model.Servicos = unityOfWork.ServicoPessoaJuridicaNegocio.Consultar(e => e.PessoaJuridica.Chave == chave).ToList();
            if (Session["usuario"] != null)
            {
                var chaveUsuarioLogado = ((UsuarioLogadoModel)Session["usuario"]).Chave;
                var avaliacao = unityOfWork.AvaliacaoNegocio
                    .Consultar(e => e.PessoaFisica.Chave == chaveUsuarioLogado
                    && e.PessoaJuridica.Chave == chave).FirstOrDefault();
                model.Avaliacao = avaliacao == null ? 0 : avaliacao.Pontuacao;
            }
            return View(model);
        }
        #endregion

        #region Login
        public ActionResult Login()
        {
            if (Session["usuario"] != null)
                return RedirectToAction("Index", "Home");
            return View();
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

        #region Cadastrar Usuário

        public ActionResult CadastrarPessoaJuridica()
        {
            if (Session["usuario"] != null && !((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public ActionResult CadastrarPessoaFisica()
        {
            if (Session["usuario"] != null && !((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult CadastrarPessoaJuridica(CadastroPessoaJuridicaViewModel model, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var endereco = Map.Mapper.DynamicMap<Endereco>(model.Endereco);
                var usuario = Map.Mapper.DynamicMap<PessoaJuridica>(model);
                try
                {

                    if (img.ContentLength > 100000)
                    {
                        ExibirMensagemErro("Imagem maior que o permitido 1Mb");
                    }
                    else
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
            string caminhoSalvo = String.Format("{0}{1}.{2}", Server.MapPath("~/images/ImagemPessoaJuridica/"), nomePessoaJuridica.Trim() + idPessoaJuridica, extensao);
            string caminhoFinal = String.Format("/images/ImagemPessoaJuridica/{0}.{1}", nomePessoaJuridica.Trim() + idPessoaJuridica, extensao);
            imagem.SaveAs(caminhoSalvo);
            return caminhoFinal;
        }

        [HttpPost]
        public ActionResult CadastrarPessoaFisica(CadastroPessoaFisicaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var endereco = Map.Mapper.DynamicMap<Endereco>(model.Endereco);
                var usuario = Map.Mapper.DynamicMap<PessoaFisica>(model);
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


        #region Minha Conta, Atualizar e Inativar usuário

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
            var usuarioLogado = (UsuarioLogadoModel)Session["usuario"];
            if (usuarioLogado != null && !usuarioLogado.IsPessoaFisica)
            {
                var usuario = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(usuarioLogado.Chave);
                var model = Map.Mapper.DynamicMap<DetalharPessoaJuridicaViewModel>(usuario);

                return View(model);
            }
            return RedirectToAction("Index", "Home");

        }

        public ActionResult MinhaContaPessoaFisica()
        {
            var usuarioLogado = (UsuarioLogadoModel)Session["usuario"];
            if (usuarioLogado != null && usuarioLogado.IsPessoaFisica)
            {
                var usuario = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(usuarioLogado.Chave);
                var model = Map.Mapper.DynamicMap<DetalharPessoaFisicaViewModel>(usuario);

                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult MinhaContaPessoaJuridica(DetalharPessoaJuridicaViewModel model, HttpPostedFileBase img)
        {
            if (ModelState.IsValid && (UsuarioLogadoModel)Session["usuario"] != null
                && ((UsuarioLogadoModel)Session["usuario"]).Chave == model.Chave)
            {
                var usuarioAtualizado = Map.Mapper.DynamicMap<PessoaJuridica>(model);
                var usuarioParaAtualizar = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(usuarioAtualizado.Chave);
                try
                {
                    if (img != null)
                        usuarioParaAtualizar.Imagem = SalvarImagem(img, usuarioParaAtualizar.Chave, usuarioParaAtualizar.Nome);

                    AtualizarPessoaJuridica(usuarioParaAtualizar, usuarioAtualizado);
                    unityOfWork.PessoaJuridicaNegocio.AtualizarConta(usuarioParaAtualizar);
                    unityOfWork.Commit();

                    ExibirMensagemSucesso("Usuário atualizado com sucesso.");
                    model = Map.Mapper.DynamicMap<DetalharPessoaJuridicaViewModel>(usuarioParaAtualizar);
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
            if (ModelState.IsValid && (UsuarioLogadoModel)Session["usuario"] != null
                && ((UsuarioLogadoModel)Session["usuario"]).Chave == model.Chave)
            {
                var usuarioAtualizado = Map.Mapper.DynamicMap<PessoaFisica>(model);
                var usuarioParaAtualizar = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(usuarioAtualizado.Chave);
                try
                {
                    AtualizarPessoaFisica(usuarioParaAtualizar, usuarioAtualizado);
                    unityOfWork.PessoaFisicaNegocio.AtualizarConta(usuarioParaAtualizar);

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

        public ActionResult InativarPessoaFisica(int chave)
        {
            if ((UsuarioLogadoModel)Session["usuario"] != null)
            {
                if (((UsuarioLogadoModel)Session["usuario"]).IsAdministrador
                || ((UsuarioLogadoModel)Session["usuario"]).Chave == chave)
                {
                    var usuario = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(chave);
                    usuario.IsHabilitado = false;
                    unityOfWork.PessoaFisicaNegocio.Atualizar(usuario);
                    unityOfWork.Commit();

                    ExibirMensagemSucesso("Usuário deletado com sucesso.");

                    if (((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    {
                        return RedirectToAction("ListarPessoaFisica");
                    }
                }
            }
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult InativarPessoaJuridica(int chave)
        {
            if ((UsuarioLogadoModel)Session["usuario"] != null)
            {
                if (((UsuarioLogadoModel)Session["usuario"]).IsAdministrador
                || ((UsuarioLogadoModel)Session["usuario"]).Chave == chave)
                {
                    var usuario = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(chave);
                    usuario.IsHabilitado = false;
                    unityOfWork.PessoaJuridicaNegocio.Atualizar(usuario);
                    unityOfWork.Commit();

                    ExibirMensagemSucesso("Usuário deletado com sucesso.");

                    if (((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
                    {
                        return RedirectToAction("ListarPessoaJuridica");
                    }
                }
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
            usuarioParaAtualizar.Password = usuarioAtualizado.Password;
            usuarioParaAtualizar.RazaoSocial = usuarioAtualizado.RazaoSocial;
            usuarioParaAtualizar.Telefone = usuarioAtualizado.Telefone;
            usuarioParaAtualizar.Nome = usuarioAtualizado.Nome;
            AtualizarEndereco(usuarioParaAtualizar.Endereco, usuarioAtualizado.Endereco);
        }

        private void AtualizarPessoaFisica(PessoaFisica usuarioParaAtualizar, PessoaFisica usuarioAtualizado)
        {
            usuarioParaAtualizar.Celular = usuarioAtualizado.Celular;
            usuarioParaAtualizar.CPF = usuarioAtualizado.CPF;
            usuarioParaAtualizar.Email = usuarioAtualizado.Email;
            usuarioParaAtualizar.Password = usuarioAtualizado.Password;
            usuarioParaAtualizar.Telefone = usuarioAtualizado.Telefone;
            usuarioParaAtualizar.Nome = usuarioAtualizado.Nome;
            AtualizarEndereco(usuarioParaAtualizar.Endereco, usuarioAtualizado.Endereco);
        }

        private void AtualizarEndereco(Endereco enderecoParaAtualizar, Endereco novoEndereco)
        {
            enderecoParaAtualizar.Rua = novoEndereco.Rua;
            enderecoParaAtualizar.Numero = novoEndereco.Numero;
            enderecoParaAtualizar.Complemento = novoEndereco.Complemento;
            enderecoParaAtualizar.Cidade = novoEndereco.Cidade;
            enderecoParaAtualizar.Bairro = novoEndereco.Bairro;
            enderecoParaAtualizar.CEP = novoEndereco.CEP;
            enderecoParaAtualizar.Estado = novoEndereco.Estado;

        }
        #endregion
    }
}