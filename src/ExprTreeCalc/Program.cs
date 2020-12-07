using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExprTreeCalc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter expression: ");
            var str = Console.ReadLine();
            var rootExpr = buildExpr(str);
            var result = ExecutePar(rootExpr);
            Console.WriteLine(result);
        }

        public static Expression<Func<int>> Transform(Expression e)
        {
            return (Expression<Func<int>>) Expression.Lambda(e);
        }

        public static int ExecutePar(Expression node, int level = 0)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                {
                    var l = (BinaryExpression) node;
                    if (level < 2)
                    {

                        var t1 = Task.Factory.StartNew(
                            () => ExecutePar(l.Left, level + 1),
                            TaskCreationOptions.AttachedToParent);
                        var t2 = Task.Factory.StartNew(
                            () => ExecutePar(l.Right, level + 1),
                            TaskCreationOptions.AttachedToParent);
                        var r = get_results(t1.Result, t2.Result, node.NodeType);
                        return r;
                    }
                    else
                    {
                        return get_results(Transform(l.Left).Compile()(), Transform(l.Right).Compile()(),
                            node.NodeType);
                    }
                    // var left = ExecutePar(l.Left);

                }
                default:
                    return (int) (((ConstantExpression) node).Value);
            }
        }

        private static int get_results(int t1Result, int t2Result, ExpressionType nodeNodeType)
        {
            switch (nodeNodeType)
            {
                case ExpressionType.Add:
                    return t1Result + t2Result;
                case ExpressionType.Subtract:
                    return t1Result - t2Result;
                case ExpressionType.Multiply:
                    return t1Result * t2Result;
                default:
                    return t1Result / t2Result;
                    
            }
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
                    // return new BinExpr(left, right, OpType.Add);
                    case 1:
                        return BinaryExpression.Subtract(left, right);
                    // return new BinExpr(left, right, OpType.Sub);
                    case 2:
                        return BinaryExpression.Multiply(left, right);
                    // return new BinExpr(left, right, OpType.Mul);
                    default:
                        return BinaryExpression.Divide(left, right);
                    // return new BinExpr(left, right, OpType.Div);
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