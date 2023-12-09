using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PetMove : MonoBehaviour {
    public Transform player;//主角
    public float speed=1f;//移动的阻尼，值越小，移动越平缓
    // Use this for initialization
    private Vector2 lookDirection = new Vector2(1,0);
    private Animator animator;
    void Awake () {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal,vertical);
        //当前玩家输入的某个轴向值不为0
        if (!Mathf.Approximately(move.x,0)||!Mathf.Approximately(move.y,0))
        {
            lookDirection.Set(move.x,move.y);//为lookDirection赋值
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X",lookDirection.x);//将lookDirection赋值给Look X，反应给混合树
        //animator.SetFloat("Look Y",lookDirection.y);
        animator.SetFloat("Distance",Vector3.Distance(player.position,transform.position));//根据Speed的值（此语句为取模长）判断判断是idle或是move状态
        
        
        //如果主角和宠物直接的距离大于5，控制宠物跟随主角移动
        if(Vector3.Distance(player.position,transform.position)>1.0f){
            PetSmothFlow ();
            
        }

    }
    //控制宠物的位置平滑移动
    void PetSmothFlow(){
        transform.position=Vector3.Lerp (transform.position,player.position,Time.deltaTime*speed);
    }
}
