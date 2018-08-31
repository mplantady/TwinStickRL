using UnityEngine;

public class ShipAgent : MonoBehaviour
{
    [SerializeField]
    private Transform _visual;

    [SerializeField]
    private MoveController _moveController;

    [SerializeField]
    private ShootController _shootController;

    private Arena _arena;
    private Rigidbody _rigidBody;

    public Arena Arena
    {
        get { return _arena;}
        set { _arena = value;}
    }

    public ShootController ShootController
    {
        get { return _shootController; }
    }

    public MoveController MoveController
    {
        get { return _moveController; }
    }

    public void Awake()
	{
        _rigidBody = GetComponentInParent<Rigidbody>();
	}

	public bool IsDone()
    {
        return false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("killZone"))
        {
            _moveController.OnCollide();
            _shootController.OnCollide();

            _arena.ArenaReset();
            _visual.GetComponent<TrailRenderer>().Clear();
            _rigidBody.velocity = Vector3.zero;
        }
    }

    public void Update()
    {
        if (_rigidBody.velocity.magnitude > 0.1f)
        {
            _visual.rotation = Quaternion.LookRotation(_rigidBody.velocity);
        }
    }

}
