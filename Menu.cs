namespace FrenchApp;

public class Menu
{
    private IInOutHandler InOutHandler { get; }
    private BaseWordRepository WordRepository { get; }

    public Menu(IInOutHandler inOutHandler, BaseWordRepository wordRepository)
    {
        InOutHandler = inOutHandler;
        WordRepository = wordRepository;
    }
    public void RunMenu()
    {
        Excercise excercise = new Excercise(WordRepository, InOutHandler);

        while (true)
        {
            InOutHandler.WriteLine("Would you like to add words (A), do execise (E) or quit the application (Q)?");

            string input = InOutHandler.ReadLine().ToUpper();

            switch (input)
            {
                case "A":
                    InOutHandler.Clear();
                    WordRepository.AddWords();
                    break;
                case "E":
                    InOutHandler.Clear();
                    excercise.DoExcercise();
                    break;
                case "Q":
                    InOutHandler.Clear();
                    InOutHandler.WriteLine("Shutting down...");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    break;
                default:
                    InOutHandler.WriteLine("Invalid input. Please, try again.");
                    Thread.Sleep(2000);
                    InOutHandler.Clear();
                    break;
            }
        }
    }
}
