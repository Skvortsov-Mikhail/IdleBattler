using UnityEngine;

[CreateAssetMenu]
public class AbilityConfiguration : ScriptableObject
{
    [Header("Enemies Parameters")]
    public int EnemiesAttackedAmount;
    public float CooldownToNextEnemy;
    public bool CanHitBoss;

    [Header("Attack Parameters")]
    public float Damage;
    public float DamageReductionMultiplierForEnemy;
    public float AttackRadius;
    public float Cooldown;

    [Header("Other")]
    public AudioClip AudioClip;
}