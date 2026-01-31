using UnityEngine;
using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using JetBrains.Annotations;

// 1. Structure pour lire le JSON envoyé par le HTML
[Serializable]
public class SocketData
{
    public string type;
    public string payload;
}

public class WebsocketManage : MonoBehaviour
{
    public static WebsocketManage Instance;
    private WebSocketServer wssv;

    [SerializeField] 
    private string messageGet;

    public List<string> connectedUsers = new List<string>();

    // File d'attente pour exécuter les actions sur le thread principal d'Unity
    private readonly Queue<Action> _mainThreadQueue = new Queue<Action>();

    // public static event Action<string, string> ChangeUserNamePlayer;
    public static event Action<string, Vector2> OnMoovePlayer;
    public static event Action<string> OnUseMask;
    public static event Action<string> OnChangeMaskToLeft;
    public static event Action<string> OnChangeMaskToRight;
    public static event Action<string> OnGetMask;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Initialisation du serveur sur le port 4242
        wssv = new WebSocketServer(4242);
        
        // On définit la route "ws://127.0.0.1:4242/Data"
        wssv.AddWebSocketService<UnityBehavior>("/Data");
        
        wssv.Start();
        Debug.Log("✅ Serveur WebSocket lancé sur ws://127.0.0.1:4242/Data");
    }

    void Update()
    {
        // On vide la file d'attente sur le thread principal
        lock (_mainThreadQueue)
        {
            while (_mainThreadQueue.Count > 0)
            {
                _mainThreadQueue.Dequeue().Invoke();
            }
        }
    }

    public void Inputaction(string id, int nbrAction)
    {
        int GAUCHE = 1;  // 00001
        int DROITE = 2;  // 00010
        int AVANT = 4;   // 00100
        int ARRIERE = 8; // 01000
        int USE_MASK = 16; // 10000
        int CHANGE_MACK_LEFT = 32;
        int CHANGE_MACK_RIGHT = 64; 
        int GET_MASK = 128;
        
        if ((nbrAction & GAUCHE) != 0 && (nbrAction & DROITE) != 0)
        {
            nbrAction &= ~GAUCHE;
            nbrAction &= ~DROITE;
        }

        if ((nbrAction & AVANT) != 0 && (nbrAction & ARRIERE) != 0)
        {
            nbrAction &= ~AVANT;
            nbrAction &= ~ARRIERE;
        }

        Vector2 moveDir = Vector2.zero;

        if ((nbrAction & GAUCHE) != 0) moveDir.x = -1;
        else if ((nbrAction & DROITE) != 0) moveDir.x = 1;

        if ((nbrAction & AVANT) != 0) moveDir.y = 1;
        else if ((nbrAction & ARRIERE) != 0) moveDir.y = -1;


        if (moveDir != Vector2.zero)
        {
            OnMoovePlayer?.Invoke(id, moveDir);
        }

        if ((nbrAction & USE_MASK) != 0)
        {
            OnUseMask?.Invoke(id);
        }

        if ((nbrAction & CHANGE_MACK_LEFT) != 0)
        {
            OnChangeMaskToLeft?.Invoke(id);
        }  
        
        if ((nbrAction & CHANGE_MACK_RIGHT) != 0)
        {
            OnChangeMaskToRight?.Invoke(id);
        }  
        
        if ((nbrAction & GET_MASK) != 0)
        {
            OnGetMask?.Invoke(id);
        }  
        

    }



    public void AddUser(string id)
    {
        if (!connectedUsers.Contains(id))
        {
            connectedUsers.Add(id);
        }
    }

    public void RemoveUser(string id)
    {
        if (connectedUsers.Contains(id))
        {
            connectedUsers.Remove(id);
        }
    }

    // Cette fonction est appelée par le thread du serveur
    public void Enqueue(Action action) => _mainThreadQueue.Enqueue(action);

    void OnApplicationQuit()
    {
        if (wssv != null) wssv.Stop();
    }
}

// 2. Comportement du serveur (Internal)
public class UnityBehavior : WebSocketBehavior
{
    protected override void OnOpen()
    {
        string id = ID; // ID unique de la session
        WebsocketManage.Instance.Enqueue(() => {
            WebsocketManage.Instance.AddUser(id);
        });
    }

    // Appelé quand l'onglet HTML est fermé
    protected override void OnClose(CloseEventArgs e)
    {
        string id = ID;
        WebsocketManage.Instance.Enqueue(() => {
            WebsocketManage.Instance.RemoveUser(id);
        });
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        WebsocketManage.Instance.Enqueue(() => {
            // 1. Lire le JSON
            SocketData data = JsonUtility.FromJson<SocketData>(e.Data);
            
            // 2. Si le type est "INPUT", on convertit le payload en int et on lance l'action
            if (data.type == "INPUT")
            {
                if (int.TryParse(data.payload, out int nbr))
                {
                    WebsocketManage.Instance.Inputaction(ID, nbr);
                }
            }
            
        });
    }



}