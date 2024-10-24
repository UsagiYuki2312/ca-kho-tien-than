using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveSnail : BulletMove
{
    protected override void Start()
    {
        hitEffect = GetComponent<BulletStatus>().hitEffect;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        if (GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        /*if(fwdPlusAfterSpawn != 0 ){
			Vector3 absoluteDirection = transform.rotation * relativeDirection;
			transform.position += absoluteDirection * fwdPlusAfterSpawn;
		}*/
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall" && !passthroughWall)
        {
            if (hitEffect)
            {
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
            if (GetComponent<BulletStatus>().bombHitSetting.enable)
            {
                GetComponent<BulletStatus>().ExplosionDamage();
            }
        }
    }
}
