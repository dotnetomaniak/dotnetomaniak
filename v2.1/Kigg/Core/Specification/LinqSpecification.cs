namespace Kigg
{
    using System;
    using System.Linq.Expressions;

    public class LinqSpecification<T> : BaseSpecification<T>
    {
        private readonly Expression<Predicate<T>> _predicate;

        public LinqSpecification(Expression<Predicate<T>> predicate)
        {
            Check.Argument.IsNotNull(predicate, "predicate");
            _predicate = predicate;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            Check.Argument.IsNotNull(candidate, "candidate");

            return _predicate.Compile().Invoke(candidate);
        }
    }
}