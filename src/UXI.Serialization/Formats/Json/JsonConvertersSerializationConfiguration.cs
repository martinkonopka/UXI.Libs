﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Serialization.Json
{
    public class JsonConvertersSerializationConfiguration : SerializationConfiguration<JsonSerializer>
    {
        public JsonConvertersSerializationConfiguration(IEnumerable<JsonConverter> converters)
        {
            Converters = converters?.ToList() ?? new List<JsonConverter>();
        }


        public JsonConvertersSerializationConfiguration(params JsonConverter[] converters) 
            : this(converters.AsEnumerable())
        { }


        public List<JsonConverter> Converters { get; }


        protected override JsonSerializer Configure(JsonSerializer serializer, DataAccess access, object settings)
        {
            IEnumerable<JsonConverter> converters = Converters;
            if (access == DataAccess.Read)
            {
                converters = Converters.Where(c => c.CanRead);
            }
            else if (access == DataAccess.Write)
            {
                converters = Converters.Where(c => c.CanWrite);
            }

            foreach (var converter in converters)
            {
                serializer.Converters.Add(converter);
            }

            return serializer;
        }
    }
}