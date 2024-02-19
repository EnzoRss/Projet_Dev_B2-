using Riptide;
using Riptide.Utils;
using UnityEngine;

public enum ClientToServerID : ushort
{
    name = 1,
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
    bool IsConnected;
    public Client Client { get; private set; }

    private void Awake()
    {
        Singleton = this;
        
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        IsConnected  = Client.Connect($"{ip}:{port}");
        if ( IsConnected )
        {
            ConnectToQueue();
        } 
    }

    private void FixedUpdate()
    {
        Client.Update();
        
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void ConnectToQueue() 
    {
        Message messsage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.name);
        messsage.AddString("coucou c'est ton père ");
        NetworkManager.Singleton.Client.Send(messsage);
        Debug.Log("message envoyer");
    }

}
