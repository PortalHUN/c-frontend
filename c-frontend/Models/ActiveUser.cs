using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_frontend.Models
{
  internal class ActiveUser
  {
    public int ID { get; private set; }
    public string Username { get; private set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; private set; }

    public ActiveUser()
    {
      ID = -1;
      Username = null;
      AccessToken = null;
      RefreshToken = null;
    }

    public void SetRefreshToken(string str)
		{
      if (RefreshToken == null) RefreshToken = str;
		}

    public void SetUsername(string str)
    {
      if (Username == null) Username = str;
    }

    public void SetID(int str)
    {
      if(ID == -1) ID = str;
    }

    public void DestroyIdentity()
    {
      ID = -1;
      Username = null;
      AccessToken = null;
      RefreshToken = null;
    }

  }
}
