using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Riptide;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ServerManager : MonoBehaviour
{
    public static int NbPlayerOnServ = 0;
    public static int NbPlayerOnSearch =0 ;
    public static int NbPlayerInGame = 0;

    public static Dictionary<ushort, Players> ListPlayerConnected = new Dictionary<ushort, Players>();
    public static Dictionary<ushort,Players> ListPlayerSearch = new Dictionary<ushort, Players>();
    public static Dictionary<ushort, Players> ListPlayerInGame = new Dictionary<ushort, Players>();

    public GameObject GO_NetworkManager;
    private NetworkManager NetworkManager;


    private void Start()
    {
       NetworkManager =  GO_NetworkManager.GetComponent<NetworkManager>();
    }

    [MessageHandler((ushort)ClientToServerID.name)]
    private static void Name(ushort fromClienId, Message message)
    {
        Debug.Log("Add username to dictionary");
        AddUsername(fromClienId, message.GetString(), message.GetUShort());
        NbPlayerOnServ++;
        Debug.Log("il y a : " + NbPlayerOnServ + " personne  sur le serveur");
    }

    [MessageHandler((ushort)(ClientToServerID.JoinQueue))]
    private static void AddtoQueue(ushort fromClienId, Message message)
    {
        ushort index = IndexOfDico(ListPlayerConnected, message.GetUShort());
        ListPlayerSearch.Add(ListPlayerConnected.Keys.ElementAt(index), ListPlayerConnected.Values.ElementAt(index));
        NbPlayerOnSearch++;
        Debug.Log("il y a : " + NbPlayerOnSearch + " personne sur la recherche de partie ");
    }

    public static void AddUsername(ushort fromClienId, string username, ushort id)
    {
        ListPlayerConnected.Add(fromClienId, new Players(username, id));
    }

    public static ushort IndexOfDico(Dictionary<ushort, Players> list, ushort id)
    {
        ushort index = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list.Values.ElementAt(i).Id == id)
            {
                index = (ushort)i;
                return index;
            }
        }
        return index;
    }

    private void FixedUpdate()
    {
        if(NbPlayerOnSearch % 2 == 0 && NbPlayerOnSearch != 0)
        {
            Debug.Log("y'a 2 personne");
            List<Players> Listplayer = new List<Players>();
            List<ushort>  ListId = new List<ushort>();
            for (int i = 0;i < 2;i++) 
            {
                Listplayer.Add(ListPlayerSearch.Values.ElementAt(0));
                ListId.Add(ListPlayerSearch.Keys.ElementAt(0));
                ListPlayerSearch.Remove(0);
            }
            StartGame(ListId,Listplayer);
        }

    }

    public  void StartGame(List<ushort> ListId, List<Players> ListPlayer)
    {
        for(int i = 0;i < 2; i++)
        {
            Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.StartGame);
            NetworkManager.server.Send(messsage, ListId.ElementAt(i));
        }
        
        Debug.Log("y'a 2 persone dans la game ");
    }

    public static void StopGame() { }

}

