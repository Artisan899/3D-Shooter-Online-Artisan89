using UnityEngine;
using Mirror;

public class SpawnEnemy : NetworkBehaviour
{

    public static SpawnEnemy Instance { get; private set; }


    [SerializeField]
    private GameObject[] spawnEnemy;
    [SerializeField]
    private Transform[] spawnPoint;

    public float startSpawnerInterval;
    private float spawnerInterval;

    public int numberOfEnemies;
    public int nowTheEnemies;


    public void Awake()
    {
        Instance = this;
    }


    public void EnemyDied(ZombieTakeDamage zombie) //При смерти зомби их колличество -1
    {
        nowTheEnemies--;
 
    }

    private int randEnemy;
    private int randPoint;

    private static GameObject[] staticSpawnEnemy;
    private static Transform[] staticSpawnPoint;



    void Start()
    {
        spawnerInterval = startSpawnerInterval;
        staticSpawnEnemy = spawnEnemy;
        staticSpawnPoint = spawnPoint;
    }

    void Update()
    {
        if (!isServer)
            return;

        if (spawnerInterval <= 0 && nowTheEnemies < numberOfEnemies) //Если прошёл спавн интервал и колличество врагов < максимального
        {
            randEnemy = Random.Range(0, staticSpawnEnemy.Length); //Рандомный враг
            randPoint = Random.Range(0, staticSpawnPoint.Length); //Рандомная позиция спавна

            GameObject enemy = Instantiate(staticSpawnEnemy[randEnemy], staticSpawnPoint[randPoint].transform.position, Quaternion.identity); //Создание врага

            NetworkServer.Spawn(enemy); // Спавн на сервере

            spawnerInterval = startSpawnerInterval; //Обнуление интервала
            nowTheEnemies++; //Колличество врагов +1
        }
        else
        {
            spawnerInterval -= Time.deltaTime; //Уменьшение интервала до спавна
        }
    }

    [ClientRpc]
    void RpcSpawnEnemy(GameObject enemy)  //Синхронизация на сервере
    {
        if (!isServer)
        {
            Instantiate(enemy, enemy.transform.position, enemy.transform.rotation);
        }
    }
}