using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaiseBahaviour : StateMachineBehaviour
{
    List<Transform> players = new List<Transform>(); // Список игроков
    Transform player;
    NavMeshAgent agent;
    float attackRange = 2;
    float chaseRange = 10;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindClosestPlayer(animator.transform); // Находим ближайшего игрока
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 4; // Скорость при погони
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Проверяем, находится ли игрок в радиусе обнаружения
        player = FindClosestPlayer(animator.transform); // Обновляем игрока перед каждым обновлением состояния
        if (player != null)
        {
            agent.SetDestination(player.position);
            float distance = Vector3.Distance(animator.transform.position, player.position);

            if (distance < attackRange)
                animator.SetBool("attaker", true); // Анимация атаки

            if (distance > chaseRange)
                animator.SetBool("chaising", false); // Анимация преследования
        }

      
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        agent.speed = 3; // Обычная скорость
    }

    Transform FindClosestPlayer(Transform currentTransform)
    {
        players.Clear(); // Очищаем список перед обновлением
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject playerObject in playerObjects)
        {
            float distanceToPlayer = Vector3.Distance(currentTransform.position, playerObject.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestPlayer = playerObject.transform;
                closestDistance = distanceToPlayer;
            }
            players.Add(playerObject.transform);
        }

        return closestPlayer;
    }
}
