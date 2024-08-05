using FluentAssertions;

namespace Mokku.UnitTests;

public class BaseInterfaceMocks
{
    [Fact]
    public void ShouldReturnCorrectValueWhenReturnValueIsProvided()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.StringReturnMethod(), x => x.Returns("response")).Build();

        var response = mock.StringReturnMethod();

        Assert.Equal("response", response);
    }

    [Fact]
    public void ShouldReturnCorrectValueWhenReturnValueFuncIsProvided()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.StringReturnMethod(), x => x.Returns(() => "response")).Build();

        var response = mock.StringReturnMethod();

        Assert.Equal("response", response);
    }

    [Fact]
    public void ShouldThrowExceptionWhenConfigured()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.StringReturnMethod(), x => x.Throws<FieldAccessException>()).Build();

        var exception = Record.Exception(mock.StringReturnMethod);

        exception.Should().BeOfType<FieldAccessException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenExceptionFactoryIsProvided()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.StringReturnMethod(), x => x.Throws(() => new FieldAccessException())).Build();

        var exception = Record.Exception(mock.StringReturnMethod);

        exception.Should().BeOfType<FieldAccessException>();
    }
}
