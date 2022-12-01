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
    public static string userPath = "user.u";
    private static ActiveUser user = new ActiveUser();

    //User Reading and Writing in file
    public static void ReadUser()
    {
      if (!File.Exists(userPath))
			{
        Output.SpacedWrite("User file not found!");
        Thread.Sleep(1000);
        Menus.MainMenu();
        return;
      }

      UserSerializer json = JsonConvert.DeserializeObject<UserSerializer>(File.ReadAllText(userPath));
      user.ID = json.ID;
      user.Username = json.Username;
      user.RefreshToken = json.RefreshToken;

      Output.SpacedWrite("User loaded!");
      Thread.Sleep(1000);
      Menus.MainMenu();
    }

    public static void SaveUser(string userPath)
    {
      string json = JsonConvert.SerializeObject(new UserSerializer() {ID=user.ID, Username= user.Username, RefreshToken= user.RefreshToken });
      File.WriteAllText(userPath, json);
    }

    public static string GetAccessToken()
		{
      return user.AccessToken;
		}

    public static void Registration()
    {
      Console.Clear();
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
        user.Username = Username;
			else
			{
        Debug.WriteLine($"{res.code}: {res.content}");
        Output.SpacedWrite(res.content);
        Thread.Sleep(1000);
        Login();
      }
      Menus.MainMenu();
    }

    public static void Login()
		{
      Console.Clear();
      string Username = Input.Username("Whats your Username?");
      string Password = Input.Password("Whats your Password?");

      FResponse res = Client.Request("/auth/login", Method.Post, new { Username, Password });
      Debug.WriteLine($"Login(): {res.content}");
      if(res.code == 200)
			{
        //Implement ID
        var json = JsonConvert.DeserializeObject(res.content);
        user.Username = Username;
        user.AccessToken = null;
			}
			else
			{
        Debug.WriteLine($"{res.code}: {res.content}");
        Output.SpacedWrite(res.content);
        Thread.Sleep(1000);
        Login();
			}
      Menus.MainMenu();
		}

    
  }

  public class UserSerializer
	{
		public int ID { get; set; }
		public string Username { get; set; }
		public string RefreshToken { get; set; }
	}
}
