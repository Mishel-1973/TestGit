using QFC.Utilities.Log.ConfigurationSettings;
using QFC.Utilities.Log.Enums;

namespace QFC.Utilities.Log.Contracts
{
    public interface ILoger<TObject> where TObject:class
    {
        LoggingFormats Format { get; }
        
        LogConfig Config { get; }

        void LogData(TObject obbjectForLogging);
    }
}
