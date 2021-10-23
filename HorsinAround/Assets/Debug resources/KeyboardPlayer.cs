using System;
using Server.Global;
using UnityEngine;

namespace Debug_resources
{
  public class KeyboardPlayer : Player
  {
    [SerializeField] private int id;
    
    [SerializeField] private KeyCode tap;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    
    private void Awake()
    {
      Party.Instance.AddPlayer(this);
      Id = id;
    }

    private void Update()
    {
      if (Controller == null) return;
      
      if (Input.GetKeyDown(tap))
        Controller.TapDown();
      if (Input.GetKeyUp(tap))
        Controller.TapUp();
      if (Input.GetKeyDown(up))
        Controller.Up();
      if (Input.GetKeyDown(down))
        Controller.Down();
      if (Input.GetKeyDown(left))
        Controller.Left();
      if (Input.GetKeyDown(right))
        Controller.Right();
    }
  }
}