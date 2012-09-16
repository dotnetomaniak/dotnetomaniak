namespace Kigg.LinqToSql.Repository
{
    using System.Diagnostics;

    using Infrastructure;

    public class ConnectionString : IConnectionString
    {
        private readonly string _value;

        public ConnectionString(IConfigurationManager configuration, string name)
        {
            Check.Argument.IsNotNull(configuration, "configuration");
            Check.Argument.IsNotEmpty(name, "name");

            _value = configuration.GetConnectionString(name);
        }

        public string Value
        {
            [DebuggerStepThrough]
            get
            {
                return _value;
            }
        }
    }
}