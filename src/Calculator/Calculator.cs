using System;

namespace Calculator
{
    public class Calculator
    {
        public static int Add(int a, int b) => a + b;
        public static int Sub(int a, int b) => a - b;
        public static int Mul(int a, int b) => a * b;
        public static int Div(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException();
            return a / b;
        }
    }
}
