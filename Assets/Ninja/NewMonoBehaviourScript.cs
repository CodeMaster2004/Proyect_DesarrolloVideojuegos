using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr= GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoverHorizontal();
        Saltar();
       
    }
    void MoverHorizontal()
    {
        animator.SetInteger("estado", 0);
        rb.linearVelocityX = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = 5;
            sr.flipX = false;
            animator.SetInteger("Estado ", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -5;
            sr.flipX = true;
            animator.SetInteger("Estado ", 1);

        }

    }
    void Saltar()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.linearVelocityY = 12.5f;
        }
    }
}
