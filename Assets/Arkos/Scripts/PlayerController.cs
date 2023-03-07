using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxHp = 100;
    [SerializeField] MeshRenderer healthGUI;
    [SerializeField] CharacterController characterController;

    [Header("Hp Colors")]
    [SerializeField] Color hp100;
    [SerializeField] Color hp75;
    [SerializeField] Color hp50;
    [SerializeField] Color hp25;
    [SerializeField] Color hp0;

    [Header("Grabbers")]
    [SerializeField] private OVRGrabber right;
    [SerializeField] private OVRGrabber left;

    public float hp = 100;

    private void Start()
    {
        hp = maxHp;
    }

    private void Update()
    {
        if (right.grabbedObject.CompareTag("CrawlSpace"))
            Crawl(right);
        else if (left.grabbedObject.CompareTag("CrawlSpace"))
        {
            Crawl(left);
        }

        if(newCrawlPosition != Vector3.zero)
        {
            if(Vector3.Distance(transform.position, newCrawlPosition) >= .1f)
            {
                characterController.Move(direction);
            }
        }
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


    private Vector3 newCrawlPosition = Vector3.zero;
    Vector3 direction;
    private void Crawl(OVRGrabber hand)
    {
        direction = hand.transform.position - hand.beginPosition;
        direction.y = 0;

        if(Vector3.Distance(hand.transform.position, hand.beginPosition) >= .5f)
        {
            hand.GrabEnd();
            newCrawlPosition = transform.position - direction;
        }

    }
}
