
using System.Collections.Generic;
using Riptide;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public NetworkManager NetworkManager;   // Référence au NetworkManager
    public HttpManagement httpHandler;      // Gestionnaire des requêtes HTTP
    public PlayerManager PlayerManager;     // Gestionnaire de joueur
    public Toggle activeToggle;             // Toggle actif
    public GameObject Prefab;               // Préfab de carte
    public GameObject ConfimButton;         // Bouton de confirmation
    public GameObject FirstPlay;            // Écran de premier tour
    public ToggleGroup Group;               // Groupe de toggles
    public TextMeshProUGUI[] PlayerUI;      // Interface utilisateur du joueur
    public GameObject GameCanva;            // Interface utilisateur du jeu
    public GameObject MenuCanva;            // Interface utilisateur du menu
    Vector3 drawPoint = new Vector3(-307, -350); // Position de dessin des cartes

    private void Start()
    {
        // Ajouter un écouteur pour gérer les messages reçus par le client
        NetworkManager.Singleton.Client.MessageReceived += OnClientMessageReceived;
    }

    // Méthode pour gérer les messages reçus par le client
    void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        // Vérifier le type de message et appeler les méthodes appropriées pour le traiter
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

    // Méthode pour traiter les actions du joueur en jeu
    public void HandlerActionInGame(Message message)
    {
        // Récupérer les informations du message
        string name = message.GetString();
        int atkTake = message.GetInt();
        int pvEnemy = message.GetInt();

        // Appliquer les dégâts à la carte du joueur
        atkTake = AtkToCard(PlayerManager.player.deck.cardOnBoardUI, PlayerManager.player.deck.cardOnBoard, atkTake);

        // Mettre à jour l'interface utilisateur
        if (atkTake > 0)
        {
            PlayerManager.player.pv -= atkTake;
            PlayerUI[1].text = "pv : " + PlayerManager.player.pv.ToString();
        }
        PlayerUI[3].text = "pv : " + pvEnemy;

        // Vérifier si le joueur a perdu
        if (PlayerManager.player.pv <= 0)
        {
            Looser(); // Méthode appelée en cas de défaite
        }
        ConfimButton.SetActive(true); // Activer le bouton de confirmation
    }

    // Méthode pour traiter la fin de la partie
    public void HandlerEndGame(Message message)
    {
        // Masquer l'interface du jeu et afficher le menu principal
        GameCanva.SetActive(false);
        MenuCanva.SetActive(true);

        // Détruire toutes les cartes affichées
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

        // Réinitialiser les données du jeu
        PlayerManager.player.deck.ResetInGame();

        // Récupérer les informations du joueur depuis la base de données
        httpHandler.StartGetPlayer("/SelectData?table=users&filter={\"username\":\"" + PlayerManager.player.username + "\",\"password\":\"" + PlayerManager.player.password + "\"}"); ;
    }

    // Méthode appelée en cas de défaite du joueur
    public void Looser()
    {
        // Créer un message pour indiquer la fin de la partie
        Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.EndGame);
        messsageToSend.AddUShort(PlayerManager.player.id);
        NetworkManager.Singleton.Client.Send(messsageToSend); // Envoyer le message au serveur
    }

    // Méthode pour traiter la première action du joueur
    public void HandlerFirstAction(Message message)
    {
        // Récupérer le nom de la carte depuis le message
        string name = message.GetString();
        Debug.Log("nom récupérer : " + name);

        // Lancer une requête HTTP pour récupérer les informations sur la carte
        httpHandler.StartGetCard("/SelectData?table=cards&filter={\"card_name\":\"" + name + "\"}");

        // Activer le bouton de confirmation
        ConfimButton.SetActive(true);
    }

    // Méthode pour terminer le tour du joueur
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

        // Calculer les dégâts à envoyer à l'adversaire
        int atkToSend = GetAtk();

        // Envoyer les informations au serveur
        Message messsageToSend = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.InGame);
        messsageToSend.AddUShort(PlayerManager.player.id);
        messsageToSend.AddString(activeToggle.name);
        messsageToSend.AddInt(atkToSend);
        messsageToSend.AddInt(PlayerManager.player.pv);
        NetworkManager.Singleton.Client.Send(messsageToSend);

        // Désactiver le bouton de confirmation
        ConfimButton.SetActive(false);
    }

    // Méthode pour appliquer les dégâts à une carte
    public int AtkToCard(List<GameObject> cardUI, List<Card> card, int atkTake)
    {
        // Parcourir toutes les cartes sur le plateau de jeu
        foreach (Card carte in card)
        {
            // Appliquer les dégâts
            if (atkTake > 0)
            {
                int temp = carte.pv;
                carte.pv -= atkTake;
                atkTake -= temp;
            }
            // Vérifier si la carte est détruite
            if (carte.pv <= 0)
            {
                // Ajouter la carte à la liste des cartes détruites
                card.Remove(carte);
                // Rechercher et détruire l'interface de la carte
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
            // Mettre à jour l'interface utilisateur de la carte
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
        return atkTake; // Retourner les dégâts restants
    }

    // Méthode pour calculer les dégâts à envoyer à l'adversaire
    public int GetAtk()
    {
        int atkToSend = 0;
        foreach (Card card in PlayerManager.player.deck.cardOnBoard)
        {
            atkToSend += card.atk;
        }
        return atkToSend;
    }

    // Méthode pour démarrer une nouvelle partie
    public void StartGame(Message message)
    {
        int temp = message.GetInt();
        string name = message.GetString();

        // Afficher l'interface du jeu et mettre à jour l'interface utilisateur
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

        // Afficher l'écran de premier tour si c'est nécessaire
        if (temp == 0)
        {
            FirstPlay.SetActive(true);
        }
    }

    // Méthode appelée lorsque le joueur effectue son premier tour
    public void Playing()
    {
        // Ajouter la carte sur le plateau de jeu
        int i = PlayerManager.player.deck.AddOnBoard(activeToggle.name);

        // Envoyer un message pour indiquer le début du premier tour
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.FirstPlay);
        messsage.AddUShort(PlayerManager.player.id);
        messsage.AddString(activeToggle.name);
        NetworkManager.Singleton.Client.Send(messsage);

        // Masquer l'écran de premier tour
        FirstPlay.SetActive(false);

        // Piocher une nouvelle carte
        Card card = new Card();
        card = PlayerManager.player.deck.DrawCard();
        GameObject prefabInstance = CardToPrefab(card, i);
        PlayerManager.player.deck.cardInHandUI.Add(prefabInstance);
    }

    // Méthode pour convertir une carte en préfab de carte
    GameObject CardToPrefab(Card card, int i)
    {
        GameObject prefabInstance = Prefab;
        Toggle togglePrefab = prefabInstance.GetComponentInChildren<UnityEngine.UI.Toggle>();

        // Configurer le toggle pour le groupe de toggles
        if (togglePrefab != null)
        {
            togglePrefab.group = Group;
            togglePrefab.name = card.card_name;
            // Ajouter un écouteur à l'événement onValueChanged du TogglePrefab
            togglePrefab.onValueChanged.AddListener((x) => { OnToggleValueChanged(togglePrefab); });
            // Vérifier si l'écouteur a été ajouté avec succès
            if (togglePrefab.onValueChanged != null && togglePrefab.onValueChanged.GetPersistentEventCount() > 0)
            {
                Debug.Log("AddListener est actif pour le TogglePrefab.");
            }
            else
            {
                Debug.LogWarning("AddListener n'est pas actif pour le TogglePrefab.");
            }
        }

        // Mettre à jour le texte des éléments de l'interface utilisateur
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

        // Instancier le préfab de carte et le positionner sur l'interface utilisateur
        GameObject Test = Instantiate(prefabInstance, new Vector3(drawPoint.x + 225 * i, drawPoint.y), Quaternion.identity);
        return Test;
    }

    // Méthode pour rejoindre la file d'attente
    public void JoinQueue()
    {
        // Créer un message pour rejoindre la file d'attente
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.JoinQueue);
        messsage.AddUShort(PlayerManager.player.id);
        NetworkManager.Singleton.Client.Send(messsage); // Envoyer le message au serveur
        Debug.Log("message to join the queue send");
    }

    // Méthode appelée lorsqu'un toggle est activé ou désactivé
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
