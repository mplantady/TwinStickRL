using UnityEngine;

public class RotationDeformer : MonoBehaviour
{
    [SerializeField]
    private int rotationSpeed = 2;

    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed);
    }

}
