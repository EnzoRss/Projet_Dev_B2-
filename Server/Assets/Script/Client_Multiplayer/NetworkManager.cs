using System.Collections.Generic;
using Riptide;
using Riptide.Utils;
using UnityEngine;


public enum ClientToServerID : ushort
{
    name = 1,
    JoinQueue = 2,
    StartGame = 3,
    FirstPlay= 4,
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

    [SerializeField] private ushort maxClient;
    [SerializeField] private ushort port;
    public Server server { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError,false);
        server = new Server();
        StartServ();
        
    }


    void StartServ()
    {
        Singleton.server.Start(port, maxClient);
    }

    private void FixedUpdate()
    {
        server.Update();
    }

    private void OnApplicationQuit()
    {
        server.Stop();
    }

  
}
