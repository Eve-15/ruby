using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RuByController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public int maxHealth = 5;//Ruby最大生命值的定义
    public int currentHealth;//Ruby的当前生命值
    public int speed=3;//Ruby的速度

    public int Health { get { return currentHealth; } }//属性的使用提高代码安全性对于currentHealth只能读取不能修改

    //Ruby的无敌时间
    public float timeInvincible = 2.0f;//无敌时间常量
    private bool isInvincible;
    private float invincibleTimer;//计时器

    private Vector2 lookDirection = new Vector2(1,0);
    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;

    public AudioSource walkAudioSource;
    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;

    private Vector3 respawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d=GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        /*int a = GetRubyHealthValue();
        Debug.Log("Ruby当前的血量是:"+a);*/
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //玩家输入监听
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal,vertical);
        //当前玩家输入的某个轴向值不为0
        if (!Mathf.Approximately(move.x,0)||!Mathf.Approximately(move.y,0))
        {
            lookDirection.Set(move.x,move.y);//为lookDirection赋值
            lookDirection.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.Play();
            }
        }
        else
        {
            walkAudioSource.Stop();
        }
        //动画的控制
        animator.SetFloat("Look X",lookDirection.x);//将lookDirection赋值给Look X，反应给混合树
        animator.SetFloat("Look Y",lookDirection.y);
        animator.SetFloat("Speed",move.magnitude);//根据Speed的值（此语句为取模长）判断判断是idle或是move状态

        //移动
        Vector2 position = transform.position;
        // //RuBy水平方向移动
        // position.x = position.x + speed*horizontal*Time.deltaTime;
        // //RuBy垂直方向移动
        // position.y = position.y + speed*vertical*Time.deltaTime;
        //Ruby位置的移动
        position = position + speed * move * Time.deltaTime;
       // transform.position = position;
       rigidbody2d.MovePosition(position);
       //无敌时间计算
       if (isInvincible)
       {
           invincibleTimer = invincibleTimer - Time.deltaTime;
           if (invincibleTimer<=0)
           {
              isInvincible = false;
           }
       }
       //修理机器人的方法(攻击)
       if (Input.GetKeyDown(KeyCode.H))
       {
        Launch();
       }
       if (Input.GetKeyDown(KeyCode.R))
       {
          Restart();
       }

       //检测是否与NPC对话
       if (Input.GetKeyDown(KeyCode.T))
       {
        new Vector2(0,1);
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position+
        Vector2.up*0.2f,lookDirection,1.5f,LayerMask.GetMask("NPC"));
        if (hit.collider!=null)
        {
            //Debug.Log("当前射线检测碰到的游戏物体是："+hit.collider.gameObject);
            NPCDialog npcDialog=hit.collider.GetComponent<NPCDialog>();
            if (npcDialog!=null)
            {
                npcDialog.DisplayDialog();
            }
        }
       }
    }

    public void ChangeHealth(int amount)
    {
        if (amount<0)
        {
            if (isInvincible)
            {
                return;
            }

            //受到伤害
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(playerHit);
        }
        currentHealth=Mathf.Clamp(currentHealth+amount,0,maxHealth);//限制血量在0~5内
        //Debug.Log(currentHealth+"/" +maxHealth);

        if (currentHealth<=0)
        {
            Respawn();
        }
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
    }

    /*public int GetRubyHealthValue()
    {
        return currentHealth;
    }*/

    private void Launch()
    {
        if (!UIHealthBar.instance.hasTask)
        {
            return;
        }
        GameObject projectileObject=Instantiate(projectilePrefab,
        rigidbody2d.position+Vector2.up*0.5f,Quaternion.identity);//克隆预制体
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection,500);
        animator.SetTrigger("Launch");
        PlaySound(attackSoundClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition;
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;//continue
    }
}
