﻿using System;
using System.Collections.Generic;
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
            var res = Divide(5, 1);
            if (res) 
                Console.WriteLine(res.Unwrap());
        }
    }
}
