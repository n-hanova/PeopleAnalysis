using AnalyticAPI.ApplicationAPI;
using AutoMapper;
using CommonCoreLibrary.Services;
using System;

namespace AnalyticAPI.Services
{
    public class MapperService : IMapperService
    {
        private readonly Mapper mapper;

        public MapperService()
        {
            MapperConfiguration configuration = new MapperConfiguration(x =>
            {
                x.CreateMap<Request, RequestViewModel>();
            });

            mapper = new Mapper(configuration);
        }

        public R Map<R>(object fromMap) => mapper.Map<R>(fromMap);
    }
}
