using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UXI.Serialization.Json.Converters;

namespace UXI.Serialization.Json
{
    public class JsonSerializationFactory : ISerializationFactory
    {
        public JsonSerializationFactory()
            : this(Enumerable.Empty<ISerializationConfiguration>())
        { }

        //public JsonSerializationFactory(params JsonConverter[] converters)
        // : this(converters.AsEnumerable())
        //{ }


        //public JsonSerializationFactory(IEnumerable<JsonConverter> converters)
        //    : this(new JsonConvertersConfiguration(converters))
        //{ }


        //public JsonSerializationFactory(IEnumerable<JsonConverter> converters, ISerializationConfiguration configuration)
        //    : this(new JsonConvertersConfiguration(converters), configuration)
        //{ }


        public JsonSerializationFactory(params ISerializationConfiguration[] configurations)
            : this(configurations.AsEnumerable())
        { }


        //public JsonSerializationFactory(IEnumerable<JsonConverter> converters, Func<JsonSerializer, DataAccess, object, JsonSerializer> configuration)
        //    : this(converters, new RelaySerializationConfiguration<JsonSerializer>(configuration))
        //{ }


        public JsonSerializationFactory(IEnumerable<ISerializationConfiguration> configurations)
        {
            Configurations = configurations?.ToList() ?? new List<ISerializationConfiguration>();
        }


        public FileFormat Format => FileFormat.JSON;


        public List<ISerializationConfiguration> Configurations { get; }



        public IDataReader CreateReaderForType(TextReader reader, Type dataType, object settings)
        {
            var serializer = CreateSerializer(DataAccess.Read, settings);

            return new JsonDataReader(reader, dataType, serializer);
        }


        public IDataWriter CreateWriterForType(TextWriter writer, Type dataType, object settings) //, SerializationConfiguration configuration)
        {
            var serializer = CreateSerializer(DataAccess.Write, settings);

            return new JsonDataWriter(writer, serializer);
        }


        public JsonSerializer CreateSerializer(DataAccess access, object settings)
        {
            var serializer = new JsonSerializer()
            {
                Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
            };

            foreach (var configuration in Configurations)
            {
                serializer = (JsonSerializer)configuration.Configure(serializer, access, settings)
                           ?? serializer;
            }

            return serializer;
        }


        //private JsonSerializer CreateSerializer(object settings)
        //{
        //    var serializer = CreateJsonSerializer();

        //    AddConverters(serializer, Converters);

        //    if (_ignoreDefaultConverters == false)
        //    {
        //       

        //        AddConverters(serializer, DefaultConverters);
        //    }

        //    return serializer;
        //}


        

        //private static void AddConverters(JsonSerializer serializer, IEnumerable<JsonConverter> converters)
        //{
        //    foreach (var converter in converters)
        //    {
        //        serializer.Converters.Add(converter);
        //    }
        //}
    }
}