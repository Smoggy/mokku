using FluentAssertions;
using Mokku.Exceptions;

namespace Mokku.UnitTests;

public class GenericClassMockTests
{
    [Fact]
    public void ShouldReturnCorrectValueWhenReturnValueIsProvided()
    {
        var mock = new Mock<GenericClass<string>>()
            .WithCallTo(x => x.ReturnMethod(Is.Any<string>()), x => x.Returns("test")).Build();

        var response = mock.ReturnMethod("test");

        Assert.Equal("test", response);
    }

    [Fact]
    public void ShouldReturnCorrectValueWhenReturnValueFuncIsProvided()
    {
        var mock = new Mock<GenericClass<string>>()
            .WithCallTo(x => x.ReturnMethod(Is.Any<string>()), x => x.Returns(() => "test")).Build();

        var response = mock.ReturnMethod("test");

        Assert.Equal("test", response);
    }

    [Fact]
    public void ShouldThrowExceptionWhenConfigured()
    {
        var mock = new Mock<GenericClass<int>>()
            .WithCallTo(x => x.ReturnMethod(1), x => x.Throws<FieldAccessException>()).Build();

        var exception = Record.Exception(() => mock.ReturnMethod(1));

        exception.Should().BeOfType<FieldAccessException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenExceptionFactoryIsProvided()
    {
        var mock = new Mock<GenericClass<int>>()
            .WithCallTo(x => x.ReturnMethod(1), x => x.Throws(() => new FieldAccessException())).Build();

        var exception = Record.Exception(() => mock.ReturnMethod(1));

        exception.Should().BeOfType<FieldAccessException>();
    }

    [Fact]
    public void ShouldConfigurePropertySetterWithAnyValueByDefault()
    {
        var calledTimes = 0;

        var mock = new Mock<GenericClass<int>>()
            .WithPropertySetter(x => x.Property, x => x.Invokes(() => calledTimes++))
            .Build();

        mock.Property = 1;
        mock.Property = 2;

        Assert.Equal(2, calledTimes);
    }

    [Fact]
    public void ShouldConfigurePropertySetterWithSpecificValueConstraint()
    {
        var calledTimes = 0;

        var mock = new Mock<GenericClass<int>>()
            .WithPropertySetter(x => x.Property, x => x.When(0).Invokes(() => calledTimes++))
            .Build();

        mock.Property = 1;
        mock.Property = 0;

        Assert.Equal(1, calledTimes);
    }

    [Fact]
    public async void ShouldConfigureAsyncMethod()
    {
        var mock = new Mock<GenericClass<string>>()
            .WithCallTo(x => x.AsyncMethod(), x => x.Returns(() => Task.FromResult("async response"))).Build();

        var response = await mock.AsyncMethod();

        Assert.Equal("async response", response);
    }

    [Fact]
    public void ShouldConfigurePropertyGetter()
    {
        var mock = new Mock<GenericClass<int>>()
            .WithCallTo(x => x.ReadonlyProperty, x => x.Returns(10)).Build();

        var response = mock.ReadonlyProperty;

        Assert.Equal(10, response);
    }

    [Fact]
    public void ShouldNotAllowToConfigurePropertySetterIfItIsNotPresent()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<GenericClass<int>>()
                .WithPropertySetter(x => x.ReadonlyProperty, x => x.DoesNothing()).Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }
}
