using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour {
    Animator dragon_anim;
    public float speed;
    float timer = 1.0f;
    int mov_dir;
    bool is_collide;
    float collision_timer;
    public int health;
    public GameObject dragon_particle;
    public GameObject projectile;
    float attack_timer = 2.0f;
    bool canAttack;
    public float projectile_speed;
    GameObject new_projectile;
    
	// Use this for initialization
	void Start () {
        dragon_anim = GetComponent<Animator>();
        canAttack = false;
        mov_dir = Random.Range(0, 4);
        is_collide = false;
        collision_timer = 0.2f;
		
	}
	
	// Update is called once per frame
	void Update () {
        movement();
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            mov_dir = Random.Range(0, 4);
            timer = 1.5f;
        }
        attack_timer -= Time.deltaTime;
        if (attack_timer <= 0)
        {
            canAttack = true;
            attack_timer = 2.0f;
        }
        if(is_collide)
        {
            collision_timer -= Time.deltaTime;
            if(collision_timer <=0)
            {
                is_collide = false;
                collision_timer = 0.2f;
            }
        }
        attack();
        
    }
    void movement ()
    {
        if (mov_dir == 0)
        { transform.Translate(0, speed * Time.deltaTime, 0); dragon_anim.SetInteger("dir", mov_dir);  }
        if (mov_dir == 1)
        { transform.Translate(0, -speed * Time.deltaTime, 0); dragon_anim.SetInteger("dir", mov_dir); }
        if (mov_dir == 2)
        { transform.Translate(-speed * Time.deltaTime, 0, 0); dragon_anim.SetInteger("dir", mov_dir); }
        if (mov_dir == 3)
        { transform.Translate(speed * Time.deltaTime, 0, 0); dragon_anim.SetInteger("dir", mov_dir);  }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            health--;
            collision.gameObject.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanMove = true;
            Destroy(collision.gameObject);

        }
        if (health <= 0)
        {

            Instantiate(dragon_particle, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            health -= 1;
            if (!collision.gameObject.GetComponent<Player>().IsInvinsicable)
            {
                collision.gameObject.GetComponent<Player>().current_health -= 1;
                collision.gameObject.GetComponent<Player>().IsInvinsicable = true;
            }


            if (health <= 0)
            {
                Instantiate(dragon_particle, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (is_collide)
                return;
            if (mov_dir == 0)
                mov_dir = 1;
            else if (mov_dir == 1)
                mov_dir = 0;
            else if (mov_dir == 2)
                mov_dir = 3;
            else if (mov_dir == 3)
                mov_dir = 2;
            is_collide = true;
        }
    }

    void attack ()
    {
        if (!canAttack)
            return;
        canAttack = false;
        if (mov_dir == 0)
        {
            GameObject new_projectile = Instantiate(projectile, transform.position, transform.rotation);
            new_projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * projectile_speed);
           
        }
        if (mov_dir == 1)
        {
            GameObject new_projectile = Instantiate(projectile, transform.position, transform.rotation);
            new_projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * projectile_speed);
            
        }
        if (mov_dir == 2)
        {
            GameObject new_projectile = Instantiate(projectile, transform.position, transform.rotation);
            new_projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * projectile_speed);
            
        }
        if (mov_dir == 3)
        {
            GameObject new_projectile = Instantiate(projectile, transform.position, transform.rotation);
            new_projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectile_speed);
            
        }
    }
}
