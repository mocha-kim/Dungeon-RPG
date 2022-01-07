using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StatsSystem
{

    [CreateAssetMenu(fileName = "New Stats", menuName = "Stats System/New Character Stats")]
    public class StatsObject : ScriptableObject
    {
        public Attribute[] attributes;

        public int level;
        public int exp;

        public int Health
        {
            get; set;
        }

        public int Mana
        {
            get; set;
        }

        public float HealthPercentage
        {
            get
            {
                int health = Health;
                int maxHealth = Health;

                foreach (Attribute attribute in attributes)
                {
                    if (attribute.type == AttributeType.Health)
                        maxHealth = attribute.value.ModifiedValue;
                }

                return (maxHealth > 0 ? ((float)health / (float)maxHealth) : 0f);
            }
        }

        public float ManaPercentage
        {
            get
            {
                int mana = Mana;
                int maxMana = Mana;

                foreach (Attribute attribute in attributes)
                {
                    if (attribute.type == AttributeType.Mana)
                        maxMana = attribute.value.ModifiedValue;
                }

                return (maxMana > 0 ? ((float)mana / (float)maxMana) : 0f);
            }
        }

        public Action<StatsObject> OnChangedStats;

        [NonSerialized]
        private bool isInitalize = false;

        public void OnEnable()
        {
            InitializeAttributes();
        }

        public void InitializeAttributes()
        {
            if (isInitalize)
                return;

            isInitalize = true;

            foreach (Attribute attribute in attributes)
                attribute.value = new ModifiableInt(OnModifiedValue);

            level = 0;
            exp = 0;

            SetBaseValue(AttributeType.Health, 100);
            SetBaseValue(AttributeType.Mana, 100);
            SetBaseValue(AttributeType.Stamina, 100);
            SetBaseValue(AttributeType.Strength, 100);

            Health = GetModifiedValue(AttributeType.Health);
            Mana = GetModifiedValue(AttributeType.Mana);
        }

        private void OnModifiedValue(ModifiableInt obj)
        {
            OnChangedStats?.Invoke(this);
        }

        public int GetModifiedValue(AttributeType type)
        {
            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == type)
                    return attribute.value.ModifiedValue;
            }

            return -1;
        }

        public void SetBaseValue(AttributeType type, int value)
        {
            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == type)
                    attribute.value.BaseValue = value;
            }
        }

        public int AddHealth(int value)
        {
            Health += value;

            OnChangedStats?.Invoke(this);

            return Health;
        }

        public int AddMana(int value)
        {
            Mana += value;

            OnChangedStats?.Invoke(this);

            return Mana;
        }
    }

}