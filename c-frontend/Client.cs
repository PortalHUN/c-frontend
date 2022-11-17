using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Threading;

namespace c_frontend
{
  internal static class Client
  {

    /// <summary>
    /// Handles client server communications
    /// For now it is checking existing config, if it does not exists yet, it asks the user and creates one.
    /// </summary>

    private static string HTTPS { get; set; }
    private static string IP { get; set; }
    private static string PORT { get; set; }
    private static string APIString { get; set; }
    private static string ConfigPath { get; set; }

    public static void Connect(string configPath)
    {
      /// <summary>
      /// Config file handling
      /// </summary>
 
      ConfigPath = configPath;
      if (!File.Exists(configPath)) AskServer();
      else ReadConnection();
    }

    private static void ReadConnection()
    {
      /// <summary>
      /// Deserializes the config.txt and uses to test and save the connection
      /// between the server.
      /// </summary>
      
      string configContent = File.ReadAllText(ConfigPath);
      Serializer json = JsonConvert.DeserializeObject<Serializer>(configContent);
      HTTPS = json.https;
      IP = json.ip;
      PORT = json.port;
      RefreshAPIString();
      if (TestConnection() == 1)
      {
        Console.WriteLine("Connected to the server...");
        Thread.Sleep(500);
      }
      else
      {
        Console.WriteLine("Connection error... Modify parameters.");
        Thread.Sleep(1000);
        AskServer();
      }
    }

    public static void SaveConnection()
    {
      /// <summary>
      /// Saving connection to a file.
      /// </summary>


      Serializer s = new Serializer() { ip = IP, https = HTTPS, port = PORT };
      var json = JsonConvert.SerializeObject(s);
      File.WriteAllText(ConfigPath, json);
    }

    private static void AskServer()
    {
      /// <summary>
      /// Handling the process of asking the user about the server.
      /// </summary>

      Console.Clear();
      GetHttps();
      GetIP();
      GetPort();
      if (TestConnection() == 1) 
      {
        Console.WriteLine("Connected to the server...");
        Thread.Sleep(500);
      }
      else
      {
        Console.WriteLine("Connection error... Modify parameters.");
        Thread.Sleep(1000);
        AskServer();
      }
      SaveConnection();
    }

    private static void GetHttps()
    {
      /// <summary>
      /// Getting HTTPS
      /// </summary>

      Console.Clear();
      Console.WriteLine("Whats the protocol? [http / https / default: http]");
      string http = Console.ReadLine();
      if (http == "") http = "http";
      http = http.ToLower();
      if (http == "http") HTTPS = "http://";
      else if (http == "https") HTTPS = "https://";
      else GetHttps();
      RefreshAPIString();
    }

    private static void GetIP()
    {
      /// <summary>
      /// Getting IP Address
      /// </summary>

      Console.Clear();
      Console.WriteLine("Whats the server ip? [default: 127.0.0.1]");
      string ip = Console.ReadLine();
      if (ip == "") ip = "127.0.0.1";
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
      RefreshAPIString();
    }

    private static void GetPort()
    {
      /// <summary>
      /// Getting PORT
      /// </summary>

      Console.Clear();
      Console.WriteLine("Whats the server port? [e.g. 3000]");
      string port = Console.ReadLine();
      if (port == "") port = "3000";
      try
      {
        Convert.ToInt32(port);

        PORT = port;
      }
      catch (Exception)
      {
        GetPort();
      }
      RefreshAPIString();
    }

    public static void RefreshAPIString()
    {
      /// <summary>
      /// Refreshing API String for easier usage
      /// </summary>
      
      APIString = $"{HTTPS}{IP}:{PORT}";
    }

    public static int TestConnection()
    {
      /// <summary>
      /// Testing Connection.
      /// </summary>

      Console.Clear();
      if (APIString == null || APIString == "") return 0;
      RestClient client = new RestClient(APIString);
      RestRequest req = new RestRequest("/");
      CancellationToken token;
      var res = new Test();
      try
      {
        res = client.GetAsync<Test>(req, token).Result;
      } catch
      {
        return -1;
      }
      
      if (res.code == 957 && res.message == "OK")
        return 1;
      else
        return -1;
    }
  }
  public class Test
  {
    public int code { get; set; }
    public string message { get; set; }
  }

  public class Serializer
  {
    public string https { get; set; }
    public string ip { get; set; }
    public string port { get; set; }
  }
}
