using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenchApp;
public interface IInOutHandler
{
    void WriteLine(string message);
    
    string ReadLine();
    void Clear();
}