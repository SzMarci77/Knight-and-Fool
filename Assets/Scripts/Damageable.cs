using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;

    Animator animator;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _health = 100;
    [SerializeField] private bool _isAlive = true;

    [Header("Invincibility Settings")]
    [SerializeField] private bool isInvincible = false;
    public float invincibilityTime = 0.25f;

    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }


    private float timeSinceHit = 0;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(Animations.isAlive, value);
            Debug.Log("IsAlive set " + value);

            if(value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }

    public int Health
    {
        get => _health;
        set
        {
            //Ne menjenjen a health 0 alá vagy maxHealth fölé
            _health = Mathf.Clamp(value, 0, MaxHealth);
            healthChanged?.Invoke(_health, MaxHealth);

            //if health = 0, character is no longer alive
            if (_health <= 0 && IsAlive)
            {
                IsAlive = false;
            }
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(Animations.lockVelocity);
        }
        set
        {
            animator.SetBool(Animations.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    //Kapott-e sebzést
    public bool Hit(int damage, Vector2 knockback)
    {
        if (_isAlive && !isInvincible)
        {
            Health -= damage;
            animator.SetTrigger(Animations.hitTrigger);
            LockVelocity = true;

            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            StartCoroutine(InvincibilityCoroutine());
            return true;
        }
        return false;
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed?.Invoke(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
