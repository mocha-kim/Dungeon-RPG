using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBattleUI : MonoBehaviour
{
    #region Variables

    private Slider hpSlider;
    [SerializeField]
    private GameObject damageTextPrefab;

    #endregion Variables

    #region Properties

    public float MinValue
    {
        get => hpSlider.minValue;
        set
        {
            hpSlider.minValue = value;
        }
    }

    public float MaxValue
    {
        get => hpSlider.maxValue;
        set
        {
            hpSlider.maxValue = value;
        }
    }

    public float CurValue
    {
        get => hpSlider.value;
        set
        {
            hpSlider.value = value;
        }
    }

    #endregion Propeties

    private void Awake()
    {
        hpSlider = gameObject.GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        GetComponent<Canvas>().enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void CreateDamageText(int damage)
    {
        if (damageTextPrefab != null)
        {
            GameObject damageTextGO = Instantiate(damageTextPrefab, transform);
            DamageText damageText = damageTextGO.GetComponent<DamageText>();
            if (damageText == null)
                Destroy(damageTextGO);

            damageText.Damage = damage;
        }
    }
}
