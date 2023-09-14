using Lab_2_videogame;

class Program
{
    static void Main()
    {
        string filePath = @"C:\Users\Aimen\OneDrive\Documents\Desktop\CSCI 2910\videogames.csv";

        // Read data from the CSV file and store it in a List of VideoGame objects
        List<VideoGame> videoGames = ReadVideoGamesFromCSV(filePath);

        // Create a dictionary to store game titles and their release years
        Dictionary<string, int> gameDictionary = new Dictionary<string, int>();

        // Create a stack to store previous guesses
        Stack<int> previousGuesses = new Stack<int>();

        // Create a queue to present game titles in randomized order
        Queue<string> gameQueue = new Queue<string>(videoGames.Select(game => game.Name).OrderBy(x => Guid.NewGuid()));

        // Create a list to keep track of duplicate game titles
        List<string> duplicateTitles = new List<string>();

        // Populate the dictionary, handling duplicates
        foreach (var game in videoGames)
        {
            if (!gameDictionary.ContainsKey(game.Name))
            {
                gameDictionary.Add(game.Name, game.Year);
            }
            else
            {
                duplicateTitles.Add(game.Name);
            }
        }

        // Welcome message
        Console.WriteLine("Welcome to the Video Game Release Year Guessing Game!");
        Console.WriteLine("Type 'quit' to exit the game.\n");

        while (true)
        {
            // Check if the game queue is empty (all games guessed)
            if (gameQueue.Count == 0)
            {
                Console.WriteLine("You've guessed all the games! Good job!");
                break;
            }

            // Get the next game title from the queue
            string currentGame = gameQueue.Dequeue();

            // Prompt the user with the game's title
            Console.WriteLine($"Guess the release year of the game: {currentGame}");
            Console.Write("Your guess (or 'quit' to exit, 'back' to see the previous guess): ");
            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "quit")
            {
                Console.WriteLine("Thanks for playing! Goodbye!");
                break;
            }
            else if (userInput.ToLower() == "back")
            {
                // Pop and display the previous guess from the stack
                if (previousGuesses.Count > 0)
                {
                    Console.WriteLine($"Previous guess: {previousGuesses.Pop()}");
                }
                else
                {
                    Console.WriteLine("No previous guesses to show.");
                }
            }
            else if (int.TryParse(userInput, out int userGuess))
            {
                // Check if the user's guess is correct
                if (gameDictionary.TryGetValue(currentGame, out int correctYear))
                {
                    if (userGuess == correctYear)
                    {
                        Console.WriteLine("Congratulations! You guessed the correct year!");
                    }
                    else
                    {
                        Console.WriteLine($"Oops! The correct year is {correctYear}. Try again!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid game title. Please try again.");
                }

                // Push the user's guess onto the stack
                previousGuesses.Push(userGuess);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid year or 'quit' to exit.");
            }

            Console.WriteLine();
        }

        
    }

    static List<VideoGame> ReadVideoGamesFromCSV(string filePath)
    {
        List<VideoGame> videoGames = new List<VideoGame>();

        try
        {
            using (var reader = new StreamReader(filePath))
            {
                // Skip the header line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    // Parse and create VideoGame objects
                    var videoGame = new VideoGame
                    {
                        Name = values[0],
                        Year = int.Parse(values[2])
                    };

                    videoGames.Add(videoGame);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error reading CSV file: " + e.Message);
        }

        return videoGames;
    }
}
//idea by me
//Helpful Links:
// reference: https://stackoverflow.com/questions/9086168/random-number-guessing-game
//Also use chat gpt for helping with my errors 