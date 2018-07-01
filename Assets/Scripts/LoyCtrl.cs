using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoyCtrl : MonoBehaviour {

    public float speed = 2f;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Update");
	}


    //A:x减小 ,rotationY = 0;
    //D:x++ ,rotationY = 180;
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            ChangeToRun();

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("run"))
            {
                this.transform.position += new Vector3(Time.fixedDeltaTime, 0, 0) * speed;
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
            }

        
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {
            ChangeToRun();

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("run"))
            {
                this.transform.position -= new Vector3(Time.fixedDeltaTime, 0, 0) * speed;
                this.transform.rotation = Quaternion.Euler(Vector3.zero);
            }

            return;
        }

        if (Input.GetKey(KeyCode.J))
        {
            anim.SetFloat("attackValue", 1);
            return;
        }


        ChangeToIdle();
    }

    void ChangeToRun()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            //anim.SetTrigger("changeToRun");
            anim.SetFloat("speedValue", 1.1f);
        }
    }

    void ChangeToIdle()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetFloat("speedValue", 0);
            anim.SetFloat("attackValue", 0);
            //anim.SetTrigger("changeToIdle");
        }
    }
}
