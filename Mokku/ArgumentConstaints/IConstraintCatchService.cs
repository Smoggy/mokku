using System.Linq.Expressions;

namespace Mokku.ArgumentConstaints;

internal interface IConstraintCatchService
{
    IArgumentConstraint TryCatchTheConstraintFromExpression(Expression expression);
}
