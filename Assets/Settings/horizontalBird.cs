using UnityEngine;

public class horizontalBird : MonoBehaviour
{
    public Transform transform;
    public Transform pos1;
    public Transform pos2;

    bool goPos1 = false;
    bool goPos2 = false;

    public float direction = 0.015f;

    void Awake()
    {
        goPos1 = true;
    }   

    void Update()
    {
        changePosition();
    } 

    void changePosition()
    {
        if (goPos1 == true)
        {
            Vector3 newPos = transform.position;
            newPos.x -= direction;
            transform.position = newPos;

            if (transform.position.x <= pos1.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                goPos1 = false;
                goPos2 = true;
            }
        }
        else
        {
            Vector3 newPos = transform.position;
            newPos.x += direction;
            transform.position = newPos;

            if (transform.position.x >= pos2.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                goPos1 = true;
                goPos2 = false;
            }
        }
    }
}
