using Riptide;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{


    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
  
    public ushort Id { get; private set; }

    public string Username { get; private set; }

    Player (string username, ushort id)
    {
        this.Username = username;
        this.Id = id;
    }
    public static void AddUsername(ushort fromClienId, string username,ushort id)
    {
        list.Add(fromClienId, new Player(username, id));
    }

    [MessageHandler((ushort)ClientToServerID.name)]
    static void Name(ushort fromClienId, Message message)
    {
        Debug.Log("Add username to dictionary");
        AddUsername(fromClienId, message.GetString(),message.GetUShort());
    }

    [MessageHandler((ushort)(ClientToServerID.JoinQueue))]
    private static void AddtoQueue (ushort fromClienId,Message message)
    {
         ushort index =IndexOfDico(list, message.GetUShort());
        //ajout a la queue pour attendre sa partie 
    }

    public static ushort IndexOfDico(Dictionary<ushort, Player> list,ushort id)
    {
        ushort index = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list.Values.ElementAt(i).Id == id)
            {
                index =  (ushort)i;
                return index;
            }
        }
        return index;
    }
}
