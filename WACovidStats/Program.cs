using System;

namespace WACovidStats
{
    class Program
    {
        static void Main(string[] args)
        {
            CaseLGA.Execute();
            TotalSummary.Execute();
            SOF.Execute();
            Console.WriteLine("Hello World!");
        }
    }
}
