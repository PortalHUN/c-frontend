using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RestSharp;
using System.Threading;

namespace c_frontend
{
  internal static class Config
  {
    private static string IP { get; set; }
    private static string HTTPS { get; set; }
    private static string PORT { get; set; }
    public static string APIString { get; set; }

    public static void CheckConfig(string configPath)
    {
      if (!File.Exists(configPath)) CreateConfig(configPath);

    }

    private static void CreateConfig(string configPath)
    {
      AskServer();

    }

    private static void AskServer()
    {
      Console.Clear();
      GetHttps();
      GetIP();
      GetPort();
      APIString = $"{HTTPS}{IP}:{PORT}";
      TestConnection();
    }

    private static void GetHttps()
    {
      Console.Clear();
      Console.WriteLine("Whats the protocol? [http / https]");
      string http = Console.ReadLine();
      http = http.ToLower();
      if (http == "http") HTTPS = "http://";
      else if (http == "https") HTTPS = "https://";
      else GetHttps();
    }

    private static void GetIP()
    {
      Console.Clear();
      Console.WriteLine("Whats the server ip? [e.g. 192.168.1.2]");
      string ip = Console.ReadLine();
      ip = ip.ToLower();
      string[] tmp = ip.Split('.');
      if (tmp.Length != 4) GetIP();
      else try
        {
          Convert.ToInt32(tmp[0]);
          Convert.ToInt32(tmp[1]);
          Convert.ToInt32(tmp[2]);
          Convert.ToInt32(tmp[3]);

          IP = ip;
        }
        catch (Exception)
        {
          GetIP();
        }
    }

    private static void GetPort()
    {
      Console.Clear();
      Console.WriteLine("Whats the server port? [e.g. 3000]");
      string port = Console.ReadLine();
      try
      {
        Convert.ToInt32(port);

        PORT = port;
      }
      catch (Exception)
      {
        GetPort();
      }
    }

    public static void TestConnection()
    {
      Console.Clear();
      RestClient client = new RestClient(APIString);
      RestRequest req = new RestRequest("/");
      CancellationToken token;
      var res = client.GetAsync<Connection>(req, token).Result;
      
      if (res.code == 957 && res.message == "OK")
      {
        Console.WriteLine("Connected to the server...");
        Thread.Sleep(500);
      }
      else
      {
        Console.WriteLine("Connection error. Try again...");
        Thread.Sleep(1000);
        AskServer();
      }
      
    }

  }
  public class Connection
  {
    public int code { get; set; }
    public string message { get; set; }
  }
}
