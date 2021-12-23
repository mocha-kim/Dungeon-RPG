using System.Collections;
using System.Collections.Generic;
using Unity_RPG.Characters;
using UnityEngine;

namespace Unity_RPG.AI
{

    public class AttackState : State<EnemyController>
    {
        private Animator animator;

        protected int hasAttack = Animator.StringToHash("Attack");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            Debug.Log(context.IsAvailableAttack);
            if (context.IsAvailableAttack)
                animator?.SetBool(hasAttack, true);
            else
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