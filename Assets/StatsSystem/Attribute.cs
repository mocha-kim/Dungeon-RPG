using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StatsSystem
{

    public enum CharacterAttribute
    {
        Health,
        Mana,
        Stamina,
        Strength
    }

    public enum AttributeType
    {
        Health,
        Mana,
        Stamina,
        Strength
    }

    public class Attribute
    {
        public AttributeType type;
        public ModifiableInt value;
    }

}