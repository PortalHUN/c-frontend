using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_frontend.Menu
{
	internal static class Input
	{

		public static string Text(string text, string def)
		{
			Console.Write($"\n | \n | [T] {text}\n | ");
			string val = Console.ReadLine().Trim() ;
			Debug.WriteLine($"'{text}': {val}");
			return val;
		}

		public static int Number(string text, bool wrong = false)
		{
			Console.Write($" | \n | [N] {text}\n | {(wrong? "Must be a number.\n | ": "")}");
			try
			{
				int val = Convert.ToInt32(Console.ReadLine().Trim()); 
				Debug.WriteLine($"'{text}': {val}");
				return val;
			}
			catch
			{
				Debug.WriteLine($"'{text}': Wrong type");
				return Number(text, true);
			}
		}

		public static string Http(string text, string def = null, bool wrong = false)
		{
			Console.Write($" | \n | [HTTP] {text}\n | {(wrong ? "Wrong format. [HTTP/HTTPS]\n | " : "")}");
			string val = Console.ReadLine().Trim().ToLower();
			if (val == "" && def!=null) val = def;
			if (val == "http" || val == "https") return val;
			else
			{
				Debug.WriteLine($"'{text}': Wrong format");
				return Http(text, def, true);
			}
		}

		public static string IP(string text, string def = null, bool wrong=false)
		{
			Console.Write($" | \n | [IP] {text}\n | {(wrong ? "Wrong format. [e.g. 127.0.0.1]\n | " : "")}");
			try
			{
				string val = Console.ReadLine().Trim();
				if (val == "" && def != null) val = def;
				string[] tmp = val.Split('.');
				Convert.ToInt32(tmp[0]);
				Convert.ToInt32(tmp[1]);
				Convert.ToInt32(tmp[2]);
				Convert.ToInt32(tmp[3]);
				return val;
			}
			catch
			{
				Debug.WriteLine($"'{text}': Wrong format");
				return IP (text, def, true);
			}
		}

	}
}
