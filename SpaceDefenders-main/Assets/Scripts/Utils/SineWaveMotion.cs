using UnityEngine;

public class SineWaveMotion : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.2f; // how far it moves
    [SerializeField] private float frequency = 2f; // how fast it oscillates
    [SerializeField] private Vector3 axis = Vector3.up;

    private Vector3 _startLocalPos;

    void Start()
    {
        _startLocalPos = transform.localPosition;
    }


    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = _startLocalPos + axis.normalized * offset;
    }
}
