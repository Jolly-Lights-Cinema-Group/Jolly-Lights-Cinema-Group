namespace Jolly_Lights_Cinema_Group;

public class Menu
{
    private string Prompt;
    private string[] Options;
    private int SelectedIndex;
    public Menu(string prompt, string[] options)
    {
        Prompt = prompt;
        Options = options;
        SelectedIndex = 0;
    }

    public void DisplayOptions()
    {
        Console.WriteLine(Prompt);
        for (int i = 0; i < Options.Length; i++)
        {
            string currentOption = Options[i];
            string prefix;

            if (i == SelectedIndex)
            {
                prefix = ">";
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                prefix = " ";
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine($"{prefix} {currentOption}");
        }
        Console.ResetColor();
    }

    public int Run()
    {
        ConsoleKey keypressed;
        do
        {
            Console.Clear();
            DisplayOptions();
            
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keypressed = keyInfo.Key;

            if (keypressed == ConsoleKey.UpArrow)
            {
                SelectedIndex--;
                if (SelectedIndex < 0)
                    SelectedIndex = Options.Length - 1;
            }
            else if (keypressed == ConsoleKey.DownArrow)
            {
                SelectedIndex++;
                if (SelectedIndex >= Options.Length)
                    SelectedIndex = 0;
            }

        } while (keypressed != ConsoleKey.Enter);
        return SelectedIndex;
    }
}