using System.Collections.Generic;
using System.Linq;
using Riptide;
using UnityEngine;

public  class Players 
{
    public ushort Id { get; private set; }

    public  string Username { get; private set; }

   public Players (string username, ushort id)
    {
        this.Username = username;
        this.Id = id;
    }

}
