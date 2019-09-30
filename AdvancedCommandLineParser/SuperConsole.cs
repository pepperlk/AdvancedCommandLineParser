using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedCommandLineParser
{

    public static class SuperConsole
    {

        public static void Log(string log)
        {
            Console.WriteLine(log);
        }

        public static void NL(int lines = 1)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.WriteLine("");
            }

        }

        public static void Clear(string info = "")
        {
            Console.Clear();

            Console.WriteLine(info);
        }

        internal static void Log(object question)
        {
            throw new NotImplementedException();
        }

        internal static string Read()
        {
            return Console.ReadLine();
        }

        internal static string ReadSecure()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            Console.WriteLine("");

            return pass;
        }

        public static string ToSecure(this string input)
        {
            var os = new List<char>();
            for (int i = 0; i < input.Length; i++)
            {
                os.Add('*');
            }

            return new string(os.ToArray());

        }

        public static string Prompt(string prompt, Func<string, bool> validator = null)
        {
            if (validator == null)
            {

                Log(prompt);
                return Console.ReadLine();
            }
            else
            {



                var valid = false;
                while (!valid)
                {
                    Log(prompt);
                    var value = Console.ReadLine();
                    valid = validator.Invoke(value);
                    if (valid)
                    {
                        return value;
                    }
                }

            }
            return "";
        }

        internal static void Error(string messqge)
        {

            lock (Console.Out)
            {
                var currfg = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Log(messqge);

                Console.ForegroundColor = currfg;
            }

        }

        public static string SecurePrompt(string prompt)
        {
            Log(prompt);
            return ReadSecure();


        }

        public static void LogAll(string content)
        {
            Console.Write(content);
        }


        public static string Menu(string title, params string[] items)
        {
            //Clear();
            Log(title);
            Log("");

            var menu = new Menu(items);
            var menuPainter = new ConsoleMenuPainter(menu);
            menu.SelectedIndex = 0;
            bool done = false;

            do
            {
                menuPainter.Paint(0, 0);

                var keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: menu.MoveUp(); break;
                    case ConsoleKey.DownArrow: menu.MoveDown(); break;
                    case ConsoleKey.Enter: done = true; break;
                }
            }
            while (!done);

            Log("");


            return menu.SelectedOption;
        }

        public static bool Confirm(string question = "Are you sure?")
        {
            bool confirmed = false;
            string Key;
            do
            {


                ConsoleKey response;
                do
                {
                    Console.Write($"{question} [y/n] ");
                    response = Console.ReadKey(false).Key;   // true is intercept key (dont show), false is show
                    if (response != ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                confirmed = response == ConsoleKey.Y;
            } while (!confirmed);
            return confirmed;
        }

    }


    public class Menu
    {
        public Menu(IEnumerable<string> items)
        {
            Items = items.ToArray();
        }


        public IReadOnlyList<string> Items { get; }

        public int SelectedIndex { get; set; } = -1; // nothing selected

        public string SelectedOption => SelectedIndex != -1 ? Items[SelectedIndex] : null;


        public void MoveUp() => SelectedIndex = Math.Max(SelectedIndex - 1, 0);

        public void MoveDown() => SelectedIndex = Math.Min(SelectedIndex + 1, Items.Count - 1);
    }


    // logic for drawing menu list
    public class ConsoleMenuPainter
    {
        readonly Menu menu;
        private readonly int currentline;

        public ConsoleMenuPainter(Menu menu)
        {
            this.menu = menu;
            this.currentline = Console.CursorTop;
        }

        public void Paint(int x, int y)
        {
            y = this.currentline + y;
            for (int i = 0; i < menu.Items.Count; i++)
            {
                Console.SetCursorPosition(x, y + i);

                var indicator = menu.SelectedIndex == i ? " <" : "  ";

                //Console.ForegroundColor = color;
                Console.WriteLine(menu.Items[i] + indicator);
            }
        }
    }


}
