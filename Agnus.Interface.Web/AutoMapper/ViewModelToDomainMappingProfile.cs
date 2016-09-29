using AutoMapper;
using Agnus.Domain.Models;
using Agnus.Interface.Web.Models;

namespace Agnus.Interface.Web.AutoMapper
{
    internal class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "ViewModelToDomainMappings";
            }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<TipoQuartoViewModel, TipoQuarto>();
        }
    }
}
