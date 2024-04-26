using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleZom : StateMachineBehaviour
{
    float timer;
    Transform player;
    float chaseRange = 10; //Радиус обнарежения игрока

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0; //таймер на обнаружения игрока
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        timer += Time.deltaTime;
        if (timer > 5)
            animator.SetBool("patrooling", true); //При не нахождении игрока патрулирует по точкам

        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < chaseRange)
            animator.SetBool("chaising", true); //При нахождении преследует игрока
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


    }


}
