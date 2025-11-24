using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float lives;
    public int damage;
    public float minSpeed;
    public float maxSpeed;
    public float resourceReward;
}
