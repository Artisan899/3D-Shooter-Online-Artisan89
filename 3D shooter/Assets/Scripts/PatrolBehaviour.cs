using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBahaviour : StateMachineBehaviour
{
    float timer;
    List<Transform> points = new List<Transform>();
    NavMeshAgent agent;
    List<Transform> players = new List<Transform>(); // Список игроков
    float chaseRange = 10;
    int currentPointIndex = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsObject = GameObject.FindGameObjectWithTag("Points").transform; //хождения по точкам 
        foreach (Transform t in pointsObject)
            points.Add(t);

        agent = animator.GetComponent<NavMeshAgent>();
        SetNextDestination();
        // Находим всех игроков и добавляем их в список
        UpdatePlayerList();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (timer > 30)
        {
            SetNextDestination();
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(points[Random.Range(0, points.Count)].position);

        if (timer > 5)
            animator.SetBool("patrooling", false);

        if (timer > 10)
            animator.SetBool("patrooling", true);

        // Проверяем, находится ли какой-либо игрок в радиусе обнаружения
        UpdatePlayerList(); // Обновляем список игроков перед каждым обновлением состояния
        foreach (Transform player in players)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(animator.transform.position, player.position);
                if (distance < chaseRange)
                {
                    animator.SetBool("chaising", true);
                    break; // Выходим из цикла, если обнаружен игрок
                }
            }
        }


    }
    private void SetNextDestination()
    {
        currentPointIndex = (currentPointIndex + 1); // Переходим к следующей точке
        agent.SetDestination(points[currentPointIndex].position);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    void UpdatePlayerList()
    {
        // Находим всех игроков и добавляем их в список
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players.Clear(); // Очищаем список перед обновлением
        foreach (GameObject playerObject in playerObjects)
        {
            players.Add(playerObject.transform);
        }
    }
}
