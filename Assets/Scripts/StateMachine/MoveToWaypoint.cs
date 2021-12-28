using System.Collections;
using System.Collections.Generic;
using Unity_RPG.Characters;
using UnityEngine;
using UnityEngine.AI;

namespace Unity_RPG.AI
{

    public class MoveToWaypoints : State<EnemyController>
    {
        private Animator animator;
        private CharacterController controller;
        private NavMeshAgent agent;

        protected int hashMove = Animator.StringToHash("Move");
        protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
            agent = context.GetComponent<NavMeshAgent>();
        }

        public override void OnEnter()
        {
            if (context.targetWaypoint == null)
                context.FindNextWaypoint();

            if (context.targetWaypoint)
            {
                agent?.SetDestination(context.targetWaypoint.position);
                animator?.SetBool(hashMove, true);
            }
        }

        public override void Update(float deltaTime)
        {
            Transform enemy = context.SearchEnemy();

            if (enemy)
            {
                if (context.IsAvailableAttack)
                    stateMachine.ChangeState<AttackState>();
                else
                    stateMachine.ChangeState<MoveState>();
            }
            else
            {
                if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
                {
                    context.FindNextWaypoint();
                    //Transform nextDst = context.FindNextWaypoint();
                    //if (nextDst)
                    //    agent.SetDestination(nextDst.position);

                    stateMachine.ChangeState<IdleState>();
                }
                else
                {
                    controller.Move(agent.velocity * deltaTime);
                    animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, deltaTime);
                }
            }
        }

        public override void OnExit()
        {
            animator?.SetBool(hashMove, false);
            agent.ResetPath();
        }
    }

}