using System;
using System.Threading;
using c_frontend.Controllers;
using RestSharp;

namespace c_frontend
{
  internal class Program
  {
    public static string configPath = "config.txt";
    public static string userPath = "user.txt";
    static void Main(string[] args)
    {
      Client.Connect(configPath);
      UserController.Registration();
    }
  }
}
