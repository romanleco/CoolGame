using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    [SerializeField] private int _attackPoints = 2;
    private bool _canAttack = true;
    private WaitForSeconds _attackCooldown = new WaitForSeconds(0.5f);
    private EnemyFly _enemyFlyScript;
    [SerializeField] private bool _destroysOnAttack;

    void Start()
    {
        if(_destroysOnAttack)
        {
            _enemyFlyScript = transform.parent.GetComponent<EnemyFly>();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && _canAttack)
        {
            GameManager.Instance.playerDamagableInterface.ReceiveDamage(_attackPoints);
            StartCoroutine("AttackCooldown");

            if(_destroysOnAttack)
            {
                _enemyFlyScript.ReceiveDamage(1000);
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return _attackCooldown;
        _canAttack = true;
    }
}
