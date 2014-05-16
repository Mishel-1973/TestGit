using QFC.Utilities.Serialization.Enums;

namespace QFC.Utilities.Serialization.Contracts
{
    interface ISerializer<T>  where T: class
    {
        SerializationFormat SerializationFormat { get; }

        string Serialize(T inputObject);

        T Deserialize(string objectString);
    }
}
