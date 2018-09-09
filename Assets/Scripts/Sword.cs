using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    float timer = 0.15f;
    float ranged_timer = 0.7f;
    public bool ranged_attack;
    float anim_timer = 0.1f;
    public GameObject destroy_effect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!ranged_attack)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanMove = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanAttack = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetInteger("attackdir", 5);
                Destroy(this.gameObject);
            }

        }
        else
        {
            ranged_timer -= Time.deltaTime;
            anim_timer   -= Time.deltaTime;
            if (ranged_timer <= 0)
            {
               GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanAttack = true;
               GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanMove = true;

                CreateParticle();
                Destroy(this.gameObject);
                
            }
            if(anim_timer <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetInteger("attackdir", 5);
            }
        }
      
       
	}

    public void CreateParticle() { Instantiate(destroy_effect, transform.position, transform.rotation); }
}
