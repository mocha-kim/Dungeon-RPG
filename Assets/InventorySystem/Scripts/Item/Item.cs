using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.InventorySystem.Item
{

    [Serializable]
    public class Item
    {
        public int id;
        public string name;

        public ItemBuff[] buffs;

        public Item()
        {
            id = -1;
            name = "";
        }

        public Item(ItemObject otherItem)
        {
            name = otherItem.name;
            id = otherItem.data.id;

            buffs = new ItemBuff[otherItem.data.buffs.Length];
            for (int i = 0; i < buffs.Length; ++i)
            {
                buffs[i] = new ItemBuff(otherItem.data.buffs[i].Min, otherItem.data.buffs[i].Max)
                {
                    status = otherItem.data.buffs[i].status
                };
            }
        }
    }

}