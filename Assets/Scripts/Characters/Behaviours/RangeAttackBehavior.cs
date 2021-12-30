using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public class RangeAttackBehavior : AttackBehaviour
    {
        #region Variables

        public ManualCollision attackCollision;

        #endregion Variables

        public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
        {
            Collider[] colliders = attackCollision?.checkOverlapBox(targetMask);

            foreach (Collider collider in colliders)
            {
                collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage, effectPrefab);
            }

            calcCooltime = 0.0f;
        }
    }
}
