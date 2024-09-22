using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;


[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviourPunCallbacks
{
    [Header("Health Controller")]
    protected float currentLife;
    [SerializeField] protected float maxLife;
    protected bool isDead;

    [Header("Energy Controller")]
    protected float currentEnergy;
    [SerializeField] protected float maxEnergy;

    [Header("Movement")]
    protected NavMeshAgent agent;
    protected Vector3 destiny;
    [SerializeField] protected LayerMask clickableLayer;
    [SerializeField] protected float moveSpeed;


    [Header("Attack System")]
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform posAttack;
    protected bool isAttacking;
    
    [Space]
    [SerializeField] protected float simpleAttackRate;
    [SerializeField] protected float simpleAttackRange;
    [SerializeField] protected float simpleAttackDamage;
    protected float nextSimpleAttackTime;
    
    [Space]
    [SerializeField] protected float specialAttackRate;
    [SerializeField] protected float specialAttackRange;
    [SerializeField] protected float specialAttackDamage;
    protected float nextSpecialAttackTime;

    protected Transform target;


    [Header("Animations")]
    [Tooltip("You need to inform Hero's Mesh Animator Component")]
    [SerializeField] protected Animator animator;

    protected RaycastHit hit;

    #region Unity Methods
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializeCharacter();
    }
    protected virtual void Update()
    {
        if (!photonView.IsMine) 
            return;


        Animations(); 
        CheckPlayerInputs();
        Chase();
        Move();
        
    }
    #endregion

    #region Health Controller Methods
    protected void TakeDamage(float _value)
    {
        currentLife = Mathf.Max(currentLife - _value, 0);

        if(currentLife == 0) Death();        
    }

    protected void Heal(float _value)
    {
        currentLife = Mathf.Min(currentLife + _value, maxLife);
    }

    protected void Death()
    {
        isDead = true;
        animator.SetBool("isDead", isDead);
    }
    #endregion

    #region Energy Controller Methods
    protected void ConsumeEnergy(float _value)
    {
        currentEnergy= Mathf.Max(currentEnergy - _value, 0);
    }
    protected void RestoreEnergy(float _value)
    {
        currentEnergy= Mathf.Min(currentEnergy + _value, maxEnergy);
    }
    #endregion

    #region Attack System
    protected virtual void SimpleAttack()
    {
        nextSimpleAttackTime = Time.time + 1f/simpleAttackRate;
        transform.LookAt(target);
        //animator.SetTrigger("Attack");
        photonView.RPC("AttackLan", RpcTarget.All);
    }

    [PunRPC]
    public void AttackLan()
    {
        animator.SetTrigger("Attack");
    }

    [PunRPC]
    public void CancelActionLan()
    {
        animator.SetTrigger("CancelActions");
    }

    protected virtual void SpecialAttack()
    {
        nextSpecialAttackTime = Time.time + 1f/specialAttackRate;        
    }

    protected virtual void Chase()
    {
        if (target == null) return;

        float _distance = Vector3.Distance(transform.position, target.position);

        if (_distance > simpleAttackRange)
            destiny = target.position;
        else
        {
            destiny = transform.position;
            
            if(Time.time >= nextSimpleAttackTime)
                SimpleAttack();
        }
    }
    #endregion

    #region Others Methods
    protected virtual void Animations()
    {
        if(agent.velocity.magnitude > 0.1f)
            animator.SetFloat("moveSpeed", 1);
        else
            animator.SetFloat("moveSpeed", 0);
    }
    protected virtual void InitializeCharacter()
    {
        destiny = transform.position;
        agent.speed = moveSpeed;
    }
    protected virtual void CheckPlayerInputs()
    {        
        //Movement
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, clickableLayer))
            {
                string tag = hit.collider.tag;
                switch (tag)
                {
                    case "Player":
                        target = hit.collider.transform;
                        break;

                    default:
                        target = null;
                        //animator.SetTrigger("CancelActions");
                        photonView.RPC("CancelActionLan", RpcTarget.All);
                        destiny = hit.point;
                        break;
                }
            }
        }
        //Simple Attack
        else if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, enemyLayer))
            {                
                target = hit.collider.transform;
            }
        }

        //Stop Movement
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            target = null;
            destiny = transform.position;
            //animator.SetTrigger("CancelActions");
            photonView.RPC("CancelActionLan", RpcTarget.All);
        }

        //Special Attack

    }
    #endregion

    #region Movement
    protected virtual void Move()
    {
        agent.SetDestination(destiny);
    }
    #endregion

}
