using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] public int maxHealth = 100;

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(
        100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }

        currentHealth.OnValueChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        Debug.Log($"{gameObject.name} health: {newValue}");

        if (newValue <= 0)
        {
            Die();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        if (currentHealth.Value > 0)
        {
            currentHealth.Value -= damage;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} is destroyed!");
        // Example: disable tank mesh
        gameObject.SetActive(false);
    }
}