namespace FrenchApp;

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class WordRepository : BaseWordRepository
{
    private IInOutHandler InOutHandler { get; }
    private Word? NewWord { get; set; }
    private List<Word>? FrenchDuplicates { get; set; }
    private List<Word>? EnglishDuplicates { get; set; }

    public WordRepository(IInOutHandler inOutHandler)
    {
        InOutHandler = inOutHandler;
    }

    public override List<Word> FindRandomWords(int count)
    {
        List<Word> words = new List<Word>();

        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string databasePath = Path.Combine(projectDirectory, "vocabularydb");

        try
        {
            using SqliteConnection connection = new SqliteConnection($"Data Source={databasePath}");
            connection.Open();

            string wordQuery = $"SELECT Id, EnglishExpression, FrenchExpression FROM UserExpressions ORDER BY RANDOM() LIMIT {count}";

            using SqliteCommand command = new SqliteCommand(wordQuery, connection);
            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Word word = new Word
                {
                    Id = reader.GetInt32(0),
                    EnglishExpression = reader.GetString(1),
                    FrenchExpression = reader.GetString(2)
                };
                words.Add(word);
            }
        }
        catch (SqliteException ex)
        {
            InOutHandler.WriteLine($"Error: {ex.Message}");
        }

        return words;
    }

    public override void AddWords()
    {
        Menu menu = new Menu(InOutHandler, this);
        NewWord = AskForInput();
        CheckInput();
        FrenchDuplicates = CheckForDuplicateFrench();
        EnglishDuplicates = CheckForDuplicateEnglish();
        InOutHandler.Clear();
        if (FrenchDuplicates.Any())
        {
            ManageFrenchDuplicates();
        }
        if (EnglishDuplicates.Any())
        {
            ManageEnglishDuplicates();
        }
        InsertIntoDb();
        Thread.Sleep(2000);
        InOutHandler.WriteLine("");
        menu.RunMenu();
    }

    private Word AskForInput()
    {

        InOutHandler.WriteLine("You'll be asked to type in a french word, its enghlish tranlation, and specify word class and gender.\n" +
            "It helps to identify words properly, so that there are no duplicates in your private repository.\n\n" +
            "Press enter to continue");
        InOutHandler.ReadLine();
        InOutHandler.Clear();
        InOutHandler.WriteLine("Write a french expression:");
        string newFrenchExpression = InOutHandler.ReadLine();

        InOutHandler.WriteLine("Write the english translation:");
        string newEnglishTranslation = InOutHandler.ReadLine();
        InOutHandler.WriteLine("Write word class (1: nom, 2: adjectif, 3: adverbe, 4: verbe):");

        string newExpressionClassInput = InOutHandler.ReadLine();


        string newExpressionClass = string.Empty;
        string newExpressionGender = string.Empty;


        switch (newExpressionClassInput)
        {
            case "1":
                newExpressionClass = "nom";
                break;
            case "2":
                newExpressionClass = "adjectif";
                break;
            case "3":
                newExpressionClass = "adverbe";
                break;
            case "4":
                newExpressionClass = "verbe";
                break;
        }

        if (newExpressionClass == "nom" || newExpressionClass == "adjectif")
        {
            InOutHandler.WriteLine("Write word gender (1: masculin, 2: féminin):");
            string newExpressionGenderInput = InOutHandler.ReadLine();


            switch (newExpressionGenderInput)
            {
                case "1":
                    newExpressionGender = "masculin";
                    break;
                case "2":
                    newExpressionGender = "féminin";
                    break;
            }
        }

        return new Word()
        {
            FrenchExpression = newFrenchExpression,
            EnglishExpression = newEnglishTranslation,
            WordClass = newExpressionClass,
            Gender = newExpressionGender
        };
    }

    private void SelectNextActionWithDuplicate()
    {
        Menu menu = new Menu(InOutHandler, this);
        InOutHandler.WriteLine($"Would you like to add a new translation to the same word (A)? \n" +
            $"To discard chanages press (D).");

        string input = InOutHandler.ReadLine().ToUpper();
        InOutHandler.Clear();


        switch (input)
        {
            case "A":
                break;
            case "D":
                InOutHandler.Clear();
                menu.RunMenu();
                break;
            default:
                InOutHandler.WriteLine("\nInvalid input. Please, try again.");
                Thread.Sleep(2000);
                InOutHandler.Clear();
                SelectNextActionWithDuplicate();
                break;

        }
    }

    private void CheckInput()
    {
        Menu menu = new Menu(InOutHandler, this);

        InOutHandler.WriteLine("\nAdd the following word to your repository? \n\n" +
            $"French Expression: {NewWord.FrenchExpression}\n" +
            $"English Translation: {NewWord.EnglishExpression}\n" +
            $"Word Class: {NewWord.WordClass}\n" +
            $"Gender: {NewWord.Gender}\n\n" +
            $"Press Enter to continue or type in (Q) to discard changes.");

        string nextActionInput = InOutHandler.ReadLine().ToUpper();
        InOutHandler.Clear();


        switch (nextActionInput)
        {
            case "Q":
                InOutHandler.Clear();
                menu.RunMenu();
                break;
            case "":
                break;
            default:
                InOutHandler.WriteLine("\nInvalid input. Please, try again.");
                Thread.Sleep(2000);
                InOutHandler.Clear();
                CheckInput();
                break;
        }
    }

    private List<Word> CheckForDuplicateFrench()
    {
        List<Word> words = new List<Word>();


        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string databasePath = Path.Combine(projectDirectory, "vocabularydb");

        using SqliteConnection connection = new SqliteConnection($"Data Source={databasePath}");
        connection.Open();

        string wordQuery = $"SELECT Id, EnglishExpression, FrenchExpression FROM UserExpressions  WHERE FrenchExpression = '{NewWord.FrenchExpression}' AND Gender = '{NewWord.Gender}' AND WordClass = '{NewWord.WordClass}'";

        using SqliteCommand command = new SqliteCommand(wordQuery, connection);
        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Word word = new Word()
            {
                Id = reader.GetInt32(0),
                EnglishExpression = reader.GetString(1),
                FrenchExpression = reader.GetString(2)
            };
            words.Add(word);
        }

        return words;
    }

    private List<Word> CheckForDuplicateEnglish()
    {
        List<Word> words = new List<Word>();

        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string databasePath = Path.Combine(projectDirectory, "vocabularydb");

        using SqliteConnection connection = new SqliteConnection($"Data Source={databasePath}");
        connection.Open();

        string wordQuery = $"SELECT Id, EnglishExpression, FrenchExpression FROM UserExpressions  WHERE EnglishExpression = '{NewWord.EnglishExpression}'";

        using SqliteCommand command = new SqliteCommand(wordQuery, connection);
        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Word word = new Word()
            {
                Id = reader.GetInt32(0),
                EnglishExpression = reader.GetString(1),
                FrenchExpression = reader.GetString(2)
            };
            words.Add(word);
        }

        return words;
    }
    private void ManageFrenchDuplicates()
    {
        Menu menu = new Menu(InOutHandler, this);
        foreach (Word word in FrenchDuplicates)
        {
            if (word.EnglishExpression == NewWord.EnglishExpression && word.FrenchExpression == NewWord.FrenchExpression)
            {
                InOutHandler.WriteLine($"You already have the word {word.FrenchExpression} translated as {NewWord.EnglishExpression} in your repository.");
                Thread.Sleep(2000);
                InOutHandler.WriteLine("");
                menu.RunMenu();
            }
        }
        foreach (Word word in FrenchDuplicates)
        {

            if (word.FrenchExpression == NewWord.FrenchExpression && word.EnglishExpression != NewWord.EnglishExpression)
            {
                InOutHandler.WriteLine($"You already have the word {NewWord.FrenchExpression} in your repository translated as {word.EnglishExpression}.");
            }
        }
        Thread.Sleep(2000);
        InOutHandler.WriteLine("");
        SelectNextActionWithDuplicate();


    }
    private void ManageEnglishDuplicates()
    {
        foreach (Word word in EnglishDuplicates)
        {
            if (word.EnglishExpression == NewWord.EnglishExpression)
            {
                InOutHandler.WriteLine($"You already have a word '{word.FrenchExpression}' in your database translated as {NewWord.EnglishExpression}.");
                InOutHandler.WriteLine($"Please, make the translation more comprehensive");
                Thread.Sleep(2000);
                InOutHandler.WriteLine("\nWrite an updated english translation:");
                NewWord.EnglishExpression = InOutHandler.ReadLine();
                InOutHandler.Clear();

            }
        }
        EnglishDuplicates.Clear();
        EnglishDuplicates = CheckForDuplicateEnglish();
        if (EnglishDuplicates.Any())
        {
            ManageEnglishDuplicates();
        }

    }

    private void InsertIntoDb()
    {
        string insertQuery = $"INSERT INTO UserExpressions (FrenchExpression, WordClass, Gender, EnglishExpression) VALUES ('{NewWord.FrenchExpression}','{NewWord.WordClass}','{NewWord.Gender}','{NewWord.EnglishExpression}');";

        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string databasePath = Path.Combine(projectDirectory, "vocabularydb");

        try
        {
            using SqliteConnection connection = new SqliteConnection($"Data Source={databasePath}");
            connection.Open();

            using SqliteCommand command = new SqliteCommand(insertQuery, connection);

            command.ExecuteNonQuery();

            InOutHandler.WriteLine($"The word '{NewWord.FrenchExpression}' has been created successfully.");
        }
        catch (SqliteException ex)
        {
            InOutHandler.WriteLine(ex.Message);
        }
    }
}


