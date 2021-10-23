using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
  private static bool mSingletonInitialised = false;

  private static T mInstance;

  public static T Instance
  {
    get
    {
      if (mInstance == null)
      {
        T[] results = FindObjectsOfType<T>();

        if (!mSingletonInitialised)
        {
          if (!mSingletonInitialised && results.Length == 0)
          {
            Debug.LogWarning("Singleton TimeManager has no active instance!");

            return null;
          }

          else if (!mSingletonInitialised && results.Length > 1)
          {
            Debug.LogWarning("Singleton TimeManager has more than 1 active instance! Number of instances " +
                             results.Length);

            return null;
          }

          else mSingletonInitialised = true;

          mInstance = results[0];
        }
      }

      return mInstance;
    }
  }

  protected static void ResetInstance()
  {
    mSingletonInitialised = false;
    mInstance = null;
  }
}