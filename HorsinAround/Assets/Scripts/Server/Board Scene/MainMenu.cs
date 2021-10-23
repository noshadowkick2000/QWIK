using System.Collections.Generic;
using Server.Global;
using Server.Networking;
using UnityEngine;

namespace Server.Board_Scene
{
  public class MainMenu : Singleton<MainMenu>
  {
    [SerializeField] private TcpServer server;

    public void StartLobby()
    {
      server.StartServer();
      CameraTransitioner.Instance.StartTransition(0);
    }

    public void StopLobby()
    {
      server.StopServer();
      CameraTransitioner.Instance.StartTransition(1);
    }
    
    [SerializeField] private Transform[] positions;
        
    private void Start()
    {
      Party.Instance.onPlayersUpdated += MoveNewPlayer;
    }

    private void MoveNewPlayer(List<Player> players)
    {
      if (players.Count == 0) return;

      for (int i = 0; i < players.Count; i++)
      {
       players[i].transform.position = positions[i].position;
       players[i].transform.rotation = positions[i].rotation; 
      }
    }
  }
}
