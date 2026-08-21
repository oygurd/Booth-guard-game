using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class visitorBehaviour : MonoBehaviour
{
    public delegate void functionDelegate();
    
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
    
    public GameObject dialogueUI;
    public Text visitorName;
    public Text visitorDialogue;
    
    private Collider[] playerCollider;
    public LayerMask playerLayerMask;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlSpline = GetComponent<SplineAnimate>();
        controlSpline.Container = carSpline;

        StartCoroutine(StartBetweenActions(5, Arrive));

    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = Physics.OverlapSphere(transform.position, 3, playerLayerMask);


        if (playerCollider.Length != 0)
        {
            openDialogue();
        }
        else
        {
            CloseDialogue();
        }
        switch (type)
        {
            case VisitorsSO.visitorType.Calm:
                CalmVisitorBehaviour();
                break;
        }
        
    }

    public IEnumerator DelayBetweenActions(float seconds, functionDelegate action) // wait and then start an action
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    public IEnumerator StartBetweenActions(float seconds, functionDelegate action)// start an action and then wait 
    {
        action();
        yield return new WaitForSeconds(seconds);
    }
    public void Arrive()
    {
        controlSpline.Play();

    }

    public void openDialogue()
    {
        dialogueUI.SetActive(true);
    }

    public void CloseDialogue()
    {
        dialogueUI.SetActive(false);
    }
    public void TellVisitorToGiveCard()
    {
        if (playerCollider.Length != 0)
        {
            // give the player the card
            GiveCard();
        }
        else
        {
            //tell the player they are too far
        }
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