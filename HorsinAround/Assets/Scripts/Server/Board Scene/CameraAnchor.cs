using UnityEngine;

namespace Server.Board_Scene
{
  public class CameraAnchor : MonoBehaviour
  {
    [SerializeField] private Transform playerAnchor;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float t;
  
    private void FixedUpdate()
    {
      transform.position = Vector3.Lerp(transform.position, playerAnchor.position + offset, t);
    }
  }
}
