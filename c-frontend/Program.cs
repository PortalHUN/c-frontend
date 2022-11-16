using System;
using System.Threading.Tasks;
using RestSharp;

namespace c_frontend
{
  internal class Program
  {
    public static string configPath = "config.txt";
    static void Main(string[] args)
    {
      Config.CheckConfig(configPath);
    }
  }
}
