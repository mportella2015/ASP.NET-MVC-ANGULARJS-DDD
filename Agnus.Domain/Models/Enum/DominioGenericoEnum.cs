using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Domain.Models.Enum
{
    public enum DominioGenericoEnum
    {
        [Description("Tipo Pessoa")]
        TipoPessoa = 1,
        [Description("Status Usuário Site")]
        StatusUsuarioSite = 2,
        [Description("Tipo Endereço")]
        TipoEndereco = 3,
        [Description("Tipo Serviço")]
        TipoServico = 4,
        [Description("Tipo Documento")]
        TipoDocumento = 5,
        [Description("CNAE")]
        Cnae = 6,
        [Description("Natureza Jurídica")]
        NaturezaJuridica = 7,
        [Description("Tipo Telefone")]
        TipoTelefone = 8,
        [Description("Tipo Uso Telefone")]
        TipoUsoTelefone = 9,
        [Description("Resultado Avaliação")]
        ResultadoAvaliacao = 10,
        [Description("Tipo Pendência Cadastro")]
        TipoPendenciaCadastro = 11,
        [Description("Status Fornecedor")]
        StatusFornecedor = 12,
        [Description("Índice de Reajuste")]
        IndiceReajuste = 13,
        [Description("Unidade")]
        Unidade = 14,
        [Description("Tipo Email")]
        TipoEmail = 15,
        [Description("Classe Conta Bancária")]
        ClasseContaBancaria = 16,
        [Description("Tipo Conta Bancária")]
        TipoContaBancaria = 17,
        [Description("Forma de Pagamento")]
        FormaPagamento = 18,
        [Description("Prazo Pagamento")]
        PrazoPagamento = 19,
        [Description("País")]
        Pais = 20,
        [Description("Regime da Empresa")]
        RegimeEmpresa = 21,
        [Description("Tipo Operação Cadastral")]
        TipoOperacaoCadastral = 22,
        [Description("Tipo Relação")]
        TipoRelacao = 23,
        [Description("Status Contrato Fornecedor")]
        StatusContratoFornecedor = 24,
        [Description("Tipo Material")]
        TipoMaterial = 25,
        [Description("Sexo")]
        Sexo = 26,
        [Description("Papel Pessoa Projeto")]
        PapelPessoaProjeto = 27,
        [Description("Tipo Investimento Projeto")]
        TipoInvestimentoProjeto = 28,
        [Description("Status Contrato Projeto")]
        StatusContratoProjeto = 29,
        [Description("Tipo_Classificacao Contrato Projeto")]
        Tipo_ClassificacaoContratoProjeto = 30,
        [Description("Tipo ObjetoDoPercentual Projeto")]
        TipoObjetoDoPercentualProjeto = 31,
        [Description("Tipo FormaVeiculacaoCredito Projeto")]
        TipoFormaVeiculacaoCreditoProjeto = 32,
        [Description("Tipo Contrato Projeto")]
        TipoContratoProjeto = 33,
        [Description("Tipo ObjetoDoPercentualComissao Contrato Elenco")]
        TipoObjetoDoPercentualComissaoContratoElenco = 34,
        [Description("Tipo Exclusividade Contrato Elenco")]
        TipoExclusividadeContratoElenco = 35,
        [Description("Tipo CreditoDebito Contrato Elenco")]
        TipoCreditoDebitoContratoElenco = 36,
        [Description("Unidade TaxaJuros Contrato Garantia")]
        UnidadeTaxaJurosContratoGarantia = 37,
        [Description("Status Projeto")]
        StatusProjeto = 38,
        [Description("MotivoRecusa Projeto")]
        MotivoRecusaProjeto = 39,
        [Description("Tipo Registro Projeto")]
        TipoRegistroProjeto = 40,
        [Description("Canal Projeto")]
        CanalProjeto = 41,
        [Description("Tipo Projeto")]
        TipoProjeto = 42,
        [Description("Tipo Produto Projeto")]
        TipoProdutoProjeto = 43,
        [Description("Nacionalidade")]
        Nacionalidade = 44,
        [Description("Tipo Audiência")]
        TipoAudiencia = 45,
        [Description("Tipo Operação")]
        TipoOperacao = 46,
        [Description("Parecer Aprovação")]
        ParecerAprovação = 47,
        [Description("Forma de Liberação Solicitação Adiantamento")]
        FormadeLiberacaoSolicitacaoAdiantamento = 48,
        [Description("Status Solicitação Adiantamento")]
        StatusSolicitacaoAdiantamento = 49,
        [Description("Objeto Aprovação Workflow")]
        ObjetoAprovacaoWorkflow = 50,
        [Description("Status Prestação de Contas Adiantamento / Solicitação Reembolso")]
        StatusPrestacaodeContasAdiantamentoSolicitacaoReembolso = 51,
        [Description("Status Item Prestação de Contas Adiantamento / Item Solicitação Reembolso")]
        StatusItemPrestacaodeContasAdiantamentoItemSolicitacaoReembolso = 52,
        [Description("Status Pedido Compra")]
        StatusPedidoCompra = 53,
        [Description("Status Item Pedido Compra")]
        StatusItemPedidoCompra = 54,
        [Description("Tipo Pedido Compra")]
        TipoPedidoCompra = 55,
        [Description("Status Realizado")]
        StatusRealizado = 56,
        [Description("Natureza Despesa")]
        NaturezaDespesa = 57,
        [Description("Objeto Comprometimento Orçamento")]
        ObjetoComprometimentoOrcamento = 58,

        [Description("Tipo Orçamento")]
        TipoOrcamento = 59,
        [Description("Status Orçamento")]
        StatusOrcamento = 60,
        [Description("Status Fase Orçamento")]
        StatusFaseOrcamento = 61,
        [Description("Operação Orçamento")]
        OperacaoOrcamento = 62,
        [Description("Moeda")]
        Moeda = 63,
        [Description("Apresentação Fases")]
        ApresentacaoFases = 64,
        [Description("Territorio")]
        Territorio = 65,
        [Description("Midia")]
        Midia = 66,
        [Description("Tipo Biblioteca")]
        TipoBiblioteca = 67,
        [Description("Status Núcleo")]
        StatusNucleo = 68,
        [Description("Tipo Template")]
        TipoTemplate = 69,
        [Description("UF")]
        UF = 70,
        [Description("Banco")]
        Banco = 71,
        [Description("Status Projeto Fundo")]
        StatusProjetoFundo = 72,
        [Description("Unidade Prazo Veiculacao")]
        UnidadePrazoVeiculacao = 73,
        [Description("Formato")]
        Formato = 74,
        [Description("Modalidade Cartão")]
        ModalidadeCartao = 75,
        [Description("Status Cartão")]
        StatusCartao = 76,
        [Description("ovimentacação Estoque")]
        TipoMovimentacao = 77,
        [Description("Solicitação Item controle Cartões")]
        SolicitacaoItemControleCartoes = 78,
        [Description("Solicitação controle Cartões")]
        SolicitacaoControleCartoes = 79,
        [Description("Agrupamento Item Conta")]
        AgrupamentoItemConta = 80,
        [Description("Tipo Documento Fiscal")]
        TipoDocumentoFiscal = 81,
        [Description("Tipo Versão Orçamento")]
        TipoVersaoOrcamento = 82,
        [Description("Operação Bancária")]
        OperacaoBancaria = 83
    }
}
