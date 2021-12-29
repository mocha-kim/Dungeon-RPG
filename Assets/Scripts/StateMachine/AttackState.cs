using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;

namespace RPG.AI
{

    public class AttackState : State<EnemyController>
    {
        private Animator animator;
        private AttackStateController attackStateController;
        private IAttackable attackable;

        protected int hashAttack = Animator.StringToHash("Attack");
        protected int hashAttackIndex = Animator.StringToHash("AttackIndex");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            attackStateController = context.GetComponent<AttackStateController>();
            attackable = context.GetComponent<IAttackable>();
        }

        public override void OnEnter()
        {
            if (attackable == null || attackable.CurrentAttackBehaviour == null)
            {
                stateMachine.ChangeState<IdleState>();
                return;
            }

            attackStateController.enterAttackStateHandler += OnEnterAttackState;
            attackStateController.exitAttackStateHandler += OnExitAttackState;

            animator?.SetInteger(hashAttackIndex, attackable.CurrentAttackBehaviour.animationIndex);

            if (context.IsAvailableAttack)
            {
                animator?.SetTrigger(hashAttack);
            }
        }

        public override void Update(float deltaTime)
        {
        }

        public override void OnExit()
        {
            attackStateController.enterAttackStateHandler -= OnEnterAttackState;
            attackStateController.exitAttackStateHandler -= OnExitAttackState;
        }

        public void OnEnterAttackState()
        {

        }

        public void OnExitAttackState()
        {
            stateMachine.ChangeState<IdleState>();
        }
    }

}