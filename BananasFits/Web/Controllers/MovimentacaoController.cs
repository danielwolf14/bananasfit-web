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

namespace Web.Controllers
{
    public class MovimentacaoController : BaseController
    {
        private const int valorFit = 5;

        #region View Comprar Fits
        public ActionResult ComprarFits()
        {
            return View();
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
            var usuario =(UsuarioLogadoModel)Session["usuario"];
            var historico = GerarHistoricoServico(usuario, pessoaFisica, academia, dataInicial, dataFinal,null);

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

        #region Comprar PagSeguro
        #endregion
    }
}