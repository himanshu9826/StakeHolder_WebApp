using AutoMapper;
using System.Collections.Generic;

namespace StakeHolder.Common.Helpers
{
    public abstract class GenericMapper<TSource, TDestination>
    {
        public static void MapObject(TSource entity, TDestination destination)
        {
            Mapper.CreateMap<TSource, TDestination>();
            Mapper.Map<TSource, TDestination>(entity, destination);
        }

        public static TDestination MapObject(TSource entity)
        {
            Mapper.CreateMap<TSource, TDestination>();

            TDestination dto = Mapper.Map<TSource, TDestination>(entity);

            return dto;
        }

        public static void CreateMapping()
        {
            Mapper.CreateMap<TSource, TDestination>();
        }

        public static List<TDestination> MapList(IEnumerable<TSource> entities)
        {
            List<TDestination> dtoList = new List<TDestination>();

            foreach (TSource entity in entities)
            {
                dtoList.Add(MapObject(entity));
            }

            return dtoList;
        }

    }

}