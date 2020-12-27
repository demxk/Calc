using System;
using Xunit;
using ExprTreeCalc;

namespace CalcTest
{
    public class CalcTest
    {
        [Theory]
        [InlineData("5/2+3-1*(1+3+2/2)", 0)]
        [InlineData("1+2+3+(1*3+4/2/2*2)", 11)]
        [InlineData("1+2", 3)]
        [InlineData("100/3+20*(7*3-20)", 53)]
        [InlineData("1-3-4-1-(1+3)", -11)]
        [InlineData("10/3", 3)]
        [InlineData("2*3*4-(1+2+10/4)+(1+1+9/(4+2-3))", 24)]
        [InlineData("(2+3-1+43/15)-(19-3*2*7/5)", -5)]
        [InlineData("999/11/2+12*11-(34/8+20)", 153)]
        [InlineData("31*(2-9+11/2+5*6)-24+13/2 + 11/(1+5) - 9*5 - 1", 805)]
        public void Test(string s, int ans)
        {
            var expr = Program.buildExpr(s);
            var visitor = new ExprVisitor();
            Assert.True(visitor.Visit((dynamic)expr) == ans);
        }
    }
}
