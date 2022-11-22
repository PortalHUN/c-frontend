using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_frontend.Menu
{
  internal static class Menus
  {
    public static void MainMenu()
    {
      //See if user is already connected to server
      //If yes, bring out the login and registration menupoint.

      List<string> points = new List<string>();
      points.Add("Play");
      points.Add("Connect to a server");
      points.Add("Quit");
      string chosen = Input.Choice(points);
      if (chosen == points[0]) return;
      else if (chosen == points[1]) Client.Connect();
      else if (chosen == points[2]) Environment.Exit(0);
    }
  }
}
