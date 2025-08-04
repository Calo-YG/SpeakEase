namespace SpeakEase.EventBus.EventBus.Contrib.Local.EventBus;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EventDiscriptorAttribute : Attribute
{
    /// <summary>
    /// 事件名称
    /// </summary>
    public string EventName { get; private set; }

    /// <summary>
    /// 是否维持一个生产者多个消费者模型
    /// </summary>
    public bool SigleReader { get; private set; }

    public EventDiscriptorAttribute(string eventName, bool sigleReader = true)
    {
        EventName = eventName;
        SigleReader = sigleReader;
    }
}