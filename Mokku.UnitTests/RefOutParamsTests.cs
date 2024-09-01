using FluentAssertions;

namespace Mokku.UnitTests;

public class RefOutParamsTests
{
    [Fact]
    public void ShouldSetRefArguments()
    {
        var refStr = "";
        string outStr;

        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.MethodWithRefAndOut(Is.Any<string>(), ref refStr, Is.Any<int>(), out outStr),
                x => x.SetsRefAndOutArguments("refStr", "outStr"))
            .Build();

        mock.MethodWithRefAndOut("", ref refStr, 1, out outStr);

        Assert.Equal("refStr", refStr);
        Assert.Equal("outStr", outStr);
    }

    [Fact]
    public void ShouldThrowAnExceptionWhenArgsAreLessThenValues()
    {
        var refStr = "";
        string outStr;

        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.MethodWithRefAndOut(Is.Any<string>(), ref refStr, Is.Any<int>(), out outStr),
                x => x.SetsRefAndOutArguments("refStr", "outStr", "redundant"))
            .Build();

        var exception = Record.Exception(() => mock.MethodWithRefAndOut("", ref refStr, 1, out outStr));

        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void ShouldReturnDefaultValuesWhenNothingIsSet()
    {
        var refStr = "";
        string outStr;

        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.MethodWithRefAndOut(Is.Any<string>(), ref refStr, Is.Any<int>(), out outStr),
                x => x.DoesNothing())
            .Build();

        mock.MethodWithRefAndOut("", ref refStr, 1, out outStr);

        Assert.Equal("", refStr);
        Assert.Equal(default, outStr);
    }
}
