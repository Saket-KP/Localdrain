using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    public float speed = 20f;
    public int damage = 25;
    public float lifetime = 3f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        if (IsServer) // only server handles lifetime
        {
            Destroy(gameObject, lifetime);
        }
    }

    void Update()
    {
        if (!IsServer) return; // server controls hit detection

        Vector3 newPosition = transform.position + transform.forward * speed * Time.deltaTime;

        if (Physics.Raycast(lastPosition, newPosition - lastPosition, out RaycastHit hit, (newPosition - lastPosition).magnitude))
        {
            PlayerHealth tank = hit.collider.GetComponent<PlayerHealth>();
            if (tank != null)
            {
                tank.TakeDamageServerRpc(damage);
            }

            // Destroy bullet on hit
            NetworkObject.Despawn();
        }

        transform.position = newPosition;
        lastPosition = newPosition;
    }
}