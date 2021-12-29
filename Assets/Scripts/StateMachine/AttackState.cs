using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;

namespace RPG.AI
{

    public class AttackState : State<EnemyController>
    {
        private Animator animator;

        protected int hashAttack = Animator.StringToHash("Attack");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            if (context.IsAvailableAttack)
            {
                animator?.SetTrigger(hashAttack);
            }
            stateMachine.ChangeState<IdleState>();
        }

        public override void Update(float deltaTime)
        {
        }

        public override void OnExit()
        {
        }
    }

}