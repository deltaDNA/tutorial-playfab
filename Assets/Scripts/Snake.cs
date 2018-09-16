using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    public float x          = 0f;
    public float y          = 0f;
    public float z          = 0f;
    public float direction  = 0f;
    public float velocity   = 0.2f;
    public int length       = 5;
    public float bodySectionLength = 1f;
    //bool vertical = false;
   // bool horizontal = true;
    public Vector3          head;    
    public List<Vector3>    snakeSections;
    
    
    LineRenderer snakeLine; 


	// Use this for initialization
	void Start () {

        snakeLine = gameObject.GetComponent<LineRenderer>();    // Create snake line renderer
        snakeLine.positionCount = length ;                      // Body length + lead and tail

        head = new Vector3(x, y, z);                            // Setup head position

        snakeSections = new List<Vector3>();        
        snakeSections.Add(head);
                  
        for (int i=1; i < this.length; i++)                    // Setup body position for each section
        {
            snakeSections.Add(new Vector3(x, y + (bodySectionLength * -i), z));                       
        }

        Debug.Log("Snake Spawned");
        //pUpdate();

    }
	

	// Update is called once per frame
	void Update () {

      
        Quaternion Rotation = Quaternion.Euler(0,  0, direction);
        Vector3 headDirection = Rotation * Vector3.up ;

        Vector3 newHead = new Vector3(); 
        newHead = head + headDirection * Time.deltaTime * velocity;

        float distanceTravelled = Vector3.Distance(newHead, head);       
        
        //for (int i = length - 1; i > 0  ; i--)
        for (int i=1; i < length; i++)
        {
            // find direction to previous segment
            
            Vector3 relativePosition = snakeSections[i] - snakeSections[i - 1] ;
            Quaternion sectionRotation = Quaternion.LookRotation(relativePosition);

            // calc segment direction
            Vector3 sectionDirection = sectionRotation * Vector3.up; 
            snakeSections[i] += sectionDirection * distanceTravelled;
            

            
            // update segment position


            //Vector3 sectionDirection = snakeSections[i-1] - snakeSections[i];
                              // //snakeSections[i] += sectionDirection * distanceTravelled; // bodySectionLength; // Time.deltaTime * velocity;
            //snakeSections[i] = snakeSections[i] + headDirection * Time.deltaTime * velocity;
            //snakeSections[i] = snakeSections[i - 1];           
        }
        head = newHead; 
             

        snakeSections[0] = head;
        for (int i=0; i < length; i++)
        {
            snakeLine.SetPosition(i, snakeSections[i]);
        }

    }
}
