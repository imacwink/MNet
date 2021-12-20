using UnityEngine;

public class STMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Speed the entity will move.")]
    private float speed = 5f;

    private Vector3 mMovePosition;

    void Awake()
    {
        mMovePosition = transform.position;
    }

    void Update()
    {
        if (speed != 0f)
            transform.position = Vector3.MoveTowards(transform.position, mMovePosition, speed * Time.deltaTime);
    }

    public void SetMovePosition(Vector3 newPosition)
    {
        mMovePosition = newPosition;
    }
}
