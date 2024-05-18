using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    List<Transform> players = new List<Transform>(); // Список игроков
    Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindClosestPlayer(animator.transform); // Находим ближайшего игрока
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindClosestPlayer(animator.transform); // Обновляем игрока перед каждым обновлением состояния
        if (player != null)
        {
            animator.transform.LookAt(player);
            float distance = Vector3.Distance(animator.transform.position, player.position);

            if (distance > 3)
                animator.SetBool("attaker", false); // Если дистанция минимальная, то атакует
        }

       
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
