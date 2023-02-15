using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour
{
    [SerializeField] private Animator animArm;
    [SerializeField] private Animator animPanel;

    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("AYO?");
        if(other.CompareTag("Player"))
        {
            animArm.SetTrigger("IsPlayerNear");
            animPanel.SetTrigger("IsPlayerNear");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animArm.SetTrigger("IsPlayerAway");
            animPanel.SetTrigger("IsPlayerAway");
        }
    }
}
