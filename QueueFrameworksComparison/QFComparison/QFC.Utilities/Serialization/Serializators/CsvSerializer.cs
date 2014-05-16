using System.IO;
using QFC.Utilities.Serialization.Contracts;
using QFC.Utilities.Serialization.Enums;
using ServiceStack.Text;

namespace QFC.Utilities.Serialization.Serializators
{
    public class CsvSerializer<T> : ISerializer<T> where T: class 
    {
        public CsvSerializer()
        {
            SerializationFormat = SerializationFormat.Csv;
        }

        public SerializationFormat SerializationFormat { get; private set; }

        public string Serialize(T inputObject)
        {
            return CsvSerializer.SerializeToString(inputObject);
        }

        public T Deserialize(string objectString)
        {
            return CsvSerializer.DeserializeFromStream<T>(GenerateStreamFromString(objectString));
        }

        private Stream GenerateStreamFromString(string input)
        {
            var stream = new MemoryStream();
            var  writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
