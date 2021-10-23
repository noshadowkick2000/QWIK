using UnityEngine;

namespace Server.Global
{
  public class Player : MonoBehaviour
  {
    protected string User = "Player";
    protected int Id;

    protected int Points = 0;
  
    protected Controller Controller;
  
    public int SetController(Controller newController)
    {
      Controller = newController;
      return Id;
    }

    public void RemoveController()
    {
      Controller = null;
    }
  
    public int GetId()
    {
      return Id;
    }

    public void GivePoints(int newPoints)
    {
      Points += newPoints;
    }

    public void LosePoints(int newPoints)
    {
      Points -= newPoints;
    }
  }
}