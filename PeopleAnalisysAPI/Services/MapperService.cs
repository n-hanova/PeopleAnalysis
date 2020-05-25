using AutoMapper;
using CommonCoreLibrary.Services;
using PeopleAnalisysAPI.Models.Rabbit;
using PeopleAnalysis.Models;

namespace PeopleAnalisysAPI.Services
{
    public class MapperService : IMapperService
    {
        private readonly Mapper mapper;

        public MapperService()
        {
            MapperConfiguration configuration = new MapperConfiguration(x =>
            {
                x.CreateMap<Request, RequestRabbit>();
            });

            mapper = new Mapper(configuration);
        }

        public R Map<R>(object fromMap) => mapper.Map<R>(fromMap);
    }
}
