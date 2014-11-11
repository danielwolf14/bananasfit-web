using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Constants.PreApproval;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;
using Web.ViewModels;
using PagedList;
using Processo.Entidades;

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
        public ActionResult ListarHistoricoServico(string currentFilter, string searchString, int? page)
        {
            IEnumerable<HistoricoCompraServico> historicoCompraServico;
            var usuario = (UsuarioLogadoModel)Session["usuario"];
            if (usuario != null)
            {
                if (usuario.IsAdministrador)
                    historicoCompraServico = unityOfWork.HistoricoCompraServicoNegocio.ConsultarTodos();
                else if(usuario.IsPessoaFisica)
                {
                    historicoCompraServico = unityOfWork.HistoricoCompraServicoNegocio.Consultar(e => e.PessoaFisica.Chave == usuario.Chave);
                }
                else 
                {
                    historicoCompraServico = unityOfWork.HistoricoCompraServicoNegocio.Consultar(e => e.Servico.PessoaJuridica.Chave == usuario.Chave);
                }
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                historicoCompraServico = historicoCompraServico.OrderBy(e => e.Data);

                int pageSize = 5;
                int pageNumber = (page ?? 1);

                return View(historicoCompraServico.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Comprar PagSeguro
        public ActionResult Comprar(string valor)
        {
            var pessoaFisica = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(((UsuarioLogadoModel)Session["usuario"]).Chave);
            //model.PessoaFisica = unityOfWork.PessoaFisicaNegocio.ConsultarTodos().FirstOrDefault();
            //model.QuantidadeFits = 300;
            bool isSandbox = false;

            EnvironmentConfiguration.ChangeEnvironment(isSandbox);

            try
            {

                AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);

                // Instantiate a new payment request
                PaymentRequest payment = new PaymentRequest();

                // Sets the currency
                payment.Currency = Currency.Brl;

                // Add an item for this payment request
                payment.Items.Add(new Item("0001", "Notebook Prata", 1, 2430.00m));


                // Add another item for this payment request
                payment.Items.Add(new Item("0002", "Notebook Rosa", 2, 150.99m));

                // Sets a reference code for this payment request, it is useful to identify this payment in future notifications.
                payment.Reference = "REF1234";

                // Sets shipping information for this payment request
                payment.Shipping = new Shipping();
                payment.Shipping.ShippingType = ShippingType.Sedex;

                //Passando valor para ShippingCost
                payment.Shipping.Cost = 10.00m;

                payment.Shipping.Address = new Address(
                    "BRA",
                    "SP",
                    "Sao Paulo",
                    "Jardim Paulistano",
                    "01452002",
                    "Av. Brig. Faria Lima",
                    "1384",
                    "5o andar"
                );

                // Sets your customer information.
                payment.Sender = new Sender(
                    "Joao Comprador",
                    "comprador@sandbox.pagseguro.com.br",
                    new Phone("11", "56273440")
                );

                // Sets the url used by PagSeguro for redirect user after ends checkout process
                payment.RedirectUri = new Uri("http://www.lojamodelo.com.br");

                // Add checkout metadata information
                payment.AddMetaData(MetaDataItemKeys.GetItemKeyByDescription("CPF do passageiro"), "123.456.789-09", 1);
                payment.AddMetaData("PASSENGER_PASSPORT", "23456", 1);

                // Another way to set checkout parameters
                payment.AddParameter("senderBirthday", "07/05/1980");
                payment.AddIndexedParameter("itemColor", "verde", 1);
                payment.AddIndexedParameter("itemId", "0003", 3);
                payment.AddIndexedParameter("itemDescription", "Mouse", 3);
                payment.AddIndexedParameter("itemQuantity", "1", 3);
                payment.AddIndexedParameter("itemAmount", "200.00", 3);

                SenderDocument senderCPF = new SenderDocument(Documents.GetDocumentByType("CPF"), "12345678909");
                payment.Sender.Documents.Add(senderCPF);

                Uri paymentRedirectUri = payment.Register(credentials);

                Console.WriteLine("URL do pagamento : " + paymentRedirectUri);
                Console.ReadKey();
            }
            catch (PagSeguroServiceException exception)
            {
                Console.WriteLine(exception.Message + "\n");

                foreach (ServiceError element in exception.Errors)
                {
                    Console.WriteLine(element + "\n");
                }
                Console.ReadKey();
            }
            return null;
        }
        #endregion
    }
}