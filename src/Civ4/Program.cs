using Serilog;
using System;

namespace Civ4
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .CreateLogger();

            Console.WriteLine("Hello Megalomaniacs!");
        }
    }
}
