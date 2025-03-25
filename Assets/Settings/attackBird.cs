using Unity.VisualScripting;
using UnityEngine;

public class attackBird : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform transform;
    public Transform player;
    public float rate = 0.015f;

    Vector2 position;
    Vector2 playerposition;
    bool getPlayerPos = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    void getPosition()
    {
        position = transform.position;
        playerposition = player.position;
        getPlayerPos = true;
    }
    void Attack()
    {

        //Vector2 attackAngle;

        float xDifference = position.x - playerposition.x;
        float yDifference = position.y - playerposition.y;
        float xDirection = (position.x - playerposition.x) * rate;
        float yDirection = (position.y - playerposition.y) * rate;

        //attack player
        while ((position.x >playerposition.x) && (position.y >playerposition.y))
        {
            position.x -= xDirection;
            position.y -= yDirection; 
            transform.position = position;
        }

        Vector2.Lerp(position, new Vector2(position.x+xDifference, position.y+yDifference), Time.deltaTime);
         
    }
}
