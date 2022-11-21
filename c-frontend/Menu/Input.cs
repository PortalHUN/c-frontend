using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_frontend.Menu
{
	internal static class Input
	{
    /// RULES
    /// ALWAYS PUT LINE BREAK AT THE END OF THE LINE, EVEN AFTER VALUE READING!!!

		public static string Text(string text, string def = null)
		{
      Console.Write(
        $" | \n" +
        $" | [Text] {text} \n" +
        $" | "
        );
      string val = Console.ReadLine().Trim() ;
			Console.Write('\n');
			Debug.WriteLine($"'{text}': {val}");
			return val;
		}

		public static int Number(string text, bool wrong = false)
		{
      Console.Write(
        $" | \n" +
        $" | [Number] {text}\n" +
        $"{(wrong ? " | Must be a number.\n" : "")}" +
        $" | "
        );
      try
			{
				int val = Convert.ToInt32(Console.ReadLine().Trim());
        Console.Write('\n');
        Debug.WriteLine($"'{text}': {val}");
				return val;
			}
			catch
			{
				Debug.WriteLine($"'{text}': Wrong type");
				return Number(text, true);
			}
		}

		public static string Http(string text, string def = null, bool wrong = false)
		{
      Console.Write(
        $" | \n" +
        $" | [HTTP] {text}\n" +
        $"{(wrong ? " | Wrong format. [HTTP/HTTPS]\n" : "")}" +
        $" | "
        );
      string val = Console.ReadLine().Trim().ToLower();
      Console.Write('\n');
      if (val == "" && def!=null) val = def;
			if (val == "http" || val == "https")
			{
        Debug.WriteLine($"'{text}': {val}");
        return val;
			}
			else
			{
				Debug.WriteLine($"'{text}': Wrong format");
				return Http(text, def, true);
			}
		}

		public static string IP(string text, string def = null, bool wrong=false)
		{
      Console.Write(
        $" | \n" +
        $" | [IP] {text}\n" +
        $"{(wrong ? " | Wrong format. [e.g. 127.0.0.1]\n" : "")}" +
        $" | "
        );
      try
			{
				string val = Console.ReadLine().Trim();
        Console.Write('\n');
        if (val == "" && def != null) val = def;
				string[] tmp = val.Split('.');
				Convert.ToInt32(tmp[0]);
				Convert.ToInt32(tmp[1]);
				Convert.ToInt32(tmp[2]);
				Convert.ToInt32(tmp[3]);
        Debug.WriteLine($"'{text}': {val}");
        return val;
			}
			catch
			{
				Debug.WriteLine($"'{text}': Wrong format");
				return IP (text, def, true);
			}
		}

		public static string Username(string text, bool wrong = false)
		{
      Console.Write(
        $" | \n" +
        $" | [Username] {text}\n" +
        $"{(wrong ? " | Must be longer than 3 characters and shorter than 25.\n" : "")}" +
        $" | "
        );
      string val = Console.ReadLine().Trim();
      //Console.Write('\n');
      if (val.Length < 3 || val.Length > 25)
			{
        Debug.WriteLine($"'{text}': Wrong format.");
        return Username(text, true);
			}
			else
			{
        Debug.WriteLine($"'{text}': {val}");
				return val;
      }
    }

		public static string Email(string text, bool wrong = false)
		{
      Console.Write(
        $" | \n" +
        $" | [Email] {text}\n" +
        $"{(wrong ? " | Must contain '@' and '.' character.\n" : "")}" +
        $" | "
        );
      string val = Console.ReadLine().Trim();
      //Console.Write('\n');
      if (!val.Contains('@') && !val.Contains('.'))
			{
				Debug.WriteLine($"'{text}': Wrong format.");
				return Email(text, true);
			}
			else 
			{
        Debug.WriteLine($"'{text}': {val}");
        return val; 
			}
    }

    public static string Password(string text, bool wrong = false)
		{
      Console.Write(
				$" | \n" +
        $" | [Password] {text}\n" +
        $"{(wrong?" | Must be atleast 8 characters long.\n":"")}" +
        $" | "
				);
      StringBuilder val = new StringBuilder();
      bool continueReading = true;
      char newLineChar = '\r';
      while (continueReading)
      {
        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
        char passwordChar = consoleKeyInfo.KeyChar;

        if (passwordChar == newLineChar)
				{
          continueReading = false;
				}
				else if(consoleKeyInfo.Key == ConsoleKey.Backspace)
				{
					if(val.Length-1!=-1)
						val.Remove(val.Length - 1, 1);
				}
        else
          val.Append(passwordChar.ToString());
      }
			Console.Write('\n');
			if (val.Length < 8)
			{
				Debug.WriteLine($"'{text}': Wrong format.");
				return Password(text, true);
			}
			else
			{
        Debug.WriteLine($"'{text}': {val}");
        return val.ToString();
			}
    }

    public static string Choice(List<string> points)
    {
      ConsoleKey key;
      Console.CursorVisible = false;
      int pointer=0;
      do
      {
        Console.Clear();
        Console.ResetColor();
        Console.Write(
          " | Use the Up and Down arrow to navigate and Enter to select.\n" +
          " | \n"
          );
        for (int i = 0; i < points.Count; i++)
        {
          Console.ResetColor();
          if (i == pointer) Console.ForegroundColor = ConsoleColor.Red;
          Console.Write($" | {points[i]}\n");
        }
        Console.ResetColor();
        Console.Write(" | \n");
        key = Console.ReadKey(true).Key;

        HandleKeys();
      } while (key!=ConsoleKey.Enter);

      Console.CursorVisible = true;
      Console.Clear();
      return points[pointer];

      void HandleKeys()
      {
        switch (key)
        {
          case ConsoleKey.UpArrow:
            if (pointer > 0) pointer--;
            break;

          case ConsoleKey.DownArrow:
            if (pointer < points.Count-1) pointer++;
            break;

          default:
            break;
        }
      }
    }

    public static bool YesNoQuestion(string text, bool def)
    {
      bool val=false;
      Console.Write(
        $" | \n" +
        $" | [Y/N] {text} [Yes/No (y/n)]\n" +
        $" | "
        );
      string input = Console.ReadLine().Trim().ToLower();
      if (input == "") input = def ? "yes" : "no";
      if (input == "yes" || input == "y") val = true;
      else if (input == "no" || input == "n") val = false;
      return val;
    }
  }
}
