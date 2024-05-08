using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;
using UnityEngine.UI;


public class ClientManager : MonoBehaviour
{
    public NetworkManager NetworkManager;
    public Player player ;
    

   


    [MessageHandler((ushort)ClientToServerID.GameStart)]
    private static void StartGame(ushort clientToServerID,Message message)
    {
        Debug.Log("la partie commence");
     

    }

}
