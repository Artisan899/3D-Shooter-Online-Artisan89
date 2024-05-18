using Mirror;
using UnityEngine;

public class ZombieTakeDamage : NetworkBehaviour
{
    public int maxHealth = 40; //здоровье зомби
    [SyncVar]
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isServer) return; // Если это не сервер, выходим

        currentHealth -= damage;
        if (currentHealth <= 0f)
        {

              RpcRespawn();

        }
    }

    [ClientRpc]
    private void RpcRespawn()
    {

        Vector3[] respawnPositions = new Vector3[] // Создаем массив векторов с различными координатами для точек спавна 
        {
            new Vector3(-7, 2, -10),
            new Vector3(10, 2, -18),
            new Vector3(12, 2, 3),
            new Vector3(7, 2, -10),
            new Vector3(12, 2, 15),
        };

        int randomIndex = Random.Range(0, respawnPositions.Length); // Выбираем случайный индекс из массива

        Vector3 respawnPosition = respawnPositions[randomIndex];   // Получаем случайный вектор из массива

        transform.position = respawnPosition;  // Перемещаем НПС на выбранные координаты

        currentHealth = maxHealth;  // Восстанавливаем здоровье после реса
    }


 
}
