using Server.Global;
using Server.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Debug_resources
{
  public class PartyCreator : MonoBehaviour
  {
    [SerializeField] private int testScene;
    [SerializeField] private TcpServer server;

    // Start is called before the first frame update
    void Start()
    {
      if (server == null) return;
      server.StartServer();
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
        LoadScene();
      if (Input.GetKeyDown(KeyCode.KeypadEnter))
        AssignDebugController();
    }

    private void AssignDebugController()
    {
      foreach (var player in Party.Instance.GetPlayers())
      {
        player.SetController(player.gameObject.AddComponent<DebugController>());
      }
    }

    private void LoadScene()
    {
      //server.StopServer();
      SceneManager.LoadScene(testScene);
    }
  }
}