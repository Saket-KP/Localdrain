using UnityEngine;
using Unity.Netcode;

public class RaycastAttack : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space)) // Fire button
        {
            ShootServerRpc();
        }
    }

    [ServerRpc]
    void ShootServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<NetworkObject>().Spawn(true);
    }
}