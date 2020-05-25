using AuthAPI.Models.Controller;
using AuthAPI.Models.Database;
using AutoMapper;
using CommonCoreLibrary.Services;
using System;

namespace AuthAPI.Services
{
    public class AutoMapperService : IMapperService
    {
        private readonly Mapper mapper;

        public AutoMapperService()
        {
            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<User, UserModel>();
                x.CreateMap<Role, RoleModel>();
                x.CreateMap<Language, LanguageModel>();
                x.CreateMap<User, UserViewModel>()
                    .ForMember(x => x.Login, x => x.MapFrom(y => y.Email))
                    .ForMember(x => x.Role, x => x.MapFrom(y => y.Role.Name));
            });

            mapper = new Mapper(config);
        }

        public R Map<R>(object fromMap) => mapper.Map<R>(fromMap);
    }
}
