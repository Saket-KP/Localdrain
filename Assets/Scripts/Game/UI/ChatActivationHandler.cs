using Unity.Netcode;
using UnityEngine;

public class ChatActivationHandler : NetworkBehaviour
{
    [Tooltip("Assign your Chat UI Panel GameObject here")]
    public GameObject chatUI;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("ChatActivationHandler: Local player spawned, enabling chat UI.");
            chatUI.SetActive(true);
        }
        else
        {
            chatUI.SetActive(false);
        }
    }
}
