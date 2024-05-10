using Mirror;
using UnityEngine;

public class ZombieTakeDamage : NetworkBehaviour
{
    public float maxHealth = 40f; //здоровье зомби
    [SyncVar]
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void CmdTakeDamage(float damage)
    {

        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
            SpawnEnemy.Instance.EnemyDied(this);
        }
    }
}
