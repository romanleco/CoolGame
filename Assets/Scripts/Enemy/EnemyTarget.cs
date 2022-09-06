using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : Enemy
{
    private WaitForSeconds _colorTimer = new WaitForSeconds(0.15f);
    private SpriteRenderer _spriteRenderer;
    protected override void Start()
    {
        base.Start();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("EnemyTarget::Start() SpriteRenderer _spriteRenderer is null");
        }
    }

    public override void ReceiveDamage(int damage)
    {
        StartCoroutine(ChangeColor());
        Debug.Log(Health);
        base.ReceiveDamage(damage);
    }

    IEnumerator ChangeColor()
    {
        _spriteRenderer.color = Color.red;
        yield return _colorTimer;
        _spriteRenderer.color = Color.white;
    }
}
