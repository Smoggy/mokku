using FakeItEasy;
using Mokku;
using SimpleConsole;

using static System.Console;

WriteLine("Hello, World!");

var mock = new Mock<ITest>()
    .WithCallTo(x => x.StringInput(""), configuration => configuration.Throws<ArgumentException>())
    .WithCallTo(x => x.BoolReturn(), configuration => configuration.Returns(true))
    .Build();

mock.BoolReturn();


var fake = A.Fake<ITest>();
A.CallTo(() => fake.StringInput(A<string>.Ignored)).DoesNothing();
A.CallTo(() => fake.BoolReturn()).Throws(() => new ArgumentException());

ReadLine();

