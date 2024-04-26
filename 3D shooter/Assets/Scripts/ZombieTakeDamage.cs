using Mirror;
using UnityEngine;

public class ZombieTakeDamage : NetworkBehaviour
{
    public float maxHealth = 40f; //�������� �����
    private float currentHealth;
    

    private void Start()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void TakeDamage(float damage)
    {
        if (!isServer) //����� ��� ������� �������
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
        NetworkServer.Destroy(gameObject); // ���������� ������ �� �������

        SpawnEnemy.Instance.EnemyDied(this);
    }

}