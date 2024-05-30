
using System.Collections.Generic;
using Riptide;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public NetworkManager NetworkManager;   // R�f�rence au NetworkManager
    public HttpManagement httpHandler;      // Gestionnaire des requ�tes HTTP
    public PlayerManager PlayerManager;     // Gestionnaire de joueur
    public Toggle activeToggle;             // Toggle actif
    public GameObject Prefab;               // Pr�fab de carte
    public GameObject ConfimButton;         // Bouton de confirmation
    public GameObject FirstPlay;            // �cran de premier tour
    public ToggleGroup Group;               // Groupe de toggles
    public TextMeshProUGUI[] PlayerUI;      // Interface utilisateur du joueur
    public GameObject GameCanva;            // Interface utilisateur du jeu
    public GameObject MenuCanva;            // Interface utilisateur du menu
    Vector3 drawPoint = new Vector3(-307, -350); // Position de dessin des cartes

    private void Start()
    {
        // Ajouter un �couteur pour g�rer les messages re�us par le client
        NetworkManager.Singleton.Client.MessageReceived += OnClientMessageReceived;
    }

    // M�thode pour g�rer les messages re�us par le client
    void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        // V�rifier le type de message et appeler les m�thodes appropri�es pour le traiter
        if (e.MessageId == (ushort)ClientToServerID.GameStart)
        {
            StartGame(e.Message);
        }
        else if (e.MessageId == (ushort)ClientToServerID.FirstPlay)
        {
            HandlerFirstAction(e.Message);
        }
        else if (e.MessageId == (ushort)ClientToServerID.InGame)
        {
            HandlerActionInGame(e.Message);
        }
        else if (e.MessageId == (ushort)ClientToServerID.EndGame)
        {
            HandlerEndGame(e.Message);
        }
    }

    // M�thode pour traiter les actions du joueur en jeu
    public void HandlerActionInGame(Message message)
    {
        // R�cup�rer les informations du message
        string name = message.GetString();
        int atkTake = message.GetInt();
        int pvEnemy = message.GetInt();

        // Appliquer les d�g�ts � la carte du joueur
        atkTake = AtkToCard(PlayerManager.player.deck.cardOnBoardUI, PlayerManager.player.deck.cardOnBoard, atkTake);

        // Mettre � jour l'interface utilisateur
        if (atkTake > 0)
        {
            PlayerManager.player.pv -= atkTake;
            PlayerUI[1].text = "pv : " + PlayerManager.player.pv.ToString();
        }
        PlayerUI[3].text = "pv : " + pvEnemy;

        // V�rifier si le joueur a perdu
        if (PlayerManager.player.pv <= 0)
        {
            Looser(); // M�thode appel�e en cas de d�faite
        }
        ConfimButton.SetActive(true); // Activer le bouton de confirmation
    }

    // M�thode pour traiter la fin de la partie
    public void HandlerEndGame(Message message)
    {
        // Masquer l'interface du jeu et afficher le menu principal
        GameCanva.SetActive(false);
        MenuCanva.SetActive(true);

        // D�truire toutes les cartes affich�es
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

        // R�initialiser les donn�es du jeu
        PlayerManager.player.deck.ResetInGame();

        // R�cup�rer les informations du joueur depuis la base de donn�es
        httpHandler.StartGetPlayer("/SelectData?table=users&filter={\"username\":\"" + PlayerManager.player.username + "\",\"password\":\"" + PlayerManager.player.password + "\"}"); ;
    }

    // M�thode appel�e en cas de d�faite du joueur
    public void Looser()
    {
        // Cr�er un message pour indiquer la fin de la partie
        Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.EndGame);
        messsageToSend.AddUShort(PlayerManager.player.id);
        NetworkManager.Singleton.Client.Send(messsageToSend); // Envoyer le message au serveur
    }

    // M�thode pour traiter la premi�re action du joueur
    public void HandlerFirstAction(Message message)
    {
        // R�cup�rer le nom de la carte depuis le message
        string name = message.GetString();
        Debug.Log("nom r�cup�rer : " + name);

        // Lancer une requ�te HTTP pour r�cup�rer les informations sur la carte
        httpHandler.StartGetCard("/SelectData?table=cards&filter={\"card_name\":\"" + name + "\"}");

        // Activer le bouton de confirmation
        ConfimButton.SetActive(true);
    }

    // M�thode pour terminer le tour du joueur
    public void Finishround()
    {
        // Ajouter la carte sur le plateau de jeu
        int i = PlayerManager.player.deck.AddOnBoard(activeToggle.name);
        PlayerManager.player.deck.PrintCardUI();

        // Piocher une nouvelle carte
        Card card = new Card();
        card = PlayerManager.player.deck.DrawCard();
        GameObject prefabInstance = CardToPrefab(card, i);
        PlayerManager.player.deck.cardInHandUI.Add(prefabInstance);

        // Calculer les d�g�ts � envoyer � l'adversaire
        int atkToSend = GetAtk();

        // Envoyer les informations au serveur
        Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.InGame);
        messsageToSend.AddUShort(PlayerManager.player.id);
        messsageToSend.AddString(activeToggle.name);
        messsageToSend.AddInt(atkToSend);
        messsageToSend.AddInt(PlayerManager.player.pv);
        NetworkManager.Singleton.Client.Send(messsageToSend);

        // D�sactiver le bouton de confirmation
        ConfimButton.SetActive(false);
    }

    // M�thode pour appliquer les d�g�ts � une carte
    public int AtkToCard(List<GameObject> cardUI, List<Card> card, int atkTake)
    {
        // Parcourir toutes les cartes sur le plateau de jeu
        foreach (Card carte in card)
        {
            // Appliquer les d�g�ts
            if (atkTake > 0)
            {
                int temp = carte.pv;
                carte.pv -= atkTake;
                atkTake -= temp;
            }
            // V�rifier si la carte est d�truite
            if (carte.pv <= 0)
            {
                // Ajouter la carte � la liste des cartes d�truites
                card.Remove(carte);
                // Rechercher et d�truire l'interface de la carte
                foreach (GameObject carteUI in cardUI)
                {
                    TextMeshProUGUI[] TextPrefab = carteUI.GetComponentsInChildren<TextMeshProUGUI>();
                    foreach (TextMeshProUGUI textMeshProUGUI in TextPrefab)
                    {
                        if (textMeshProUGUI.name == "Name" && textMeshProUGUI.text == carte.card_name)
                        {
                            cardUI.Remove(carteUI);
                            Destroy(carteUI);
                            break;
                        }
                    }
                }
            }
            // Mettre � jour l'interface utilisateur de la carte
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
        return atkTake; // Retourner les d�g�ts restants
    }

    // M�thode pour calculer les d�g�ts � envoyer � l'adversaire
    public int GetAtk()
    {
        int atkToSend = 0;
        foreach (Card card in PlayerManager.player.deck.cardOnBoard)
        {
            atkToSend += card.atk;
        }
        return atkToSend;
    }

    // M�thode pour d�marrer une nouvelle partie
    public void StartGame(Message message)
    {
        int temp = message.GetInt();
        string name = message.GetString();

        // Afficher l'interface du jeu et mettre � jour l'interface utilisateur
        GameCanva.SetActive(true);
        PlayerUI[0].text = PlayerManager.player.username;
        PlayerUI[1].text = PlayerManager.player.pv.ToString();
        PlayerUI[2].text = name;
        PlayerUI[3].text = "20";

        // Piocher les cartes initiales
        for (int i = 0; i < 4; i++)
        {
            Card card = PlayerManager.player.deck.DrawCard();
            GameObject prefabInstance = CardToPrefab(card, i);
            PlayerManager.player.deck.cardInHandUI.Add(prefabInstance);
        }

        // Afficher l'�cran de premier tour si c'est n�cessaire
        if (temp == 0)
        {
            FirstPlay.SetActive(true);
        }
    }

    // M�thode appel�e lorsque le joueur effectue son premier tour
    public void Playing()
    {
        // Ajouter la carte sur le plateau de jeu
        int i = PlayerManager.player.deck.AddOnBoard(activeToggle.name);

        // Envoyer un message pour indiquer le d�but du premier tour
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.FirstPlay);
        messsage.AddUShort(PlayerManager.player.id);
        messsage.AddString(activeToggle.name);
        NetworkManager.Singleton.Client.Send(messsage);

        // Masquer l'�cran de premier tour
        FirstPlay.SetActive(false);

        // Piocher une nouvelle carte
        Card card = new Card();
        card = PlayerManager.player.deck.DrawCard();
        GameObject prefabInstance = CardToPrefab(card, i);
        PlayerManager.player.deck.cardInHandUI.Add(prefabInstance);
    }

    // M�thode pour convertir une carte en pr�fab de carte
    GameObject CardToPrefab(Card card, int i)
    {
        GameObject prefabInstance = Prefab;
        Toggle togglePrefab = prefabInstance.GetComponentInChildren<UnityEngine.UI.Toggle>();

        // Configurer le toggle pour le groupe de toggles
        if (togglePrefab != null)
        {
            togglePrefab.group = Group;
            togglePrefab.name = card.card_name;
            // Ajouter un �couteur � l'�v�nement onValueChanged du TogglePrefab
            togglePrefab.onValueChanged.AddListener((x) => { OnToggleValueChanged(togglePrefab); });
            // V�rifier si l'�couteur a �t� ajout� avec succ�s
            if (togglePrefab.onValueChanged != null && togglePrefab.onValueChanged.GetPersistentEventCount() > 0)
            {
                Debug.Log("AddListener est actif pour le TogglePrefab.");
            }
            else
            {
                Debug.LogWarning("AddListener n'est pas actif pour le TogglePrefab.");
            }
        }

        // Mettre � jour le texte des �l�ments de l'interface utilisateur
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

        // Instancier le pr�fab de carte et le positionner sur l'interface utilisateur
        GameObject Test = Instantiate(prefabInstance, new Vector3(drawPoint.x + 225 * i, drawPoint.y), Quaternion.identity);
        return Test;
    }

    // M�thode pour rejoindre la file d'attente
    public void JoinQueue()
    {
        // Cr�er un message pour rejoindre la file d'attente
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.JoinQueue);
        messsage.AddUShort(PlayerManager.player.id);
        NetworkManager.Singleton.Client.Send(messsage); // Envoyer le message au serveur
        Debug.Log("message to join the queue send");
    }

    // M�thode appel�e lorsqu'un toggle est activ� ou d�sactiv�
    public void OnToggleValueChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            // Si un nouveau bouton est s�lectionn�, d�s�lectionner l'ancien (s'il existe)
            if (activeToggle != null && activeToggle != toggle)
            {
                activeToggle.isOn = false;
            }
            activeToggle = toggle;
        }
        else if (activeToggle == toggle)
        {
            // Si le bouton d�s�lectionn� �tait le bouton actif, le marquer comme null
            activeToggle = null;
        }
    }
}
