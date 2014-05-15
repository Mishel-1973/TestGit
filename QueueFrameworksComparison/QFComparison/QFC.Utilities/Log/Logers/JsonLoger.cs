using System.IO;
using QFC.Utilities.Log.ConfigurationSettings;
using QFC.Utilities.Log.Contracts;
using QFC.Utilities.Log.Enums;
using ServiceStack.Text;

namespace QFC.Utilities.Log.Logers
{
    public class JsonLoger<T> : ILoger<T> where T : class
    {
        public JsonLoger(LogConfig config)
        {
            Config = config;
            Format = LoggingFormats.Json;
        }

        public LoggingFormats Format { get; private set; }

        public LogConfig Config { get; private set; }

        public void LogData(T obbjectForLogging)
        {
            using (var streamWriter = new StreamWriter(Config.SourceFilePath, Config.IsAppend))
            {
                JsonSerializer.SerializeToWriter(obbjectForLogging, streamWriter);
            }
        }
    }
}
