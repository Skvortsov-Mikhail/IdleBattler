﻿using UnityEngine;

[CreateAssetMenu]
public class EnemyConfiguration : ScriptableObject
{
    public float MaxHP;
    public float Damage;
    public float AttackTime;
    public float MovementSpeed;

    public float RespawnTime;

    public Sprite VisualModel;
}