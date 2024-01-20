using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [Tooltip("Where my HP is stored in scene")]
    [SerializeField] protected Durability myHP;
    [Tooltip("The visual object for viewing HP")]
    [SerializeField] protected GameObject hpBar;

    // Start is called before the first frame update
    void Start()
    {
        myHP.onDmg += matchNewHP;
    }
    private void OnDestroy()
    {
        myHP.onDmg -= matchNewHP;
    }

    public void matchNewHP()
    {
        float newHP;
        // If we're "dead" then just make the bar empty instead of divding by 0 or negative
        if (myHP.hp <= 0f)
        {
            newHP = 0f;
        }
        // Otherwise shrink the bar relative to max HP
        else
        {
            newHP = myHP.maxHP / myHP.hp;
        }

        hpBar.transform.localScale.Set(1f, newHP, 1f);
    }

}
