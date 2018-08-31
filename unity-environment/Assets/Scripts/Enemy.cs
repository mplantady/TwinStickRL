using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

    [SerializeField]
    public float speed;

    [SerializeField]
    private bool _randomDirection;

    [SerializeField]
    private GameObject _explodePrefab;

    private Transform _target;
    private Rigidbody _rigidBody;

    private bool _kill;

    public void Initialize(Transform target)
    {
        _rigidBody = GetComponent<Rigidbody>();
        _target = target;

        if (_randomDirection)
            transform.rotation = Quaternion.AngleAxis(Random.Range(-180, 180), Vector3.up);
    }

    public void UpdateStep() 
    {
        if (_randomDirection)
            _rigidBody.velocity = transform.forward.normalized * speed;
        else
            _rigidBody.velocity = (_target.position - transform.position).normalized * speed;
    }

    public void Kill()
    {
        _kill = true;
        Instantiate(_explodePrefab, transform.position, Quaternion.identity);
    }

    public bool IsAlive()
    {
        return !_kill;
    }
}
