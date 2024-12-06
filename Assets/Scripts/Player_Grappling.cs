using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Grappling : MonoBehaviour
{
    [SerializeField] private float grappleLength;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private float grappleSpeed;

    private Vector3 grapplePoint;
    private DistanceJoint2D joint;

    private BoxCollider2D boxCollider;

    private Rigidbody2D body;

    private bool isGrappled = true;
    private GameObject constGrapple;

    private void Awake(){
        joint = gameObject.GetComponent<DistanceJoint2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    private void Update(){
        GameObject findGrapple = existGrapplePoint().Item1;
        float staticGrappleLength = existGrapplePoint().Item2;
        if (isGrappled);
            if (Input.GetKey(KeyCode.W) && Input.GetMouseButton(1)){
                Grapple(constGrapple, staticGrappleLength);
                transform.position = Vector2.Lerp(transform.position,new Vector3(findGrapple.transform.position.x, findGrapple.transform.position.y-grappleLength), grappleSpeed*Time.deltaTime); 
            }
        else if(findGrapple!=null){
            Animator grappleAnim = findGrapple.GetComponent<Animator>();
            grappleAnim.SetTrigger("Highlighted");
            if(Input.GetMouseButton(1)){
               Grapple(findGrapple, staticGrappleLength);
               isGrappled = true;}
               constGrapple = findGrapple;
        }

        if(Input.GetMouseButtonUp(1)){
                joint.enabled = false;
                rope.enabled = false;
                isGrappled = false;
            }
        if(rope.enabled == true){
            rope.SetPosition(1, transform.position);
            }  
            
    }
    

    private void Grapple(GameObject grappleObject, float lengthOfGrapple){
        grapplePoint = grappleObject.transform.position;
        grapplePoint.z =0;
        joint.connectedAnchor = grapplePoint;
        joint.enabled = true;
        joint.distance = lengthOfGrapple;
        rope.SetPosition(0, grapplePoint);
        rope.SetPosition(1, transform.position);
        rope.enabled = true;

    }


    private Tuple<GameObject, float> existGrapplePoint(){
        RaycastHit2D[] raycastHit = Physics2D.BoxCastAll(
        transform.position, new Vector2(.2f, 10), 0, new Vector2(transform.localScale.x, 0), 5, grappleLayer);
        GameObject tMin = null;
        if (raycastHit.Length == 0)
        return Tuple.Create(tMin, 0f);
        else{
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach(RaycastHit2D i in raycastHit){    
            float dist = i.distance;
            if (dist < minDist && i.transform.position.y > currentPos.y){
                tMin = i.transform.gameObject;
                minDist = dist;
            }   
        }
        float distance = Vector2.Distance(transform.position, tMin.transform.position);
        return Tuple.Create(tMin,distance);       
        }
    }
}
        
