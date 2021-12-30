using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public class MeleeAttackBehavior : AttackBehaviour
    {
        #region Variables

        public ManualCollision attackCollision;

        #endregion Variables

        public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
        {
            target.GetComponent<IDamagable>()?.TakeDamage(damage, effectPrefab);

            calcCooltime = 0.0f;
        }
    }
}
