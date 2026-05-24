using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //FSM
    private enum State { idle, run }
    private State state = State.idle;

    //Inspector variables
    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;


    [Header("Audio")]
    [SerializeField] private AudioSource walking;
    [SerializeField] private AudioSource jumping;

    [Header("Image Variables")]
    [SerializeField] private GameObject AndyAPP;
    [SerializeField] private RectTransform Andy;

    [Header("Stress")]
    [SerializeField] private CalculateStress caller;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        Movement();
        AnimationState();
        anim.SetInteger("State", (int)state); //sets animation based on Enumerator state
    }



    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        //Moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1.612918f, 1.612918f);
        }
        //Moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1.612918f, 1.612918f);
        }



    }


    private void AnimationState()
    {
        if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.run;
        }
        else
        {
            state = State.idle;
        }
    }

    private void Footstep()
    {
        walking.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AndysAdventureHouse"))
        {
            AndyAPP.SetActive(false);
            CalculateStress.UpdateStress(-5);
            Andy.GetComponent<RectTransform>().localPosition = new Vector3(-730, -153, 0);
        }
    }
}