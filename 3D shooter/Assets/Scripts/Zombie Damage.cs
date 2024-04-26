using Mirror;
using UnityEngine;

public class EntityDamage : NetworkBehaviour
{
    public float damage = 10f; // урон зомби

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //столкновение с игроком
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
