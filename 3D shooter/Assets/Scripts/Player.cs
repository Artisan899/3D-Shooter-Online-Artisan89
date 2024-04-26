using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public Slider sliderBar;

    [SyncVar(hook = nameof(OnHealthChanged))]
    private float currHealth = 100f; // ��������� �������� ������

    [Server]
    public void TakeDamage(float damage)
    {
        if (currHealth <= 0f) return; // ���� ����� ��� �����, �� ������� ����

        currHealth -= damage; // �������� ��������
        if (currHealth <= 0f)
        {
            currHealth = 0f; // ������������� �������� � 0, ����� �������� �������������� ��������
            Respawn();
        }
    }

    [Server]
    private void Respawn()
    {
        Vector3[] respawnPositions = new Vector3[] // ������� ������ �������� � ���������� ������������ ��� ����� ������ 
        {
            new Vector3(-7, 2, -10),
            new Vector3(10, 2, -18),
            new Vector3(12, 2, 3),
            new Vector3(7, 2, -10),
            new Vector3(12, 2, 15),
            
        };

        int randomIndex = Random.Range(0, respawnPositions.Length); // �������� ��������� ������ �� �������
       
        Vector3 respawnPosition = respawnPositions[randomIndex];   // �������� ��������� ������ �� �������
        
        transform.position = respawnPosition;  // ���������� ������ �� ��������� ����������
        
        currHealth = 100f;  // ��������������� �������� ����� ����

        
        RpcRespawn(respawnPosition, currHealth);  // ��������� ������� � �������� �� ������ ��������
    }

    [ClientRpc]
    private void RpcRespawn(Vector3 respawnPosition, float newHealth)
    {
        transform.position = respawnPosition;    // ��������� ������� ������ �� �������

        currHealth = newHealth;   // ��������� �������� �� �������

        sliderBar.value = newHealth;    // ��������� �������� Slider �� �������
    }

    [Client]
    private void OnHealthChanged(float oldValue, float newValue)
    {
        sliderBar.value = newValue;  // ��������� �������� Slider �� �������
    }
}