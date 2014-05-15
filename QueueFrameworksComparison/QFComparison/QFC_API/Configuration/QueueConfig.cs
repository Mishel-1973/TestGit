namespace QFC.Contracts.Configuration
{
    public class QueueConfig
    {
        /// <summary>
        /// Gets or sets message queue host url.
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// Gets or sets path to log file.
        /// </summary>
        public string LogFilePath { get; set; }
        
        /// <summary>
        /// Gets or sets Subscriber Id.
        /// </summary>
		public string SubscriberId { get; set; }
    }
}
