using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject cannon;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform player;

    [SerializeField] private float attackRange;
    [SerializeField] private float attackCd;
    

    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;

    private void FixedUpdate()
    {
        if(Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            AttackPlayer();
        }
    }

    private bool isAiming = true;

    private void AttackPlayer()
    {
        cannon.transform.LookAt(player);
        CheckInSight();

    }

    private void CheckInSight()
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;

        if(isAiming && Physics.Raycast(ray, out hit))
        {
            if(isAiming && hit.collider.CompareTag("Player"))
            {
                isAiming = false;
                StartCoroutine(Attack());
            } 
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(.1f);
        Instantiate(bullet, shootPoint.position, shootPoint.rotation).GetComponent<Rigidbody>().velocity = shootPoint.forward * bulletSpeed;
        audioSource.Play();
        yield return new WaitForSeconds(attackCd);
        isAiming = true;
    }
}
