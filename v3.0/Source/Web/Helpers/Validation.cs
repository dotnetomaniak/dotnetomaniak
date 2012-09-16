namespace Kigg.Web
{
    using System;
    using System.Linq.Expressions;

    public class Validation
    {
        public Validation(Expression<Func<bool>> expression, string errorMessage)
        {
            Check.Argument.IsNotNull(expression, "expression");
            Check.Argument.IsNotEmpty(errorMessage, "errorMessage");

            Expression = expression;
            ErrorMessage = errorMessage;
        }

        public Expression<Func<bool>> Expression
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}