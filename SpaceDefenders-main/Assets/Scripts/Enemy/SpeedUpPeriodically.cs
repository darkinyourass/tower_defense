using System.Collections;
using UnityEngine;

public class SpeedUpPeriodically : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyData _data;
    private bool _isSpedUp;

    [SerializeField] private float multiplier = 3f;
    [SerializeField] private float minInterval = 0.4f;
    [SerializeField] private float maxInterval = 1.8f;

    private AssignRandomSprite _spriteController;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        if (_enemy != null)
        {
            _data = _enemy.Data;
        }
        _spriteController = GetComponentInChildren<AssignRandomSprite>();
    }

    private void OnEnable()
    {
        if (_enemy != null && _data != null)
        {
            StartCoroutine(SpeedCycleRoutine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (_enemy != null && _data != null)
        {
            _enemy.CurrentSpeed = Random.Range(_data.minSpeed, _data.maxSpeed);
        }
    }

    private IEnumerator SpeedCycleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            if (_isSpedUp)
            {
                _enemy.CurrentSpeed = Random.Range(_data.minSpeed, _data.maxSpeed);
                _spriteController?.SetAlternate(false);
            }
            else
            {
                float normalSpeed = Random.Range(_data.minSpeed, _data.maxSpeed);
                _enemy.CurrentSpeed = normalSpeed * multiplier;
                _spriteController?.SetAlternate(true); 
            }

            _isSpedUp = !_isSpedUp;
        }
    }
}
