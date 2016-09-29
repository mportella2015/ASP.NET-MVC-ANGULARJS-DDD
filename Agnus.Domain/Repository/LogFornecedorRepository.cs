using Agnus.Domain.Models;
using Agnus.Domain.Repository;
using Agnus.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Repository.Implementations
{
    public class LogFornecedorRepository : ILogFornecedorRepository
    {
        private IGenericRepository<Fornecedor> _fornRep;
        private IGenericRepository<FornecedorEmail> _emailRep;
        private IGenericRepository<FornecedorEndereco> _enderecoRep;
        private IGenericRepository<FornecedorAvaliacao> _avaliacaoRep;
        private IGenericRepository<FornecedorCNAE> _cnaeRep;
        private IGenericRepository<FornecedorContato> _contatoRep;
        private IGenericRepository<FornecedorContrato> _contratoRep;                
        private IGenericRepository<FornecedorDocumento> _docRep;
        private IGenericRepository<FornecedorMaterial> _materialRep;
        private IGenericRepository<FornecedorServico> _servicoRep;
        private IGenericRepository<FornecedorSocio> _socioRep;
        private IGenericRepository<FornecedorTelefone> _telefoneRep;

        public LogFornecedorRepository(DbContext context, IGenericRepository<Fornecedor> fornRep, IGenericRepository<FornecedorEmail> emailRep, IGenericRepository<FornecedorEndereco> enderecoRep,
            IGenericRepository<FornecedorAvaliacao> avaliacaoRep, IGenericRepository<FornecedorCNAE> cnaeRep, IGenericRepository<FornecedorContato> contatoRep,
            IGenericRepository<FornecedorContrato> contratoRep, IGenericRepository<FornecedorDocumento> docRep, IGenericRepository<FornecedorMaterial> materialRep,
            IGenericRepository<FornecedorServico> servicoRep, IGenericRepository<FornecedorSocio> socioRep, IGenericRepository<FornecedorTelefone> telefoneRep)
        {
            var newContext = new AgnusContext();

            _fornRep = fornRep;
            _emailRep = emailRep;
            _enderecoRep = enderecoRep;
            _avaliacaoRep = avaliacaoRep;
            _cnaeRep = cnaeRep;
            _contatoRep = contatoRep;
            _contratoRep = contratoRep;
            _docRep = docRep;
            _materialRep = materialRep;
            _servicoRep = servicoRep;
            _socioRep = socioRep;
            _telefoneRep = telefoneRep;

            _fornRep.ChangeContext(newContext);
            _emailRep.ChangeContext(newContext);
            _enderecoRep.ChangeContext(newContext);
            _avaliacaoRep.ChangeContext(newContext);
            _cnaeRep.ChangeContext(newContext);
            _contatoRep.ChangeContext(newContext);
            _contratoRep.ChangeContext(newContext);
            _docRep.ChangeContext(newContext);
            _materialRep.ChangeContext(newContext);
            _servicoRep.ChangeContext(newContext);
            _socioRep.ChangeContext(newContext);
            _telefoneRep.ChangeContext(newContext);
        }

        public Domain.IAuditableEntity BuscarEntitidadeLogFornecedor(BaseEntity entity)
        {
            var id = entity.Id;
            
            if (entity is Fornecedor)
                return _fornRep.FindById(id);
            if (entity is FornecedorEmail)
                return _emailRep.FindById(id);
            if (entity is FornecedorEndereco)
                return _enderecoRep.FindById(id);
            if (entity is FornecedorAvaliacao)
                return _avaliacaoRep.FindById(id);
            if (entity is FornecedorCNAE)
                return _cnaeRep.FindById(id);
            if (entity is FornecedorContato)
                return _contatoRep.FindById(id);
            if (entity is FornecedorContrato)
                return _contratoRep.FindById(id);
            if (entity is FornecedorDocumento)
                return _docRep.FindById(id);
            if (entity is FornecedorMaterial)
                return _materialRep.FindById(id);
            if (entity is FornecedorServico)
                return _servicoRep.FindById(id);
            if (entity is FornecedorSocio)
                return _socioRep.FindById(id);
            if (entity is FornecedorTelefone)
                return _telefoneRep.FindById(id);
            
            return null;
        }
    }
}
