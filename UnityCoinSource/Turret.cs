using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject Bullet;
    public float FixedBulletDelay;
    public float BulletSpeed;
    public Transform BulletPosition;
    [Range (-1, 1)]
    public int Direction;
    public bool ShouldStartShooting;

    private float BulletDelay;

    private void Awake()
    {
        if (ShouldStartShooting)
        {
            BulletDelay = 0;
        } else
        {
            BulletDelay = FixedBulletDelay;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BulletDelay > 0)
        {
            BulletDelay -= Time.deltaTime;
        } else if (BulletDelay <= 0) {
            BulletDelay = FixedBulletDelay;
            ShootBullet();
        }   
    }

    private void ShootBullet()
    {
        GameObject ShotBullet = Instantiate(Bullet, BulletPosition.position, Quaternion.identity);
        ShotBullet.GetComponent<Rigidbody2D>().velocity = new Vector3(BulletSpeed*Direction, 0, 0);
    }

}
