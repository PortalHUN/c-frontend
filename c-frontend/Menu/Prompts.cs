using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_frontend.Menu
{
  internal static class Prompts
  {
    public static string PromptUsername(string help = null)
    {
      Console.Clear();
      if (help != null) Console.WriteLine(help);
      Console.WriteLine("Username: ");
      string uname = Console.ReadLine().Trim();
      if (uname.Length > 2 && uname.Length < 17) return uname;
      else return PromptPassword("Must:\n -Between 3 and 16 characters\n");
    }

    public static string PromptEmail(string help = null)
    {
      Console.Clear();
      if (help != null) Console.WriteLine(help);
      Console.WriteLine("Email: ");
      string email = Console.ReadLine().Trim();
      if (email.Contains('@') && email.Contains('.')) return email;
      else return PromptPassword("Form:\n -example@example.com\n"); 

    }

    public static string PromptPassword(string help = null)
    {
      Console.Clear();
      if (help != null) Console.WriteLine(help);
      Console.WriteLine("Password: ");
      string password = Console.ReadLine();
      if (password.Length > 7 && password.Length < 17) return password;
      else return PromptPassword("Must:\n -Lowercase Character\n -Uppercase Character\n -8 Characters long\n");
    }
  }
}
