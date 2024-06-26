namespace FrenchApp;

public class Excercise
{
    private IInOutHandler InOutHandler { get; }
    private BaseWordRepository WordRepository { get; }

    public Excercise(BaseWordRepository wordRepository, IInOutHandler inOutHandler)
    {
        WordRepository = wordRepository;
        InOutHandler = inOutHandler;
    }

    public void DoExcercise()
    {
        int count = 5;
        List<Word> randomWords = WordRepository.FindRandomWords(count);

        InOutHandler.WriteLine($"Translate {count} following words.");
        InOutHandler.WriteLine($"Press Enter to continue.");
        InOutHandler.ReadLine();
        InOutHandler.Clear();

        foreach (Word word in randomWords)
        {
            InOutHandler.WriteLine(word.EnglishExpression);
            string translationInput = InOutHandler.ReadLine();
            InOutHandler.Clear();
            if (translationInput.Trim() == word.FrenchExpression)
            {
                InOutHandler.WriteLine("Well done");
                word.Result = Result.correct;
            }
            else
            {
                InOutHandler.WriteLine($"This is wrong. Correct translation is {word.FrenchExpression}.");
                word.Result = Result.wrong;

            }
            InOutHandler.ReadLine();
            InOutHandler.Clear();
        }

        IEnumerable<Word> CorrectAnswers = randomWords.Where(w => w.Result == Result.correct);

        int numberOfCorrectAnswers = CorrectAnswers.Count();
        int score = 100 * numberOfCorrectAnswers / count;

        InOutHandler.WriteLine($"Your score is {score} %");
        InOutHandler.ReadLine();
        InOutHandler.Clear();
        Menu menu = new Menu(InOutHandler, WordRepository);
        menu.RunMenu();
        

    }
 }

