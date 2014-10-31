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

namespace Web.Controllers
{
    public class MovimentacaoController : BaseController
    {

        #region View Comprar Fits
        public ActionResult ComprarFits()
        {
            return View();
        }
        #endregion

        #region Historico Compra Fitis
        public ActionResult HistoricoCompraFits()
        {
            return View();
        }
        #endregion

        #region Historico Compra Servico
        public ActionResult HistoricoCompraServico()
        {
            return View();
        }
        #endregion

        private const int valorFit = 5;

        public ActionResult Comprar(string valor)
        {
            var pessoaFisica = unityOfWork.PessoaFisicaNegocio.BuscarPorChave(((UsuarioLogadoModel)Session["usuario"]).Chave);
            //model.PessoaFisica = unityOfWork.PessoaFisicaNegocio.ConsultarTodos().FirstOrDefault();
            //model.QuantidadeFits = 300;
            bool isSandbox = true;

            EnvironmentConfiguration.ChangeEnvironment(isSandbox);

            try
            {

                //AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);

                AccountCredentials credentials = new AccountCredentials("raphael.marques.info@gmail.com", "361D9F58B14749E6A11CB74E4BA51A1E");

                // Instantiate a new payment request
                PaymentRequest payment = new PaymentRequest();
                payment.Currency = Currency.Brl;
                payment.Items.Add(new Item("0001", "Notebook Prata", 1, 2430.00m));
                payment.Reference = "REF1234";
                payment.Shipping = new Shipping();
                payment.Shipping.ShippingType = ShippingType.Sedex;
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
                payment.Sender = new Sender(
                    "Joao Comprador",
                    "xxxxxx.xxxxx@sandbox.pagseguro.com.br",
                    new Phone("11", "56273440")
                );
                payment.RedirectUri = new Uri("http://www.lojamodelo.com.br");
                payment.AddMetaData(MetaDataItemKeys.GetItemKeyByDescription("CPF do passageiro"), "123.456.789-09", 1);

                SenderDocument senderCPF = new SenderDocument(Documents.GetDocumentByType("CPF"), "12345678909");
                payment.Sender.Documents.Add(senderCPF);
                payment.PreApproval = new PreApproval();
                var now = DateTime.Now;
                // Only works with Manual
                payment.PreApproval.Charge = Charge.Manual;

                payment.PreApproval.Name = "Seguro contra roubo do Notebook";
                payment.PreApproval.AmountPerPayment = 100.00m;
                payment.PreApproval.MaxAmountPerPeriod = 100.00m;
                payment.PreApproval.Details = string.Format("Todo dia {0} será cobrado o valor de {1} referente ao seguro contra roubo do Notebook.", now.Day, payment.PreApproval.AmountPerPayment.ToString("C2"));
                payment.PreApproval.Period = Period.Monthly;
                payment.PreApproval.DayOfMonth = now.Day;
                payment.PreApproval.InitialDate = now;
                payment.PreApproval.FinalDate = now.AddMonths(6);
                payment.PreApproval.MaxTotalAmount = 600.00m;
                payment.PreApproval.MaxPaymentsPerPeriod = 1;

                payment.ReviewUri = new Uri("http://www.lojamodelo.com.br/revisao");

                Uri paymentRedirectUri = payment.Register(credentials);

                return Redirect(paymentRedirectUri.AbsoluteUri);
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

            //    AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);

            //    // Instantiate a new payment request
            //    PaymentRequest payment = new PaymentRequest();

            //    // Sets the currency
            //    payment.Currency = Currency.Brl;

            //    // Add an item for this payment request
            //    payment.Items.Add(new Item("0001", "Fits", model.QuantidadeFits, model.QuantidadeFits*valorFit));

            //    // Sets a reference code for this payment request, it is useful to identify this payment in future notifications.
            //    payment.Reference = "REF1234";

            //    // Sets shipping information for this payment request
            //    payment.Shipping = new Shipping();
            //    payment.Shipping.ShippingType = ShippingType.Sedex;

            //    //Passando valor para ShippingCost
            //    payment.Shipping.Cost = 0m;

            //    payment.Shipping.Address = new Address(
            //        "BRA",
            //        model.PessoaFisica.Endereco.Estado,
            //        model.PessoaFisica.Endereco.Cidade,
            //        model.PessoaFisica.Endereco.Bairro,
            //        model.PessoaFisica.Endereco.CEP,
            //        model.PessoaFisica.Endereco.Rua,
            //        model.PessoaFisica.Endereco.Numero,
            //        model.PessoaFisica.Endereco.Complemento
            //    );

            //    // Sets your customer information.
            //    payment.Sender = new Sender(
            //        model.PessoaFisica.Nome,
            //        model.PessoaFisica.Email,
            //        new Phone(model.PessoaFisica.Telefone.Substring(0,2), model.PessoaFisica.Telefone.Substring(2))
            //    );

            //    // Sets the url used by PagSeguro for redirect user after ends checkout process
            //    payment.RedirectUri = new Uri("http://www.bananasfit.com.br/movimentacao/checkout");

            //    // Add checkout metadata information
            //    payment.AddMetaData(MetaDataItemKeys.GetItemKeyByDescription("CPF do passageiro"), model.PessoaFisica.CPF, 1);
            //    //payment.AddMetaData("PASSENGER_PASSPORT", "23456", 1);

            //    // Another way to set checkout parameters
            //    //payment.AddParameter("senderBirthday", "07/05/1980");
            //    //payment.AddIndexedParameter("itemColor", "verde", 1);
            //    //payment.AddIndexedParameter("itemId", "0003", 3);
            //    //payment.AddIndexedParameter("itemDescription", "Mouse", 3);
            //    //payment.AddIndexedParameter("itemQuantity", "1", 3);
            //    //payment.AddIndexedParameter("itemAmount", "200.00", 3);

            //    SenderDocument senderCPF = new SenderDocument(Documents.GetDocumentByType("CPF"), model.PessoaFisica.CPF);
            //    payment.Sender.Documents.Add(senderCPF);

            //    Uri paymentRedirectUri = payment.Register(credentials);

            //    return Redirect(paymentRedirectUri.AbsoluteUri);
            //    //Console.WriteLine("URL do pagamento : " + paymentRedirectUri);
            //    //Console.ReadKey();
            //}
            //catch (PagSeguroServiceException exception)
            //{
            //    Console.WriteLine(exception.Message + "\n");

            //    foreach (ServiceError element in exception.Errors)
            //    {
            //        Console.WriteLine(element + "\n");
            //    }
            //    Console.ReadKey();
            //}
            return null;
        }

    }
}