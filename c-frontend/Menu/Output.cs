using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_frontend.Menu
{
  internal static class Output
  {
    /// RULES
    /// ALWAYS PUT LINE BREAK AT THE END OF THE LINE, EVEN AFTER VALUE READING!!! <summary>

    public static void SingleWrite(string text)
    {
      Console.Write(
        $" | {text}\n"
        );
    }

    public static void SpacedWrite(string text)
    {
      Console.Write(
        $" | \n" +
        $" | {text}\n" +
        $" | \n"
        );
    }
  }
}
