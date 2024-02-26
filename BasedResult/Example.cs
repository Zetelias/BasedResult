#define ENABLE_IMPLICIT_UNWRAPPING

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasedResult;

namespace Example
{
    internal class Example
    {
        public static Result<double, string> Divide(double x, double y)
        {
            if (x == 0)
                return "Division by 0 error";
            return x / y;
        }

        public static void UseDivide()
        {
            double x = 8.4;
            double y = 5.7;
            Divide(x ,y).Match(
                ok => Console.WriteLine($"{x}/{y} = {ok}"),
                err => Console.WriteLine($"{x}/{y} produced error: {err}")
                );
        }
        
    }
}
