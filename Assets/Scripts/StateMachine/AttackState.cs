using System.Collections;
using System.Collections.Generic;
using Unity_RPG.Characters;
using UnityEngine;

namespace Unity_RPG.AI
{

    public class AttackState : State<EnemyController>
    {
        private Animator animator;

        protected int hashAttack = Animator.StringToHash("Attack");
        protected int hashRandomAttack = Animator.StringToHash("RandomAttack");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            if (context.IsAvailableAttack)
            {
                animator?.SetInteger(hashRandomAttack, Random.Range(0, 1));
                animator?.SetTrigger(hashAttack);
            }
            else
            {
                stateMachine.ChangeState<IdleState>();
            }
        }

        public override void Update(float deltaTime)
        {
        }
    }

}