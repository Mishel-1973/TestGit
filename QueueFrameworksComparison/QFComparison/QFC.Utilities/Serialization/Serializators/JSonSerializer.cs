using QFC.Utilities.Serialization.Contracts;
using QFC.Utilities.Serialization.Enums;
using ServiceStack.Text;

namespace QFC.Utilities.Serialization.Serializators
{
    public class JSonSerializer<T> : ISerializer<T> where T: class
    {
        public JSonSerializer()
        {
            SerializationFormat = SerializationFormat.Json;
        }

        public SerializationFormat SerializationFormat { get; private set; }

        public string Serialize(T inputObject)
        {
            return JsonSerializer.SerializeToString(inputObject);
        }

        public T Deserialize(string objectString)
        {
            return JsonSerializer.DeserializeFromString<T>(objectString);
        }
    }
}
