using AutoMapper;
using Agnus.Domain.Models;
using Agnus.Interface.Web.Models;

namespace Agnus.Interface.Web.AutoMapper
{
    internal class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "DomainToViewModelMappings";
            }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<TipoQuarto, TipoQuartoViewModel>();
        }
    }
}

