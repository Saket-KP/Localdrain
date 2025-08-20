using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameNonAR : NetworkBehaviour
{

    [SerializeField] private Button startHost;
    [SerializeField] private Button startClient;
    void Start()
    {
        startHost.onClick.AddListener(call:() =>
        {
            NetworkManager.Singleton.StartHost();

        });

        startClient.onClick.AddListener(call:() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
 
}
