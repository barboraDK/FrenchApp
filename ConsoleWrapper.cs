using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenchApp
{
    internal class ConsoleWrapper : IInOutHandler
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
        public string ReadLine()
        {
            return Console.ReadLine();
        }
        public void Clear()
        {
            Console.Clear();
        }
    }
}
