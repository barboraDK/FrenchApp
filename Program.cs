namespace FrenchApp;

class Program 
{
     static void Main()
    {
        ConsoleWrapper consoleWrapper = new ConsoleWrapper();
        WordRepository wordRepository = new WordRepository(consoleWrapper);
        Menu menu = new Menu(consoleWrapper, wordRepository);

        menu.RunMenu();
    }
}
