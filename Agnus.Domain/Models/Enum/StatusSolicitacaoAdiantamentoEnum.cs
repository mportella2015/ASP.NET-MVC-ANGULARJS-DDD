namespace Agnus.Domain.Models.Enum
{
    public enum StatusSolicitacaoAdiantamentoEnum
    {
        Registrada = 1,
        EmAprovacao = 2,
        Aprovada = 3,  //(Aguardando prestação de contas)
        Reprovada = 4,
        EmRevisao = 5,
        Cancelada = 6,
        Fechada = 7 //(Toda a prestação de contas foi feita)
    }
}