using Debug_resources;
using UnityEngine;

namespace Client
{
  public class SwipeInput : MonoBehaviour
  {
    public enum Direction
    {
      TapDown = 0,
      Up = 1,
      Right = 2,
      Down = 3,
      Left = 4,
      TapUp = 5
    }

    [SerializeField] private float moveThreshold;

    private bool engaged = false;

    private int totalChangeX;
    private int totalChangeY;

    private TcpController ct;
    private JoyStickPhysics js;
    private DebugConsole db;

    // Start is called before the first frame update
    void Start()
    {
      ct = FindObjectOfType<TcpController>(true);
      js = FindObjectOfType<JoyStickPhysics>(true);
      db = FindObjectOfType<DebugConsole>(true);
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
          ct.SendCommand(Direction.TapDown);
          db.Log(Direction.TapDown.ToString());
          engaged = true;
          js.SetDragging(true);
      }

    }

    private void TrackTouch()
    {
      try
      {
        if (Mathf.Abs(totalChangeX) > moveThreshold ||
            Mathf.Abs(totalChangeY) > moveThreshold)
        {
          Direction dir;

          if (Mathf.Abs(totalChangeX) > Mathf.Abs(totalChangeY))
          {
            dir = totalChangeX > 0 ? Direction.Right : Direction.Left;
          }
          else
          {
            dir = totalChangeY > 0 ? Direction.Up : Direction.Down;
          }

          ct.SendCommand(dir);
          db.Log(dir.ToString());

          Disengage();
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
        {
          ct.SendCommand(Direction.TapUp);
          db.Log(Direction.TapUp.ToString());
          
          Disengage();
        }
        else
        {
          Touch temp = Input.GetTouch(0);
          totalChangeX += (int) temp.deltaPosition.x;
          totalChangeY += (int) temp.deltaPosition.y;
          js.DragJoyStick(temp.deltaPosition.x, temp.deltaPosition.y);
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

        js.SetDragging(false);
      }
    }
  }
}