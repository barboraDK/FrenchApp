namespace FrenchApp;

public static class Menu
{
    public static void SelectActivity()
    {
        Console.WriteLine("Would you like to add words (A) or do execise (E)?");

        string input = Console.ReadLine().ToUpper();
        
        switch (input)
        {
            case "A":
                Console.Clear();
                WordAdder wordAdder = new WordAdder();
                wordAdder.AddWords();
                break;
            case "E":
                Console.Clear();
                Excercise excercise = new Excercise();
                excercise.DoExcercise();
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("Invalid input. Please, try again.");
                Thread.Sleep(2000);
                Console.Clear();
                SelectActivity();
                break;
        }
    }

    public static void SelectNextActivity()
    {

        Console.WriteLine("Would you like to go back to main menu (M) or quit the application (Q)?");

        string input2 = Console.ReadLine().ToUpper();

        switch (input2)
        {
            case "M":
                Console.Clear();
                SelectActivity();
                break;
            case "Q":
                Console.Clear();
                Console.WriteLine("Shutting down...");
                Thread.Sleep(2000);
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("Invalid input. Please, try again.");
                Thread.Sleep(2000);
                Console.Clear();
                SelectNextActivity();
                break;
        }
    }
}
