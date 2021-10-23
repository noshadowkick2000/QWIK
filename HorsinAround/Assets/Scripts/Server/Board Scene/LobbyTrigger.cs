using UnityEngine;

namespace Server.Board_Scene
{
  public class LobbyTrigger : MonoBehaviour
  {
    [SerializeField] private Camera lobbyCamera;
    private Camera mainCam;

    private void Awake()
    {
      mainCam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
      MainMenu.Instance.StartLobby();
      lobbyCamera.enabled = true;
      mainCam.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
      MainMenu.Instance.StopLobby();
      lobbyCamera.enabled = false;
      mainCam.enabled = true;
    }
  }
}
