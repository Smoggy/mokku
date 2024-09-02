# Mokku

Small library designed to simplify the creation of mock objects for unit testing or other purposes.
It provides a fluent API to create, configure, and verify mocks with minimal setup.


## Configuring a Mock

```
var mock = new Mock<IFoo>()
    .WithCallTo(x => x.SomeMethod(
            Is.A<string>.That.Matches(x => x.StartsWith("tes")),
            Is.An<int>.That.IsGreaterThan(5),
            Is.A<DateTime>.That.Matches(x => x.Date == DateTime.Today.Date)),
        x => x.Returns(10))
    .WithCallTo(x => x.Property,
        x => x.DoesNothing())
     .WithPropertySetter(x => x.VirtualProperty, 
        x => x.When(() => Is.A<string>.That.Contains("specificValue")).Throws<FieldAccessException>())
    .Build();
```