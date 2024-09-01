using System.Linq.Expressions;

namespace Mokku.RuleConfigurations;

public interface IPropertySetterConfiguration<TMember> :
    IDoNothingConfiguration<IPropertySetterConfiguration<TMember>>,
    IThrowExceptionConfiguration<IPropertySetterConfiguration<TMember>>,
    ICallbackConfiguration<IPropertySetterConfiguration<TMember>>;

public interface IPropertySetterWithArgumentConstraintConfiguration<TMember> : IPropertySetterConfiguration<TMember>
{
    IPropertySetterConfiguration<TMember> When(TMember value);

    IPropertySetterConfiguration<TMember> When(Expression<Func<TMember>> constraintExpression);
}