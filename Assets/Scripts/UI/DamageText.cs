using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI textMeshPro;

    public float delayTimeToDestroy = 1.0f;

    #endregion Variables

    public int Damage
    {
        get
        {
            if (textMeshPro != null)
                return int.Parse(textMeshPro.text);
            return 0;
        }
        set
        {
            if (textMeshPro != null)
                textMeshPro.text = value.ToString();
        }
    }

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delayTimeToDestroy);
    }
}
