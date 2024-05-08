using Riptide;
using Riptide.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ClientToServerID : ushort
{
    name = 1,
    JoinQueue = 2,
    GameStart = 3,
    InGame =4,

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

    [SerializeField] private string ip;
    [SerializeField] private string port;
    public bool IsConnected;
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
        
    }

    private bool Connection()
    {
        bool connected = false;
        connected = Client.Connect($"{ip}:{port}");
        return connected;
    }


    public void SendName() 
    {
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.name);
        messsage.AddString("test");
        messsage.AddUShort(2);
        NetworkManager.Singleton.Client.Send(messsage);
        Debug.Log("message envoyer");
    }

   /* public void JoinQueue()
    {
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.JoinQueue);
        messsage.AddUShort(2);
        NetworkManager.Singleton.Client.Send(messsage);
        Debug.Log("message to join the queue send");
    }*/

}
