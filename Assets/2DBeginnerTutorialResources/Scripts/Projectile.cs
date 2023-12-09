using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction,float force)
    {
        rigidbody2d.AddForce(direction*force);
    }

    
    // Update is called once per frame
    private void Update()
    {
        if (transform.position.magnitude>100)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("当前子弹碰撞到的游戏物体是："+collision.gameObject);
        EnemyController enemyController = collision.gameObject.
        GetComponent<EnemyController>();
        if (enemyController!=null)
        {
            enemyController.Fix();
            
        }
        Destroy(gameObject);
    }
}
