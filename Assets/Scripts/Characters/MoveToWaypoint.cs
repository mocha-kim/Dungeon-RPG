using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{

    public class MoveToWaypoints : State<EnemyController>
    {
        private Animator animator;
        private CharacterController controller;
        private NavMeshAgent agent;
        private EnemyController_Patrol patrolController;

        protected int hashIsMoving = Animator.StringToHash("IsMoving");
        protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

        private Transform targetWaypoint = null;
        private int waypointIndex = 0;

        private Transform[] Waypoints => ((EnemyController_Patrol)context)?.waypoints;

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
            agent = context.GetComponent<NavMeshAgent>();

            patrolController = context as EnemyController_Patrol;
        }

        public override void OnEnter()
        {
            if (targetWaypoint == null)
                FindNextWaypoint();

            if (targetWaypoint)
            {
                agent?.SetDestination(targetWaypoint.position);
                animator?.SetBool(hashIsMoving, true);
            }
            else
                stateMachine.ChangeState<IdleState>();
        }

        public override void Update(float deltaTime)
        {
            if (context.Target)
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
                    FindNextWaypoint();
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
            animator?.SetBool(hashIsMoving, false);
            agent.ResetPath();
        }

        public Transform FindNextWaypoint()
        {
            targetWaypoint = null;

            if (Waypoints != null && Waypoints.Length > 0)
            {
                targetWaypoint = Waypoints[waypointIndex];
                waypointIndex = (waypointIndex + 1) % Waypoints.Length;
            }
            return targetWaypoint;
        }
    }

}