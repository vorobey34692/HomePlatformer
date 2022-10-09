using UnityEngine;

public class Heatrh : CollectebleObjects
{
    [SerializeField] private AudioSource _destroySound;
    private int _numberHealth = 1;

    private void Start()
    {
        Inicialize(_destroySound);
    }

    protected override void Test(PlayerHealth stats)
    {
        stats.AddHealth(_numberHealth);
        base.Test(stats);
    }
}
