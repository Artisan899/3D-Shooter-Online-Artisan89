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


    public void EnemyDied(ZombieTakeDamage zombie) 
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

        if (spawnerInterval <= 0 && nowTheEnemies < numberOfEnemies) 
        {
            randEnemy = Random.Range(0, staticSpawnEnemy.Length); 
            randPoint = Random.Range(0, staticSpawnPoint.Length); 

            GameObject enemy = Instantiate(staticSpawnEnemy[randEnemy], staticSpawnPoint[randPoint].transform.position, Quaternion.identity);

            NetworkServer.Spawn(enemy, connectionToClient); 

            spawnerInterval = startSpawnerInterval; 
            nowTheEnemies++; 
        }
        else
        {
            spawnerInterval -= Time.deltaTime; 
        }
    }

    [ClientRpc]
    void RpcSpawnEnemy(GameObject enemy)  
    {
        if (!isServer)
        {
            Instantiate(enemy, enemy.transform.position, enemy.transform.rotation);
        }
    }
}
