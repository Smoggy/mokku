using Mokku.ArgumentConstaints;
using Mokku.Interfaces;
using System.Reflection;

namespace Mokku.InterceptionRules;

internal class MethodExpressionCallRule(ParsedExpression expression, List<IArgumentConstraint> argumentConstraints) : BaseInterceptionRule(expression, argumentConstraints)
{
    private static readonly Func<object?[]> DefaultRefAndOutArgumentsSetter = () => [];
    private Func<object?[]> refAndOutArgumentsSetter = DefaultRefAndOutArgumentsSetter;

    public override void Apply(IFakeObjectCall fakeObjectCall)
    {
        base.Apply(fakeObjectCall);

        SetRefAndOutArguments(fakeObjectCall);
    }

    public void SetRefAndOutFunc(Func<object?[]> valueFactory)
    {
        refAndOutArgumentsSetter = valueFactory;
    }

    protected void SetRefAndOutArguments(IFakeObjectCall fakeObjectCall)
    {
        if (refAndOutArgumentsSetter == DefaultRefAndOutArgumentsSetter)
            return;

        var values = refAndOutArgumentsSetter.Invoke();
        var indexes = GetIndexesOfRefAndOutArguments(fakeObjectCall.MethodInfo.GetParameters());
        if (values.Length != indexes.Count)
        {
            // TODO add proper exception type
            throw new ArgumentException("");
        }

        foreach(var pair in values.Zip(indexes, (v, i) => new {Value = v, Index = i}))
        {
            fakeObjectCall.SetArgumentValue(pair.Index, pair.Value);
        }
    }

    protected static List<int> GetIndexesOfRefAndOutArguments(ParameterInfo[] parameters)
    {
        var result = new List<int>();

        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType.IsByRef)
            {
                result.Add(i);
            }
        }

        return result;
    }
}
