using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public Slider sliderBar;

    [SyncVar(hook = nameof(OnHealthChanged))]
    private float currHealth = 100f; // Ќачальное здоровье игрока

    [Server]
    public void TakeDamage(float damage)
    {
        if (currHealth <= 0f) return; // ≈сли игрок уже мертв, не наносим урон

        currHealth -= damage; // ќтнимаем здоровье
        if (currHealth <= 0f)
        {
            currHealth = 0f; // ”станавливаем здоровье в 0, чтобы избежать отрицательного значени€
            Respawn();
        }
    }

    [Server]
    private void Respawn()
    {
        Vector3[] respawnPositions = new Vector3[] // —оздаем массив векторов с различными координатами дл€ точек спавна 
        {
            new Vector3(-7, 2, -10),
            new Vector3(10, 2, -18),
            new Vector3(12, 2, 3),
            new Vector3(7, 2, -10),
            new Vector3(12, 2, 15),
            
        };

        int randomIndex = Random.Range(0, respawnPositions.Length); // ¬ыбираем случайный индекс из массива
       
        Vector3 respawnPosition = respawnPositions[randomIndex];   // ѕолучаем случайный вектор из массива
        
        transform.position = respawnPosition;  // ѕеремещаем игрока на выбранные координаты
        
        currHealth = 100f;  // ¬осстанавливаем здоровье после реса

        
        RpcRespawn(respawnPosition, currHealth);  // ќбновл€ем позицию и здоровье на других клиентах
    }

    [ClientRpc]
    private void RpcRespawn(Vector3 respawnPosition, float newHealth)
    {
        transform.position = respawnPosition;    // ќбновл€ем позицию игрока на клиенте

        currHealth = newHealth;   // ќбновл€ем здоровье на клиенте

        sliderBar.value = newHealth;    // ќбновл€ем значение Slider на клиенте
    }

    [Client]
    private void OnHealthChanged(float oldValue, float newValue)
    {
        sliderBar.value = newValue;  // ќбновл€ем значение Slider на клиенте
    }
}