#if (UNITY_IOS || UNITY_ANDROID) 
#   define MOBILE_PLATFORM
#endif
using UnityEngine;
using System.Collections.Generic;

public class FPSCharacterControl : MonoBehaviour
{
    [SerializeField] FPSJoystick m_joystick = null;
    [SerializeField] Rigidbody m_characterController = null;
    [SerializeField] GameObject m_camera = null;
    [SerializeField] GameObject myMob = null;
    [SerializeField] GameObject myGun = null;
	[SerializeField] float maxVelocityChange = 10.0f;
	[SerializeField] float m_speed = 10.0f;
    //[SerializeField] mob_script mob = null;
    NetworkView netview { get { return GetComponent<NetworkView>(); } }
    private Vector3 m_movingVector;
    private Vector2 m_rotationVector;
    float yVelocity = 0;
    private bool m_isDead = false;
	Vector3 prevMouse = Vector3.zero;

	private List<ContactPoint> m_activeCollisions = null;

    void Start()
    {
        m_movingVector = new Vector3(0, 0, 0);
        m_rotationVector = new Vector2(0, 0);
        myGun.transform.parent = m_camera.transform;
        //if (networkView && !networkView.isMine)
       // {
       //     myMob.animation.PlayQueued("run", QueueMode.PlayNow);
       // }
		prevMouse = Input.mousePosition;
		m_activeCollisions = new List<ContactPoint>();
        if (GameObject.Find("JumpButton"))
        {
            //Debug.Log("Finded");
            GameObject.Find("JumpButton").GetComponent<GUIButton>().OnStartPress = delegate
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + 20, rigidbody.velocity.z);
            };
        }
		
    }

    void Update()
    {
		if(prevMouse == Vector3.zero)
		{
			prevMouse = Input.mousePosition;
		}
        if (networkView && networkView.isMine)
        {//Debug.Log("---------- CollusionDir ----------");
		//	foreach(ContactPoint cpoint in m_activeCollisions)
		//	{
		//		Debug.Log("Points: " + cpoint.normal);
		//	}
			myMob.SetActive(false);
            foreach (Camera cam in GameObject.FindObjectsOfType(typeof(Camera)))
            { if (cam != m_camera.GetComponent<Camera>())cam.enabled = false; }
            m_camera.GetComponent<Camera>().enabled = true;
#if MOBILE_PLATFORM
			#region forTouch
            if (GetComponent<mob_script>().health > 0)
            {
             
                //-------------------------------------------------------------------------------------------- 
                if (m_joystick.rightDiffVector.x != 0 || m_joystick.rightDiffVector.y != 0)
                {
                    m_rotationVector = m_joystick.rightDiffVector;
                    m_rotationVector /= 3;
                    if (m_rotationVector.x > 15) m_rotationVector.x = 15;
                    if (m_rotationVector.x < -15) m_rotationVector.x = -15;
                    if (m_rotationVector.y > 10) m_rotationVector.y = 10;
                    if (m_rotationVector.y < -10) m_rotationVector.y = -10;
                    //m_rotationVector.Normalize();
                    if (Mathf.Abs(m_joystick.rightDiffVector.x) > 1)
                    {
                        transform.Rotate(Vector2.up, m_rotationVector.x * 0.5f, Space.World);
                    }
                    if (Mathf.Abs(m_joystick.rightDiffVector.y) > 1)
                    {
                        m_camera.transform.Rotate(Vector2.right, -(m_rotationVector.y * 0.5f), Space.Self);
                    }
                }
                //m_rotationVector = m_joystick.rightVector;
                //m_rotationVector.Normalize();
                //if (Mathf.Abs(m_joystick.rightVector.x) > 5)
                //{
                //    transform.Rotate(Vector2.up, m_rotationVector.x * 2, Space.World);
                //}
                //if (Mathf.Abs(m_joystick.rightVector.y) > 5)
                //{
                //    m_camera.transform.Rotate(Vector2.right, -(m_rotationVector.y * 1.5f), Space.Self);
                //}
                //--------------------------------------------------------------------------------------------
                if ((m_joystick.leftVector.magnitude > 10))// && (m_joystick.leftDiffVector.x != 0) && (m_joystick.leftDiffVector.y != 0))
                {
                    //Vector3 MovingVector = 
                    Vector3 movement = transform.TransformDirection(new Vector3(m_joystick.leftVector.x, 0, m_joystick.leftVector.y));
                    m_movingVector = new Vector3(movement.x, 0, movement.z);
                    m_movingVector /= 180;
                    if (m_movingVector.x > 0.8f) m_movingVector.x = 0.8f;
                    if (m_movingVector.z > 0.8f) m_movingVector.z = 0.8f;
                    if (m_movingVector.x < -0.8f) m_movingVector.x = -0.8f;
                    if (m_movingVector.z < -0.8f) m_movingVector.z = -0.8f;
					Debug.Log("Move");
                    
                }
                else
                {
                    m_movingVector = new Vector3(0, 0, 0);
                }
                //if (!m_characterController.isGrounded)
                //{
                //    yVelocity += Physics.gravity.y * Time.deltaTime;
                //}
                //else if (yVelocity < 0)
                //{
                //    yVelocity = 0;
                //}
                //m_movingVector.y += yVelocity;
                Vector3 currPos = transform.position;
                //m_characterController.Move(m_movingVector);
                //m_characterController.MovePosition(currPos+m_movingVector);
				PlayerMoveBy(m_movingVector);
            }
			#endregion
#else
			#region forKeyBoard
			m_rotationVector = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);//(Input.mousePosition-prevMouse);
            m_rotationVector *= 10;
            if (m_rotationVector.x > 15) m_rotationVector.x = 15;
            if (m_rotationVector.x < -15) m_rotationVector.x = -15;
            if (m_rotationVector.y > 10) m_rotationVector.y = 10;
            if (m_rotationVector.y < -10) m_rotationVector.y = -10;
            //m_rotationVector.Normalize();
            if (Mathf.Abs(m_rotationVector.x) > 1)
            {
                transform.Rotate(Vector2.up, m_rotationVector.x * 0.5f, Space.World);
            }
            if (Mathf.Abs(m_rotationVector.y) > 1)
            {
                m_camera.transform.Rotate(Vector2.right, -(m_rotationVector.y * 0.5f), Space.Self);
            }
			//Debug.Log("MousePos: "+m_rotationVector);
			//------------------------------------------
			Vector3 currPosMove = transform.position;
			Vector3 moveVector = Vector3.zero;
			if(Input.GetKey(KeyCode.W))
			{
				moveVector.z += 0.5f;
			}
			if(Input.GetKey(KeyCode.S))
			{
				moveVector.z -= 0.5f;
			}
			if(Input.GetKey(KeyCode.A))
			{
				moveVector.x -= 0.5f;
			}
			if(Input.GetKey(KeyCode.D))
			{
				moveVector.x += 0.5f;
			}
			if(Input.GetMouseButtonDown(0))
			{
				GetComponent<mob_script>().Shot();
			}
			 
			 PlayerMoveBy(transform.TransformDirection(moveVector*3));
			//m_characterController.MovePosition(currPosMove + transform.TransformDirection(moveVector));
			prevMouse = Input.mousePosition;
			if(Input.GetKeyDown(KeyCode.Space))
			{Debug.Log("Jump");/*if(rigidbody.velocity.y == 0) */
				//rigidbody.AddForce(new Vector3(0,20,0));
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + 20, rigidbody.velocity.z);
			}
			//Input.ResetInputAxes();
			#endregion
#endif
        }
        else
        {
            myGun.SetActive(false);
            SyncedMovement();
        }
        //Debug.Log(GetComponent<mob_script>().health);
        //Debug.Log(m_isDead);
        if (!m_isDead && GetComponent<mob_script>().health <= 0)
        {
            m_isDead = true;
            myMob.animation.PlayQueued("death", QueueMode.PlayNow);
        }
    }

    void OnGUI()
    {
 //       Rect rectButtonJump = new Rect(Screen.width - 105, Screen.height - 105, 100, 100);
 //       if (GUI.Button(rectButtonJump, "Jump"))
 //       {
//			if(rigidbody.velocity.y ==0)
//			rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y+20, rigidbody.velocity.z);
//        }
    }

    private Vector2 RotateVectorByAngle(Vector2 t_vec, float t_angle)
    {
        Vector2 tmp = new Vector2(0,0);
        float tmpAng = t_angle * Mathf.Rad2Deg;
        tmp.x = t_vec.x * Mathf.Cos(tmpAng) - t_vec.y * Mathf.Sin(tmpAng);
        tmp.y = t_vec.x * Mathf.Sin(tmpAng) + t_vec.y * Mathf.Cos(tmpAng);
        return tmp;
    }

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Vector3 syncStartRotation = Vector3.zero;
    private Vector3 syncEndRotation = Vector3.zero;

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 tmpPos = new Vector3(0, 0, 0);
        Vector3 tmpRot = new Vector3(0, 0, 0);
        if (stream.isWriting)
        {
            tmpPos = transform.position;
            tmpRot = transform.eulerAngles;
            stream.Serialize(ref tmpPos);
            stream.Serialize(ref tmpRot);
		}
        else
        {
            stream.Serialize(ref tmpPos);
            stream.Serialize(ref tmpRot);
            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
            syncStartPosition = transform.position;
            syncEndPosition = tmpPos;
            syncStartRotation = transform.eulerAngles;
            syncEndRotation = tmpRot;
        }
    }
	
    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        transform.eulerAngles = Vector3.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
    }
	
	private void PlayerMoveBy(Vector3 t_movingVector)
	{
		t_movingVector *= m_speed;
		Vector3 velocity = rigidbody.velocity;
	    Vector3 velocityChange = (t_movingVector - velocity);
	    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	    velocityChange.y = 0;
		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	}
	
	private void PlayerMoveTo(Vector3 t_newPos)
	{
		rigidbody.MovePosition(t_newPos);
	}
	
	//void OnCollisionStay(Collision collisionInfo)
	//{
	//	foreach(ContactPoint cpoint in collisionInfo.contacts)
	//	{
	//		if(cpoint.normal.y < 0.5f)
	//		{rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);Debug.Log(cpoint.normal);}	
	//	}
	//}
}
