using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed=3;

    private Rigidbody2D rigidbody2d;
    //轴向控制
    public bool vertical;
    //方向控制
    private int direction = 1;
    //方向改变间隔，常量
    public float changeTime = 3;
    //计时器
    private float timer;

    private Animator animator;
    //当前敌人是否故障
    private bool broken;

    public ParticleSystem smokeEffect;

    private AudioSource audioSource;

    public AudioClip fixedSound;

    public AudioClip[] hitSounds;
    public GameObject hitEffectParticle;
    // Start is called before the first frame update
    void Start()
    {
        
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator=GetComponent<Animator>();
        // animator.SetFloat("MoveX",direction);
        // animator.SetBool("Vertical",vertical);
        PlayMoveAnimation();
        broken = true;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!broken)
        {
            //已修好，那么不再移动
            return;
        }
        timer -= Time.deltaTime;

        if (timer<0)
        {
            direction=-direction;
            //animator.SetFloat("MoveX",direction);
            timer = changeTime;
            PlayMoveAnimation();
        }

        Vector2 position = rigidbody2d.position;

        if (vertical)//垂直轴向
        {
            position.y=position.y + Time.deltaTime * speed * direction;
        }
        else//水平轴向
        {
            position.x=position.x + Time.deltaTime * speed * direction;
        }
        rigidbody2d.MovePosition(position);
    }
    //触发检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RuByController rubyController = collision.gameObject.GetComponent<RuByController>();
        if (rubyController!=null)
        {
            rubyController.ChangeHealth(-1);
            
        }
    }
    //控制移动动画的方法
    private void PlayMoveAnimation()
    {
        if (vertical)//垂直轴向动画的控制
        {
            animator.SetFloat("MoveX",0);
            animator.SetFloat("MoveY",direction);
        }
        else//水平轴向动画的控制
        {
            animator.SetFloat("MoveX",direction);
            animator.SetFloat("MoveY",0);
        }
    }
     
    //修复机器人的函数
    public void Fix()
    {
        Instantiate(hitEffectParticle,transform.position,Quaternion.identity);
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        int randomNum = Random.Range(0,2);
        audioSource.Stop();
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(hitSounds[randomNum]);
        audioSource.PlayOneShot(fixedSound);
        Invoke("PlayFixedSound",1f);
        UIHealthBar.instance.fixedNum++;
    }
    private void PlayFixedSound()
    {
        audioSource.PlayOneShot(fixedSound);
    }
}
