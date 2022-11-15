using System;
using RestSharp;

namespace c_frontend
{
  internal class Program
  {
    static void Main(string[] args)
    {
      string url = "http://localhost:3000/";
      getAPI(url);

      Console.ReadKey();
    }

    public static void getAPI(string url)
    {
      RestClient client = new RestClient();
      RestRequest request = new RestRequest(url);
      Console.WriteLine(client.Get(request).Content);
    }

    class Post
    {
      public string Title { get; set; }
      public string Body { get; set; }
      public int UserId { get; set; }
    }
  }
}
