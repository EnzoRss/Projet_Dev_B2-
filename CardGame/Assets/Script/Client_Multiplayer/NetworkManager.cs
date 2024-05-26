using Riptide;
using Riptide.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public enum ClientToServerID : ushort
{
    name = 1,
    JoinQueue = 2,
    GameStart = 3,
    FirstPlay = 4,
    InGame = 5,

}
public class NetworkManager : MonoBehaviour
{

    private static NetworkManager _singleton;

    public static NetworkManager Singleton
    {
        get => _singleton;
        private set 
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exist , destroying duplicate");
            }
        }
    }

    private string ip;
    [SerializeField] private string port;
    public bool IsConnected;
    public GameObject ConnectButton;
    public GameObject MenuCanva;
    public GameObject InputIP;
    public  PlayerManager PlayerManager;
    public Client Client { get; private set; }

    private void Awake()
    {
        Singleton = this;
        
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
    }
    

    private void FixedUpdate()
    {
        Client.Update();
        
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void ConnectClicked()
    {
        IsConnected = Singleton.Connection();
        if (IsConnected )
        {
            ConnectButton.SetActive(false);
            MenuCanva.SetActive(true);
        }
    }

    public void GetIp()
    {
        ip = InputIP.GetComponent<TMP_InputField>().text;
        InputIP.SetActive(false);
    }

    private bool Connection()
    {
        bool connected = false;
        
        connected = Client.Connect($"{ip}:{port}", useMessageHandlers: false);
        return connected;
    }


    public void SendName() 
    {
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.name);
        messsage.AddString(PlayerManager.player.username);
        messsage.AddUShort(PlayerManager.player.id);
        NetworkManager.Singleton.Client.Send(messsage);
        Debug.Log("message envoyer");
    }

   public void JoinQueue()
    {
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.JoinQueue);
        messsage.AddUShort(PlayerManager.player.id);
        NetworkManager.Singleton.Client.Send(messsage);
        Debug.Log("message to join the queue send");
        MenuCanva.SetActive(false);
    }

}
