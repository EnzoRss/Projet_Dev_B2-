using Riptide;
using Riptide.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

// Enumération des identifiants de messages Client vers Serveur
public enum ClientToServerID : ushort
{
    name = 1,        // Nom du joueur
    JoinQueue = 2,   // Rejoindre la file d'attente
    GameStart = 3,   // Début de la partie
    FirstPlay = 4,   // Premier tour
    InGame = 5,      // En jeu
    EndGame = 6      // Fin de la partie
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;

    // Instance unique de NetworkManager
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate.");
            }
        }
    }

    private string ip;         // Adresse IP du serveur
    [SerializeField] private string port;   // Port du serveur
    public bool IsConnected;   // Indique si le client est connecté
    public GameObject ConnectButton;   // Bouton de connexion
    public GameObject MenuCanva;       // Interface utilisateur du menu principal
    public GameObject InputIP;         // Champ de saisie de l'adresse IP
    public PlayerManager PlayerManager; // Gestionnaire de joueur
    public Client Client { get; private set; } // Client Riptide

    private void Awake()
    {
        Singleton = this; // Définir cette instance comme l'instance unique de NetworkManager
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false); // Initialisation du journal Riptide

        Client = new Client(); // Création d'une instance de client
    }

    private void FixedUpdate()
    {
        Client.Update(); // Mettre à jour le client Riptide
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect(); // Se déconnecter du serveur lors de la fermeture de l'application
    }

    // Méthode appelée lors du clic sur le bouton de connexion
    public void ConnectClicked()
    {
        IsConnected = Singleton.Connection(); // Tenter la connexion au serveur
        if (IsConnected)
        {
            ConnectButton.SetActive(false); // Désactiver le bouton de connexion
            MenuCanva.SetActive(true);     // Activer l'interface utilisateur du menu principal
        }
    }

    // Méthode pour obtenir l'adresse IP saisie par l'utilisateur
    public void GetIp()
    {
        ip = InputIP.GetComponent<TMP_InputField>().text; // Récupérer l'adresse IP saisie
        InputIP.SetActive(false); // Désactiver le champ de saisie de l'adresse IP
    }

    // Méthode pour établir la connexion au serveur
    private bool Connection()
    {
        bool connected = false;
        connected = Client.Connect($"{ip}:{port}", useMessageHandlers: false); // Tenter la connexion avec l'adresse IP et le port spécifiés
        return connected;
    }

    // Méthode pour envoyer le nom du joueur au serveur
    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.name); // Créer un message avec le nom du joueur
        message.AddString(PlayerManager.player.username); // Ajouter le nom du joueur au message
        message.AddUShort(PlayerManager.player.id);       // Ajouter l'identifiant du joueur au message
        NetworkManager.Singleton.Client.Send(message);    // Envoyer le message au serveur
        Debug.Log("Message sent."); // Afficher un message de débogage
    }

    // Méthode pour rejoindre la file d'attente de la partie
    public void JoinQueue()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.JoinQueue); // Créer un message de demande de rejoindre la file d'attente
        message.AddUShort(PlayerManager.player.id); // Ajouter l'identifiant du joueur au message
        NetworkManager.Singleton.Client.Send(message); // Envoyer le message au serveur
        Debug.Log("Message to join the queue sent."); // Afficher un message de débogage
        MenuCanva.SetActive(false); // Masquer l'interface utilisateur du menu principal
    }
}
