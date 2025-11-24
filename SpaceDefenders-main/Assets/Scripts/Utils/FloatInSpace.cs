using UnityEngine;

public class FloatInSpace : MonoBehaviour
{
    const float MOVE_AMPLITUDE = 0.1f;
    const float MOVE_SPEED = 0.5f;

    private Vector3 _startLocalPos;
    private Vector3 _seed;

    void Start()
    {
        _startLocalPos = transform.localPosition;
        _seed = new Vector3(Random.value * 100f, Random.value * 100f, 0f);
    }

    void Update()
    {
        float t = Time.time * MOVE_SPEED;
        float offsetX = (Mathf.PerlinNoise(t + _seed.x, 0) - 0.5f) * 2f * MOVE_AMPLITUDE;
        float offsetY = (Mathf.PerlinNoise(0, t + _seed.y) - 0.5f) * 2f * MOVE_AMPLITUDE;

        transform.localPosition = _startLocalPos + new Vector3(offsetX, offsetY, 0);
    }
}
