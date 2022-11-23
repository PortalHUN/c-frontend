using c_frontend.Menu;
using c_frontend.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace c_frontend.Controllers
{
  internal static class UserController
  {
    private static ActiveUser user = new ActiveUser();

    //User Reading and Writing in file
    public static void ReadUser(string userPath)
    {
      if (!File.Exists(userPath)) Console.WriteLine();
    }

    public static void SaveUser(string userPath)
    {
      string json = JsonConvert.SerializeObject(new {user.ID, user.Username, user.RefreshToken });
      File.WriteAllText(userPath, json);
    }

    public static void Registration()
    {
      //Prompting for informations
      string Username = (Input.Username("Whats your Username?"));
      string Email = Input.Email("Whats your Email?");
      string Password = Input.Password("Whats your Password?");
      Debug.WriteLine($"Username: {Username}, Email: {Email}");

      //request
      FResponse res = Client.Request("/auth/register", Method.Post, new {Username, Email, Password});
      Output.SpacedWrite(res.content);
      Debug.WriteLine($"Registration(): {res.content}");
      Thread.Sleep(1000);
      if (res.code == 200)
        user.SetUsername(Username);
      else
        Registration();
    }

    
  }

  
}
