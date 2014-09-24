using Map = AutoMapper;
using Web.ViewModels;
using Processo;
using Processo.Database;
using Processo.Entidades;
using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Web.Controllers
{
    public class ServicoController : BaseController
    {
        public ActionResult Qrcode(string codigo)
        {
            ViewBag.Codigo = codigo;
            return View();
        }

        public ActionResult Listar(string currentFilter, string searchString, int? page)
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

                var servicos = unityOfWork.ServicoNegocio.Consultar(e => e.IsHabilitado);

                if (!String.IsNullOrEmpty(searchString))
                {
                    servicos = servicos.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
                }

                servicos = servicos.OrderBy(e => e.Nome);

                int pageSize = 5;
                int pageNumber = (page ?? 1);

                return View(servicos.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Associar()
        {
            ViewBag.Servicos = unityOfWork.ServicoNegocio
                .Consultar(e => e.IsHabilitado)
                .Select(e => new SelectListItem { Value = e.Chave.ToString(), Text = e.Nome });
            var usuario = ((UsuarioLogadoModel)Session["usuario"]);
            if (usuario != null && !usuario.IsPessoaFisica)
            {
                var servicos = unityOfWork.ServicoPessoaJuridicaNegocio.Consultar(e => e.IsHabilitado && e.PessoaJuridica.Chave == usuario.Chave);
                servicos = servicos.OrderBy(e => e.Servico.Nome);
                int pageSize = servicos.Count();
                return View(servicos.ToPagedList(1, pageSize == 0 ? 1 : pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Associar(string servico, string valor)
        {
            if (string.IsNullOrEmpty(servico))
            {
                //TODO:Validar Serviço
                ExibirMensagemErro("Serviço é um campo obrigatório.");
                return RedirectToAction("Associar");
            }
            if (string.IsNullOrEmpty(valor))
            {
                //TODO: Validar Valor
                ExibirMensagemErro("Valor é um campo obrigatório.");
                return RedirectToAction("Associar");
            }

            var servicoParaAdicionar = unityOfWork.ServicoNegocio.BuscarPorChave(Convert.ToInt32(servico));
            var servicoPJuridica = unityOfWork.ServicoPessoaJuridicaNegocio.Consultar(e => e.Servico.Chave == servicoParaAdicionar.Chave).FirstOrDefault();

            var pJuridica = unityOfWork.PessoaJuridicaNegocio.BuscarPorChave(((UsuarioLogadoModel)Session["usuario"]).Chave);
            if (servicoPJuridica != null)
            {
                servicoPJuridica.Valor = Convert.ToInt32(valor);
                servicoPJuridica.IsHabilitado = true;
                unityOfWork.ServicoPessoaJuridicaNegocio.Atualizar(servicoPJuridica);
            }
            else
            {
                //Tratar Objeto
                servicoPJuridica = new ServicoPessoaJuridica
                {
                    IsHabilitado = true,
                    PessoaJuridica = pJuridica,
                    QRCode = pJuridica.Chave.ToString() + servicoParaAdicionar.Chave.ToString(),
                    Servico = servicoParaAdicionar,
                    Valor = Convert.ToInt32(valor)
                };
                unityOfWork.ServicoPessoaJuridicaNegocio.Inserir(servicoPJuridica);
            }
            unityOfWork.Commit();

            ExibirMensagemSucesso("Cadastrado com sucesso");
            return RedirectToAction("Associar");
        }

        public ActionResult InativarServicoPessoaJuridica(int chave)
        {
            if (((UsuarioLogadoModel)Session["usuario"]) != null)
            {
                var servico = unityOfWork.ServicoPessoaJuridicaNegocio.BuscarPorChave(chave);
                if (servico.PessoaJuridica.Chave == ((UsuarioLogadoModel)Session["usuario"]).Chave)
                {
                    servico.IsHabilitado = false;
                    unityOfWork.ServicoPessoaJuridicaNegocio.Atualizar(servico);
                    unityOfWork.Commit();
                }
            }
            return RedirectToAction("Associar");
        }

        public ActionResult ListarAssociar()
        {
            if (Session["usuario"] != null)
            {
                var servicos = unityOfWork.ServicoPessoaJuridicaNegocio.Consultar(e => e.IsHabilitado);

                //if (!String.IsNullOrEmpty(searchString))
                //{
                //    usuarios = usuarios.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
                //}

                servicos = servicos.OrderBy(e => e.Servico.Nome);

                int pageSize = servicos.Count();
                //int pageNumber = (page ?? 1);
                return View(servicos.ToPagedList(1, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Cadastrar(CadastroServicoViewModel model, HttpPostedFileBase img)
        {
            if (Session["usuario"] != null && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
            {
                if (img == null)
                {
                    ExibirMensagemErro("Imagem é um campo obrigatório.");
                }
                else if (ModelState.IsValid)
                {
                    var servico = Map.Mapper.DynamicMap<Servico>(model);
                    servico.IsHabilitado = true;
                    try
                    {
                        unityOfWork.ServicoNegocio.Cadastrar(servico);
                        unityOfWork.Commit();
                        servico.Imagem = SalvarImagem(img, servico.Chave, servico.Nome);
                        unityOfWork.ServicoNegocio.Atualizar(servico);
                        unityOfWork.Commit();
                        ExibirMensagemSucesso("Serviço cadastrado com sucesso.");
                    }
                    catch (NegocioException ex)
                    {
                        TratarMensagemException(ex);
                    }
                    return RedirectToAction("Listar");
                }
            }
            else
                return RedirectToAction("Index", "Home");

            return View();
        }

        public ActionResult Cadastrar()
        {
            return PartialView();
        }

        public ActionResult Inativar(int chave)
        {
            if (Session["usuario"] != null && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
            {
                var servico = unityOfWork.ServicoNegocio.BuscarPorChave(chave);
                servico.IsHabilitado = false;
                unityOfWork.ServicoNegocio.Atualizar(servico);
                unityOfWork.Commit();
                ExibirMensagemSucesso("Serviço deletado com sucesso.");
                return RedirectToAction("Listar", "Servico");
            }
            else
                ExibirMensagemErro("Erro ao deletar serviço.");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Atualizar(int chave)
        {
            var servico = unityOfWork.ServicoNegocio.BuscarPorChave(chave);
            var model = Map.Mapper.DynamicMap<AtualizarServicoViewModel>(servico);

            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Atualizar(AtualizarServicoViewModel model, HttpPostedFileBase imagem)
        {
            if (Session["usuario"] != null && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
            {
                var servico = unityOfWork.ServicoNegocio.BuscarPorChave(model.Chave);
                servico.Nome = model.Nome;
                if (imagem != null)
                    servico.Imagem = SalvarImagem(imagem, servico.Chave, servico.Nome);
                unityOfWork.ServicoNegocio.Atualizar(servico);
                unityOfWork.Commit();
                return RedirectToAction("Listar", "Servico");
            }
            ExibirMensagemErro("Erro ao atualizar serviço.");
            return RedirectToAction("Listar", "Servico");
        }

        public string SalvarImagem(HttpPostedFileBase imagem, int idServico, string nomeServico)
        {
            string[] strName = imagem.FileName.Split('.');
            string extensao = strName[strName.Count() - 1];
            string caminhoSalvo = String.Format("{0}{1}.{2}", Server.MapPath("~/images/ImagemServico/"), nomeServico + idServico, extensao);
            string caminhoFinal = String.Format("/images/ImagemServico/{0}.{1}", nomeServico + idServico, extensao);
            imagem.SaveAs(caminhoSalvo);
            return caminhoFinal;
        }

    }
}