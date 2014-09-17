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

                int pageSize = 10;
                int pageNumber = (page ?? 1);

                return View(servicos.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }
               
        public ActionResult Associar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(CadastroServicoViewModel model, HttpPostedFileBase img)
        {
            if (Session["usuario"] != null && ((UsuarioLogadoModel)Session["usuario"]).IsAdministrador)
            {
                if (ModelState.IsValid)
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
                        return RedirectToAction("Listar");
                    }
                    catch (NegocioException ex)
                    {
                        TratarMensagemException(ex);
                    }
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

        public ActionResult Atualizar()
        {

            return View();
        }

        private void Atualizar(Servico servicoParaAtualizar, Servico servicoAtualizado)
        {
            servicoParaAtualizar.Nome = servicoAtualizado.Nome;
            servicoParaAtualizar.Imagem = servicoAtualizado.Imagem;
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