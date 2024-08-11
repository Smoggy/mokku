namespace Mokku.UnitTests;

public class MethodArgumentConstraintsTests
{
    [Fact]
    public void ShouldMatchOnExactValueWhenNoConstraintSpecified()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.IntReturnWithArguments("a", 6, DateTime.Today), x => x.Returns(10))
            .Build();

        Assert.Equal(10, mock.IntReturnWithArguments("a", 6, DateTime.Today));
        Assert.Equal(default, mock.IntReturnWithArguments("test", 1, DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public void ShouldMatchAnyArgumentWhenIsAnySpecified()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.IntReturnWithArguments(Is.Any<string>(), Is.Any<int>(), Is.Any<DateTime>()), x => x.Returns(10))
            .Build();

        Assert.Equal(10, mock.IntReturnWithArguments("a", 6, DateTime.Today));
    }

    [Fact]
    public void ShouldMatchConditionWhenPredicateSpecified()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.IntReturnWithArguments(
                    Is.A<string>.That.Matches(x => x.StartsWith("tes")),
                    Is.An<int>.That.IsGreaterThan(5),
                    Is.A<DateTime>.That.Matches(x => x.Date == DateTime.Today.Date)),
                x => x.Returns(10))
            .Build();

        Assert.Equal(10, mock.IntReturnWithArguments("test", 6, DateTime.Today));
        Assert.Equal(default, mock.IntReturnWithArguments("aaaa", 2, DateTime.Today));
    }

    [Fact]
    public void ShouldMatchEveryArgumentConditionForParamsArguments()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.MethodWithParams(Is.Any<string>(), Is.Any<string>()), x => x.Returns(10))
            .Build();

        Assert.Equal(10, mock.MethodWithParams("test", "test"));
        Assert.Equal(default, mock.MethodWithParams("test"));
    }

    [Fact]
    public void ShouldMatchArrayConditionForParamsArguments()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.MethodWithParams(Is.Any<string[]>()), x => x.Returns(10))
            .Build();

        Assert.Equal(10, mock.MethodWithParams("test", "test"));
    }
}