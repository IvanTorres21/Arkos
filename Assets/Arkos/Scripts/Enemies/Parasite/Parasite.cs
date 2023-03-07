using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Parasite : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform playerFace;
    Animator anim;
    NavMeshAgent agent;
    OVRGrabbable grabbable;
    Rigidbody rb;

    ParasiteBehaviour currentState;

    [SerializeField] OVRGrabber leftHand;
    [SerializeField] OVRGrabber rightHand;

    private int hp = 2;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<OVRGrabbable>();
        currentState = new ParasiteIdle(this.gameObject, player, anim, agent, grabbable, playerFace);
    }

    private void Update()
    {
        currentState = currentState.Process();

        if(grabbable.isGrabbed && agent.enabled)
        {
            rb.useGravity = true;
            agent.enabled = false;
            transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, -2, transform.GetChild(0).localPosition.z);
        }

        if(grabbable.isGrabbed)
        {
           
            OVRInput.SetControllerVibration(.1f, .5f, grabbable.grabbedBy == leftHand ? OVRInput.Controller.LHand : OVRInput.Controller.RHand);
        } else if (inFace)
        {
            transform.localPosition = new Vector3(0, 0f, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Weapon"))
        {
            hp -= 1;
            GetComponentInChildren<ParticleSystem>().Play();
            if(hp < 0)
            {
                Destroy(gameObject);
            }
        }
        else if (!collision.collider.CompareTag("Player") && agent.enabled == false)
        {
            agent.enabled = true;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            
            transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, 0, transform.GetChild(0).localPosition.z);
        }
    }


    private bool inFace = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Face") && other.gameObject.transform.childCount == 0)
        {
            inFace = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.parent = other.gameObject.transform;
            transform.localPosition = new Vector3(0, 0f, 0);
            transform.localRotation = Quaternion.Euler(new Vector3(100, transform.localRotation.y, transform.localRotation.z));
        }
    }
}
