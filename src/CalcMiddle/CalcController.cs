using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace CalcMiddle
{
    public class Calculator
    {
        public static int Add(int a, int b) => a + b;
        public static int Sub(int a, int b) => a - b;
        public static int Mul(int a, int b) => a * b;
        public static int Div(int a, int b)
        {
            if(b == 0)
                throw new DivideByZeroException();
            return a / b;
        }

    }
    public class Result
    {
        public Result(int result)
        {
            this.result = result;
        }

        public bool isOk { get; set; } = true;
        public int result { get; set; }
    }

    [ApiController]
    public class CalcController : ControllerBase
    {
        // GET
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Result> Get(string op, int a, int b)
        {
            var result = new Result(0);
            switch (op)
            {
                case "Add":
                    result.result = Calculator.Add(Convert.ToInt32(a), Convert.ToInt32(b));
                    
                    break;
                case "Sub":
                    result.result = Calculator.Sub(Convert.ToInt32(a), Convert.ToInt32(b));
                    break;
                case "Mul":
                    result.result = Calculator.Mul(Convert.ToInt32(a), Convert.ToInt32(b));
                    break;
                default:
                    try
                    {
                        result.result = Calculator.Div(Convert.ToInt32(a), Convert.ToInt32(b));
                    }
                    catch (DivideByZeroException)
                    {
                        result.isOk = false;
                    }

                    break;
            }

            return result;
        }
    }
}
