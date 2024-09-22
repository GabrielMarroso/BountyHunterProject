using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed;
    public float damage;
    public Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 targetPos = target.position;
        targetPos.y = 1f;
        transform.LookAt(targetPos);
        rb.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target.gameObject)
        {
            Debug.Log("Deal damage!");
        }
        Destroy(gameObject);
    }

    public void SetBullet(Transform _target,float _damage)
    {
        target = _target;
        damage = _damage;
    }

}
