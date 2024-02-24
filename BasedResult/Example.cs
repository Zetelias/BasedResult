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
            var result = Divide(Double.Epsilon, Double.Pi);
            if (result.IsOk)
            {
                
            }
        }
    }
}
