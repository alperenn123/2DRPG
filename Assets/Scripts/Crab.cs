using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {
    public int health;
    public float speed;
    public float timer;
    float collison_timer = 0.2f;
    int movement_dir;
    bool is_collide;
    public GameObject crab_particle;
    SpriteRenderer sprite_render;
    public Sprite FacingUp;
    public Sprite FacingDown;
    public Sprite FacingLeft;
    public Sprite FacingRight;
	// Use this for initialization
	void Start () {
        sprite_render = GetComponent<SpriteRenderer>();
        movement_dir = Random.Range(0, 4);
        is_collide = false;

    }
	
	// Update is called once per frame
	void Update () {
        
        
        timer -= Time.deltaTime;


        if (timer <= 0)
        {
                timer = 1.0f;
                movement_dir = Random.Range(0, 4);

         }
        if(is_collide)
        {
            collison_timer -= Time.deltaTime;
            if (collison_timer <= 0)
            {
                is_collide = false;

                collison_timer = 0.2f;
            }


        }
       
        movement();
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            collision.GetComponent<Sword>().CreateParticle();
            health -= 1;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanMove = true;
            Destroy(collision.gameObject);
        }
        if (health <= 0)
        {
            
            Instantiate(crab_particle, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
       

       
        
    }
    public void movement ()
    {
        switch (movement_dir)
        {
            //down
            case 0:
                transform.Translate(0, -speed * Time.deltaTime ,0);
                sprite_render.sprite = FacingDown;
                break;
            //up
            case 1:
                transform.Translate(0, speed * Time.deltaTime, 0);
                sprite_render.sprite = FacingUp;
                break;
            //left
            case 2:
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                sprite_render.sprite = FacingLeft;
                break;
            //right
            case 3:
                transform.Translate(speed * Time.deltaTime, 0, 0);
                sprite_render.sprite = FacingRight;
                break;
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
                Instantiate(crab_particle, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (is_collide)
                return;
            if (movement_dir == 0)
                movement_dir = 1;
            else if (movement_dir == 1)
                movement_dir = 0;
            else if (movement_dir == 2)
                movement_dir = 3;
            else if (movement_dir == 3)
                movement_dir = 2;
            is_collide = true;
        }
    }
}
