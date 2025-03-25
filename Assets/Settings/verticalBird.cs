using UnityEngine;

public class verticalBird : MonoBehaviour
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
            newPos.y -= direction;
            transform.position = newPos;

            if (transform.position.y <= pos1.position.y)
            {
                goPos1 = false;
                goPos2 = true;
            }
        }
        else
        {
            Vector3 newPos = transform.position;
            newPos.y += direction;
            transform.position = newPos;

            if (transform.position.y >= pos2.position.y)
            {
                goPos1 = true;
                goPos2 = false;
            }
        }
    }
}
