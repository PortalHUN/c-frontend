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
      user.SetUsername(Prompts.PromptUsername());
      string Email = Prompts.PromptEmail();
      string Password = Prompts.PromptPassword();
      Debug.WriteLine($"Username: {user.Username}, Email: {Email}");

      //request
      FResponse res = Client.Request("/auth/register", Method.Post, new {user.Username, Email, Password});
      if(res.code == 200)
      {
        Console.WriteLine(res.content);
        Debug.WriteLine($"UserController.Registration(): {res.content}");
        Thread.Sleep(1000);
      }
      else
      {
        Console.WriteLine(res.content);
        Debug.WriteLine($"PostUser(): {res.content}");
        Thread.Sleep(1000);
        Registration();
      }
    }

    
  }

  
}
