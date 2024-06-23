namespace FrenchApp;

public class Excercise
{

    public void DoExcercise()
    {
        int count = 5;
        WordRepository wordRepository = new WordRepository(count);
        List<Word> randomWords = wordRepository.FindRandomWords();

        Console.WriteLine($"Translate {count} following words.");
        Console.WriteLine($"Press Enter to continue.");
        Console.ReadLine();
        Console.Clear();

        foreach (Word word in randomWords)
        {
            Console.WriteLine(word.EnglishExpression);
            string translationInput = Console.ReadLine();
            Console.Clear();
            if (translationInput.Trim() == word.FrenchExpression)
            {
                Console.WriteLine("Well done");
                word.Result = Result.correct;
            }
            else
            {
                Console.WriteLine($"This is wrong. Correct translation is {word.FrenchExpression}.");
                word.Result = Result.wrong;

            }
            Console.ReadLine();
            Console.Clear();
        }

        IEnumerable<Word> CorrectAnswers = randomWords.Where(w => w.Result == Result.correct);

        int numberOfCorrectAnswers = CorrectAnswers.Count();
        int score = 100 * numberOfCorrectAnswers / count;

        Console.WriteLine($"Your score is {score} %");
        Console.ReadLine();
        Console.Clear();
        Menu.SelectNextActivity();

    }
 }

