using AutoMapper;
using CovidonusApi.Helpers;
using CovidonusApi.Models;
using CovidonusApi.Models.DTOs;
using System;
using System.Collections.Generic;

namespace CovidonusApi.Repositories
{
    public class CoreRepository
    {
        protected IMapper mapper;
        protected CovidonusContext db = new CovidonusContext();
        protected static IEnumerable<StateWiseData> MenuList;
        protected static IEnumerable<Resource> ResourceList;
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

        public static void MapCreated<T>(T obj, string userName = null) where T : Auditor
        {
            if (obj != null)
            {
                obj.Created = DateTime.Now;
                obj.CreatedBy = userName;
                obj.IsActive = true;
            }
        }
        public static void MapModified<T>(T obj, string userName = null) where T : Auditor
        {
            if (obj != null)
            {
                obj.Modified = DateTime.Now;
                obj.ModifiedBy = userName;
            }
        }
    }
}