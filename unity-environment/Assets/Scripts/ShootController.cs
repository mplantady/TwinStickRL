using MLAgents;
using UnityEngine;

public class ShootController : Agent {

    private const int RaycastEnemyCount = 16;
    private const int RaycastEnemyLength = 12;
    private const int RaycastEnemyMask = (1 << 10);

    private const int AimDirections = 16;
    private const int ShootOffset = 4;

    [SerializeField]
    private int _delayBetweenShoot = 15;

    [SerializeField]
    private GameObject _prefabProjectile;

    private bool _waitReset;
    private int _delayShoot = 0;
    private Academy _academy;
    private Rigidbody _rigidBody;

    public override void InitializeAgent(Academy academy)
    {
        base.InitializeAgent(academy);
        _academy = academy;

        _rigidBody = GetComponentInParent<Rigidbody>();
    }

    public override void CollectObservations()
    {
        PhysicHelper.RadialRaycasts(transform.position, RaycastEnemyMask, RaycastEnemyCount, RaycastEnemyLength, AddVectorObs);

        AddVectorObs(_delayShoot / (float)_delayBetweenShoot);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Being able to skip the result of an internal brain is usefull in early training stage of the external brain
        if (brain.brainType == BrainType.Internal && _academy.resetParameters["skip_internal_brain"] >= 1)
            return;

        if (_waitReset)
        {
            SetReward(-1f);
            Done();

            return;
        }

        int input = (int)vectorAction[0];
        if (input > 0 && _delayShoot > _delayBetweenShoot)
        {
            var fire = Instantiate(_prefabProjectile, transform.position, Quaternion.AngleAxis((360 / AimDirections) * (input-1), Vector3.up));
            fire.transform.position = fire.transform.position + fire.transform.forward * ShootOffset; 
           
            _delayShoot = 0;
        }
        _delayShoot++;

        AddReward(0.1f);
    }

    public void OnCollide()
    {
        _waitReset = true;
    }

    public override void AgentReset()
    {
        _waitReset = false;
    }
}
