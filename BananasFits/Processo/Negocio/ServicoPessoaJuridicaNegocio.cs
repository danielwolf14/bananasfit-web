using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processo.Database.Interfaces;
using Processo.Database.Repositorios;
using Processo.Entidades;
using Processo.Database;
using Processo.Negocio.Interfaces;

namespace Processo.Negocio
{
    public class ServicoPessoaJuridicaNegocio : NegocioBase<ServicoPessoaJuridica>, IServicoPessoaJuridicaNegocio
    {
        private PessoaFisicaNegocio pessoaFisicaNegocio;
        private HistoricoCompraServicoNegocio historicoCompraServicoNegocio;

        internal ServicoPessoaJuridicaNegocio(DatabaseContext contexto)
            : base(contexto)
        {
            this.repositorio = new ServicoPessoaJuridicaRepositorio(contexto);
            this.pessoaFisicaNegocio = new PessoaFisicaNegocio(contexto);
            this.historicoCompraServicoNegocio = new HistoricoCompraServicoNegocio(contexto);
        }

        public void Cadastrar(ServicoPessoaJuridica servicoPessoaJuridica)
        {
            var mensagens = new List<string>();
            VerificarNegocioException(mensagens);
            base.Inserir(servicoPessoaJuridica);
        }

        public void Comprar(string qrCode, int chavePessoaFisica)
        {
            IList<string> mensagens = new List<string>();

            var servico = repositorio.Consultar(e => e.QRCode == qrCode).SingleOrDefault();
            var usuario = pessoaFisicaNegocio.BuscarPorChave(chavePessoaFisica);

            ValidarCompra(usuario, servico, mensagens);
            VerificarNegocioException(mensagens);
            usuario.QuantidadeMoedas -= servico.Valor;
            servico.PessoaJuridica.QuantidadeMoedas += servico.Valor;
            base.Atualizar(servico);

            //Quantidade de moedas 
            //historicoCompraServicoNegocio.Cadastrar(new HistoricoCompraServico
            // {
            //     //NomeServico = servico.Servico.Nome,
            //     //NomeUsuario = usuario.Nome,
            //     //NomePessoaJuridica = servico.PessoaJuridica.Nome,
            //     //Servico = servico,
            //     //PessoaFisica = usuario,
            //     //valor = servico.Valor,
            //     //data = DateTime.Now
            //     PessoaFisica = usuario.Chave,
            //     ServicoPessoaJuridica = servico.Chave,
            //     valor = servico.Valor,
            //     data = DateTime.Now
            // });

            pessoaFisicaNegocio.Atualizar(usuario);
        }

        private void ValidarCompra(PessoaFisica pessoaFisica, ServicoPessoaJuridica servico, IList<string> mensagens)
        {
            if (pessoaFisica == null)
                mensagens.Add("Pessoa física inexistente");
            if (servico == null)
                mensagens.Add("QRCode com código inválido.");
            if (pessoaFisica.QuantidadeMoedas < servico.Valor)
                mensagens.Add("Fits insuficientes.");


        }


    }
}
