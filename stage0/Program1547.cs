// See https://aka.ms/new-console-template for more information
using System;

namespace Targil10
{
    partial class program
    {
        static void Main(string[] args)
        {
            Welcome1547();
            Welcome2894();
            Console.ReadKey();
        }

        static partial void Welcome2894();
        private static void Welcome1547()
        {
            Console.Write("Enter your name: ");
            string name = System.Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}