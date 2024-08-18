using UnityEngine;

public class Destructible : MonoBehaviour
{
    protected float m_MaxHP;
    protected float m_Damage;

    protected float _currentHP;

    public virtual void ApplyDamage(float damage)
    {
        _currentHP = Mathf.Clamp(_currentHP - damage, 0, m_MaxHP);
    }
}