using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake2 : MonoBehaviour {
    //http://coffeebreakcodes.com/sample-projects-unity/2d-snake-game-unity3d-c/
    public GameObject food;
    public GameObject tail;
    public Transform rBorder;
    public Transform lBorder;
    public Transform tBorder;
    public Transform bBorder;
    private List<GameObject> tailSections = new List<GameObject>();
    bool vertical = false;
    bool horizontal = true;
    bool eat = false;

    private float speed = 0.020f;
	Vector2 vector = Vector2.up;
	Vector2 moveVector;

    // Use this for initialization
    void Start () {
        InvokeRepeating("Movement", 0.1f, speed);
        SpawnFood();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow) && horizontal) {
            horizontal = false;
            vertical = true;
            vector = Vector2.right;
        } else if (Input.GetKey(KeyCode.UpArrow) &&  vertical) {
            horizontal = true;
            vertical = false;
            vector = Vector2.up;
        } else if (Input.GetKey(KeyCode.DownArrow) && vertical) {
            horizontal = true;
            vertical = false;
            vector = -Vector2.up;
        } else if (Input.GetKey(KeyCode.LeftArrow) && horizontal) {
            horizontal = false;
            vertical = true;
            vector = -Vector2.right;
        }
        moveVector = vector / 20f;

    }

    void Movement()
    {

        Vector3 ta = transform.position;
        if (eat)
        {
            if (speed > 0.002){
                speed = speed - 0.002f;
            }


           GameObject g = (GameObject)Instantiate(tail, ta, Quaternion.identity);
            tailSections.Insert(0, g);
            Debug.Log(speed);
            eat = false;
        }
        else if (tailSections.Count > 0) {
            tailSections.Last().transform.position = ta;
            tailSections.Insert(0, tailSections.Last());
            tailSections.RemoveAt(tailSections.Count - 1);
        }

        transform.Translate(moveVector);//* Time.deltaTime);





    }
    public void SpawnFood()
    {
        int x = (int)Random.Range(lBorder.position.x, rBorder.position.x);
        int y = (int)Random.Range(bBorder.position.y, tBorder.position.y);

        Instantiate(food, new Vector2(x, y), Quaternion.identity);
    }
    void OnTriggerEnter(Collider c)
    {

        if (c.name.StartsWith("food"))
        {
            
            eat = true;
            Destroy(c.gameObject);
            SpawnFood();
        }
    }
}
