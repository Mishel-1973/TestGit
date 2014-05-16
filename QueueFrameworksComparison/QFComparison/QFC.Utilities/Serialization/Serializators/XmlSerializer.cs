using QFC.Utilities.Serialization.Contracts;
using QFC.Utilities.Serialization.Enums;
using ServiceStack.Text;

namespace QFC.Utilities.Serialization.Serializators
{
    public class XmlSerializer<T> : ISerializer<T> where T:class 
    {
        public XmlSerializer()
        {
            SerializationFormat = SerializationFormat.Xml;
        }

        public SerializationFormat SerializationFormat { get; private set; }

        public string Serialize(T inputObject)
        {
            return XmlSerializer.SerializeToString(inputObject);
        }

        public T Deserialize(string objectString)
        {
            return XmlSerializer.DeserializeFromString<T>(objectString);
        }
    }
}
