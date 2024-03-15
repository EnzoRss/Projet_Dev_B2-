using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public NetworkManager NetworkManager;


    [MessageHandler((ushort)ClientToServerID.GameStart)]
    private static void StartGame(ushort clientToServerID,Message message)
    {
        Debug.Log("la partie commence");
    }

}
