using AutoMapper;
using CovidonusApi.Helpers;
using CovidonusApiV2.Models;
using CovidonusApiV2.Models.DTOs;
using System.Collections.Generic;

namespace CovidonusApiV2.Repositories
{
    public class CoreRepository
    {
        protected IMapper mapper;
        protected static IEnumerable<StateWiseData> MenuList;
        protected static IEnumerable<Resource> ResourceList;
        protected static CovidNews News;
        protected IMapper GetMapper()
        {
            return CovidonusMapper.GetMapper();
        }

        public T ConvertModels<T, T2>(T2 t2)
        {
            if (mapper == null)
            {
                mapper = GetMapper();
            }
            return mapper.Map<T>(t2);
        }

        public void MapModels<T, T2>(T source, T2 destination)
        {
            if (mapper == null)
            {
                mapper = GetMapper();
            }
            mapper.Map(source, destination);
        }
        public static string NewsApiKey
        {
            get; set;
        }

    }
}