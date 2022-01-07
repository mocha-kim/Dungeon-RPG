using System;
using System.Collections;
using System.Collections.Generic;
using RPG.InventorySystem.Inventory;
using RPG.InventorySystem.Items;
using RPG.StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Attribute = RPG.StatsSystem.Attribute;

public class PlayerStatsUI : MonoBehaviour
{
    public InventoryObject equipment;
    public StatsObject playerStats;

    public TextMeshProUGUI[] attributeText;

    private void OnEnable()
    {
        playerStats.OnChangedStats += OnChangedStats;

        if (equipment != null && playerStats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdate += OnEquipItem;
            }
        }

        UpdateAttributeTexts();
    }

    private void OnDisable()
    {
        playerStats.OnChangedStats -= OnChangedStats;

        if (equipment != null && playerStats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate -= OnRemoveItem;
                slot.OnPostUpdate -= OnEquipItem;
            }
        }
    }

    private void UpdateAttributeTexts()
    {
        attributeText[0].text = playerStats.GetModifiedValue(AttributeType.Health).ToString("n0");
        attributeText[1].text = playerStats.GetModifiedValue(AttributeType.Mana).ToString("n0");
        attributeText[2].text = playerStats.GetModifiedValue(AttributeType.Stamina).ToString("n0");
        attributeText[3].text = playerStats.GetModifiedValue(AttributeType.Strength).ToString("n0");
    }

    private void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
            return;

        foreach (ItemBuff buff in slot.item.buffs)
        {
            foreach (Attribute attribute in playerStats.attributes)
            {
                if (attribute.type == buff.status)
                    attribute.value.AddModifier(buff);
            }
        }
    }

    private void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
            return;

        foreach (ItemBuff buff in slot.item.buffs)
        {
            foreach (Attribute attribute in playerStats.attributes)
            {
                if (attribute.type == buff.status)
                    attribute.value.RemoveModifier(buff);
            }
        }
    }

    private void OnChangedStats(StatsObject obj)
    {
        UpdateAttributeTexts();
    }

}
