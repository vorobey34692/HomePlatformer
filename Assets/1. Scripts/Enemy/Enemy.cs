using UnityEngine;
using DG.Tweening;
using Player;

#if UNITY_EDITOR

using UnityEditor;

#endif

public abstract class Enemy : MonoBehaviour, IDamageble
{
    [SerializeField] protected int Damage;
    [field: SerializeField] public int Health { get; protected set; }
    [SerializeField] private Renderer[] _enemyRenderer;
    [SerializeField] protected AudioSource DamageSound;
    [SerializeField] protected GameObject DieEffect;
    private CheckerPosition _checker;
    protected Transform PlayerTransform;

#if UNITY_EDITOR
    [field: SerializeField] public float VisibilityDistanceX { get; protected set; }
    [field: SerializeField] public float VisibilityDistanceY { get; protected set; }
#endif

    protected virtual void Start()
    {
        _checker = FindObjectOfType<CheckerPosition>();
        _checker.AddToPositionList(this);
        PlayerTransform = FindObjectOfType<PlayerMove>().transform;
    }

    private void Touch(Collider collider)
    {
        if (collider.attachedRigidbody != null)
            if (collider.attachedRigidbody.TryGetComponent(out PlayerHealth player))
                player.ApplayDamage(Damage);
    }

    public void ApplayDamage(int damage)
    {
        Health -= damage;
        PlayDamageEffects();
        PlayDamageSound();
        if (Health <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
        Instantiate(DieEffect, DamageSound.transform.position, Quaternion.identity, DamageSound.transform);
    }

    protected virtual void DieOnAnyCollision(Collider collider)
    {
        if (!collider.isTrigger)
            Die();
    }

    protected virtual void PlayDamageEffects()
    {
        if (_enemyRenderer == null) return;
        for (int i = 0; i < _enemyRenderer.Length; i++)
        {
            for (int j = 0; j < _enemyRenderer[i].materials.Length; j++)
            {
                Material material = _enemyRenderer[i].materials[j];
                material.SetColor("_EmissionColor", Color.clear);
                material.DOColor(Color.red, "_EmissionColor", 0.1f).SetLoops(10, LoopType.Yoyo).Restart(true);
            }
        }
    }

    protected virtual void PlayDamageSound()
    {
        if (DamageSound == null) return;
        if (Health <= 0)
        {
            DamageSound.transform.SetParent(null);
            Destroy(DamageSound.gameObject, DamageSound.clip.length);
            return;
        }
        DamageSound.Play();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Touch(other);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Touch(collision.collider);
    }

    protected virtual void OnDestroy()
    {
        _checker.RemoveToPositionList(this);
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.gray;
        Handles.DrawWireCube(transform.position, new Vector3(VisibilityDistanceX, VisibilityDistanceY, 0));
    }

#endif
}