using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{

    public class MoveState : State<EnemyController>
    {
        private Animator animator;
        private CharacterController controller;
        private NavMeshAgent agent;

        protected int hashIsMoving = Animator.StringToHash("IsMoving");
        protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
            agent = context.GetComponent<NavMeshAgent>();
        }

        public override void OnEnter()
        {
            agent.stoppingDistance = context.AttackRange;
            agent?.SetDestination(context.Target.position);
            animator?.SetBool(hashIsMoving, true);
        }

        public override void Update(float deltaTime)
        {
            if (context.Target)
            {
                agent.SetDestination(context.Target.position);
            }

            controller.Move(agent.velocity * deltaTime);
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, deltaTime);
            }
            else
            {
                if (!agent.pathPending)
                {
                    animator.SetFloat(hashMoveSpeed, 0f);
                    animator.SetBool(hashIsMoving, false);
                    agent.ResetPath();

                    stateMachine.ChangeState<IdleState>();
                }
            }
        }

        public override void OnExit()
        {
            agent.stoppingDistance = 0.0f;
            agent.ResetPath();
            animator.SetFloat(hashMoveSpeed, 0f);
            animator.SetBool(hashIsMoving, false);
        }
    }

}