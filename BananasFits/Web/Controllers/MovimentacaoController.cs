using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.ViewModels;
using PagedList;
using Processo.Entidades;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using PayPal.Api.Payments;
using PayPal;

namespace Web.Controllers
{
    public class MovimentacaoController : BaseController
    {
        private const int valorFit = 5;

        #region View Comprar Fits
        public ActionResult ComprarFits()
        {
            ViewBag.MesVencimento = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "Janeiro", Value = "01"},
                new SelectListItem{ Text = "Fevereiro", Value = "02"},
                new SelectListItem{Text = "Março", Value = "03"},
                new SelectListItem{Text = "Abril", Value = "04"},
                new SelectListItem{Text = "Maio", Value = "05"},
                new SelectListItem{Text = "Junho", Value = "06"},
                new SelectListItem{Text = "Julho", Value = "07"},
                new SelectListItem{Text = "Agosto", Value = "08"},
                new SelectListItem{Text = "Setembro", Value = "09"},
                new SelectListItem{Text = "Outubro", Value = "10"},
                new SelectListItem{Text = "Novembro", Value = "11"},
                new SelectListItem{Text = "Dezembro", Value = "12"},
            };
            List<SelectListItem> anos = new List<SelectListItem>();

            for (int i = 0; i < 11; i++)
            {
                anos.Add(new SelectListItem
                {
                    Value = DateTime.Now.AddYears(i).Year.ToString(),
                    Text = DateTime.Now.AddYears(i).Year.ToString()
                });
            }
            ViewBag.AnoVencimento = anos;

            ViewBag.TipoCartao = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "Visa", Value = "visa"},
                new SelectListItem{ Text = "MasterCard", Value = "master"},
            };

            ViewBag.Valor = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "1 Fit ->  R$5,00", Value = "1"},
                new SelectListItem{ Text = "5 Fits ->  R$25,00", Value = "5"},
                new SelectListItem{ Text = "10 Fits ->  R$50,00", Value = "10"},
                new SelectListItem{ Text = "15 Fits ->  R$75,00", Value = "15"},
            };

            return View();
        }
        [HttpPost]
        public ActionResult ComprarFits(ComprarFitsViewModel model)
        {
            Util.PayPalNegocio paypalNegocio = new Util.PayPalNegocio("AUoYdBAqgl5mugEOu-xrxNeLj0DW2CohcYODtyxzsozi-me48ymybDi6dtw2",
          "ELyImxCvpvxoiFRyfqzScMZbfo84f2Au4l-TJX78ymKuHskG_pDAcJHHt3uf", "sandbox", 5);

            var usuario = (UsuarioLogadoModel)Session["usuario"];

            CreditCard creditCard = new CreditCard();
            creditCard.number = model.NumeroCartao;
            creditCard.type = model.TipoCartao;
            creditCard.expire_month = Convert.ToInt32(model.Mes);
            creditCard.expire_year = Convert.ToInt32(model.Ano);
            creditCard.first_name = usuario.Email;
            creditCard.cvv2 = Convert.ToInt32(model.Cvv);


            if (usuario.IsPessoaFisica)
            {
                var pessoaFisica = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(usuario.Chave);
                //realizou pagamento
                paypalNegocio.EfetuarCompra(pessoaFisica,
                    creditCard, model.QuantidadeFits);
                //creditou fits
                unityOfWork.PessoaFisicaNegocio.CreditarFits(pessoaFisica, model.QuantidadeFits);
            }
            else
                ExibirMensagemErro("Compra não autorizada para usuários que sejam pessoa jurídica.");

            ExibirMensagemSucesso("Compra realizada com sucesso.");
            return RedirectToAction("ComprarFits");
        }
        #endregion

        #region View Historico Compra Fitis
        public ActionResult HistoricoCompraFits()
        {
            return View();
        }
        #endregion

        #region Listar Histórico de Serviços
        public ActionResult ListarHistoricoServico(string pessoaFisica, string academia,
            DateTime? dataInicial, DateTime? dataFinal, int? page)
        {
            var usuario = (UsuarioLogadoModel)Session["usuario"];
            if (usuario != null)
            {
                var historicoCompraServico = GerarHistoricoServico(usuario, pessoaFisica, academia, dataInicial, dataFinal, page);
                ViewBag.PessoaFisica = pessoaFisica;
                ViewBag.Academia = academia;
                ViewBag.DataInicial = dataInicial;
                ViewBag.DataFinal = dataFinal;
                ViewBag.Total = historicoCompraServico.Sum(e => e.Valor);

                historicoCompraServico = historicoCompraServico.OrderByDescending(e => e.Data);

                int pageSize = 10;
                int pageNumber = (page ?? 1);

                return View(historicoCompraServico.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GerarExcel(string pessoaFisica, string academia,
            DateTime? dataInicial, DateTime? dataFinal, int? page)
        {
            var usuario = (UsuarioLogadoModel)Session["usuario"];
            var historico = GerarHistoricoServico(usuario, pessoaFisica, academia, dataInicial, dataFinal, null);

            IWorkbook workbook = new XSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("Historico");

            int rowNumer = 0;

            IRow row = sheet.CreateRow(rowNumer);
            ICell cell;

            ICellStyle style = workbook.CreateCellStyle();
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            style.FillPattern = FillPattern.SolidForeground;

            cell = row.CreateCell(0);
            cell.SetCellValue("Pessoa Física");
            cell.CellStyle = style;

            cell = row.CreateCell(1);
            cell.SetCellValue("Serviço");
            cell.CellStyle = style;

            cell = row.CreateCell(2);
            cell.SetCellValue("Academia");
            cell.CellStyle = style;

            cell = row.CreateCell(3);
            cell.SetCellValue("Data");
            cell.CellStyle = style;

            cell = row.CreateCell(4);
            cell.SetCellValue("Valor");
            cell.CellStyle = style;

            //---- row

            foreach (var item in historico)
            {
                rowNumer++;
                row = sheet.CreateRow(rowNumer);
                row.CreateCell(0).SetCellValue(item.NomePessoaFisica);
                row.CreateCell(1).SetCellValue(item.NomeServico);
                row.CreateCell(2).SetCellValue(item.NomePessoaJuridica);
                row.CreateCell(3).SetCellValue(item.Data.ToString());
                row.CreateCell(4).SetCellFormula(item.Valor.ToString());

            }

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);

            return File(stream.ToArray(), //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                string.Format("HistoricoCompraServico.xlsx", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
        }

        public IEnumerable<HistoricoCompraServico> GerarHistoricoServico(UsuarioLogadoModel usuario, string pessoaFisica, string academia,
            DateTime? dataInicial, DateTime? dataFinal, int? page)
        {

            IEnumerable<HistoricoCompraServico> historicoCompraServico;
            if (usuario.IsAdministrador)
                historicoCompraServico = unityOfWork.HistoricoCompraServicoNegocio.ConsultarTodos();
            else if (usuario.IsPessoaFisica)
            {
                historicoCompraServico = unityOfWork.HistoricoCompraServicoNegocio.Consultar(e => e.PessoaFisica.Chave == usuario.Chave);
            }
            else
            {
                historicoCompraServico = unityOfWork.HistoricoCompraServicoNegocio.Consultar(e => e.Servico.PessoaJuridica.Chave == usuario.Chave);
            }
            if (!string.IsNullOrEmpty(academia))
            {
                historicoCompraServico = historicoCompraServico.Where(e => e.NomePessoaJuridica.ToUpper().Contains(academia.ToUpper()));
            }
            if (dataInicial != null && dataFinal != null)
            {
                historicoCompraServico = historicoCompraServico.Where(e => e.Data.Date >= dataInicial.Value.Date && e.Data.Date <= dataFinal.Value.Date);
            }
            if (!string.IsNullOrEmpty(pessoaFisica))
            {
                historicoCompraServico = historicoCompraServico.Where(e => e.NomePessoaFisica.ToUpper().Contains(pessoaFisica.ToUpper()));
            }
            return historicoCompraServico;
        }

        #endregion


    }
}