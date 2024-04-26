using Mirror;
using UnityEngine;

public class ZombieTakeDamage : NetworkBehaviour
{
    public float maxHealth = 40f; //здоровье зомби
    private float currentHealth;
    

    private void Start()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void TakeDamage(float damage)
    {
        if (!isServer) //метод дл€ сервера сервера
            return; 

        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    [Server]
    private void Die()
    {
        NetworkServer.Destroy(gameObject); // ”ничтожаем объект на сервере

        SpawnEnemy.Instance.EnemyDied(this);
    }

}