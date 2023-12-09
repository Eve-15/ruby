using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip audioClip;

    public GameObject effectParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RuByController ruByController=collision.GetComponent<RuByController>();
        //当前发生触发检测的游戏物体对象身上是否有RuByController脚本
        if(ruByController!=null)
        {
            //有RuByController脚本
            //Ruby是否满血
            if (ruByController.currentHealth<ruByController.maxHealth)
            {
                //Ruby是不满血状态
                 //ruByController.Health=ruByController.Health+1;
                 ruByController.ChangeHealth(1);
                 ruByController.PlaySound(audioClip);
                 Instantiate(effectParticle,transform.position,Quaternion.identity);
                 Destroy(gameObject);
            }
           
        }
        
    }
}
