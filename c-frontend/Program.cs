using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using c_frontend.Controllers;
using c_frontend.Menu;
using RestSharp;

namespace c_frontend
{
  internal class Program
  {
    public static string userPath = "user.txt";
    static void Main(string[] args)
    {
      Menus.MainMenu();
    }
  }
}
