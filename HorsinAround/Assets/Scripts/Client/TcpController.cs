using System;
using System.IO;
using System.Net.Sockets;
using Debug_resources;
using Server.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
  public class TcpController : MonoBehaviour
  {
    [SerializeField] private DebugConsole debugConsole;
    
    private const int Port = 8052;

    private TcpClient tcpClient;
    private NetworkStream hostStream;
    private StreamWriter writer;

    private int id;

    public bool StartClient(string ip, string userName)
    {
      try
      {
        debugConsole.Log("Attempting connection at " + ip);
        
        tcpClient = new TcpClient(ip, Port);
        hostStream = tcpClient.GetStream();
        writer = new StreamWriter(new BufferedStream(hostStream));
        
        debugConsole.Log("Connection made");
      
        Write(CommProtocol.Connect+CommProtocol.Delimiter+userName);
        writer.Flush();
        
        debugConsole.Log("Sending connection request");
        
        StreamReader sr = new StreamReader(new BufferedStream(hostStream));
        string[] response = sr.ReadLine()?.Split(CommProtocol.Delimiter);
        if (response != null) id = int.Parse(response[response.Length - 1]);
        return true;
      }
      catch (Exception e)
      {
        debugConsole.Log("On client connect exception " + e);
        return false;
      }
    }

    public void StopClient()
    {
      if (writer == null) return;
      Write(CommProtocol.Disconnect);
      writer.Close();
      hostStream.Close();
      tcpClient.Close();
    }

    /*private void Update()
    {
      if (writer == null) return;
      // TODO TEMP DEBUG REMOVE
      if (Input.GetKeyDown(KeyCode.Space))
      {
        Write(CommProtocol.TapDown);
      }
    }*/

    public void SendCommand(SwipeInput.Direction command)
    {
      switch (command)
      {
        case SwipeInput.Direction.TapDown:
          Write(CommProtocol.TapDown);
          break;
        case SwipeInput.Direction.Up:
          Write(CommProtocol.Up);
          break;
        case SwipeInput.Direction.Right:
          Write(CommProtocol.Right);
          break;
        case SwipeInput.Direction.Down:
          Write(CommProtocol.Down);
          break;
        case SwipeInput.Direction.Left:
          Write(CommProtocol.Left);
          break;
        case SwipeInput.Direction.TapUp:
          Write(CommProtocol.TapUp);
          break;
      }
    }

    private void Write(string data)
    {
      try
      {
        writer.WriteLine(data);
        writer.Flush();
      }
      catch
      {
        ClientMain.Instance.ActivateConnectionView();
      }
    }

    public int GetId()
    {
      return id;
    }

    private void OnDestroy()
    {
      StopClient();
    }
  }
}