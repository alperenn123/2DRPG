using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public Transform reward_pos;
    public GameObject reward_potion;
    Animator dragon_anim;
    public float speed;
    float timer = 1.2f;
    float special_attack_timer = 0.5f;
    int mov_dir;
    bool is_collide;
    float collision_timer;
    public int health;
    public GameObject witch_particle;
    public GameObject projectile;
    float attack_timer = 2.0f;
    bool canAttack;
    public float projectile_speed;
    GameObject new_projectile;
    public GameObject projectile_particle;
    // Use this for initialization
    void Start()
    {
        dragon_anim = GetComponent<Animator>();
        canAttack = false;
        mov_dir = 2;
        is_collide = false;
        collision_timer = 0.2f;

    }

    // Update is called once per frame
    void Update()
    {
        movement();
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            switch (mov_dir)
            {
                case 0: mov_dir = 3;break;
                case 1: mov_dir = 2;break;
                case 2: mov_dir = 0; break;
                case 3: mov_dir = 1;break;
            }
            
            timer = 1.2f;
            
        }
        special_attack_timer -= Time.deltaTime;

        if (special_attack_timer <= 0)
        {
            special_attack();
            special_attack();
            special_attack_timer = 0.5f;
        }
        attack_timer -= Time.deltaTime;
        if (attack_timer <= 0)
        {
            canAttack = true;
            attack_timer = 2.0f;
        }
        if (is_collide)
        {
            collision_timer -= Time.deltaTime;
            if (collision_timer <= 0)
            {
                is_collide = false;
                collision_timer = 0.2f;
            }
        }
        attack();

    }
    void movement()
    {
        if (mov_dir == 0)
        { transform.Translate(0, speed * Time.deltaTime, 0); dragon_anim.SetInteger("dir", mov_dir); }
        if (mov_dir == 1)
        { transform.Translate(0, -speed * Time.deltaTime, 0); dragon_anim.SetInteger("dir", mov_dir); }
        if (mov_dir == 2)
        { transform.Translate(-speed * Time.deltaTime, 0, 0); dragon_anim.SetInteger("dir", mov_dir); }
        if (mov_dir == 3)
        { transform.Translate(speed * Time.deltaTime, 0, 0); dragon_anim.SetInteger("dir", mov_dir); }

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

            Instantiate(witch_particle, transform.position, transform.rotation);
            Instantiate(reward_potion, reward_pos.position, reward_pos.rotation);
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            if (!collision.gameObject.GetComponent<Player>().IsInvinsicable)
            {
                collision.gameObject.GetComponent<Player>().current_health -= 1;
                collision.gameObject.GetComponent<Player>().IsInvinsicable = true;
            }


            if (health <= 0)
            {
                Instantiate(witch_particle, transform.position, transform.rotation);
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

    void attack()
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

    void special_attack()
    {
        
        GameObject new_projectile = Instantiate(projectile, this.transform.position, this.transform.rotation);
        int random_dir = Random.Range(0, 4);
        if (random_dir == 0) new_projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * projectile_speed);
        if (random_dir == 1) new_projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * projectile_speed);
        if (random_dir == 2) new_projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * projectile_speed);
        if (random_dir == 3) new_projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectile_speed);

    }
}
