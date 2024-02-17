using Riptide;
using Riptide.Utils;
using UnityEngine;

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

    //[SerializeField] private string ip;
    [SerializeField] private string port;
    public Client Client { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError,false);
        
        Client = new Client();
        Client.Connect($"127.0.0.1:{port}");
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }
}
