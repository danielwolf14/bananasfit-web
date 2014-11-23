using PayPal;
using PayPal.Api.Payments;
using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Util
{
    public class PayPalNegocio
    {
        private string clientId;
        private string clientSecret;
        private string mode;
        private int valorFits;

        public PayPalNegocio(string clientId, string clientSecret, string mode, int valorFits)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.mode = mode;
            this.valorFits = valorFits;
        }

        public void EfetuarCompra(PessoaFisica pessoaFisica, CreditCard creditCard, int quantidadeFits)
        {
            Dictionary<string, string> payPalConfig = new Dictionary<string, string>();
            payPalConfig.Add("mode", this.mode);

            OAuthTokenCredential tokenCredential = new OAuthTokenCredential(this.clientId, this.clientSecret, payPalConfig);

            string accessToken = tokenCredential.GetAccessToken();

            Address billingAddress = new Address();
            billingAddress.line1 = string.Format("{0} Num {1}",pessoaFisica.Endereco.Rua, pessoaFisica.Endereco.Numero) ;
            billingAddress.city = pessoaFisica.Endereco.Cidade;
            billingAddress.country_code = "BR";
            billingAddress.postal_code = pessoaFisica.Endereco.CEP;
            billingAddress.state = pessoaFisica.Endereco.Estado;

            creditCard.billing_address = billingAddress;

            Details amountDetails = new Details();
            amountDetails.subtotal = (quantidadeFits * valorFits).ToString();
            amountDetails.tax = "0.00";
            amountDetails.shipping = "0.00";

            Amount amount = new Amount();
            amount.total = (quantidadeFits * valorFits).ToString();
            amount.currency = "USD";
            amount.details = amountDetails;

            Transaction transaction = new Transaction();
            transaction.amount = amount;
            transaction.description = string.Format("Este pagamento foi efetuado por {0}, na quantia de {1} fits", pessoaFisica.Nome, quantidadeFits);

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(transaction);

            FundingInstrument fundingInstrument = new FundingInstrument();
            fundingInstrument.credit_card = creditCard;

            List<FundingInstrument> fundingInstruments = new List<FundingInstrument>();
            fundingInstruments.Add(fundingInstrument);

            Payer payer = new Payer();
            payer.funding_instruments = fundingInstruments;
            payer.payment_method = "credit_card";

            Payment payment = new Payment();
            payment.intent = "sale";
            payment.payer = payer;
            payment.transactions = transactions;

            Payment createdPayment = payment.Create(accessToken);            
        }
    }
}