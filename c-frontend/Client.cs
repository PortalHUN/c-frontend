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
using Newtonsoft.Json.Linq;
using c_frontend.Controllers;

namespace c_frontend
{
  internal static class Client
  {
    public static string configPath = "client.conn";

    /// <summary>
    /// Handles client server communications
    /// For now it is checking existing config, if it does not exists yet, it asks the user and creates one.
    /// </summary>

    public static RestClient client { get; private set; }
    public static bool connected { get; private set; }
    private static string HTTPS { get; set; }
    private static string IP { get; set; }
    private static string PORT { get; set; }

    public static Cookie cookie = new Cookie();


    public static void Disconnect()
		{
      connected = false;
      Menus.MainMenu();
		}

    public static void Connect()
    {
      List<string> points = new List<string>()
      {
        "Read existing connection",
        "Use new connection",
      };
      string val = Input.Choice(points);

      if(val == points[0]) ReadConnection();
      else if(val == points[1]) Configure();


      void ReadConnection()
      {
        Console.Clear();
        if (!File.Exists(configPath))
        {
          Console.Clear();
          Output.SpacedWrite("Configuration file not found.\n | Returning to the main menu...");
          Thread.Sleep(1000);
          Menus.MainMenu();
          return;
        }
        
        Serializer json = JsonConvert.DeserializeObject<Serializer>(File.ReadAllText(configPath));
        HTTPS = json.https;
        IP = json.ip;
        PORT = json.port;
        client = new RestClient($"{HTTPS}{IP}:{PORT}");
        FResponse res = Request("/", Method.Get);
        if (res.code != 200)
        {
          Output.SpacedWrite($"{res.code} - {res.content}");
          bool wrong = Input.YesNoQuestion("Do you want to reconfigure?", true);
          if (wrong) Configure();
          else
          {
            Console.Clear();
            Output.SpacedWrite("Returning to the main menu...");
            Thread.Sleep(1000);
            Menus.MainMenu();
          }
          return;
        }
        connected = true;
        Console.Clear();
        Output.SpacedWrite("Successfully connected to the server.\n | Returning to the main menu...");
        Thread.Sleep(1000);
        Menus.MainMenu();
      }

      void Configure()
      {
        Console.Clear();
        HTTPS = Input.Http("Server connection: HTTP/HTTPS", "http") + "://";
        IP = Input.IP("Server connection: IP", "127.0.0.1");
        PORT = Convert.ToString(Input.Number("Server connection: Port"));
        client = new RestClient($"{HTTPS}{IP}:{PORT}");
        FResponse res = Request("/", Method.Get);
        if (res.code != 200) 
        {
          Output.SpacedWrite($"No server connection.");
          bool wrong = Input.YesNoQuestion("Do you want to reconfigure?", true);
          if (wrong) Configure();
          else
          {
            Console.Clear();
            Output.SpacedWrite("Returning to the main menu...");
            Thread.Sleep(1000);
            Menus.MainMenu();
          }
          return;
        }
        connected = true;
        bool save = Input.YesNoQuestion("Do you want to save this configuration?", true);
        if (save) SaveConfiguration();
        else Menus.MainMenu();
      }

      void SaveConfiguration()
      {
        Serializer s = new Serializer() { ip = IP, https = HTTPS, port = PORT };
        string json = JsonConvert.SerializeObject(s);
        File.WriteAllText(configPath, json);
        Console.Clear();
        Output.SpacedWrite("Successfully saved the server.\n | Returning to the main menu...");
        Menus.MainMenu();
      }
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

      //Adding access token to header
      if (UserController.GetAccessToken() != null) req.AddHeader("Authorization", "Bearer "+UserController.GetAccessToken());

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
        if (res.Cookies.Count != 0)
				{
          if (res.Cookies[0].Name == "jwt") cookie = res.Cookies[0];
          Debug.WriteLine(res.Cookies[0]); //Refresh Token
        }
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
