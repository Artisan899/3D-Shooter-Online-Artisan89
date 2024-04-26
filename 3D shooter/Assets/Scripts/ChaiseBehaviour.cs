using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaiseBahaviour : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    float attackRange = 2;
    float chaseRange = 10;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 4; //�������� ��� ������
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���������, ��������� �� ����� � ������� �����������
        if (player != null)
        {
            agent.SetDestination(player.position);
            float distance = Vector3.Distance(animator.transform.position, player.position);

            if (distance < attackRange)
                animator.SetBool("attaker", true); //�������� �����

            if (distance > chaseRange)
                animator.SetBool("chaising", false); //�������� �������������
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        agent.speed = 3; //������� ��������
    }
}