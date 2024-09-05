using FluentAssertions;
using Mokku.Exceptions;

namespace Mokku.UnitTests;

public class InterfaceMockTests
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

    [Fact]
    public void ShouldInvokeAdditionalActions()
    {
        var firstActionClosure = false;
        var secondActionClosure = 1;

        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.Bar(),
                x => x.Invokes(() => firstActionClosure = true)
                       .Invokes(() => ++secondActionClosure)
                       .DoesNothing())
            .Build();

        mock.Bar();

        Assert.True(firstActionClosure);
        Assert.Equal(2, secondActionClosure);
    }

    [Fact]
    public void ShouldConfigurePropertySetterWithAnyValueByDefault()
    {
        var calledTimes = 0;

        var mock = new Mock<IFoo>()
            .WithPropertySetter(x => x.StringProperty, x => x.Invokes(() => calledTimes++))
            .Build();

        mock.StringProperty = string.Empty;
        mock.StringProperty = "test";

        Assert.Equal(2, calledTimes);
    }

    [Fact]
    public void ShouldConfigurePropertySetterWithSpecificValueConstraint()
    {
        var calledTimes = 0;

        var mock = new Mock<IFoo>()
            .WithPropertySetter(x => x.StringProperty, x => x.When("specificValue").Invokes(() => calledTimes++))
            .Build();

        mock.StringProperty = string.Empty;

        mock.StringProperty = "specificValue";

        Assert.Equal(1, calledTimes);
    }

    [Fact]
    public async void ShouldConfigureAsyncMethod()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.AsyncMethod(), x => x.Returns(() => Task.FromResult("async response"))).Build();

        var response = await mock.AsyncMethod();

        Assert.Equal("async response", response);
    }

    [Fact]
    public void ShouldConfigurePropertyGetter()
    {
        var mock = new Mock<IFoo>()
            .WithCallTo(x => x.IntProperty, x => x.Returns(10)).Build();

        var response = mock.IntProperty;

        Assert.Equal(10, response);
    }

    [Fact]
    public void ShouldNotAllowToConfigurePropertySetterIfItIsNotPresent()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<IFoo>()
                .WithPropertySetter(x => x.IntProperty, x => x.DoesNothing()).Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }
}
