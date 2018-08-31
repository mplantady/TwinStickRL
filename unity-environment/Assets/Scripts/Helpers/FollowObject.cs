using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    private const float WeightOfOrigin = 3.0f;

    [SerializeField]
    public Transform _target;

    [SerializeField]
    public float _speed = 2;

	void FixedUpdate () {

        if (_target == null)
            return;

        Vector3 targetPos = (_target.position + Vector3.up * transform.position.y + new Vector3(0,120,0) * WeightOfOrigin) / (WeightOfOrigin + 1);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _speed);
	}
}
