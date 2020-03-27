using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    public bool rightFace;
    public float groundDist;
    public Transform hitBlast;

    public static float playerX;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        rightFace = true;
        groundDist = GetComponent<CapsuleCollider2D>().bounds.extents.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("Run", true);
            ChangeFace(false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("Run", true);
            ChangeFace(true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("Run", false);
            rb.velocity = Vector2.zero;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("Run", false);
            rb.velocity = Vector2.zero;
        }

        if (Input.GetKey(KeyCode.H))
            anim.SetBool("Fight", true);
        if (Input.GetKeyUp(KeyCode.H))
            anim.SetBool("Fight", false);
        playerX = transform.position.x;

        if (Input.GetKeyDown(KeyCode.Space) && Grounded())
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        Vector3 moveVec = new Vector3();

        if(Input.GetKey(KeyCode.D))
            moveVec = transform.right;
        else if (Input.GetKey(KeyCode.A))
            moveVec = -transform.right;
        rb.MovePosition(transform.position + moveVec * Time.deltaTime * speed);
        */
        Vector2 moveVec = Vector2.zero;
        if (Input.GetKey(KeyCode.D))
            rb.velocity = new Vector2(speed, rb.velocity.y);//rb.MovePosition(rb.position + Vector2.right * Time.deltaTime * speed);
        else if (Input.GetKey(KeyCode.A))
            rb.velocity = new Vector2(-speed, rb.velocity.y);//rb.MovePosition(rb.position - Vector2.right * Time.deltaTime * speed);
        
    }

    public void Attack()
    {
        /*
        RaycastHit hit;
        Vector3 rightDir;
        if (rightFace)
            rightDir = transform.right;
        else rightDir = -transform.right;
        if (Physics.SphereCast(transform.position, 0.5f, rightDir, out hit, 0.5f))
        {
            Enemy en = hit.collider.GetComponent<Enemy>();
            if (en != null)
                en.GetHit();
        }
        */
        Vector3 sphereCenter;
        if (rightFace)
            sphereCenter = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        else sphereCenter = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
        Collider2D[] coll = /*Physics.OverlapSphere(sphereCenter, 0.7f)*/Physics2D.OverlapCircleAll(sphereCenter, 0.7f);
        int i = 0;
        bool hitWas = false;
        while(i < coll.Length)
        {
            if (coll[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                coll[i].SendMessage("GetHit");
                if (!hitWas)
                    hitWas = true;
            }
            i++;
        }
        if(hitWas)
        {
            AudioManager.instance.PlayPunch();
            hitBlast.position = sphereCenter;
            hitBlast.gameObject.SetActive(true);
            hitWas = false;
        }
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 sphereCenter = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        Gizmos.DrawSphere(sphereCenter, 0.7f);
    }
    */

    bool Grounded()
    {
        /*
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundDist+0.03f))
        {
            return true;
        }
        else return false;
        */
        Vector2 point = new Vector2(transform.position.x, transform.position.y - groundDist);
        Collider2D[] coll = /*Physics.OverlapSphere(sphereCenter, 0.7f)*/Physics2D.OverlapCircleAll(point, 0.1f);
        for(int i = 0; i < coll.Length; i++)
        {
            if (coll[i].gameObject.layer == LayerMask.NameToLayer("Floor"))
                return true;
        }
        return false;
    }

    void ChangeFace(bool right)
    {
        if (!right/*left*/)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            rightFace = false;
        }
        else if (right)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            rightFace = true;
        }
    }
}
