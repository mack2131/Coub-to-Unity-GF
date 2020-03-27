using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public float curHp;
    public float speed;
    public bool spriteOriginalRight;
    public Rigidbody2D rb;
    public enum deathDirection { toCamera, back, UpBack}
    public deathDirection dd;
    public GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        FaceToPlayer();
    }

    void FixedUpdate()
    {
        /*
        Vector3 moveDir = new Vector3(PlayerController.playerX, 0, 0) - transform.position;
        rb.MovePosition(transform.position + moveDir.normalized * Time.deltaTime * speed);
        */
        if (curHp > 0)
        {
            float delta = PlayerController.playerX - transform.position.x;
            if (delta > 0)
                rb.velocity = new Vector2(speed, rb.velocity.y);
            else if (delta < 0)
                rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    void FaceToPlayer()
    {
        float delta = PlayerController.playerX - transform.position.x;
        if (!spriteOriginalRight)/*спрайт изначально нарисован смотрящим влево*/
        {
            if (delta < 0)
                GetComponent<SpriteRenderer>().flipX = false;
            else if (delta > 0)
                GetComponent<SpriteRenderer>().flipX = true;
        }
        else/*спрайт изначально нарисован смотрящим вправо*/
        {
            if (delta > 0)
                GetComponent<SpriteRenderer>().flipX = false;
            else if (delta < 0)
                GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void GetHit()
    {
        curHp -= 1;
        if(curHp <= 0)
        {
            curHp = 0;
            gameObject.layer = LayerMask.NameToLayer("Dead");
            GetComponent<Animator>().SetBool("Dead", true);
            DeathPush();
            StartCoroutine("Afterlife");
            StartCoroutine("BloodTrail");
            //Destroy(GetComponent<BoxCollider>());
        }
    }

    void DeathPush()
    {
        float delta = PlayerController.playerX - transform.position.x;
        if(dd == deathDirection.UpBack)
        {
            if (delta > 0)
                rb.AddForce((transform.up * 2 + -transform.right) * 25, ForceMode2D.Impulse);
            else if (delta < 0)
                rb.AddForce((transform.up * 2 + transform.right) * 25, ForceMode2D.Impulse);
        }
        else if (dd == deathDirection.toCamera)
        {
            /*
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce((transform.up * 2 - transform.forward) * 4, ForceMode2D.Impulse);
            */
        }
        else if(dd == deathDirection.back)
        {
            /*
            if (delta > 0)
                rb.AddForce(-transform.right * 5, ForceMode2D.Impulse);
            else if (delta < 0)
                rb.AddForce(transform.right * 5, ForceMode2D.Impulse);
                */
            if (delta > 0)
                rb.AddForce(-Vector2.right * 50, ForceMode2D.Impulse);
            else if (delta < 0)
                rb.AddForce(Vector2.right * 50, ForceMode2D.Impulse);
        }
    }

    IEnumerator Afterlife()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }

    IEnumerator BloodTrail()
    {
        while(true)
        {
            GameObject bl = blood;
            bl.transform.position = transform.position;
            Instantiate(bl/*, transform, true*/);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
