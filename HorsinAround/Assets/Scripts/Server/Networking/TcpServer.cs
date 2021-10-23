using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Server.Global;
using UnityEngine;

namespace Server.Networking
{
  public class TcpServer : Singleton<TcpServer>
  {
    [Header("Prefab for HostPlayer containing HostPlayer component")] [SerializeField]
    private GameObject playerPrefab;

    [Header(
      "Root GameObject for all connection related objects. Put all HostPlayers and tcp scripts as children of this object")]
    [SerializeField]
    private GameObject serverRoot;

    [Header("Max number of concurrent players")] [SerializeField]
    private int maxPlayers = 8;

    private TcpListener tcpListener;
    private Thread tcpListenerThread;

    private IPAddress ip;

    private const int Port = 8052;

    private string output;

    private bool terminate = false;

    private bool active = false;

    private List<TcpClient> queuedClients = new List<TcpClient>();

    private bool[] takenIds;

    private void Awake()
    {
      takenIds = new bool[maxPlayers];
      DontDestroyOnLoad(serverRoot);
    }

    private int GetId()
    {
      for (int i = 0; i < takenIds.Length; i++)
      {
        if (!takenIds[i])
        {
          takenIds[i] = true;
          return i;
        }
      }

      return -1;
    }

    public void ReturnId(int id)
    {
      takenIds[id] = false;
    }

    public void StartServer()
    {
      active = true;

      int indexShortest = 0;
      int shortestLength = 100;

      int counter = 0;

      foreach (var address in Dns.GetHostAddresses(Dns.GetHostName()))
      {
        print(address.ToString());
        if (address.ToString().Length < shortestLength)
        {
          indexShortest = counter;
          shortestLength = address.ToString().Length;
        }

        counter++;
      }

      ip = Dns.GetHostAddresses(Dns.GetHostName())[indexShortest];

      tcpListenerThread = new Thread(ListenForIncomingRequests);
      tcpListenerThread.Start();

      Debug.Log("Server started");
    }

    private void ListenForIncomingRequests()
    {
      try
      {
        tcpListener = new TcpListener(ip, Port);
        tcpListener.Start();
        Debug.Log("Server listening on " + ip + " : " + Port);
        while (active)
        {
          queuedClients.Add(tcpListener.AcceptTcpClient());
        }
      }
      catch (Exception e)
      {
        Debug.Log(e);
        terminate = true;
      }
    }

    private void CreateNewPlayer(TcpClient newClient)
    {
      int newId = GetId();

      if (newId == -1)
      {
        Debug.Log("Server full");
        return;
      }

      Debug.Log("New client connected");
      HostPlayer newPlayer = Instantiate(playerPrefab, serverRoot.transform).GetComponent<HostPlayer>();
      newPlayer.Init(newClient, newId);
    }

    public void StopServer()
    {
      active = false;
      tcpListener.Stop();

      Debug.Log("Server stopped");
    }

    public void ClearCurrentPlayers()
    {
      queuedClients.Clear();
      Party.Instance.ResetPlayers();
      takenIds = new bool[8];
      foreach (var hostPlayer in serverRoot.GetComponentsInChildren<Transform>())
      {
        if (hostPlayer == serverRoot.transform) continue;
        Destroy(hostPlayer.gameObject);
      }
    }

    private void Update()
    {
      if (terminate)
      {
        StopServer();
        terminate = false;
      }

      if (queuedClients.Count == 0) return;

      foreach (var client in queuedClients)
      {
        CreateNewPlayer(client);
      }

      queuedClients.Clear();
    }
  }
}