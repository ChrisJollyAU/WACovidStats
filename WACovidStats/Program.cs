﻿using System;

namespace WACovidStats
{
    class Program
    {
        static void Main(string[] args)
        {
            CaseLGA.Execute();
            TotalSummary.Execute();
            SOF.Execute();
            SOFDate.Execute();
            DetailsDate.Execute();
            Console.WriteLine("Hello World!");
        }
    }
}
