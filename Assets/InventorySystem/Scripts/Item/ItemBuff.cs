using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAttribute
{
    HP,
    Stamina,
    Strength
}

[Serializable]
public class ItemBuff
{
    public CharacterAttribute status;
    public int value;

    [SerializeField]
    private int min;
    public int Min => min;

    [SerializeField]
    private int max;
    public int Max => max;

    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;

        GenerateValue();
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }

    public void AddValue(ref int v)
    {
        v += value;
    }
}
