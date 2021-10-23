using System;
using UnityEngine;
using UnityEngine.UI;

namespace Debug_resources
{
  public class DebugConsole : MonoBehaviour
  {
    [SerializeField] private GameObject console;
    [SerializeField] private Text consoleText;
    
    private string log = "";
    private bool logChanged;
    
    public void Log(string msg)
    {
      log = log.Insert(0, "D: " + msg + Environment.NewLine);
      logChanged = true;
    }

    private void Update()
    {
      if (!logChanged) return;
      consoleText.text = log;
      logChanged = false;
    }

    public void SwitchConsole()
    {
      console.SetActive(!console.activeInHierarchy);
    }
  }
}
