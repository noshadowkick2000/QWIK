using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ClientMain : Singleton<ClientMain>
    {
        [SerializeField] public InputField ip;
        [SerializeField] public InputField userName;

        [SerializeField] public GameObject controllerViewRoot;
        [SerializeField] public GameObject connectionViewRoot;
        [SerializeField] public GameObject loadingScreen;
        [SerializeField] public GameObject joyStick;

        [SerializeField] public Color[] idColours;
        [SerializeField] public Image controllerBackground;

        private bool connected = false;
        private bool connectAttempted = false;

        private TcpController ct;

        private void Awake()
        {
            ct = GetComponent<TcpController>();
        }

        public void Connect()
        {
            (new Thread(StartServer)).Start();

            ShowLoadingScreen();
        }

        public void Disconnect()
        {
            ct.StopClient();
            ActivateConnectionView();
        }

        private void ShowLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }

        private void StartServer()
        {
            connected = ct.StartClient(ip.text, userName.text);
            connectAttempted = true;
        }

        private void ActivateControllerView()
        {
            controllerViewRoot.SetActive(true);
            connectionViewRoot.SetActive(false);
            loadingScreen.SetActive(false);
            
            joyStick.SetActive(true);

            controllerBackground.color = idColours[ct.GetId()];
        }

        public void ActivateConnectionView()
        {
            connectionViewRoot.SetActive(true);
            controllerViewRoot.SetActive(false);
            loadingScreen.SetActive(false);
            
            joyStick.SetActive(false);
        }

        private void Update()
        {
            if (!connectAttempted) return;
            connectAttempted = false;
            if (connected)
                ActivateControllerView();
            else
                ActivateConnectionView();
        }
    }
}
