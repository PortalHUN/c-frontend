using c_frontend.Controllers;
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

      //Points of the menu
      List<string> points = new List<string>();
      points.Add("Play");
      if(!Client.connected) points.Add("Connect to a server");
      else
			{
        points.Add("Register");
        points.Add("Login");
        points.Add("Disconnect from the server");
			}
      points.Add("Quit");

      //Handle
      string chosen = Input.Choice(points);
      if (chosen == "Play") return;
      else if (chosen == "Connect to a server") Client.Connect();
      else if (chosen == "Register") UserController.Registration();
      else if (chosen == "Login") UserController.Login();
      else if (chosen == "Disconnect from the server") Client.Disconnect();
      else if (chosen == "Quit") Environment.Exit(0);
    }
  }
}
