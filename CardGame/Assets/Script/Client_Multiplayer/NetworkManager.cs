using Riptide;
using Riptide.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

// Enum�ration des identifiants de messages Client vers Serveur
public enum ClientToServerID : ushort
{
    name = 1,        // Nom du joueur
    JoinQueue = 2,   // Rejoindre la file d'attente
    GameStart = 3,   // D�but de la partie
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
    public bool IsConnected;   // Indique si le client est connect�
    public GameObject ConnectButton;   // Bouton de connexion
    public GameObject MenuCanva;       // Interface utilisateur du menu principal
    public GameObject InputIP;         // Champ de saisie de l'adresse IP
    public PlayerManager PlayerManager; // Gestionnaire de joueur
    public Client Client { get; private set; } // Client Riptide

    private void Awake()
    {
        Singleton = this; // D�finir cette instance comme l'instance unique de NetworkManager
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false); // Initialisation du journal Riptide

        Client = new Client(); // Cr�ation d'une instance de client
    }

    private void FixedUpdate()
    {
        Client.Update(); // Mettre � jour le client Riptide
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect(); // Se d�connecter du serveur lors de la fermeture de l'application
    }

    // M�thode appel�e lors du clic sur le bouton de connexion
    public void ConnectClicked()
    {
        IsConnected = Singleton.Connection(); // Tenter la connexion au serveur
        if (IsConnected)
        {
            ConnectButton.SetActive(false); // D�sactiver le bouton de connexion
            MenuCanva.SetActive(true);     // Activer l'interface utilisateur du menu principal
        }
    }

    // M�thode pour obtenir l'adresse IP saisie par l'utilisateur
    public void GetIp()
    {
        ip = InputIP.GetComponent<TMP_InputField>().text; // R�cup�rer l'adresse IP saisie
        InputIP.SetActive(false); // D�sactiver le champ de saisie de l'adresse IP
    }

    // M�thode pour �tablir la connexion au serveur
    private bool Connection()
    {
        bool connected = false;
        connected = Client.Connect($"{ip}:{port}", useMessageHandlers: false); // Tenter la connexion avec l'adresse IP et le port sp�cifi�s
        return connected;
    }

    // M�thode pour envoyer le nom du joueur au serveur
    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.name); // Cr�er un message avec le nom du joueur
        message.AddString(PlayerManager.player.username); // Ajouter le nom du joueur au message
        message.AddUShort(PlayerManager.player.id);       // Ajouter l'identifiant du joueur au message
        NetworkManager.Singleton.Client.Send(message);    // Envoyer le message au serveur
        Debug.Log("Message sent."); // Afficher un message de d�bogage
    }

    // M�thode pour rejoindre la file d'attente de la partie
    public void JoinQueue()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.JoinQueue); // Cr�er un message de demande de rejoindre la file d'attente
        message.AddUShort(PlayerManager.player.id); // Ajouter l'identifiant du joueur au message
        NetworkManager.Singleton.Client.Send(message); // Envoyer le message au serveur
        Debug.Log("Message to join the queue sent."); // Afficher un message de d�bogage
        MenuCanva.SetActive(false); // Masquer l'interface utilisateur du menu principal
    }
}
