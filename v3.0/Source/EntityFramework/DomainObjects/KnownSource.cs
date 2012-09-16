namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class KnownSource: IKnownSource
    {
        public KnownSourceGrade Grade
        {
            get
            {
                return (KnownSourceGrade)SourceGrade;
            }
            set
            {
                SourceGrade = (int) value;
            }
        }
    }
}
