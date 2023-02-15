using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxHp = 100;
    [SerializeField] MeshRenderer healthGUI;

    [Header("Hp Colors")]
    [SerializeField] Color hp100;
    [SerializeField] Color hp75;
    [SerializeField] Color hp50;
    [SerializeField] Color hp25;
    [SerializeField] Color hp0;

    public float hp = 100;

    private void Start()
    {
        hp = maxHp;
    }

    public void GetHurt(float damage)
    {
        hp -= damage;
        healthGUI.material.EnableKeyword("_EMISSION");

        if(hp / maxHp * 100 <= 0)
        {
            healthGUI.material.SetColor("_EmissionColor", hp0);
        } else if (hp / maxHp * 100 <= 25)
        {
            healthGUI.material.SetColor("_EmissionColor", hp25);
        } else if (hp / maxHp * 100 <= 50)
        {
            healthGUI.material.SetColor("_EmissionColor", hp50);
        } else if (hp / maxHp * 100 <= 75)
        {
            healthGUI.material.SetColor("_EmissionColor", hp75);
        } else if (hp / maxHp * 100 <= 100)
        {
            healthGUI.material.SetColor("_EmissionColor", hp100);
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Ouch");
    }
}
