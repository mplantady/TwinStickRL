using UnityEngine;

public class StraightShot : MonoBehaviour {

	void FixedUpdate ()
    {
        transform.position = transform.position + transform.forward;
	}

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.1f);

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.Kill();
        }

        GetComponent<Collider>().isTrigger = true;
    }
}
