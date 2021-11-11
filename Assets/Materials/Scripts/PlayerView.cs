using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public float maxRotationDegreePerSecond = 75f;
    public float mouseRotaionSpeed = 100f;
    public float gyroRotationSpeed = 70f;

#if UNITY_EDITOR
	public float mouseRotationSpeed = 100f;
#endif

    [Range(0, 45)]
    public float maxPitchUpAngle = 45f;

    [Range(0, 45)]
    public float maxPitchDownAngle = 5f;
    private Rect lookRect;
    private int lookTouchID = -1;
    private Vector2 touchOrigin;

    void start()
    {
        lookRect = new Rect(0, 0, Screen.width / 2, Screen.height * 0.75f);

#if UNITY_EDITOR
		Cursor.visible = false;
#endif
    }
    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
           
            if (Input.gyro.enabled){
                
                GyroInput();
            }
            else{  
                TouchInput();
            }
            TouchInput(); 

#if UNITY_EDITOR
			MouseInput ();
#endif
        }
    }
  private  void GyroInput()
    {
        Vector3 rotation = Input.gyro.rotationRate * gyroRotationSpeed;

        RotateView(new Vector3(-rotation.x, -rotation.y, 0));
    }

    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            // Save touch input that first touches the lookRect
            if (lookTouchID == -1)
            {
                foreach (Touch touch in Input.touches)
                {
                    // Only register begin touches
                    if (touch.phase != TouchPhase.Began)
                        continue;

                    // Only use touches inside lookRect
                    if (!lookRect.Contains(touch.position))
                        continue;

                    // Save touch and exit the loop
                    lookTouchID = touch.fingerId;
                    touchOrigin = touch.position;
                    break;
                }
            }

            foreach (Touch touch in Input.touches)
            {
                // Only process saved touch id
                if (touch.fingerId != lookTouchID)
                    continue;

                Vector3 touchDistance = touch.position - touchOrigin;

                // Limit rotation on touch
                Vector3 clampedRotation = Vector3.ClampMagnitude(new Vector3(-touchDistance.y, touchDistance.x), maxRotationDegreePerSecond);

                // Rotate view
                RotateView(clampedRotation);

                // Clear saved touch id on release
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    lookTouchID = -1;
                }

                // Exit the loop
                break;
            }
        }
    }

#if UNITY_EDITOR
    private void MouseInput()
    {
        Vector3 rotation = new Vector3 (-Input.GetAxis ("Mouse Y"), Input.GetAxis ("Mouse X"), 0 ) ;
        RotateView(rotation * mouseRotaionSpeed);
    }
#endif
    private void RotateView (Vector3 rotation)
    {
        //Rotate Player
        transform.Rotate( rotation * Time.deltaTime);

        if (Input.gyro.enabled)
        {
            // Reset local z rotation
            Vector3 localEuler = transform.localEulerAngles;
            transform.localRotation = Quaternion.Euler(localEuler.x, localEuler.y, 0);
        }

        //Limit Player rotation pitch
        float playerPitch = LimitPitch();
        //Apply clamped pitch and clear roll
        transform.rotation = Quaternion.Euler(playerPitch, transform.eulerAngles.y, 0);

    }
    private float LimitPitch()
    {
        float playerPitch = transform.eulerAngles.x;
        float maxPitchUp = 360 - maxPitchUpAngle;
        float maxPitchDown = maxPitchDownAngle;
        if (playerPitch > 180 && playerPitch < maxPitchUp)
        {
            //Limit pitch up
            playerPitch = maxPitchUp;

        }else if (playerPitch < 180 && playerPitch > maxPitchDown)
        {
            //limit pitch down 
            playerPitch = maxPitchDown;
        }
        return playerPitch;
    }
}
