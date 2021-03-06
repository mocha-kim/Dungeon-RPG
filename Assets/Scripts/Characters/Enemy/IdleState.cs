using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;

namespace RPG.AI
{

    public class IdleState : State<EnemyController>
    {
        public bool isPatrol = false;
        private float minIdleTime = 0.0f;
        private float maxIdleTime = 3.0f;
        private float idleTime = 0.0f;

        private Animator animator;
        private CharacterController controller;

        protected int hashIsMoving = Animator.StringToHash("IsMoving");
        protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
        }

        public override void OnEnter()
        {
            animator?.SetBool(hashIsMoving, false);
            animator?.SetFloat(hashMoveSpeed, 0);
            controller?.Move(Vector3.zero);

            if (isPatrol)
            {
                idleTime = Random.Range(minIdleTime, maxIdleTime);
            }
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
            else if (isPatrol && stateMachine.ElapsedTimeInState > idleTime)
            {
                stateMachine.ChangeState<MoveToWaypoints>();
            }
        }

        public override void OnExit()
        {
        }
    }

}