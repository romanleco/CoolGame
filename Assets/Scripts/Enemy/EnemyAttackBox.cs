using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    private int _attackPoints = 25;
    private bool _canAttack = true;
    private WaitForSeconds _attackCooldown = new WaitForSeconds(0.5f);
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && _canAttack)
        {
            GameManager.Instance.playerDamagableInterface.ReceiveDamage(_attackPoints);
            StartCoroutine("AttackCooldown");
        }
    }

    IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return _attackCooldown;
        _canAttack = true;
    }
}
