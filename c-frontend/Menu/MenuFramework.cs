using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_frontend.Menu
{
  internal class MenuFramework
  {
    public string menuTitle { get; set; }
    public int pointerY { get; set; }
    public int menuLength { get; set; }
    public List<dynamic> Items { get; set; }

    public MenuFramework(List<dynamic> items, string menuTitle = null)
    {
      this.menuTitle = " | "+menuTitle;
      Items = items;
      pointerY = 0;
      menuLength = items.Count + menuTitle!=""? 1:0;
    }

    public void OpenMenu()
    {
      Console.CursorVisible = false;
      ConsoleKey key;
      do {
        Console.Clear();
        if (menuTitle != null) Console.WriteLine($" |\n{menuTitle}");
        for (int i = 0; i < Items.Count; i++)
        {
          if (pointerY == i) Console.ForegroundColor = ConsoleColor.Red;
          else Console.ForegroundColor = ConsoleColor.Gray;

          if (Items[i] is MenuItemFunction)
          {
            var item = Items[i];
            Console.WriteLine($"{item.Name}");

          }
          else if (Items[i] is MenuItemChoose)
          {
            var item = Items[i];
            Console.WriteLine($"{item.Name}: <{item.Items[item.ItemPointer].Value}>");

          }
          else if (Items[i] is MenuItemInput)
          {
            var item = Items[i];
            Console.WriteLine($"{item.Name}: '{item.Value}'");

          }
        }
        key = Console.ReadKey().Key;
        HandleKey(key);
      } while (key != ConsoleKey.Escape);
      
    }

    public void HandleKey(ConsoleKey key)
    {
      switch (key)
      {
        case ConsoleKey.UpArrow: pointerY--; break;
        case ConsoleKey.DownArrow: pointerY++; break;

        default:
          break;
      }
    }
  }

  class MenuItemFunction
  {
    public string Name { get; set; }
    public Action Function { get; set; }
    public string executeValue { get; set; }

    public MenuItemFunction(string name, Action function, string executeValue=null)
    {
      Name = " | " + name;
      Function = function; 
      this.executeValue = executeValue;
    }
  }

  class MenuItemChoose
  {
    public string Name { get; set; }
    public List<MenuChoice> Items { get; set; }
    public int ItemPointer { get; set; }
    public int cursorPosition { get; set; }


    public MenuItemChoose(string name, List<MenuChoice> items)
    {
      Name = " | " + name;
      Items = items;
      ItemPointer = 0;
      cursorPosition = Name.Length + 3;
    }
  }

  class MenuChoice
  {
    public string Value { get; set; }
    public Action Function { get; set; }
    public string executeValue { get; set; }

    public MenuChoice(string value, Action function, string executeValue=null)
    {
      Value = value;
      Function = function;
      this.executeValue = executeValue;
    }
  }

  class MenuItemInput
  {
    public string Name { get; set; }
    public string Value { get; set; }
    public int cursorPosition { get; set; }

    public MenuItemInput(string name, string value=null)
    {
      Name= " | " + name;
      Value=value;
      cursorPosition = Name.Length + 3;
    }
  }

}
