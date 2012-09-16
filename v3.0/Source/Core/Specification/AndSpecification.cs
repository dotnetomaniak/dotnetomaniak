namespace Kigg
{
    public class AndSpecification<T> : BaseSpecification<T>
    {
        private readonly ISpecification<T> _leftHandSide;
        private readonly ISpecification<T> _rightHandSide;

        internal AndSpecification(ISpecification<T> leftHandSide, ISpecification<T> rightHandSide)
        {
            Check.Argument.IsNotNull(leftHandSide, "leftHandSide");
            Check.Argument.IsNotNull(rightHandSide, "rightHandSide");

            _leftHandSide = leftHandSide;
            _rightHandSide = rightHandSide;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            Check.Argument.IsNotNull(candidate, "candidate");

            return _leftHandSide.IsSatisfiedBy(candidate) && _rightHandSide.IsSatisfiedBy(candidate);
        }
    }
}