using System.Runtime.InteropServices;

namespace DeepCloner.Core.Tests;

[TestFixture]
public class UnnamedType
{
    private unsafe class UnnamedTypeContainer
    {
        public int Value;
        public object? Object;
        //FullName of this type may be null
        public delegate*<IServiceProvider, object> Builder;
    }

    [Test]
    public unsafe void UnnamedTypeTest()
    {
        //Arrange
        var array = new[] { 1, 2, 3 };
        var builder = (IntPtr)GCHandle.Alloc(array, GCHandleType.Pinned);
        var obj = new UnnamedTypeContainer()
        {
            Value = 1,
            Object = new object(),
            Builder = (delegate*<IServiceProvider, object>)builder
        };
        //Act
        var result = obj.DeepClone();
        //Assert
        Assert.Multiple(() =>
        {
            // UnnamedTypeContainer is cloned
            Assert.That(result, Is.Not.EqualTo(obj));
            // Value is copied
            Assert.That(result.Value, Is.EqualTo(obj.Value));
            // Object is cloned
            Assert.That(result.Object, Is.Not.EqualTo(obj.Object));
            // Builder references same instance
            Assert.That(result.Builder == obj.Builder, Is.True);
        });
    }
}