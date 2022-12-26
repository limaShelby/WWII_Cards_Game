using MainLib;
using System.Text.Json;

namespace ConsoleUI
{
    public static class GameSetup
    {
        static string PATH;
        static Player player1;
        static Player player2;
        static List<Card> deck1;
        static List<Card> deck2;
        public static void SetGame(string path)
        {
            PATH = path;
            // If "SetDecks()" or "SetMode()" return -meaning the user typed home- then it's over
            if (SetDecks()) return;
            if (SetMode()) return;

            Console.Clear();

            // Starting game
            Manager manager = new Manager(player1, player2);
            string result = manager.Start();
            Console.Write(result);
        }
        private static bool SetDecks()
        {
            Console.Clear();

            // Printing available decks
            List<string> files = Directory.EnumerateDirectories(PATH).Select(f => f.Split('\\').Last()).ToList();
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            //** Getting decks
            while (true)
            {
                Console.WriteLine("Player1 pick your deck");
                Console.Write("> ");
                string order = Console.ReadLine();

                if (order is "home") return true;

                if (files.Any(f => order == f))
                {
                    // Building deck
                    deck1 = BuildDeck(order);
                    // Remove selected deck from available decks so players cannot have same decks
                    files.Remove(order);
                    break;
                }
                else
                {
                    UI.ErrorMessage("Wrong deck");
                }
            }
            while (true)
            {
                Console.WriteLine("Player2 pick your deck");
                Console.Write("> ");
                string order = Console.ReadLine();

                if (order is "home") return true;

                if (files.Any(f => order == f))
                {
                    // Building deck
                    deck2 = BuildDeck(order);
                    break;
                }
                else
                {
                    UI.ErrorMessage("Wrong deck");
                }
            }

            return false;
        }
        private static bool SetMode()
        {
            Console.Clear();

            // Initializing players according to selected mode
            while (true)
            {
                Console.WriteLine("Select game mode");
                Console.Write("> ");
                string order = Console.ReadLine();

                if (order is "home") return true;

                if (order == "human")
                {
                    player1 = new UserPlayer("User1", deck1);
                    player2 = new UserPlayer("User2", deck2);
                    break;
                }
                else if (order == "bot")
                {
                    player1 = new UserPlayer("User", deck1);
                    player2 = new AI_Player("Bot", deck2);
                    break;
                }
                else
                {
                    UI.ErrorMessage();
                }
            }

            return false;
        }
        private static List<Card> BuildDeck(string path)
        {
            List<Card> deck = new List<Card>();

            // Building a deck, deserializing cards initializer stores as jsons
            foreach (var fileName in Directory.GetFiles(@"..\Decks\" + path))
            {
                string json = File.ReadAllText(fileName);
                if (Path.GetFileName(fileName).Split('_').Any(word => word == "order"))
                {
                    OrderInitializer initializer = JsonSerializer.Deserialize<OrderInitializer>(json);
                    deck.Add(initializer.CreateOrder());
                    deck.Add(initializer.CreateOrder());
                }
                else
                {
                    UnitInitializer initializer = JsonSerializer.Deserialize<UnitInitializer>(json);
                    deck.Add(initializer.CreateUnit());
                    deck.Add(initializer.CreateUnit());
                }
            }

            // returing a shufled deck
            return deck.Shuffle();
        }
    }
}