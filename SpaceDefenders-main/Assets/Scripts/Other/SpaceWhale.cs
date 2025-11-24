using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpaceWhale : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float minX = -15f;
    [SerializeField] private float maxX = 15f;
    [SerializeField] private float minSpeed = 0.2f;
    [SerializeField] private float maxSpeed = 0.5f;
    [SerializeField] private float maxYOffset = 4f;

    [Header("Animation Settings")]
    [SerializeField] private string[] animationStates;

    private Animator _animator;
    private float _speed;
    private float _startY;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _startY = transform.position.y;
        InitializePosition();
        InitializeAnimation();
    }


    void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
        if (transform.position.x >= maxX)
        {
            ResetPosition();
            ResetAnimation();
        }
    }

    private void InitializePosition()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(-maxYOffset, maxYOffset - 2f);
        transform.position = new Vector3(randomX, _startY + randomY, transform.position.z);
    }

    private void ResetPosition()
    {
        float randomYOffset = Random.Range(-maxYOffset, maxYOffset - 2f);
        transform.position = new Vector2(minX, _startY + randomYOffset);
        _speed = Random.Range(minSpeed, maxSpeed);
    }

    private void InitializeAnimation()
    {
        if (animationStates.Length == 0) return;
        string startAnim = animationStates[Random.Range(0, animationStates.Length)];
        _animator.Play(startAnim, 0, 0f);
    }

    private void ResetAnimation()
    {
        if (animationStates.Length == 0) return;
        string nextAnim = animationStates[Random.Range(0, animationStates.Length)];
        _animator.Play(nextAnim, 0, 0f);
    }
}
