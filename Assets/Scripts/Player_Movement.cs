using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private Vector3 defaultScale;
    private bool noBoost;
    private bool jumpTwo = false;
    private Player_Boost boostScript;

    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private float speed;
    [SerializeField]private float jumpPower;

    [SerializeField]private float boostPower;

    [SerializeField]private GameObject boost;
    
    
    private void Awake(){
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        boostScript = boost.GetComponent<Player_Boost>();
        defaultScale = transform.localScale;
    }
    private void Update(){

        if(Input.GetKeyDown(KeyCode.Space) && jumpTwo && !isGrounded()){
            jumpTwo = false;
            Boost();}
        else if(Input.GetKeyDown(KeyCode.Space)){
            jumpTwo = true;
            Jump();}
        if (isGrounded()){
            noBoost = true;}
            
        if (!isGrounded() && onWall())
        return;
        else{
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
        transform.localScale = defaultScale;
        else if (horizontalInput < -0.01f)
        transform.localScale = new Vector3(-1*defaultScale.x, defaultScale.y, 0);

        anim.SetBool("Run", horizontalInput!= 0);
        anim.SetBool("Grounded", isGrounded());
        
        }
    }

    private void Boost(){
        if(!isGrounded() && noBoost){
        body.velocity = new Vector2(body.velocity.x, boostPower);
        transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x)*defaultScale.x, defaultScale.y, 0);
        noBoost = false;
        boostScript.Start();
        }
        

    }
    private void Jump(){
        if (isGrounded()){
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        anim.SetTrigger("Jump");
        transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x)*defaultScale.x, defaultScale.y, 0);
    }}

    private bool isGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

}
