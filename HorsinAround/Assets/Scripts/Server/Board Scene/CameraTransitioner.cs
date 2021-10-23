using System.Collections;
using UnityEngine;

namespace Server.Board_Scene
{
  public class CameraTransitioner : Singleton<CameraTransitioner>
  {
    [SerializeField] private Transform[] transforms;
    [SerializeField] private float transitionTime;

    public void StartTransition(int index)
    {
      StartCoroutine(Transition(transforms[index]));
    }

    private IEnumerator Transition(Transform newTransform)
    {
      Vector3 startPosition = transform.position;
      Vector3 goalPosition = newTransform.position;

      Quaternion startRotation = transform.rotation;
      Quaternion goalRotation = newTransform.rotation;
      
      float startTime = Time.time;
      float t;

      while (true)
      {
        t = (Time.time - startTime) / transitionTime;
        
        if (t >= 1) break;
        
        transform.position = Vector3.Lerp(startPosition, goalPosition, t);
        transform.rotation = Quaternion.Lerp(startRotation, goalRotation, t);
        yield return null;
      }
    }
  }
}
