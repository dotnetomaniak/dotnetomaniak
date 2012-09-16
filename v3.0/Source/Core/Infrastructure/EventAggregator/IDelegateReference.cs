namespace Kigg.Infrastructure
{
    using System;

    public interface IDelegateReference
    {
        Delegate Target
        {
            get;
        }
    }
}