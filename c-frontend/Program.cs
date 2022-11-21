using System;
using System.Collections.Generic;
using System.Threading;
using c_frontend.Controllers;
using c_frontend.Menu;
using RestSharp;

namespace c_frontend
{
  internal class Program
  {
    public static string configPath = "config.txt";
    public static string userPath = "user.txt";
    static void Main(string[] args)
    {
      Console.WindowWidth = 120;
      Console.WindowHeight = 33;

      MenuFramework MainMenu = new MenuFramework(new List<dynamic>()
      {
        new MenuItemFunction("Function hello!", Hello),
        new MenuItemChoose("Class", new List<MenuChoice>()
        {
          new MenuChoice("Mage", Hello, ""),
          new MenuChoice("Scout", Hello)
        }),
        new MenuItemInput("Username")
      }
      , "Main Menu"); 
      MainMenu.OpenMenu();
      Console.ReadKey();
    }
    public static void Hello()
    {
      Console.WriteLine($"Hello!");
    }
  }
}
