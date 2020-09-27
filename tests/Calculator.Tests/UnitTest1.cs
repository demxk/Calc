using System;
using Xunit;
using Calculator;

namespace Calculator.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(3, Calculator.Add(1, 2));
            Assert.Equal(1, Calculator.Add(-1, 2));
            Assert.Equal(4, Calculator.Sub(3, -1));
            Assert.Equal(2, Calculator.Mul(1, 2));
            Assert.Equal(555, Calculator.Mul(5, 111));
            Assert.Equal(6, Calculator.Div(19, 3));
            // ReSharper disable once HeapView.BoxingAllocation
            Assert.ThrowsAny<Exception>(() => Calculator.Div(1, 0));
        }
    }
}