using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        else if( e.MessageId == (ushort)ClientToServerID.EndGame)
        {
            HandlerEndGame(e.Message);
        }
    }


    public void HandlerActionInGame(Message message)
    {

        string name = message.GetString();
        httpHandler.StartGetCard("/SelectData?table=cards&filter={\"card_name\":\"" + name + "\"}");
        int atkTake = message.GetInt();
        int pvEnemy = message.GetInt();
        Debug.Log("atk take start :  " + atkTake);
        atkTake = AtkToCard(PlayerManager.player.deck.cardOnBoardUI, PlayerManager.player.deck.cardOnBoard, atkTake);
        Debug.Log("atk take final :  " + atkTake);
        if (atkTake > 0)
        {
            PlayerManager.player.pv -= atkTake;
            PlayerUI[1].text = "pv : "+PlayerManager.player.pv.ToString();
            
        }
        PlayerUI[3].text = "pv : " + pvEnemy;
        if (PlayerManager.player.pv <= 0)
        {
            Looser();
        }
        ConfimButton.SetActive(true);
        
    }


    public void HandlerEndGame(Message message)
    {
        GameCanva.SetActive(false);
        MenuCanva.SetActive(true);
        
        foreach (GameObject card in PlayerManager.player.deck.cardInHandUI)
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
        PlayerManager.player.deck.ResetInGame();
        httpHandler.StartGetPlayer("/SelectData?table=users&filter={\"username\":\"" + PlayerManager.player.username + "\",\"password\":\"" + PlayerManager.player.password + "\"}"); ;
    }


    public void Looser()
    {
        Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.EndGame);
        messsageToSend.AddUShort(PlayerManager.player.id);
        NetworkManager.Singleton.Client.Send(messsageToSend);
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
        PlayerManager.player.deck.PrintCardUI();
        Card card = new Card();
        card = PlayerManager.player.deck.DrawCard();
        GameObject prefabInstance = CardToPrefab(card, i);
        PlayerManager.player.deck.cardInHandUI.Add(prefabInstance);
        int atkToSend = GetAtk();
        AtkToCard(PlayerManager.player.deck.cardOnBoardEnemyUI, PlayerManager.player.deck.cardOnBoardEnemy,atkToSend); 
        Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.InGame);
        messsageToSend.AddUShort(PlayerManager.player.id);
        messsageToSend.AddString(activeToggle.name);
        messsageToSend.AddInt(atkToSend);
        messsageToSend.AddInt(PlayerManager.player.pv);
        NetworkManager.Singleton.Client.Send(messsageToSend);
        ConfimButton.SetActive(false);
    }

    public int AtkToCard(List<GameObject> cardUI, List<Card> card, int atkTake)
    {

        List<Card> cards = new List<Card>();
        List<GameObject> cardsUI = new List<GameObject>();
        List<int> index = new List<int>();
        foreach (Card carte in card)
        {
            if (atkTake > 0)
            {
                int temp = carte.pv;
                carte.pv -= atkTake;
                atkTake -= temp;
            }
            if (carte.pv <= 0)
            {
                cards.Add(carte);
                foreach (GameObject carteUI in cardUI)
                {
                    TextMeshProUGUI[] TextPrefab = carteUI.GetComponentsInChildren<TextMeshProUGUI>();
                    foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
                    {
                        if (textMeshProUGUI.name == "Name" && textMeshProUGUI.text == carte.card_name)
                        {
                            cardsUI.Add(carteUI);
                        }
                    }
                }
            }
            else
            {
                foreach (GameObject carteUI in cardUI)
                {
                    bool correctcard = false;
                    TextMeshProUGUI[] TextPrefab = carteUI.GetComponentsInChildren<TextMeshProUGUI>();
                    foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
                    {
                        if (textMeshProUGUI.name == "Name" && textMeshProUGUI.text == carte.card_name)
                        {
                            correctcard = true;
                        }

                        if (textMeshProUGUI.name == "PV" && correctcard)
                        {
                            textMeshProUGUI.text = "pv : " + carte.pv;
                        }
                    }
                }
            }
        }

        foreach (Card card1 in cards)
        {
            card.Remove(card1);
        }
        foreach (GameObject card2 in cardsUI)
        {
            cardUI.Remove(card2);
            Destroy(card2);
        }
        return atkTake;
       } 
    


    public int GetAtk()
    {
        int atkToSend = 0;
        foreach (Card card in PlayerManager.player.deck.cardOnBoard)
        {
            Debug.Log("atk de la carte :  " + card.atk);
            atkToSend += card.atk;
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

        if (temp == 0)
        {
            FirstPlay.SetActive(true);
        }
    }

    public void Playing()
    {
        int i = PlayerManager.player.deck.AddOnBoard(activeToggle.name);
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
