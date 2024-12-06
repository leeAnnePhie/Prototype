using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Boost : MonoBehaviour
{
    private Animator anim;
    private float lifetime;
    [SerializeField] private float lifeTimePeriod;
    [SerializeField]private Transform boostPoint;

    private void Awake(){

    anim = GetComponent<Animator>();
        
    }

    private void Update(){
        gameObject.transform.position = boostPoint.position;
       
        lifetime += Time.deltaTime;
        if (lifetime > lifeTimePeriod) gameObject.SetActive(false);
    }

    public void Start(){
        lifetime = 0;
        gameObject.SetActive(true);
        gameObject.transform.position = boostPoint.position;
    }
        
    }
