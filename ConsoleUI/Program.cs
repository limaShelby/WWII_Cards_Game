using MainLib;
namespace ConsoleUI
{
    class Program
    {
        const string PATH = @"..\Decks";
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                UI.Intro();
                string order = Console.ReadLine();

                if (order == "play") GameSetup.SetGame(PATH);
                else if (order == "create") DeckSetup.ManageDecks(PATH);
                else if (order == "exit") break;
                else UI.ErrorMessage();
                Console.ReadLine();
            }
        }
    }
}