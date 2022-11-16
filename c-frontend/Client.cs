using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Threading;

namespace c_frontend
{
  internal static class Client
  {
    private static string HTTPS { get; set; }
    private static string IP { get; set; }
    private static string PORT { get; set; }
    private static string APIString { get; set; }
    private static string ConfigPath { get; set; }

    public static void CheckConfig(string configPath)
    {
      ConfigPath = configPath;
      if (!File.Exists(configPath)) AskServer();
      else ReadConfig();
    }

    private static void ReadConfig()
    {
      string configContent = File.ReadAllText(ConfigPath);
      Serializer json = JsonConvert.DeserializeObject<Serializer>(configContent);
      HTTPS = json.https;
      IP = json.ip;
      PORT = json.port;
      APIString = json.apiString;
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

    private static void AskServer()
    {
      Console.Clear();
      GetHttps();
      GetIP();
      GetPort();
      APIString = $"{HTTPS}{IP}:{PORT}";
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

    public static int TestConnection()
    {
      Console.Clear();
      if (APIString == null || APIString == "") return 0;
      RestClient client = new RestClient(APIString);
      RestRequest req = new RestRequest("/");
      CancellationToken token;
      Test res = new Test();
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

    public static void SaveConnection()
    {
      Serializer s = new Serializer() { ip = IP, https = HTTPS, port = PORT, apiString = APIString };
      var json = JsonConvert.SerializeObject(s);
      File.WriteAllText(ConfigPath, json);
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
    public string apiString { get; set; }
  }
}
