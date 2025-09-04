namespace SpeakEase.MQ.Diagnostics
{
    internal class DiagnosticsName
    {
        public const string MaomiMQ = "SpeakEase.MQ";
        public const string EventBus = "SpeakEase.MQ.EventBus";
        public const string Consumer = "SpeakEase.MQ.Consumer";
        public const string Publisher = "SpeakEase.MQ.Publisher";

        public static class Listener
        {
            public const string Publisher = "MaomiMQPublisherHandlerDiagnosticListener";
            public const string Consumer = "MaomiMQConsumerHandlerDiagnosticListener";
        }

        public static class ActivitySource
        {
            public const string Publisher = "SpeakEase.MQ.Publisher";
            public const string Consumer = "SpeakEase.MQ.Consumer";

            public const string Fallback = "SpeakEase.MQ.Fallback";
            public const string Execute = "SpeakEase.MQ.Execute";
            public const string Retry = "SpeakEase.MQ.Retry";

            public const string EventBusExecute = "SpeakEase.MQ.EventBus.Execute";
        }

        public static class Meter
        {
            public const string Publisher = "SpeakEase.MQ.Publisher";
            public const string Consumer = "SpeakEase.MQ.Consumer";

            public const string PublisherMessageCount = "maomimq_publisher_message_count";
            public const string PublisherMessageSent = "maomimq_publisher_message_sent";
            public const string PublisherFaildMessageCount = "maomimq_publisher_message_faild_count";
        }

        public static class Event
        {
            public const string PublisherStart = ActivitySource.Publisher + ".Start";
            public const string PublisherStop = ActivitySource.Publisher + ".Stop";
            public const string PublisherExecption = ActivitySource.Publisher + ".Execption";

            public const string ConsumerStart = ActivitySource.Consumer + ".Start";
            public const string ConsumerStop = ActivitySource.Consumer + ".Stop";
            public const string ConsumerExecption = ActivitySource.Consumer + ".Execption";

            public const string FallbackStart = "SpeakEase.MQ.Fallback" + ".Start";
            public const string FallbackStop = "SpeakEase.MQ.Fallback" + ".Stop";
            public const string FallbackExecption = "SpeakEase.MQ.Fallback" + ".Execption";
        }
    }
}
