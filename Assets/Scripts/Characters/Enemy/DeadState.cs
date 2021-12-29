using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{

    public class DeadState : State<EnemyController>
    {
        private Animator animator;

        protected int hashIsAlive = Animator.StringToHash("IsAlive");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            animator?.SetBool(hashIsAlive, false);
        }

        public override void Update(float deltaTime)
        {
            if (stateMachine.ElapsedTimeInState > 3.0f)
                GameObject.Destroy(context.gameObject);
        }

        public override void OnExit()
        {
        }
    }

}