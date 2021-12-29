using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public interface IDamagable
    {
        bool IsAlive
        {
            get;
        }

        void TakeDamage(int damage, GameObject hitEffectPrefabs);
    }

}
