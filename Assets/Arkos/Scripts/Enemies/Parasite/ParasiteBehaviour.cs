using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ParasiteBehaviour
{
    protected enum STATE { 
        IDLE,
        PURSUE,
        JUMP,
        GRABBING,
        GRABBED,
        RECOVER
    }

    protected STATE name;
    protected EVENT stage;
    protected ParasiteBehaviour nextState;

    protected GameObject npc;
    protected Transform player;
    protected Transform playerFace;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected OVRGrabbable grabbable;

    protected float pursueRange = 10f;

    public ParasiteBehaviour(GameObject npc, Transform player, Animator anim, NavMeshAgent agent, OVRGrabbable grabbable, Transform playerFace)
    {
        this.npc = npc;
        this.player = player;
        this.anim = anim;
        this.agent = agent;
        this.grabbable = grabbable;
        this.playerFace = playerFace;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public ParasiteBehaviour Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    protected bool CheckPlayerInFront(Transform player)
    {
        Vector3 direction = player.position - npc.transform.position;

        float angle = Vector3.Angle(npc.transform.forward, direction);

        if (Vector3.Distance(player.position, npc.transform.position) < pursueRange && angle < 90f)
        {
            return true;
        }

        return false;
    }

    protected bool CheckGrabbed()
    {
        return grabbable.isGrabbed;
    }
}

public class ParasiteIdle : ParasiteBehaviour
{
    public ParasiteIdle(GameObject npc, Transform player, Animator anim, NavMeshAgent agent, OVRGrabbable grabbable, Transform playerFace) : base(npc, player, anim, agent, grabbable, playerFace)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("IsIdle");
        base.Enter();
    }

    public override void Exit()
    {
        anim.ResetTrigger("IsIdle");
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(CheckPlayerInFront(player))
        {
            nextState = new ParasitePursue(npc, player, anim, agent, grabbable, playerFace);
            stage = EVENT.EXIT;
        }

        if (CheckGrabbed())
        {
            nextState = new ParasiteGrabbed(npc, player, anim, agent, grabbable, playerFace);
            stage = EVENT.EXIT;
        }
    }
}

public class ParasitePursue : ParasiteBehaviour
{
    public ParasitePursue(GameObject npc, Transform player, Animator anim, NavMeshAgent agent, OVRGrabbable grabbable, Transform playerFace) : base(npc, player, anim, agent, grabbable, playerFace)
    {
        name = STATE.PURSUE;
    }

    public override void Enter()
    {
        anim.SetTrigger("IsWalking");
        agent.isStopped = false;
        agent.speed = 6;
        base.Enter();
    }

    public override void Exit()
    {
        anim.ResetTrigger("IsWalking");
        agent.isStopped = true;
        agent.speed = 0;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(agent.enabled)
        {
            Vector3 pos = player.position;
            if ((npc.transform.rotation.eulerAngles.x > 260 && 280 > npc.transform.rotation.eulerAngles.x) || (npc.transform.rotation.eulerAngles.x > 80 && 100 > npc.transform.rotation.eulerAngles.x))
            {
                pos.z = npc.transform.position.z;
            }
            else if ((npc.transform.rotation.eulerAngles.z > 260 && 280 > npc.transform.rotation.eulerAngles.z) || (npc.transform.rotation.eulerAngles.z > 80 && 100 > npc.transform.rotation.eulerAngles.z))
            {

                pos.x = npc.transform.position.x;
            }
            else
            {
                pos.y = npc.transform.position.y;
            }

            if (Vector3.Distance(npc.transform.position, player.position) < 3f)
            {
                nextState = new ParasiteJump(npc, player, anim, agent, grabbable, playerFace);
                stage = EVENT.EXIT;

            }


            if (CheckGrabbed())
            {
                nextState = new ParasiteGrabbed(npc, player, anim, agent, grabbable, playerFace);
                stage = EVENT.EXIT;
            }

            agent.SetDestination(pos);
        }
    }
}

public class ParasiteJump : ParasiteBehaviour
{
    Rigidbody rb;

    public ParasiteJump(GameObject npc, Transform player, Animator anim, NavMeshAgent agent, OVRGrabbable grabbable, Transform playerFace) : base(npc, player, anim, agent, grabbable, playerFace)
    {
        name = STATE.JUMP;
    }

    public override void Enter()
    {
        rb = npc.GetComponent<Rigidbody>();
        anim.SetTrigger("IsJump");
        agent.enabled = false;
        rb.useGravity = true;
        JumpTo();
        base.Enter();
    }

    public override void Exit()
    {
        rb.useGravity = false;
        anim.ResetTrigger("IsJump");
        agent.enabled = true;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(npc.transform.parent == playerFace)
        {
            nextState = new ParasiteGrabbing(npc, player, anim, agent, grabbable, playerFace);
            stage = EVENT.EXIT;
        }

        if (CheckGrabbed())
        {
            nextState = new ParasiteGrabbed(npc, player, anim, agent, grabbable, playerFace);
            stage = EVENT.EXIT;
        }
    }

    private void JumpTo()
    {

        Vector3 p = playerFace.position;



        float gravity = Physics.gravity.magnitude;

        // Selected angle in radians

        float angle = 40 * Mathf.Deg2Rad;



        // Positions of this object and the target on the same plane

        Vector3 planarTarget = new Vector3(p.x, 0, p.z);

        Vector3 planarPostion = new Vector3(npc.transform.position.x, 0, npc.transform.position.z);



        // Planar distance between objects

        float distance = Vector3.Distance(planarTarget, planarPostion);

        // Distance along the y axis between objects

        float yOffset = npc.transform.position.y - p.y;



        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));



        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));



        // Rotate our velocity to match the direction between the two objects

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;



        // Fire!

        rb.velocity = finalVelocity;
    }
}

public class ParasiteGrabbed : ParasiteBehaviour
{
    

    public ParasiteGrabbed(GameObject npc, Transform player, Animator anim, NavMeshAgent agent, OVRGrabbable grabbable, Transform playerFace) : base(npc, player, anim, agent, grabbable, playerFace)
    {
        name = STATE.GRABBED;
    }

    public override void Enter()
    {
        anim.SetTrigger("IsGrabbed");
        base.Enter();
    }

    public override void Exit()
    {
        anim.ResetTrigger("IsGrabbed");
        npc.transform.parent = null;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!CheckGrabbed())
        {
            nextState = new ParasiteRecover(npc, player, anim, agent, grabbable, playerFace);
            stage = EVENT.EXIT;
        }
    }
}

public class ParasiteGrabbing : ParasiteBehaviour
{
    private float time = 0f;

    public ParasiteGrabbing(GameObject npc, Transform player, Animator anim, NavMeshAgent agent, OVRGrabbable grabbable, Transform playerFace) : base(npc, player, anim, agent, grabbable, playerFace)
    {
        name = STATE.GRABBING;
    }

    public override void Enter()
    {
        anim.SetTrigger("IsGrabbing");
        npc.transform.GetChild(1).gameObject.SetActive(true);
        base.Enter();
    }

    public override void Exit()
    {
        anim.ResetTrigger("IsGrabbing");
        npc.transform.GetChild(1).gameObject.SetActive(false);
        
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;
        if(time >= 1f)
        {
            time = 0f;
            player.gameObject.GetComponent<PlayerController>().GetHurt(10);
        }
        if (CheckGrabbed())
        {
            nextState = new ParasiteGrabbed(npc, player, anim, agent, grabbable, playerFace);
            stage = EVENT.EXIT;
        }
    }
}

public class ParasiteRecover : ParasiteBehaviour
{
    float time = 0f;

    public ParasiteRecover(GameObject npc, Transform player, Animator anim, NavMeshAgent agent, OVRGrabbable grabbable, Transform playerFace) : base(npc, player, anim, agent, grabbable, playerFace)
    {
        name = STATE.RECOVER;
    }

    public override void Enter()
    {
        anim.SetTrigger("IsIdle");
        base.Enter();
    }

    public override void Exit()
    {
        anim.ResetTrigger("IsIdle");
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;
        if (time > 1f)
        {
            nextState = new ParasitePursue(npc, player, anim, agent, grabbable, playerFace);
            stage = EVENT.EXIT;
        }
    }
}
