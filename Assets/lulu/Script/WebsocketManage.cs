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

    private readonly Queue<Action> _mainThreadQueue = new Queue<Action>();

    // public static event Action<string, string> ChangeUserNamePlayer;
    public static event Action<string, Vector2> OnMoovePlayer;
    public static event Action<string> OnUseMask;
    public static event Action<string> OnChangeMaskToLeft;
    public static event Action<string> OnChangeMaskToRight;
    public static event Action<string> OnGetMask;


    public static event Action OnIceWorld;
    public static event Action OnFireBall;
    public static event Action OnCupCake;
    public static event Action OnNo;
    public static event Action OnScreamer;
    public static event Action OnReverseColor;
    public static event Action OnTile;
    public static event Action OnCoffee;
    public static event Action OnAperitif;
    public static event Action OnImposteur;

    
    public static event Action OnStartGame;
    public static event Action OnResetGame;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        wssv = new WebSocketServer(System.Net.IPAddress.Any, 4242);

        wssv.AddWebSocketService<UnityBehavior>("/Data");

        wssv.Start();
        Debug.Log("✅ Serveur WebSocket lancé sur ws://127.0.0.1:4242/Data");
    }

    void Update()
    {
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

    public void EventAction(int nbrEvent)
    {
        int ICE_WORLD = 1;   // 2^0
        int FIRE_BALL = 2;   // 2^1
        int CUP_CAKE = 4;   // 2^2
        int NO = 8;   // 2^3
        int SCREAMER = 16;  // 2^4
        int REVERSE_COLOR = 32;  // 2^5
        int TILE = 64;  // 2^6
        int COFFEE = 128; // 2^7
        int APERITIF = 256; // 2^8
        int IMPOSTEUR = 512; // 2^9
        int START = 1024; // 2^10
        int RESET = 2048; // 2^10

        if ((nbrEvent & ICE_WORLD) != 0) OnIceWorld?.Invoke();
        if ((nbrEvent & FIRE_BALL) != 0) OnFireBall?.Invoke();
        if ((nbrEvent & CUP_CAKE) != 0) OnCupCake?.Invoke();
        if ((nbrEvent & NO) != 0) OnNo?.Invoke();
        if ((nbrEvent & SCREAMER) != 0) OnScreamer?.Invoke();
        if ((nbrEvent & REVERSE_COLOR) != 0) OnReverseColor?.Invoke();
        if ((nbrEvent & TILE) != 0) OnTile?.Invoke();
        if ((nbrEvent & COFFEE) != 0) OnCoffee?.Invoke();
        if ((nbrEvent & APERITIF) != 0) OnAperitif?.Invoke();
        if ((nbrEvent & IMPOSTEUR) != 0) OnImposteur?.Invoke();
        if ((nbrEvent & START) != 0) OnStartGame?.Invoke();
        if ((nbrEvent & RESET) != 0) OnResetGame?.Invoke();

        if (nbrEvent != 0) Debug.Log($"🎭 Events Globaux déclenchés (Somme : {nbrEvent})");
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
            FactoryManager.Instance.RemovePlayer(id);
        }
    }

    public void Enqueue(Action action) => _mainThreadQueue.Enqueue(action);

    void OnApplicationQuit()
    {
        if (wssv != null) wssv.Stop();
    }
}

public class UnityBehavior : WebSocketBehavior
{
    protected override void OnOpen()
    {
        string id = ID; // ID unique de la session
        WebsocketManage.Instance.Enqueue(() =>
        {
            WebsocketManage.Instance.AddUser(id);
        });
    }

    // Appelé quand l'onglet HTML est fermé
    protected override void OnClose(CloseEventArgs e)
    {
        string id = ID;
        WebsocketManage.Instance.Enqueue(() =>
        {
            WebsocketManage.Instance.RemoveUser(id);
        });
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        WebsocketManage.Instance.Enqueue(() =>
        {
            SocketData data = JsonUtility.FromJson<SocketData>(e.Data);

            if (int.TryParse(data.payload, out int nbr))
            {
                if (data.type == "INPUT")
                {
                    WebsocketManage.Instance.Inputaction(ID, nbr);
                }
                else if (data.type == "EVENT")
                {
                    WebsocketManage.Instance.EventAction(nbr);
                }
            }
        });
    }



}