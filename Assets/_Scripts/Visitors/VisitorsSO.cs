using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "VisitorsSO", menuName = "Visitors Scriptable Objects/VisitorsSO")]
public class VisitorsSO : SerializedScriptableObject
{
    public enum visitorType
    {
        Calm,
        Annoying,
        Rushing,
        Family,
        TwoPeople,
        Masked,
        Alien,
        Suspicious,
        Questioning
    }

    public string visitorName;

    public string arrivingDialogue;
    public string givingCardDialogue;
    public string refusingGivingCardDialogue;
    
    public string gateOpeningDialogue;
    public string cardIsFakeDialogue;
    
    public string leavingDialogue;
    public string breachingGateDialogue;
    
    public string questioningDialogue;

    public string whatIsMyLuggageAnswer;

    public bool hasFakeCard;
    public bool isDangerous;
    public bool hasLuggage;
    public bool willBreach;
    public bool willQuestion;
    

}
