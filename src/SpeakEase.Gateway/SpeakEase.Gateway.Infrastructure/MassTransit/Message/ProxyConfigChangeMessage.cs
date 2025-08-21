namespace SpeakEase.Gateway.Infrastructure.MassTransit.Message
{
    // TODO: Add message to update proxy configuration  
    public class ProxyConfigChangeMessage
    {
        /// <summary>
        /// The app id of the app that needs to be updated
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// The name of the app that needs to be updated
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// The event id of the event that triggered the update
        /// </summary>
        public string EventId { get; set; }
    }
}
