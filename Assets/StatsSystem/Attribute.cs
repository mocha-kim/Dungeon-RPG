using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StatsSystem
{
    public enum AttributeType
    {
        Health,
        Mana,
        Stamina,
        Strength
    }

    [Serializable]
    public class Attribute
    {
        public AttributeType type;
        public ModifiableInt value;
    }

}