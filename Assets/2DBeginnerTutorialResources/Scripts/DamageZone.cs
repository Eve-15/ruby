using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        RuByController ruByController=collision.GetComponent<RuByController>();
        //当前发生触发检测的游戏物体对象身上是否有RuByController脚本
        if(ruByController!=null)
        {
            ruByController.ChangeHealth(-1);
            
        }

    }
}
