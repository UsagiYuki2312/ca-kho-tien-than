using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Damage : BulletStatus
{
    [SerializeField] GameObject snail;
    public void Update()
    {
        transform.position = snail.transform.position;
    }
}
