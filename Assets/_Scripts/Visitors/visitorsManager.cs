using System.Collections.Generic;
using UnityEngine;

public class visitorsManager : MonoBehaviour
{
    public bool dayStarted;
    
    public bool newVisitor;
    public int maxVisitors;
    
    public List<GameObject> possibleVisitors = new List<GameObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      GameObject newVisitor = Instantiate(possibleVisitors[0].gameObject, transform.position, Quaternion.identity);;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
