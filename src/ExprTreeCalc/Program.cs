using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ExprTreeCalc
{

    interface IEvaluatable<T> where T: Expression
    {
        public int Eval(T expr);
    }

    interface IEvaluatable
    {
        public int Evaluate(Expression expr);
    }

    class Executer: IEvaluatable,
        IEvaluatable<BinaryExpression>,
        IEvaluatable<ConstantExpression>
    {
        public Executer() {}
        public int Evaluate(Expression expr)
        {
            return this.Eval((dynamic)expr);
        }
        public int Eval(Expression expr)
        {
            System.Console.WriteLine("InvalidOperation");
            throw new InvalidOperationException();
        }
        
        public int Eval(BinaryExpression binexrp)
        {
            var l = Evaluate(binexrp.Left);
            var r = Evaluate(binexrp.Right);
            return Program.get_res(l, r, binexrp.NodeType);
            
        }
        public int Eval(ConstantExpression idexpr)
        {
            return (int) idexpr.Value;
        }
    }
    class Program
    {
        public static int get_res(int l, int r, ExpressionType e)
        {
            switch (e)
            {
                case ExpressionType.Add:
                    return l + r;
                case ExpressionType.Subtract:
                    return l - r;
                case ExpressionType.Multiply:
                    return l * r;
                default:
                    return l / r;
                    
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter expression: ");
            var str = Console.ReadLine();
            str = string.Concat(str.Where(c => !Char.IsWhiteSpace(c)));
            var rootExpr = buildExpr(str);
            var executer = new Executer();
            Console.WriteLine($"Answer: {executer.Evaluate(rootExpr)}");
        }

        public static Expression buildExpr(string str)
        {
            var (op, pos) = FindPos(ref str);
            if (op != -1)
            {
                var left = buildExpr(str.Substring(0, pos));
                var right = buildExpr(str.Substring(pos + 1, str.Length - pos - 1));
                switch (op)
                {
                    case 0:
                        return BinaryExpression.Add(left, right);
                    case 1:
                        return BinaryExpression.Subtract(left, right);
                    case 2:
                        return BinaryExpression.Multiply(left, right);
                    default:
                        return BinaryExpression.Divide(left, right);
                }
            }
            else
            {
                if (str.Length > 2 && str[0] == '(' && str[^1] == ')')
                    return buildExpr(str.Substring(1, str.Length - 2));

                else
                {
                    return Expression.Constant(Int32.Parse(str.Trim()));
                }
            }
        }

        // -1 no op
        // 0 add
        // 1 sub
        // 2 mut , 3 div
        private static (int, int) FindPos(ref string s)
        {
            var balance = 0;
            var isfound = false;
            var found = (-1, -1);
            for (int i = s.Length - 1; i >= 0; --i)
            {
                switch (s[i])
                {
                    case ')':
                        ++balance;
                        continue;
                    case '(':
                        --balance;
                        continue;
                    case '+':
                        if (balance == 0)
                            return (0, i);
                        break;
                    case '-':
                        if (balance == 0)
                            return (1, i);
                        break;
                    case '*':
                        if (balance == 0 && !isfound)
                        {
                            isfound = true;
                            found = (2, i);
                        }

                        break;
                    case '/':
                        if (balance == 0 && !isfound)
                        {
                            isfound = true;
                            found = (3, i);
                        }

                        break;
                }
            }

            if (isfound)
                return found;
            return (-1, 0);
        }
    }
}
