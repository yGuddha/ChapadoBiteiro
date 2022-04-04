using UnityEngine;
using System;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float movementSpeed;
    [SerializeField] float movementSpeedModifiers;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpForceModifiers;
    [SerializeField] float life;
    [SerializeField] float maxLife;
    [SerializeField] float maxLifeModifiers;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileSpeedModifiers;
    [SerializeField] float projectileLifeTime;
    [SerializeField] float projectileLifeTimeModifiers;
    [SerializeField] float damage;
    [SerializeField] float damageModifiers;

    public Action<float> OnLifeChange;
    public Action OnDeath;

    public int LookDir = 1;

    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public float MovementSpeedModifiers
    {
        get => movementSpeedModifiers;
        set => movementSpeedModifiers = value;
    }

    public float JumpForce
    {
        get => jumpForce;
        set => jumpForce = value;
    }

    public float JumpForceModifiers
    {
        get => jumpForceModifiers;
        set => jumpForceModifiers = value;
    }

    public float Life
    {
        get => life;
        set
        {
            life = value;
            OnLifeChange?.Invoke(life);

            if(life <= 0)
            {
                Destroy(this.gameObject);
                OnDeath?.Invoke();
            }
        }
    }

    public float ProjectileSpeed 
    {
        get => projectileSpeed;
        set => projectileSpeed = value;
    }

    public float ProjectileSpeedModifiers 
    {
        get => projectileSpeedModifiers;
        set => projectileSpeedModifiers = value;
    }

    public float ProjectileLifeTime
    {
        get => projectileLifeTime;
        set => projectileLifeTime = value;
    }

    public float ProjectileLifeTimeModifiers 
    {
        get => projectileLifeTimeModifiers;
        set => projectileLifeTimeModifiers = value;
    }

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    public float DamageModifiers
    {
        get => damageModifiers;
        set => damageModifiers = value;
    }
}
