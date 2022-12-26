using System.Text.Json;
using MainLib;
using InterpreterLib;
namespace ConsoleUI
{
    public static class DeckSetup
    {
        static string PATH;
        public static void ManageDecks(string path)
        {
            PATH = path;
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Manage your decks");
                Console.Write("> ");
                string order = Console.ReadLine();
                string[] orderWords = order.Split();

                if (order == "home") break;

                if (order == "show")
                {
                    // Printing all the available decks
                    foreach (var file in Directory.EnumerateDirectories(PATH).Select(f => f.Split('\\').Last()))
                    {
                        Console.WriteLine(file);
                    }
                }
                else if (orderWords.Length == 2)
                {
                    if (orderWords[0] == "delete")
                    {
                        // Deleting an existing and not default deck
                        DeleteDeck(orderWords[1]);
                    }
                    else if (orderWords[0] == "create" && orderWords[1] == "deck")
                    {
                        // If an CreateDeck() return true, meaning the build was not succesful then delete the incomplete deck
                        if (CreateDeck(out string file)) Directory.Delete(file, true);
                    }
                    else if (orderWords[0] == "show")
                    {
                        // Printing cards of typed deck if it exists
                        ShowDeck(orderWords[1]);
                    }
                }
                else
                {
                    UI.ErrorMessage("Wrong order");
                }
            }

        }
        private static bool CreateDeck(out string path)
        {
            // Setting a correct new decks path
            string name = GetName("deck");
            path = @$"..\Decks\{name}Deck";

            // Creating deck directory
            Directory.CreateDirectory(path);

            // Adding cards to the new deck 'til it reaches 16
            while (Directory.GetFiles(path).Length < 16)
            {
                // If typed "done" quit deck building
                if (Console.ReadLine() == "done") return true;
                // Creating card
                CreateCard(path);
                Console.Clear();
            }
            return false;
        }
        private static void DeleteDeck(string directory)
        {
            // Forbidding default decks deletion
            if (directory == "AmericanDeck" || directory == "GermanDeck")
            {
                UI.ErrorMessage("Cannot erase default decks");
                return;
            }

            // Creating the lis of all decks
            List<string> files = Directory.EnumerateDirectories(PATH).Select(f => f.Split('\\').Last()).ToList();

            // If the input deck exist, then erase it. If it doesn't, then throw an error message
            if (files.Contains(directory)) Directory.Delete(@"..\Decks\" + directory, true);
            else UI.ErrorMessage("That deck does not exist");
        }
        private static void ShowDeck(string name)
        {
            // Printing all cards cards contained in a deck
            if (Directory.EnumerateDirectories(PATH).Select(f => f.Split('\\').Last()).Any(f => f == name))
            {
                foreach (var card in Directory.GetFiles(@"..\Decks\" + name))
                {
                    System.Console.WriteLine(Path.GetFileName(card));
                }
            }
        }
        private static void CreateCard(string path)
        {
            Console.Clear();
            string type;
            while (true)
            {
                // Checking for card's type
                Console.WriteLine("Type card's type:");
                type = Console.ReadLine();
                if (type == "order" || type == "unit") break;
                UI.ErrorMessage("Wrong type, it can only be a unit or a order");
            }

            if (type == "order")
            {
                CreateOrder(path);
            }
            else
            {
                CreateUnit(path);
            }
        }
        private static void CreateOrder(string path)
        {
            // Getting user's inputs
            string name = GetName("card");
            string order = GetOrder("order");
            string description = GetDescription();

            // Building initializer
            OrderInitializer initializer = new OrderInitializer { Name = name, Order = order, Description = description };
            // Serializing data
            string serialized = JsonSerializer.Serialize(initializer);
            // Storing data as a json file
            File.WriteAllText(path + @$"\order_{name}.json", serialized);
        }
        private static void CreateUnit(string path)
        {
            // Getting user's inputs
            string name = GetName("card");
            string order = GetOrder("unit");
            string description = GetDescription();
            int cost = GetIntegerValue("cost");
            int damage = GetIntegerValue("damage");
            int defense = GetIntegerValue("defense");

            // Building initializer
            UnitInitializer initializer = new UnitInitializer { Name = name, Cost = cost, Damage = damage, Defense = defense, Order = order, Description = description };
            // Serializing data
            string serialized = JsonSerializer.Serialize(initializer);
            // Storing data as a json file
            File.WriteAllText(path + @$"\{name}.json", serialized);
        }

        // This method gets a non-empty user input
        private static string GetName(string obj)
        {
            string name;
            do
            {
                Console.WriteLine($"Type {obj}'s name: ");
                name = Console.ReadLine();
            } while (name == "");

            return name;
        }

        // This method gets a user input
        private static string GetDescription()
        {
            Console.WriteLine("Type card's description: ");
            return Console.ReadLine();
        }

        // This method gets a valid order from the user
        private static string GetOrder(string type)
        {
            string order;

            while (true)
            {
                Console.WriteLine("Type card's order: ");
                order = Console.ReadLine();
                if (order == "" && type == "unit") return order;
                try
                {
                    // If the input can be parsed the it's valid
                    Lexer lexer = new Lexer(order);
                    Parser parser = new Parser(lexer);
                    parser.Parse();
                    break;
                }
                catch
                {
                    UI.ErrorMessage("Syntax error");
                }
            }

            return order;
        }
        // This method gets an integer input from the user
        private static int GetIntegerValue(string prop)
        {
            int parseResult = 0;
            while (parseResult == 0)
            {
                Console.WriteLine($"Type card's {prop}: ");
                _ = int.TryParse(Console.ReadLine(), out parseResult);
            }

            return parseResult;
        }
    }
}