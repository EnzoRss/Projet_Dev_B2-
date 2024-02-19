using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
  
    public ushort Id { get; private set; }
    public string Username { get; private set; }

    [MessageHandler((ushort)ClientToServerID.name)]
    static void Name(ushort fromeClienId, Message message)
    {
        Debug.Log("le message quoi");
        AddtoQueue(fromeClienId, message.GetString());
    }

    private static void AddtoQueue (ushort fromeClienId,string username)
    {
        Debug.Log(username + "join the queue");

    }
}
