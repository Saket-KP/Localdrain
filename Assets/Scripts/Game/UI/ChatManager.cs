// using Unity.Netcode;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;

// public class ChatManager : NetworkBehaviour
// {
//     private TMP_InputField chatInputField;
//     private Text chatLogText; // Legacy Text
//     private Button sendButton;
//     private ScrollRect scrollRect;

//     private bool hasStartedLoop = false;

//     public override void OnNetworkSpawn()
//     {
//         // Automatically find UI in scene
//         chatInputField = GameObject.Find("ChatInputField")?.GetComponent<TMP_InputField>();
//         chatLogText = GameObject.Find("ChatLogText")?.GetComponent<Text>();
//         sendButton = GameObject.Find("SendButton")?.GetComponent<Button>();
//         scrollRect = GameObject.FindObjectOfType<ScrollRect>();

//         if (chatInputField == null || chatLogText == null || sendButton == null || scrollRect == null)
//         {
//             Debug.LogError("[ChatManager] UI components not found. Make sure names match exactly.");
//             return;
//         }

//         if (IsOwner)
//         {
//             sendButton.onClick.AddListener(OnSendButtonClicked);
//             chatInputField.interactable = true;
//             sendButton.interactable = true;
//         }
//         else
//         {
//             chatInputField.interactable = false;
//             sendButton.interactable = false;
//         }

//         if (IsServer && !hasStartedLoop)
//         {
//             hasStartedLoop = true;
//             StartCoroutine(SendUIDMessageLoop());
//             Debug.Log("[ChatManager] Host started sending auto messages.");
//         }
//     }

//     private void OnSendButtonClicked()
//     {
//         string message = chatInputField.text;

//         if (!string.IsNullOrWhiteSpace(message))
//         {
//             SendChatMessageServerRpc(message.Trim());
//             chatInputField.text = "";
//             chatInputField.ActivateInputField();
//         }
//     }

//     private IEnumerator SendUIDMessageLoop()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(1f);

//             string msg = $"Auto message from host UID {NetworkManager.Singleton.LocalClientId}";
//             BroadcastChatMessageClientRpc(msg);
//         }
//     }

//     [ServerRpc(RequireOwnership = false)]
//     private void SendChatMessageServerRpc(string message, ServerRpcParams rpcParams = default)
//     {
//         ulong senderId = rpcParams.Receive.SenderClientId;
//         string formattedMessage = $"Player {senderId}: {message}";
//         BroadcastChatMessageClientRpc(formattedMessage);
//     }

//     [ClientRpc]
//     private void BroadcastChatMessageClientRpc(string message)
//     {
//         if (chatLogText == null) return;

//         chatLogText.text += message + "\n";
//         Canvas.ForceUpdateCanvases();
//         scrollRect.verticalNormalizedPosition = 0f;
//     }
// }
