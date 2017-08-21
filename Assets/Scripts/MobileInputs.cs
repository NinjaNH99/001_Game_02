using UnityEngine;

public class MobileInputs : MonoSingleton<MobileInputs>
{
    public bool release;
    public Vector2 swipeDelta;
    public bool hold;
    private Vector2 initialPosition;

    protected void Awake()
    {
        release = hold = false;
        swipeDelta = Vector2.zero;
        initialPosition = Input.mousePosition;
    }

    private void Update()
    {
        if (GameController.updateInputs)
        {
            release = false;
            swipeDelta = Vector2.zero;

            #region Standalone Inputs
            if (Input.GetMouseButtonDown(0))
            {
                initialPosition = Input.mousePosition;
                hold = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                release = true;
                hold = false;
                swipeDelta = (Vector2)Input.mousePosition - initialPosition;
            }

            if (hold)
            {
                swipeDelta = (Vector2)Input.mousePosition - initialPosition;
            }
            #endregion

            #region Mobiles Inputs
            if (Input.touches.Length != 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    hold = true;
                    initialPosition = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                {
                    release = true;
                    hold = false;
                    swipeDelta = Input.touches[0].position - initialPosition;
                    //Reset();
                }
                if (hold)
                {
                    swipeDelta = Input.touches[0].position - initialPosition;
                }
            }
            #endregion
        }
    }

    public void Reset()
    {
        initialPosition = swipeDelta = Vector2.zero;
        release = false;
    }

}
