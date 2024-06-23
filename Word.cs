using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FrenchApp;

public class Word
{
    public int Id { get; set; }
    public string EnglishExpression { get; set; }
    public string FrenchExpression { get; set; }
    public string? WordClass { get; set; }
    public string? Gender { get; set; }
    public Result Result { get; set; }
}

public enum Result
{
    correct,
    wrong
}