using FluentAssertions;
using Mokku.Exceptions;

namespace Mokku.UnitTests;

public class ClassMockTests
{
    [Fact]
    public void ShouldNotCreateMockForSealedClass()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<SealedClass>().Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }

    [Fact]
    public void ShouldNotAllowToConfigureNonVirtualMethods()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<TestBaseClass>()
                .WithCallTo(x => x.NonVirtualMethod(Is.Any<int>()), x => x.DoesNothing())
                .Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }

    [Fact]
    public void ShouldNotAllowToConfigureSealedMethods()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<TestDerivedClass>()
                .WithCallTo(x => x.ActionMethod(Is.Any<int>()), x => x.DoesNothing())
                .Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }

    [Fact]
    public void ShouldNotAllowToConfigureStaticMethods()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<TestDerivedClass>()
                .WithCallTo(_ => TestDerivedClass.StaticMethod(), x => x.DoesNothing())
                .Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }

    [Fact]
    public void ShouldNotAllowToConfigureNotVirtualPropertySetter()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<TestBaseClass>()
             .WithPropertySetter(x => x.NonVirtualProperty, x => x.DoesNothing())
             .Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }

    [Fact]
    public void ShouldAllowToConfigureVirtualPropertySetterWithAnyValueByDefault()
    {
        var calledTimes = 0;

        var mock = new Mock<TestBaseClass>()
            .WithPropertySetter(x => x.VirtualProperty, x => x.Invokes(() => calledTimes++))
            .Build();

        mock.VirtualProperty = string.Empty;
        mock.VirtualProperty = "test";

        Assert.Equal(2, calledTimes);
    }

    [Fact]
    public async void ShouldAllowToConfigureAsyncMethod()
    {
        var mock = new Mock<TestBaseClass>()
            .WithCallTo(x => x.AsyncMethod(), x => x.Returns(() => Task.FromResult("async response"))).Build();

        var response = await mock.AsyncMethod();

        Assert.Equal("async response", response);
    }

    [Fact]
    public void ShouldAllowToConfigureVirtualPropertySetterWithSpecificValueConstraint()
    {
        var calledTimes = 0;

        var mock = new Mock<TestBaseClass>()
            .WithPropertySetter(x => x.VirtualProperty, x => x.When("specificValue").Invokes(() => calledTimes++))
            .Build();

        mock.VirtualProperty = string.Empty;

        mock.VirtualProperty = "specificValue";

        Assert.Equal(1, calledTimes);
    }

    [Fact]
    public void ShouldAllowToConfigureVirtualPropertySetterWithCustomConstraint()
    {
        var calledTimes = 0;

        var mock = new Mock<TestBaseClass>()
            .WithPropertySetter(x => x.VirtualProperty, x => x.When(() => Is.A<string>.That.Contains("specificValue", StringComparison.OrdinalIgnoreCase)).Invokes(() => calledTimes++))
            .Build();

        mock.VirtualProperty = string.Empty;

        mock.VirtualProperty = "specificValue";

        Assert.Equal(1, calledTimes);
    }

    [Fact]
    public void ShouldNotAllowToConfigurePropertySetterIfItIsNotPresent()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<TestBaseClass>()
                .WithPropertySetter(x => x.VirtualPropertyWithoutSetter, x => x.DoesNothing()).Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }
}
