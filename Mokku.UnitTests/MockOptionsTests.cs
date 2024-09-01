namespace Mokku.UnitTests;

using FluentAssertions;
using Xunit;

public class MockOptionsTests
{
    [Fact]
    public void MockShouldImplementAllInterfacesWhenPassingType()
    {
        var mock = new Mock<IFoo>(x => x.ShouldImplement(typeof(IDisposable)).ShouldImplement(typeof(IConvertible))).Build();

        mock.Should()
            .BeAssignableTo<IDisposable>().And
            .BeAssignableTo<IConvertible>().And
            .BeAssignableTo<IFoo>();
    }

    [Fact]
    public void MockShouldImplementAdditionalInterfacesWhenPassingType()
    {
        var mock = new Mock<TestBaseClass>(x => x.ShouldImplement(typeof(IFoo)).ShouldImplement(typeof(IConvertible))).Build();

        mock.Should()
            .BeAssignableTo<IFoo>().And
            .BeAssignableTo<IConvertible>().And
            .BeAssignableTo<TestBaseClass>();
    }

    [Fact]
    public void MockShouldImplementAllInterfacesWhenPassingGenerics()
    {
        var mock = new Mock<IFoo>(x => x.ShouldImplement<IDisposable>().ShouldImplement<IConvertible>()).Build();

        mock.Should()
            .BeAssignableTo<IDisposable>().And
            .BeAssignableTo<IConvertible>().And
            .BeAssignableTo<IFoo>();
    }

    [Fact]
    public void MockShouldImplementAdditionalInterfacesWhenPassingGenerics()
    {
        var mock = new Mock<TestBaseClass>(x => x.ShouldImplement<IFoo>().ShouldImplement<IConvertible>()).Build();

        mock.Should()
            .BeAssignableTo<IFoo>().And
            .BeAssignableTo<IConvertible>().And
            .BeAssignableTo<TestBaseClass>();
    }

    [Fact]
    public void ShouldThrowAnExceptionIfAdditionalInterfaceIsClass()
    {
        var mock = new Mock<TestBaseClass>(x => x.ShouldImplement<IFoo>().ShouldImplement<IConvertible>()).Build();

        var exception = Record.Exception(() => new Mock<IFoo>(x => x.ShouldImplement<TestBaseClass>()).Build());

        exception.Should().BeOfType<ArgumentException>();
    }
}
