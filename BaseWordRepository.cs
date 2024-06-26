using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenchApp;

public abstract class BaseWordRepository
{
    public abstract List<Word> FindRandomWords(int count);
    public abstract void AddWords();
}
