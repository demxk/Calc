using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace CalcMiddle
{
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
                case "add":
                    result.result = Calculator.Calculator.Add(Convert.ToInt32(a), Convert.ToInt32(b));
                    break;
                case "sub":
                    result.result = Calculator.Calculator.Sub(Convert.ToInt32(a), Convert.ToInt32(b));
                    break;
                case "mul":
                    result.result = Calculator.Calculator.Mul(Convert.ToInt32(a), Convert.ToInt32(b));
                    break;
                default:
                    try
                    {
                        result.result = Calculator.Calculator.Div(Convert.ToInt32(a), Convert.ToInt32(b));
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
