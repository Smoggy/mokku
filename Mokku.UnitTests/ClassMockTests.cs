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
    public void ShouldNotAllowToConfigureNotVirtualPropertySetter()
    {
        var exception = Record.Exception(() =>
        {
            var mock = new Mock<TestBaseClass>()
             .WithPropertySetter(x => x.NonVirtualProperty, x => x.When(Is.Any<string>()).DoesNothing())
             .Build();
        });

        exception.Should().BeOfType<ConfigurationException>();
    }

    [Fact]
    public void ShouldAllowToConfigureVirtualPropertySetter()
    {
        var wasCalled = false;

        var mock = new Mock<TestBaseClass>()
            .WithPropertySetter(x => x.VirtualProperty, x => x.When(Is.Any<string>()).Invokes(() => wasCalled = true))
            .Build();

        Assert.True(wasCalled);
    }
}
