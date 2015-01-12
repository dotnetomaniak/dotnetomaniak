namespace Kigg.Service
{
    using Infrastructure;

    public class UpcommingEventEventArgs
    {
        public UpcommingEventEventArgs(string name, string link)
        {
            EventName = name;
            EventLink = link;
        }

        public string EventName { private set; get; }

        public string EventLink { private set; get; }
    }

    public class UpcommingEventEvent : BaseEvent<UpcommingEventEventArgs>
    {
    }
}
