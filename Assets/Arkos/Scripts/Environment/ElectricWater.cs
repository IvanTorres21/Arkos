using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWater : MonoBehaviour
{
    [SerializeField] private int damage;
    private const float TICK_SPEED = .1f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(DamagePlayer(other.gameObject.GetComponent<PlayerController>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
    }

    private IEnumerator DamagePlayer(PlayerController player)
    {
        while(true)
        {
            yield return new WaitForSeconds(TICK_SPEED);
            player.GetHurt(damage);
        }
    }


}
