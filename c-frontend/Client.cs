using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using c_frontend;
using c_frontend.Menu;

namespace c_frontend
{
  internal static class Client
  {
    public static string configPath = "config.txt";

    /// <summary>
    /// Handles client server communications
    /// For now it is checking existing config, if it does not exists yet, it asks the user and creates one.
    /// </summary>

    public static RestClient client { get; private set; }
    private static string HTTPS { get; set; }
    private static string IP { get; set; }
    private static string PORT { get; set; }
    private static string APIString { get; set; }


    public static void Connect()
    {
      //Remodel UserInterface of Connect();
      //Need to read connection with a public function if needed.
      //Remove TestConnection function
      //Save Connection prompt if the user want to save it.
      //Remove and replace RefreshAPIString.
      //Dont touch request syntax otherwise it may not work anymore.
      //bool connected variable.


      

      /*if (!File.Exists(configPath)) AskServer();
      bool b = Input.YesNoQuestion("Do you want to connect to a server?", true);
      if (b) AskServer();
      else */

      /*if (!File.Exists(configPath)) AskServer();
      else ReadConnection();*/
    }

    private static void ReadConnection()
    {
      /// <summary>
      /// Deserializes the config.txt and uses to test and save the connection
      /// between the server.
      /// </summary>
      /*string configContent = File.ReadAllText(ConfigPath);
      Serializer json = JsonConvert.DeserializeObject<Serializer>(configContent);
      HTTPS = json.https;
      IP = json.ip;
      PORT = json.port;
      RefreshAPIString();

      FResponse test = TestConnection();
      if (test.code == 200)
      {
        Console.WriteLine("Connected to the server...");
        Thread.Sleep(500);
      }
      else
      {
        Console.WriteLine($"{test.code}: {test.content} (Connect to a correct server.)");
        Thread.Sleep(1000);
        AskServer();
      }*/
    }

    private static void SaveConnection()
    {
      /// <summary>
      /// Saving connection to a file.
      /// </summary>


      Serializer s = new Serializer() { ip = IP, https = HTTPS, port = PORT };
      string json = JsonConvert.SerializeObject(s);
      File.WriteAllText(configPath, json);
    }

    private static void AskServer()
    {
      /// <summary>
      /// Handling the process of asking the user about the server.
      /// </summary>


      Console.Clear();
      HTTPS = Input.Http("Server connection: HTTP/HTTPS", "http")+"://";
      IP = Input.IP("Server connection: IP", "127.0.0.1");
      PORT = Convert.ToString(Input.Number("Server connection: Port"));
      RefreshAPIString();
      FResponse test = TestConnection();
      if (test.code == 200)
      {
        Console.WriteLine("Connected to the server...");
        Thread.Sleep(500);
      }
      else
      {
        Console.WriteLine($"{test.code}: {test.content}");
        Thread.Sleep(1000);
        AskServer();
      }
      SaveConnection();
    }

    private static void RefreshAPIString()
    {
      /// <summary>
      /// Refreshing API String for easier usage
      /// </summary>

      APIString = $"{HTTPS}{IP}:{PORT}";
      client = new RestClient(APIString);
    }

    public static FResponse TestConnection()
    {
      /// <summary>
      /// Testing Connection.
      /// </summary>

      Console.Clear();
      FResponse res = Request("/", Method.Get);
      return res;

    }

    public static FResponse Request(string serverPath, Method method=Method.Get, object payload = null, List<KeyValuePair> headers = null, List<KeyValuePair> queryParams = null)
    {
      //Because we use FResponse class you need to deserialize the content after using this Function
      if (HTTPS == null || IP == null || PORT == null) return new FResponse(503, "No server connection config.");

      if (serverPath == "") return new FResponse(400, "Missing Syntax.");
      RestRequest req = new RestRequest(serverPath, method);
      CancellationToken token;

      //Adding headers to the request
      if(headers != null)
        for (int i = 0; i < headers.Count; i++)
          req.AddHeader(headers[i].name, headers[i].value);

      //Adding Query Params
      if (queryParams != null)
        for (int i = 0; i < queryParams.Count; i++)
          req.AddQueryParameter(queryParams[i].name, queryParams[i].value);

      //Adding JSON Body
      if(payload != null)
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
