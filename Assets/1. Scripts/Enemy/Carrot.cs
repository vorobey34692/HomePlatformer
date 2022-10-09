using UnityEngine;

public class Carrot : Enemy
{
    [SerializeField] private Rigidbody _carrotRigibody;
    [SerializeField] private float _carrotSpeedMove = 10;
    private int _damage = 1;
    private int _health = 1;

    private void Start()
    {
        Inicialize(_damage, _health);
        MoveToPlayer();
    }

    protected override void MoveToPlayer()
    {
        Vector3 toPlayer = (PlayerTransform.position - transform.position).normalized;
        _carrotRigibody.velocity = toPlayer * _carrotSpeedMove;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        Die();
    }
}
