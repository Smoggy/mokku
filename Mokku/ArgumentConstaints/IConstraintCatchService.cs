using System.Linq.Expressions;

namespace Mokku.ArgumentConstaints;

/// <summary>
/// Allows to catch the result of argumnent constraint action
/// </summary>
internal interface IConstraintCatchService
{
    IArgumentConstraint TryCatchTheConstraintFromExpression(Expression expression);
}
