using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;
using Web.ViewModels;

namespace Web.Controllers
{
    public class MovimentacaoController : BaseController
    {
        private const int valorFit = 5;

        public ActionResult Comprar(MovimentacaoViewModel model)
        {
            model.PessoaFisica = unityOfWork.PessoaFisicaNegocio.ConsultarTodos().FirstOrDefault();
            model.QuantidadeFits = 300;
            bool isSandbox = true;

            EnvironmentConfiguration.ChangeEnvironment(isSandbox);

            try
            {

                AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);

                // Instantiate a new payment request
                PaymentRequest payment = new PaymentRequest();

                // Sets the currency
                payment.Currency = Currency.Brl;

                // Add an item for this payment request
                payment.Items.Add(new Item("0001", "Fits", model.QuantidadeFits, model.QuantidadeFits*valorFit));
                
                // Sets a reference code for this payment request, it is useful to identify this payment in future notifications.
                payment.Reference = "REF1234";

                // Sets shipping information for this payment request
                payment.Shipping = new Shipping();
                //payment.Shipping.ShippingType = ShippingType.Sedex;

                //Passando valor para ShippingCost
                payment.Shipping.Cost = 0m;

                payment.Shipping.Address = new Address(
                    "BRA",
                    model.PessoaFisica.Endereco.Estado,
                    model.PessoaFisica.Endereco.Cidade,
                    model.PessoaFisica.Endereco.Bairro,
                    model.PessoaFisica.Endereco.CEP,
                    model.PessoaFisica.Endereco.Rua,
                    model.PessoaFisica.Endereco.Numero,
                    model.PessoaFisica.Endereco.Complemento
                );

                // Sets your customer information.
                payment.Sender = new Sender(
                    model.PessoaFisica.Nome,
                    model.PessoaFisica.Email,
                    new Phone(model.PessoaFisica.Telefone.Substring(0,2), model.PessoaFisica.Telefone.Substring(2))
                );

                // Sets the url used by PagSeguro for redirect user after ends checkout process
                payment.RedirectUri = new Uri("http://www.bananasfit.com.br/movimentacao/checkout");

                // Add checkout metadata information
                payment.AddMetaData(MetaDataItemKeys.GetItemKeyByDescription("CPF do passageiro"), model.PessoaFisica.CPF, 1);
                //payment.AddMetaData("PASSENGER_PASSPORT", "23456", 1);

                // Another way to set checkout parameters
                //payment.AddParameter("senderBirthday", "07/05/1980");
                //payment.AddIndexedParameter("itemColor", "verde", 1);
                //payment.AddIndexedParameter("itemId", "0003", 3);
                //payment.AddIndexedParameter("itemDescription", "Mouse", 3);
                //payment.AddIndexedParameter("itemQuantity", "1", 3);
                //payment.AddIndexedParameter("itemAmount", "200.00", 3);

                SenderDocument senderCPF = new SenderDocument(Documents.GetDocumentByType("CPF"), model.PessoaFisica.CPF);
                payment.Sender.Documents.Add(senderCPF);

                Uri paymentRedirectUri = payment.Register(credentials);

                return Redirect(paymentRedirectUri.AbsoluteUri);
                //Console.WriteLine("URL do pagamento : " + paymentRedirectUri);
                //Console.ReadKey();
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
        
    }
}