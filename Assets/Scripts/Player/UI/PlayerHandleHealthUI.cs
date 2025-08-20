using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandleHealthUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text healthText;   // optional
    [SerializeField] private Slider healthSlider;   // assign in Inspector
    PlayerHealth playerHealth;

    private Camera _mainCamera;

    public override void OnNetworkSpawn()
    {
        _mainCamera = Camera.main;

        AllPlayerDataManager.Instance.OnPlayerHealthChanged += InstanceOnOnPlayerHealthChangedServerRpc;
        InstanceOnOnPlayerHealthChangedServerRpc(GetComponentInParent<NetworkObject>().OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void InstanceOnOnPlayerHealthChangedServerRpc(ulong id)
    {
        if (GetComponentInParent<NetworkObject>().OwnerClientId == id)
        {
            SetHealthUIClientRpc(id);
        }
    }

    private void Update()
    {
        if (_mainCamera)
        {
            // Make the health bar always face the camera
            transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
                             _mainCamera.transform.rotation * Vector3.up);
        }
    }

    [ClientRpc]
    void SetHealthUIClientRpc(ulong id)
    {
        float currentHealth = AllPlayerDataManager.Instance.GetPlayerHealth(id);
        int maxHealth = playerHealth.maxHealth;

        if (healthText != null)
            healthText.text = currentHealth.ToString();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public override void OnNetworkDespawn()
    {
        AllPlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnPlayerHealthChangedServerRpc;
    }
}