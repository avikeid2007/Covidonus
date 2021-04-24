using AutoMapper;
using CovidonusApiV2.Models;
using CovidonusApiV2.Models.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CovidonusApi.Helpers
{
    public static class CovidonusMapper
    {
        public static IMapper GetMapper()
        {
            IConfigurationProvider configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CasesTimeSeries, CasesTimeSeries>()
                .ForMember(s => s.DateFull, t => t.MapFrom(x => ConvertDate(x.Dateymd)));
                cfg.CreateMap<StateWiseData, StateWiseData>()
                 .ForMember(s => s.DistrictData, t => t.Ignore())
                .ForMember(s => s.Id, t => t.Ignore());
                cfg.CreateMap<DistrictWiseData, DistrictWiseData>()
                .ForMember(s => s.DeltaId, t => t.Ignore())
                .ForMember(s => s.Id, t => t.Ignore())
                .ForMember(s => s.StateCode, t => t.Ignore())
                 .ForMember(s => s.Delta, t => t.Ignore());
                cfg.CreateMap<DeltaData, DeltaData>()
                .ForMember(s => s.Id, t => t.Ignore());
                cfg.CreateMap<Tested, Tested>();
                cfg.CreateMap<DeltaData, Delta>();

            });
            return new Mapper(configuration);
        }

        private static string GetDeathRation(CasesTimeSeries x)
        {
            if (x.TotalConfirmed == 0)
            {
                return string.Empty;
            }
            double per = (double)x.TotalDeceased / (double)x.TotalConfirmed * 100;
            return Convert.ToString(Math.Round(per, 2));
        }

        private static DateTime ConvertDate(DateTime date)
        {
            return date;
        }
    }
    internal class MultiFormatDateConverter : DateTimeConverterBase
    {
        public IList<string> DateTimeFormats { get; set; } = new[] { "yyyy-MM-dd" };

        public DateTimeStyles DateTimeStyles { get; set; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var val = IsNullableType(objectType);
            if (reader.TokenType == JsonToken.Null)
            {
                if (!val)
                {
                    throw new JsonSerializationException(
                        string.Format(CultureInfo.InvariantCulture, "Cannot convert null value to {0}.", objectType));
                }
            }

            Type underlyingObjectType;
            if (val)
            {
                underlyingObjectType = Nullable.GetUnderlyingType(objectType)!;
            }
            else
            {
                underlyingObjectType = objectType;
            }

            if (reader.TokenType == JsonToken.Date)
            {
                if (underlyingObjectType == typeof(DateTimeOffset))
                {
                    if (!(reader.Value is DateTimeOffset))
                    {
                        return new DateTimeOffset((DateTime)reader.Value);
                    }

                    return reader.Value;
                }

                if (reader.Value is DateTimeOffset)
                {
                    return ((DateTimeOffset)reader.Value).DateTime;
                }

                return reader.Value;
            }

            if (reader.TokenType != JsonToken.String)
            {
                var errorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    "Unexpected token parsing date. Expected String, got {0}.",
                    reader.TokenType);
                throw new JsonSerializationException(errorMessage);
            }

            var dateString = (string)reader.Value;
            if (underlyingObjectType == typeof(DateTimeOffset))
            {
                foreach (var format in this.DateTimeFormats)
                {
                    // adjust this as necessary to fit your needs
                    if (DateTimeOffset.TryParseExact(dateString, format, CultureInfo.InvariantCulture, this.DateTimeStyles, out var date))
                    {
                        return date;
                    }
                }
            }

            if (underlyingObjectType == typeof(DateTime))
            {

                foreach (var format in this.DateTimeFormats)
                {
                    // adjust this as necessary to fit your needs
                    if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, this.DateTimeStyles, out var date))
                    {
                        return date;
                    }
                }
            }

            throw new Newtonsoft.Json.JsonException("Unable to parse \"" + dateString + "\" as a date.");
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public static bool IsNullableType(Type t)
        {
            if (t.IsGenericTypeDefinition || t.IsGenericType)
            {
                return t.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            return false;
        }
    }
}