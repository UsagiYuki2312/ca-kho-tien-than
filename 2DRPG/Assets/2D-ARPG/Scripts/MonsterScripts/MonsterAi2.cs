using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAi2 : MonsterAi
{
    [Header("Attack")]
    public GameObject Damage;
    protected override IEnumerator Attack()
    {
        Debug.LogWarning("Attafck");
        cancelAttack = false;
        if (!stat.flinch && !stat.freeze && !attacking)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            attacking = true;
            if (whileAttack == WhileAtkMon.MeleeFwd)
            {
                StartCoroutine(MeleeDash());
            }
            if (anim && attackAnimationTrigger[c] != "")
            {
                anim.SetTrigger(attackAnimationTrigger[c]);
            }
            LookAtTarget();
            if (aimAtTarget && followTarget)
            {
                //attackPoint.LookAt(followTarget.position);
                Vector3 dir = followTarget.position - attackPoint.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                attackPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            yield return new WaitForSeconds(attackCast);

            if (!cancelAttack)
            {
                if (attackVoice && !stat.flinch)
                {
                    GetComponent<AudioSource>().PlayOneShot(attackVoice);
                }
                Damage.GetComponent<BulletStatus>().Setting(stat.atk, stat.matk, "Enemy", this.gameObject);
                Vector3 dir = (followTarget.position - attackPoint.position).normalized;
               // GetComponent<Rigidbody2D>().velocity = new Vector2(dir.x, 0) * movSpd * 2;
                c++;
                if (c >= attackAnimationTrigger.Length)
                {
                    c = 0;
                }
                yield return new WaitForSeconds(attackDelay);
               // GetComponent<Rigidbody2D>().AddForce(Vector2.zero);
                attacking = false;
                CheckDistance();
                /*if(distance > approachDistance + 0.55f){
					c = 0;
				}*/
            }
            else
            {
                c = 0;
                attacking = false;
            }
        }
    }
}
