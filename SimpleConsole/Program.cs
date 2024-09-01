using FakeItEasy;
using Mokku;
using SimpleConsole;

using static System.Console;

WriteLine("Hello, World!");

var wasCalled = false;

var mock = new Mock<DerivedFoo>()
    .WithCallTo(x => x.VirtualString(Is.Any<string>()), configuration => configuration.Invokes(() => wasCalled = true))
    .Build();

mock.VirtualString("a"); 


var wasCalled1 = false;

var mock1 = new Mock<GenericDeviced<int>>()
    .WithCallTo(x => x.Method(Is.Any<int>()), c => c.Throws<Exception>())
    .Build();

var res = mock1.Method(1);

var a = A.Fake<Foo>();

A.CallTo(() => a.VirtualString(""));

ReadLine();
