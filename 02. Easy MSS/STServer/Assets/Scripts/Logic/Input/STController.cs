using UnityEngine;
using Manager;

[RequireComponent(typeof(STMovement))]
public class STController : MonoBehaviour
{
    private STMovement mMovement;

    private bool mKey_Left = false;
    private bool mKey_Right = false;
    private bool mKey_Up = false;
    private bool mKey_Down = false;

    private float mStep = 0.5f;

    void Awake()
    {
        mMovement = GetComponent<STMovement>();
    }

    void FixedUpdate()
    {
        mKey_Left = UnityEngine.Input.GetKey(KeyCode.LeftArrow) || UnityEngine.Input.GetKey(KeyCode.A);
        mKey_Right = UnityEngine.Input.GetKey(KeyCode.RightArrow) || UnityEngine.Input.GetKey(KeyCode.D);
        mKey_Up = UnityEngine.Input.GetKey(KeyCode.UpArrow) || UnityEngine.Input.GetKey(KeyCode.W);
        mKey_Down = UnityEngine.Input.GetKey(KeyCode.DownArrow) || UnityEngine.Input.GetKey(KeyCode.S);

        if (mKey_Left | mKey_Right | mKey_Up | mKey_Down)
        {
            var moveInput = Vector2.zero;
            if (this.mKey_Right)
            {
                moveInput.x = mStep;
            }
            else if (this.mKey_Left)
            {
                moveInput.x = -mStep;
            }

            if (this.mKey_Up)
            {
                moveInput.y = mStep;
            }
            else if (this.mKey_Down)
            {
                moveInput.y = -mStep;
            }

            STEntityManager.GetInstance().SendPosition(transform.position.x + moveInput.x, 0, transform.position.z + moveInput.y);
        }
    }
}
