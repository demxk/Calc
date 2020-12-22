using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ExprTreeCalc
{
    class ExprVisitor : ExpressionVisitor
    {
        public ExprVisitor()
        {
        }

        public int Visit(BinaryExpression expr)
        {
            var l = Visit((dynamic) expr.Left);
            var r = Visit((dynamic) expr.Right);
            switch (expr.NodeType)
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

        public int Visit(ConstantExpression expr)
        {
            return (int) expr.Value;
        }
    }

    class Program
    {
        public static int evaluate(int l, int r, ExpressionType e)
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

        public static void Main(string[] args)
        {
            Console.WriteLine("Enter expression: ");
            var str = Console.ReadLine();
            var rootExpr = buildExpr(str);
            var visitor = new ExprVisitor();
            Console.WriteLine($"Answer: {visitor.Visit((dynamic)rootExpr)}");
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
                str = str.Trim();
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
