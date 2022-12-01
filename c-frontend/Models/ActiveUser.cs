using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_frontend.Models
{
  internal class ActiveUser
  {
    public int ID { get; set; }
    public string Username { get;  set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get;  set; }

    public ActiveUser()
    {
      ID = -1;
      Username = null;
      AccessToken = null;
      RefreshToken = null;
    }
  }
}
