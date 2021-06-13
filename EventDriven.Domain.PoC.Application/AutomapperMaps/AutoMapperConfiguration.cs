using AutoMapper;

namespace EventDriven.Domain.PoC.Application.AutomapperMaps
{
    public class AutoMapperConfiguration
    {
        // For the static method, you need to initialize AutoMapper with your config
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });

            return config;
        }
    }
}