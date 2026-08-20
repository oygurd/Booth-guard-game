using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class visitorBehaviour : MonoBehaviour
{
    public GameObject car;
    public SplineContainer carSpline;
    private SplineAnimate controlSpline;

    public bool newCarSpawned;
    
    public VisitorsSO visitorSO;
    public VisitorsSO.visitorType type;

    public enum npcSequence
    {
        driving,
        parked,
        greetings,
        givingCard,
        notGivingCard,
        talking,
        explainingTrunk,
        breaching,
        leaving
    }

    public npcSequence sequence;

    public Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlSpline = car.GetComponent<SplineAnimate>();
        controlSpline.Container = carSpline;

    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case VisitorsSO.visitorType.Calm:
                CalmVisitorBehaviour();
                break;
        }
        
       
    }

   

    public void Arrive()
    {
        controlSpline.Play();

    }

    public void openDialogue()
    {
        
    }

    public void TellVisitorToGiveCard()
    {
        
    }

    public void GiveCard()
    {
        
    }

    public void RefuseToGiveCard()
    {
        
    }
    

    public void CalmVisitorBehaviour()
    {
        
    }
}