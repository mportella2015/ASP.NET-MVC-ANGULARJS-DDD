using Agnus.Domain.Models;
using Agnus.Domain.Models.Enum;
using Agnus.Framework;
using Agnus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Correios.Net;

namespace Agnus.Service.Implementations
{
    public class ProjetoService : EntityService<Projeto>, IProjetoService
    {
        private ITabelaGenericaDominioService _tabelaGenericaService = DependencyResolver.Current.GetService<ITabelaGenericaDominioService>();
        private IEntityService<ProjetoHistoricoStatus> _projetoHistoricoStatusService = DependencyResolver.Current.GetService<IEntityService<ProjetoHistoricoStatus>>();
        private IEntityService<ProjetoHistoricoTitulo> _projetoHistoricoTituloService = DependencyResolver.Current.GetService<IEntityService<ProjetoHistoricoTitulo>>();
        private IEntityService<ProjetoPessoa> _projetoPessoaService = DependencyResolver.Current.GetService<IEntityService<ProjetoPessoa>>();
        private IEntityService<Projeto> _projetoService = DependencyResolver.Current.GetService<IEntityService<Projeto>>();
        private IEntityService<Nucleo> _nucleoService = DependencyResolver.Current.GetService<IEntityService<Nucleo>>();

        public ProjetoService(IUnitOfWork unitOfWork, IGenericRepository<Projeto> repository)
            : base(unitOfWork, repository)
        {
        }

        public void AtualizarStatus(long idProjeto, long codStatusProjeto, string justificativa)
        {
            var statusProjeto = _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.StatusProjeto, (int)codStatusProjeto);            

            var projeto = GetBy(idProjeto);
            projeto.StatusProjeto = statusProjeto;                        
            projeto.DataStatus = DateTime.Now;
            Save(projeto);

            var historicoStatus = new ProjetoHistoricoStatus();
            historicoStatus.Projeto = projeto;
            historicoStatus.StatusProjeto = statusProjeto;
            if (justificativa != null)
                historicoStatus.TxtJustificativa = justificativa;
            _projetoHistoricoStatusService.Save(historicoStatus);

            _unitOfWork.Commit();
        }


        public void AtualizacaoTitulo(long idProjeto, string novoTitulo)
        {
            var projeto = GetBy(idProjeto);
            if (projeto.TxtTitulo.ToUpper().Equals(novoTitulo.ToUpper()))
                return;

            NotificarAtualizacaoTitulo(idProjeto, novoTitulo);
        }


        public void NotificarAtualizacaoTitulo(long idProjeto, string novoTitulo)
        {
            var projeto = GetBy(idProjeto);

            var historicoTitulo = new ProjetoHistoricoTitulo();
            historicoTitulo.Projeto = projeto;
            historicoTitulo.TxtTitulo = novoTitulo;
            historicoTitulo.DataCadastro = DateTime.Now;

            _projetoHistoricoTituloService.Save(historicoTitulo);
        }


        public void AtribuirNumeroProjeto(long idProjeto, Projeto entity)
        {
            var projeto = GetBy(idProjeto);
            projeto.NumProjeto = entity.ProjetoBase == null ? string.Format("{0}00", entity.GerarPrimeiraParteNumeroProjeto()) : 
                    entity.NumProjeto = string.Format("{0}{1}", entity.ProjetoBase.GerarPrimeiraParteNumeroProjeto(), (entity.ProjetoBase.Projetos.Count + 1).ToString().PadLeft(2, '0'));

            Save(projeto);

            _unitOfWork.Commit();
        }

        public void CriarEquipePadrao(long idProjeto)
        {            
            var projeto = GetBy(idProjeto);

            var codigoNucleo = _nucleoService.GetBy(projeto.IdNucleo);

            if (codigoNucleo.Codigo == (long)NucleoEnum.Publicidade)
            {
                CriarProjetoPessoaPadrao(idProjeto, PapelEnum.CoordenadorProducao);
                CriarProjetoPessoaPadrao(idProjeto, PapelEnum.DiretorCena);
                CriarProjetoPessoaPadrao(idProjeto, PapelEnum.Atendimento);
            }
            else
            {
                CriarProjetoPessoaPadrao(idProjeto, PapelEnum.CoordenadorProducao);
                CriarProjetoPessoaPadrao(idProjeto, PapelEnum.DiretorCena);
                CriarProjetoPessoaPadrao(idProjeto, PapelEnum.Roteirista);
            }
        }

        private void CriarProjetoPessoaPadrao(long idProjeto, PapelEnum papelEnum)
        {
            var projetoPessoa = new ProjetoPessoa();

            if (papelEnum == PapelEnum.Atendimento)
            {
                projetoPessoa.IdProjeto = idProjeto;
                projetoPessoa.Papel = _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.PapelPessoaProjeto, (int)papelEnum);
                projetoPessoa.ValPercentualAtendimento = 0;
            }
            else
            {
                projetoPessoa.IdProjeto = idProjeto;
                projetoPessoa.Papel = _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.PapelPessoaProjeto, (int)papelEnum);
            }

            _projetoPessoaService.Save(projetoPessoa);
        }

        public void DefinirDataLimiteFundo(Projeto projeto, int nucleo)
        {
            if (nucleo != (long)NucleoEnum.Cinema && nucleo != (long)NucleoEnum.TV)
            {
                projeto.DataPrevisaoFilmagem = projeto.DataPrevisaoFilmagem.HasValue ? projeto.DataPrevisaoFilmagem : DateTime.Now;
                projeto.DataLimiteFundo = projeto.DataPrevisaoFilmagem.Value.AddDays(10);
            }
            else
            {
                projeto.DataLimiteFundo = null;
            }

            _projetoService.Save(projeto);
        }

        public IEnumerable<Projeto> GetProjetosPorNucleos(ICollection<Nucleo> nucleosUsuario)
        {
            return nucleosUsuario.SelectMany(x => x.Projetos);
        }

        public IEnumerable<Projeto> FiltrarPojetosPorListaStatus(IEnumerable<Projeto> projetos, long[] idsConteudoDominio)
        {
            return projetos.Where(z => idsConteudoDominio.Contains(z.IdStatusProjeto));
        }




        public IEnumerable<Projeto> GetProjetosPorUsuario(UsuarioSistema usuarioSistema)
        {
            if (usuarioSistema == null)
                throw new CustomServiceException("Usuário inválido. Por favor verifique o usuário informado");
            IEnumerable<Projeto> projetos = new List<Projeto>();
            if (usuarioSistema.Nucleos.Count() > 0)
                projetos = this.GetProjetosPorNucleos(usuarioSistema.Nucleos);
            else if (usuarioSistema.Projetos.Count() > 0)
                projetos = usuarioSistema.Projetos;
            return projetos.OrderBy(x=>x.TxtTitulo);
        }


        public ConteudoTabelaDominio SelecionarTipoVersaoUsuario(UsuarioSistema usuarioSistema)
        {
            //TODO:implementar de acordo com o núcleo do usuário
            return _tabelaGenericaService.BuscarConteudoPor(DominioGenericoEnum.TipoVersaoOrcamento, (int)TipoVersaoOrcamentoEnum.Producao);
        }


        public bool ExisteOrcamentoAprovado(long idProjeto)
        {
            var projeto = this.GetBy(idProjeto);
            return projeto.Orcamentos.Any(x => x.StatusOrcamento.Codigo == (int)StatusOrcamentoEnum.Aprovado);
        }
    }
}
