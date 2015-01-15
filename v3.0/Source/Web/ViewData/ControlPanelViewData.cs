namespace Kigg.Web
{
    public class ControlPanelViewData
    {
        public string ErrorMessage
        {
            get;
            set;
        }

        public int NewCount
        {
            get;
            set;
        }

        public int UnapprovedCount
        {
            get;
            set;
        }

        public int PublishableCount
        {
            get;
            set;
        }

        public bool IsAdministrator
        {
            get;
            set;
        }

        public int UnapprovedEventsCount
        {
            get; 
            set;
        }
    }
}