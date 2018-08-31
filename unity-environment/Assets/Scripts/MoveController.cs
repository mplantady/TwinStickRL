using MLAgents;
using UnityEngine;

public class MoveController : Agent {

    private const int RaycastEnemyCount = 16;
    private const int RaycastEnemyLength = 12;
    private const int RaycastEnemyMask = (1 << 10);

    private const int RaycastWallCount = 8;
    private const int RaycastWallLength = 14;
    private const int RaycastWallMask = (1 << 8);

    public const float MaxVelocity = 50;
    public const float Acceleration = 6f;

    private Rigidbody _rigidBody;
    private bool _waitReset;
    private Academy _academy;

    public override void InitializeAgent(Academy academy)
    {
        base.InitializeAgent(academy);
        _academy = academy;

        _rigidBody = GetComponentInParent<Rigidbody>();
    }

    public override void CollectObservations()
    {
        Vector3 pos = transform.position;

        // Proximity raycast to find enemies
        PhysicHelper.RadialRaycasts(pos, RaycastEnemyMask, RaycastEnemyCount, RaycastEnemyLength, AddVectorObs);

        // Additional raycasts at a further distance to help determine best global direction (a heatmap could probably be better here)
        PhysicHelper.RadialRaycasts(pos, RaycastEnemyMask, RaycastEnemyCount, RaycastEnemyLength, AddVectorObs, RaycastEnemyLength + 1);

        // Detect Walls
        PhysicHelper.RadialRaycasts(pos, RaycastWallMask, RaycastWallCount, RaycastWallLength, AddVectorObs);

        AddVectorObs(_rigidBody.velocity.x / MaxVelocity);
        AddVectorObs(_rigidBody.velocity.z / MaxVelocity);
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
        Vector3 newVelocity = _rigidBody.velocity;
        if (input == 1)
        {
            newVelocity = new Vector3(_rigidBody.velocity.x + Acceleration, 0, _rigidBody.velocity.z);
        }

        if (input == 2)
        {
            newVelocity = new Vector3(_rigidBody.velocity.x - Acceleration, 0, _rigidBody.velocity.z);
        }

        if (input == 3)
        {
            newVelocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z + Acceleration);
        }

        if (input == 4)
        {
            newVelocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z - Acceleration);
        }

        if (newVelocity.magnitude > MaxVelocity)
        {
            newVelocity = newVelocity.normalized * MaxVelocity;
        }

        _rigidBody.velocity = newVelocity;

        AddReward(0.1f);
    }

    public void OnCollide()
    {
        _waitReset = true;
    }

    public override void AgentReset()
    {
        _waitReset = false;
        _rigidBody.velocity = Vector3.zero;
    }
}
