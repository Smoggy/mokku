namespace Mokku.ArgumentConstaints;

internal interface ITypedArgumentConstraint : IArgumentConstraint
{
    Type ArgumentType { get; }
}
