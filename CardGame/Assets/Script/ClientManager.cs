using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Riptide;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ClientManager : MonoBehaviour
{
    public NetworkManager NetworkManager;
    public HttpManagement httpHandler;
    public PlayerManager PlayerManager;
    public Toggle activeToggle;
    public GameObject Prefab;
    public GameObject ConfimButton;
    public GameObject FirstPlay;
    public ToggleGroup Group;
    public TextMeshProUGUI[] PlayerUI;
    public GameObject GameCanva;
    public GameObject MenuCanva;
    Vector3 drawPoint = new Vector3(-307, -350);

    private void Start()
    {
        NetworkManager.Singleton.Client.MessageReceived += OnClientMessageReceived;
       
    }
    void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        // Vérifier le type de message
        if (e.MessageId == (ushort)ClientToServerID.GameStart)
        {
            // Appeler la méthode pour traiter le message
            StartGame(e.Message);
        }
        else if(e.MessageId == (ushort)ClientToServerID.FirstPlay)
        {
            HandlerFirstAction(e.Message);
        }
        else if (e.MessageId == (ushort)ClientToServerID.InGame)
        {
            HandlerActionInGame(e.Message);
        }
    }


    public void HandlerActionInGame(Message message)
    {

        string name = message.GetString();
        httpHandler.StartGetCard("/SelectData?table=cards&filter={\"card_name\":\"" + name + "\"}");
        int atkTake = message.GetInt();
        int pvEnemy = message.GetInt();
        Debug.Log("atk take start :  " + atkTake);
        for (int i = 0; i < PlayerManager.player.deck.cardOnBoard.Count; i++)
        {
            Debug.Log(i);
            if (atkTake > 0)
            {
                int temp = PlayerManager.player.deck.cardOnBoard.ElementAt(i).pv;
                PlayerManager.player.deck.cardOnBoard.ElementAt(i).pv -= atkTake;
                atkTake -= temp;
            }
            if (PlayerManager.player.deck.cardOnBoard.ElementAt(i).pv <= 0)
            {
                Destroy(PlayerManager.player.deck.cardOnBoardUI.ElementAt(i)) ;
                PlayerManager.player.deck.cardOnBoard.RemoveAt(i);
                
            }
            else
            {
                TextMeshProUGUI[] TextPrefab = PlayerManager.player.deck.cardOnBoardUI.ElementAt(i).GetComponentsInChildren<TextMeshProUGUI>();
                foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
                {
                    if (textMeshProUGUI.name == "PV")
                    {
                        textMeshProUGUI.text = "pv : " + PlayerManager.player.deck.cardOnBoard.ElementAt(i).pv;
                    }
                }
            }
        }
        Debug.Log("atk take final :  " + atkTake);
        if (atkTake > 0)
        {
            PlayerManager.player.pv -= atkTake;
            PlayerUI[1].text = "pv : "+PlayerManager.player.pv.ToString();
            PlayerUI[3].text = "pv : " + pvEnemy;
        }
        if(PlayerManager.player.pv <= 0)
        {
            Looser();
        }
        ConfimButton.SetActive(true);
        
    }

    public void Looser()
    {
        Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.InGame);
        messsageToSend.AddUShort(PlayerManager.player.id);
        messsageToSend.AddString("le joueur est mort");
        NetworkManager.Singleton.Client.Send(messsageToSend);
        GameCanva.SetActive(false);
        MenuCanva.SetActive(true);
        PlayerManager.player.deck.ResetInGame();
        foreach(GameObject card in PlayerManager.player.deck.cardInHandUI)
        {
            Destroy(card);
        }
        foreach (GameObject card in PlayerManager.player.deck.cardOnBoardEnemyUI)
        {
            Destroy(card);
        }
        foreach (GameObject card in PlayerManager.player.deck.cardOnBoardUI)
        {
            Destroy(card);
        }
    }


    public void HandlerFirstAction(Message message)
    {
        string name = message.GetString();
        Debug.Log("nom récupérer : " + name);
        if (httpHandler == null)
        {
            Debug.Log("httphandler est null");
        }
        httpHandler.StartGetCard("/SelectData?table=cards&filter={\"card_name\":\"" + name + "\"}");
        ConfimButton.SetActive(true);
    }


    public void Finishround() 
    {
        int i = PlayerManager.player.deck.AddOnBoard(activeToggle.name);
        Card card = new Card();
        card = PlayerManager.player.deck.DrawCard();
        GameObject prefabInstance = CardToPrefab(card, i);
        PlayerManager.player.deck.cardInHandUI.Add(prefabInstance);
        int atkToSend = GetAtk();
        Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.InGame);
        messsageToSend.AddUShort(PlayerManager.player.id);
        messsageToSend.AddString(activeToggle.name);
        messsageToSend.AddInt(atkToSend);
        messsageToSend.AddInt(PlayerManager.player.pv);
        NetworkManager.Singleton.Client.Send(messsageToSend);
        ConfimButton.SetActive(false);
    }


    public int GetAtk()
    {
        int atkToSend = 0;
        foreach (Card card in PlayerManager.player.deck.cardOnBoard)
        {
            atkToSend = card.atk;
        }
        Debug.Log("atk = " + atkToSend);
        return atkToSend;
    }

    public void StartGame(Message message)
    {
        //PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        Debug.Log("la partie commence");
        int temp = message.GetInt();
        string name = message.GetString(); 
        GameCanva.SetActive(true);
        PlayerUI[0].text = PlayerManager.player.username;
        Debug.Log("pv :  "+PlayerManager.player.pv.ToString());
        PlayerUI[1].text = PlayerManager.player.pv.ToString();
        PlayerUI[2].text = name;
        PlayerUI[3].text = "20";


        for (int i = 0; i < 4; i++)
        {
            Card card = PlayerManager.player.deck.DrawCard();
            GameObject prefabInstance = CardToPrefab(card, i);
            PlayerManager.player.deck.cardInHandUI.Add(prefabInstance);
        }
        Debug.Log("getint : " + temp);
        if (temp == 0)
        {
            FirstPlay.SetActive(true);
        }
    }

    public void Playing()
    {
        int i =PlayerManager.player.deck.AddOnBoard(activeToggle.name);
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.FirstPlay);
        messsage.AddUShort(PlayerManager.player.id);
        messsage.AddString(activeToggle.name);
        NetworkManager.Singleton.Client.Send(messsage);
        Debug.Log("Fin du tour");
        FirstPlay.SetActive(false);
        Card card = new Card();
        card = PlayerManager.player.deck.DrawCard();
        GameObject prefabInstance = CardToPrefab(card, i);
        PlayerManager.player.deck.cardInHandUI.Add(prefabInstance);
    }

    GameObject CardToPrefab(Card card, int i)
    {
        GameObject prefabInstance = Prefab;
        Toggle togglePrefab = prefabInstance.GetComponentInChildren<UnityEngine.UI.Toggle>();

        if (togglePrefab != null)
        {
            togglePrefab.group = Group;
            togglePrefab.name = card.card_name;
            // Ajoutez un écouteur à l'événement onValueChanged du TogglePrefab
            togglePrefab.onValueChanged.AddListener((x)=>{ OnToggleValueChanged(togglePrefab); });
            // Vérifiez si l'écouteur a été ajouté avec succès
            if (togglePrefab.onValueChanged != null && togglePrefab.onValueChanged.GetPersistentEventCount() > 0)
            {
                Debug.Log("AddListener est actif pour le TogglePrefab.");
            }
            else
            {
                Debug.LogWarning("AddListener n'est pas actif pour le TogglePrefab.");
            }
        }

        TextMeshProUGUI[] TextPrefab = prefabInstance.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
        {
            if (textMeshProUGUI.name == "PV")
            {
                textMeshProUGUI.text = "pv : " + card.pv;
            }
            else if (textMeshProUGUI.name == "Name")
            {
                textMeshProUGUI.text = card.card_name;
            }
            else if (textMeshProUGUI.name == "ATK")
            {
                textMeshProUGUI.text = "atk : " + card.atk;
            }
            else if (textMeshProUGUI.name == "Description")
            {
                textMeshProUGUI.text = card.description;
            }
        }

        GameObject Test = Instantiate(prefabInstance, new Vector3(drawPoint.x + 225 * i, drawPoint.y), Quaternion.identity);
        return Test;
    }


    public void JoinQueue()
    {
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.JoinQueue);
        messsage.AddUShort(PlayerManager.player.id);
        NetworkManager.Singleton.Client.Send(messsage);
        Debug.Log("message to join the queue send");
    }


    public void OnToggleValueChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            // Si un nouveau bouton est sélectionné, désélectionner l'ancien (s'il existe)
            if (activeToggle != null && activeToggle != toggle)
            {
                activeToggle.isOn = false;
            }

            activeToggle = toggle;

        }
        else if (activeToggle == toggle)
        {
            // Si le bouton désélectionné était le bouton actif, le marquer comme null
            activeToggle = null;
        }
    }
}
