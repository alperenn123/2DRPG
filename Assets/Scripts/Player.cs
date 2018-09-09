using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour {
    public float speed;
    Animator anim_control;
    public Image[] hearth_container;
    public int max_health;
    public int current_health;
    public bool IsInvinsicable;
    public GameObject sword;
    public int thrust_power;
    public bool CanMove;
    public bool CanAttack;
    float timer = 1.0f;
    SpriteRenderer Lrenderer;
    public GameObject projectile_particle;
	// Use this for initialization
	void Start () {
        anim_control = GetComponent<Animator>();
        CanMove = true;
        if(PlayerPrefs.HasKey("max_health"))
        {
            loadGame();
            Debug.Log("inside");
        }
        else
        {
            current_health = max_health;
        }
        
        draw_health();
        CanAttack = true;
        IsInvinsicable = false;
        Lrenderer = gameObject.GetComponent<SpriteRenderer>();
       

    }
	
	// Update is called once per frame
	void Update () {
        if (current_health <= 0)
        {
            SceneManager.LoadScene(0);
        }
        draw_health();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {        
           
            attack(thrust_power);
        }

        movement();
        if (IsInvinsicable)
        {
            timer -= Time.deltaTime;
            int flicker_num = Random.Range(0, 100);
            if (flicker_num <= 50) Lrenderer.enabled = false;
            if (flicker_num > 50) Lrenderer.enabled = true;
            if (timer <= 0)
            {
                timer = 1.0f;
                IsInvinsicable = false;
                Lrenderer.enabled = true;
            }
                
        }
    }

    void movement()
    {
        if (!CanMove)
            return;


            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
                anim_control.SetInteger("dir", 0);
                anim_control.speed = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
                anim_control.SetInteger("dir", 1);
                anim_control.speed = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                anim_control.SetInteger("dir", 2);
                anim_control.speed = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
                anim_control.SetInteger("dir", 3);
                anim_control.speed = 1;
            }
            else
            {
                anim_control.speed = 0;
            }
        
    }

    void draw_health()
    {
        for(int i =0; i<hearth_container.Length; i++)
        {
            hearth_container[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < current_health;i++)
        {
            hearth_container[i].gameObject.SetActive(true);
            
        }
        
    }

    void input_check(ref int val)
    {
        

        if ((val) < 0)
            (val) = 0;
        if ((val) >= 5)
            val = 5;

      
    }
    void attack (int pThrust_Power)
    {
        
        if (!CanAttack)
            return;
        
        CanAttack = false;
        CanMove = false;
        GameObject new_sword = Instantiate(sword, transform.position, transform.rotation);
        if (current_health == max_health)
        {
            new_sword.GetComponent<Sword>().ranged_attack = true;
            pThrust_Power = 750;
            CanMove = true;

        }
       
        int sword_dir = anim_control.GetInteger("dir");
        anim_control.SetInteger("attackdir", sword_dir);

        if (sword_dir == 0)
        {
            new_sword.transform.Rotate(0, 0, 0);
            new_sword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * pThrust_Power);
        }
            
        if (sword_dir == 1)
        {
            new_sword.transform.Rotate(0, 0, 180);
            new_sword.GetComponent<Rigidbody2D>().AddForce(Vector2.down * pThrust_Power);
        }
            
        if (sword_dir == 2)
        {
            new_sword.transform.Rotate(0, 0, 90);
            new_sword.GetComponent<Rigidbody2D>().AddForce(Vector2.left * pThrust_Power);

            
        }
            
        if (sword_dir == 3)
        {
            new_sword.transform.Rotate(0, 0, -90);
            new_sword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * pThrust_Power);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            if (IsInvinsicable)
                return;
            current_health -= 1;
          
            
            Instantiate(projectile_particle, transform.position, transform.rotation);
       


            Destroy(collision.gameObject);
            IsInvinsicable = true;
        }
        if (collision.gameObject.tag == "Potion")
        {
            
            Destroy(collision.gameObject);
            if (max_health == 5)
                return;
            max_health++;

            current_health = max_health;


        }
    }

    public void saveGame()
    {
        PlayerPrefs.SetInt("max_health", max_health);
        PlayerPrefs.SetInt("current_health", current_health);
    }
    public void loadGame()
    {
        max_health = PlayerPrefs.GetInt("max_health", max_health);
        current_health = PlayerPrefs.GetInt("current_health", current_health);
    }
   
}
