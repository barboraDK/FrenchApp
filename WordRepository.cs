using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenchApp;

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class WordRepository
{
    private readonly int _count;

    public WordRepository(int count)
    {
        _count = count;
    }

    public List<Word> FindRandomWords()
    {
        List<Word> words = new List<Word>();

        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string databasePath = Path.Combine(projectDirectory, "vocabularydb");


        try
        {
            using SqliteConnection connection = new SqliteConnection($"Data Source={databasePath}");
            connection.Open();

            string wordQuery = $"SELECT Id, EnglishExpression, FrenchExpression FROM UserExpressions ORDER BY RANDOM() LIMIT {_count}";

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
            Console.WriteLine($"Error: {ex.Message}");
        }

        return words;
    }
}


