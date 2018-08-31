using UnityEngine;

public class BouncyDeformer : MonoBehaviour
{
    private float randomStart;

    private void Awake()
    {
        randomStart = Random.Range(-3, 3);
    }

    void Update()
    {
        transform.localScale = new Vector3(1 + 0.1f * Mathf.Cos(randomStart + Time.realtimeSinceStartup * 5), 1 + 0.1f * Mathf.Cos(randomStart + 0.5f + Time.realtimeSinceStartup * 8), 1 + 0.1f * Mathf.Sin(randomStart + Time.realtimeSinceStartup * 10));
    }
}
