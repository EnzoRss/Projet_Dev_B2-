using System;
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
    private static NetworkManager NetworkManager;
   


    private void Start()
    {
       NetworkManager =  GO_NetworkManager.GetComponent<NetworkManager>();
    }


    [MessageHandler((ushort)ClientToServerID.FirstPlay)]
    private static void FirstMoove(ushort fromClienId, Message message)
    {
        ushort id = message.GetUShort();
        
        if (ListPlayerInGame.Values.ElementAt(0).Id == id)
        {
            Debug.Log("message viens de " + ListPlayerInGame.Values.ElementAt(0).Username);
            Debug.Log("Messsage envoyer a : " + ListPlayerInGame.Keys.ElementAt(1));
            string name = message.GetString();
            Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.FirstPlay);
            messsageToSend.AddString(name);
            NetworkManager.server.Send(messsageToSend, ListPlayerInGame.Keys.ElementAt(1));
        } else
        {
            Debug.Log("message viens de " + ListPlayerInGame.Values.ElementAt(1).Username);
            Debug.Log("Messsage envoyer a : " + ListPlayerInGame.Keys.ElementAt(0));
            string name = message.GetString();
            Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.FirstPlay);
            messsageToSend.AddString(name);
            NetworkManager.server.Send(messsageToSend, ListPlayerInGame.Keys.ElementAt(0));
        }
    }

    [MessageHandler((ushort)ClientToServerID.InGame)]
    private static void ClientToClient(ushort fromClienId, Message message)
    {
        ushort id = message.GetUShort();
        string name = message.GetString();
        int atk = message.GetInt();
        int pv = message.GetInt();
        Debug.Log("name : "+name);

        Message messageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.InGame);
        messageToSend.AddString(name);
        messageToSend.AddInt(atk);
        messageToSend.AddInt(pv);
        

        if (ListPlayerInGame.Values.ElementAt(0).Id == id)
        {
            Debug.Log("message ClientToClient viens de  " + ListPlayerInGame.Values.ElementAt(0).Username);
            Debug.Log("Messsage ClientToClient  envoyer a : " + ListPlayerInGame.Keys.ElementAt(1));
            NetworkManager.server.Send(messageToSend, ListPlayerInGame.Keys.ElementAt(1));
        }
        else 
        {
            Debug.Log("message ClientToClient viens de " + ListPlayerInGame.Values.ElementAt(1).Username);
            Debug.Log("Messsage ClientToClient envoyer a : " + ListPlayerInGame.Keys.ElementAt(0));
            NetworkManager.server.Send(messageToSend, ListPlayerInGame.Keys.ElementAt(0));
        }
    }

    [MessageHandler((ushort)ClientToServerID.EndGame)]
    private static void EndGame(ushort fromClienId, Message message)
    {
        ushort id = message.GetUShort();
        string str = "partie finie";
        for (int i = 0; i < 2; i++)
        {
            Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.EndGame);
            message.AddString(str);
            NetworkManager.server.Send(messsage, ListPlayerInGame.Keys.ElementAt(i));
        }
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
        Debug.Log(ListPlayerConnected.Values.ElementAt(index).Username);
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
                Debug.Log("add to list : " + ListPlayerSearch.Values.ElementAt(i).Username);
                Listplayer.Add(ListPlayerSearch.Values.ElementAt(i));
                ListId.Add(ListPlayerSearch.Keys.ElementAt(i));
                ListPlayerInGame.Add(ListPlayerSearch.Keys.ElementAt(i),ListPlayerSearch.Values.ElementAt(i));
                ListPlayerSearch.Remove((ushort)i);

                NbPlayerOnSearch--;
            }
            StartGame(ListId,Listplayer);
        }

    }

    public  void StartGame(List<ushort> ListId, List<Players> ListPlayer)
    {
        foreach (var player in ListPlayer)
        {
            Debug.Log("message début de partie envoyer au joueur " + player.Username);

        }
        for(int i = 0;i < 2; i++)
        {
            Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.StartGame);
            messsage.AddInt(i);
            if (i == 0) 
            { 
                messsage.AddString(ListPlayer.ElementAt(i + 1).Username); 
            }
            else
            {
                messsage.AddString(ListPlayer.ElementAt(i - 1).Username);
            }
           
            NetworkManager.server.Send(messsage, ListId.ElementAt(i));
            Debug.Log("message début de partie envoyer au joueur " + ListPlayer.ElementAt(i).Username);
        }
        
        Debug.Log("y'a 2 persone dans la game ");
    }

    public static void StopGame() { }

}

