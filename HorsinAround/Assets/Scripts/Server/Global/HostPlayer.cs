using System.IO;
using System.Net.Sockets;
using System.Threading;
using Server.Networking;
using TMPro;
using UnityEngine;

namespace Server.Global
{
  public class HostPlayer : Player
  {
    private TcpClient connectedClient;
    private NetworkStream clientStream;
    private StreamReader reader;

    private Thread readerThread;

    private bool initialized = false;
    private string commandBuffer = "";

    private bool terminate = false;

    private bool setName = false;

    private TextMeshPro nameText;
    
    public void Init(TcpClient newClient, int newId)
    {
      connectedClient = newClient;
      clientStream = connectedClient.GetStream();
      reader = new StreamReader(new BufferedStream(clientStream));

      Id = newId;

      nameText = GetComponentInChildren<TextMeshPro>();

      StartReading();
    }

    private void StartReading()
    {
      readerThread = new Thread(ReadInput);
      readerThread.IsBackground = true;
      readerThread.Start();

      Party.Instance.AddPlayer(this);
    }

    private void ReadInput()
    {
      try
      {
        while (true)
        {
          string command = reader.ReadLine();
          
          print("Player " + Id + ": " + command);

          if (command == null) break;

          if (initialized)
          {
            if (command == CommProtocol.Disconnect)
              break;

            commandBuffer = command;
          }
          else
          {
            string[] split = command.Split(CommProtocol.Delimiter);
            if (split.Length != 2) return;
            if (split[0] != CommProtocol.Connect) return;
            User = split[1];
            
            StreamWriter sw = new StreamWriter(new BufferedStream(clientStream));
            sw.WriteLine(CommProtocol.Connect+CommProtocol.Delimiter+Id);
            sw.Flush();
            initialized = true;

            setName = true;
          }
        }
      }
      catch
      {
        // ignored
      }

      Debug.Log("Client disconnected");
      terminate = true;
    }

    private void DisconnectPlayer()
    {
      Party.Instance.RemovePlayer(this);
      TcpServer.Instance.ReturnId(Id);
      CloseNetwork();
      Destroy(gameObject);
    }

    private void CloseNetwork()
    {
      reader.Close();
      clientStream.Close();
      connectedClient.Close();
    }

    private void Update()
    {
      if (setName)
      {
        nameText.text = User;
      }
      
      if (terminate) DisconnectPlayer();
      if (commandBuffer == "") return;
      //print(gameObject.name + commandBuffer);
      if (Controller == null) return;

      switch (commandBuffer)
      {
        case CommProtocol.TapDown:
          Controller.TapDown();
          break;
        case CommProtocol.TapUp:
          Controller.TapUp();
          break;
        case CommProtocol.Up:
          Controller.Up();
          break;
        case CommProtocol.Down:
          Controller.Down();
          break;
        case CommProtocol.Left:
          Controller.Left();
          break;
        case CommProtocol.Right:
          Controller.Right();
          break;
      }

      commandBuffer = "";
    }

    private void OnDestroy()
    {
      CloseNetwork();
    }
  }
}