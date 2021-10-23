using Debug_resources;
using UnityEngine;

namespace Client
{
  public class SwipeInputTester : MonoBehaviour
  {
//    public enum Direction
//    {
//      TapDown = 0,
//      Up = 1,
//      Right = 2,
//      Down = 3,
//      Left = 4,
//      TapUp = 5
//    }

    [SerializeField] private float moveThreshold;

    private bool engaged = false;

    private int totalChangeX;
    private int totalChangeY;

    private DebugConsole db;

    //private TcpController ct;
    //private JoyStickPhysics js;

    // Start is called before the first frame update
    void Start()
    {
      //ct = GetComponent<TcpController>();
      db = GetComponent<DebugConsole>();
    }

    // Update is called once per frame
    void Update()
    {
      if (!engaged)
        CheckNewTouch();
      else
        TrackTouch();
    }

    private void CheckNewTouch()
    {
      if (Input.touchCount <= 0) return;
      if (Input.GetTouch(0).phase == TouchPhase.Began)
      {
        db.Log("TapDown");
        //ct.SendCommand(SwipeInput.Direction.TapDown);
        engaged = true;
        //js.SetDragging(true);
      }
    }

    private void TrackTouch()
    {
      try
      {
        if (Mathf.Abs(totalChangeX) > moveThreshold ||
            Mathf.Abs(totalChangeY) > moveThreshold)
        {
          SwipeInput.Direction dir;

          if (Mathf.Abs(totalChangeX) > Mathf.Abs(totalChangeY))
          {
            dir = totalChangeX > 0 ? SwipeInput.Direction.Right : SwipeInput.Direction.Left;
          }
          else
          {
            dir = totalChangeY > 0 ? SwipeInput.Direction.Up : SwipeInput.Direction.Down;
          }

          db.Log(dir.ToString());
          //ct.SendCommand(dir);

          Disengage();
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
        {
          db.Log("TapUp");
          //ct.SendCommand(SwipeInput.Direction.TapUp);

          Disengage();
        }
        else
        {
          Touch temp = Input.GetTouch(0);
          totalChangeX += (int) temp.deltaPosition.x;
          totalChangeY += (int) temp.deltaPosition.y;
          //js.DragJoyStick(temp.deltaPosition.x, temp.deltaPosition.y);
        }
      }
      catch
      {
        Disengage();
      }

      void Disengage()
      {
        engaged = false;
        totalChangeX = 0;
        totalChangeY = 0;

        //js.SetDragging(false);
      }
    }
  }
}