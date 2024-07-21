namespace Mokku;

internal class ProxyCreationResult
{
    public List<string> Errors { get; } = [];
    public object? ProxyObject { get; }

    public ProxyCreationResult(object proxyObject)
    {
        ProxyObject = proxyObject;
    }

    public ProxyCreationResult(string error)
    {
        Errors.Add(error);
    }

    public bool IsSuccess => ProxyObject is not null;
}
