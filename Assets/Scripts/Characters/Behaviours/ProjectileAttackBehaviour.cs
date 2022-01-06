using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public class ProjectileAttackBehaviour : AttackBehaviour
    {
        public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
        {
            if (target == null)
                return;

            Vector3 projectilePosition = startPoint?.position ?? transform.position;
            if (effectPrefab)
            {
                GameObject projectileGO = GameObject.Instantiate<GameObject>(effectPrefab, projectilePosition, Quaternion.identity);
                projectileGO.transform.forward = transform.forward;

                Projectile projectile = projectileGO.GetComponent<Projectile>();
                if (projectile)
                {
                    projectile.owner = gameObject;
                    projectile.target = target;
                    projectile.attackBehaviour = this;
                }
            }

            calcCooltime = 0.0f;
        }
    }

}