using System;
using System.Collections.Generic; // Incorrect ordering
using System.Linq;

namespace TestNamespace // Incorrect namespace declaration
{
    class Program // Should be PascalCase and indented
    {
        static void Main(string[] args)
        {
            var x = new List<string>(); // Should use explicit type
            foreach (string item in x) // Should use 'var' here
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Hello World!"); // Missing newline after method
        }

        public int MyField; // Should have 'private' access modifier
    }
}