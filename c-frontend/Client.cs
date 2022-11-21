using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using c_frontend;

namespace c_frontend
{
  internal static class Client
  {

    /// <summary>
    /// Handles client server communications
    /// For now it is checking existing config, if it does not exists yet, it asks the user and creates one.
    /// </summary>

    public static RestClient client { get; private set; }
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

    private static void SaveConnection()
    {
      /// <summary>
      /// Saving connection to a file.
      /// </summary>


      Serializer s = new Serializer() { ip = IP, https = HTTPS, port = PORT };
      string json = JsonConvert.SerializeObject(s);
      File.WriteAllText(ConfigPath, json);
    }

    private static void AskServer()
    {
      /// <summary>
      /// Handling the process of asking the user about the server.
      /// </summary>

      Console.Clear();
      InpHttps();
      InpIP();
      InpPort();
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

    private static void InpHttps()
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
      else InpHttps();
      RefreshAPIString();
    }

    private static void InpIP()
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
      if (tmp.Length != 4) InpIP();
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
          InpIP();
        }
      RefreshAPIString();
    }

    private static void InpPort()
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
        InpPort();
      }
      RefreshAPIString();
    }

    private static void RefreshAPIString()
    {
      /// <summary>
      /// Refreshing API String for easier usage
      /// </summary>

      APIString = $"{HTTPS}{IP}:{PORT}";
      client = new RestClient(APIString);
    }

    public static int TestConnection()
    {
      /// <summary>
      /// Testing Connection.
      /// </summary>

      Console.Clear();
      if (APIString == null || APIString == "") return -1;
      RestRequest req = new RestRequest("/");
      CancellationToken token;
      try
      {
        RestResponse res = client.GetAsync(req, token).Result;
        Debug.WriteLine($"Connection testing: '/' '{res.StatusCode.GetHashCode()}' {res.Content}");
        if (res.StatusCode == HttpStatusCode.OK) return 1;
        else return -1;
      } catch
      {
        return -1;
      }


    }

    public static FResponse Request(string serverPath, Method method=Method.Get, object payload = null, List<KeyValuePair> headers = null, List<KeyValuePair> queryParams = null)
    {
      //Because we use FResponse class you need to deserialize the content after using this Function

      if (serverPath == "" || payload == null) return new FResponse(400, "Missing Syntax.");
      RestRequest req = new RestRequest(serverPath, method);
      CancellationToken token;

      //Adding headers to the request
      if(headers != null)
        for (int i = 0; i < headers.Count; i++)
          req.AddHeader(headers[i].name, headers[i].value);

      //Adding Query Params
      if(queryParams != null)
        for (int i = 0; i < queryParams.Count; i++)
          req.AddQueryParameter(queryParams[i].name, queryParams[i].value);

      //Adding JSON Body
      req.AddJsonBody(payload);

      try
      {
        RestResponse res = client.ExecuteAsync(req, token).Result;
        Debug.WriteLine($"{method.ToString().ToUpper()}: '{serverPath}' '{res.StatusCode.GetHashCode()}' {res.Content}");
        return new FResponse(res.StatusCode.GetHashCode(), res.Content);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.ToString());
        return new FResponse(500, "Couldn't react server.");
      }
    }
  }

  public class KeyValuePair
  {
    public string name;
    public string value;

    public KeyValuePair(string name, string value)
    {
      this.name = name;
      this.value = value;
    }
  }

  public class FResponse
  {
    //Frontend Response
    public int code;
    public string content;

    public FResponse(int code, string message)
    {
      this.code = code;
      this.content = message;
    }
  }

  public class Serializer
  {
    public string https { get; set; }
    public string ip { get; set; }
    public string port { get; set; }
  }
}
