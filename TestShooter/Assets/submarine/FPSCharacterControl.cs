﻿using UnityEngine;
using System.Collections;

public class FPSCharacterControl : MonoBehaviour
{
    [SerializeField] FPSJoystick m_joystick = null;
    [SerializeField] CharacterController m_characterController = null;
    [SerializeField] GameObject m_camera = null;
    [SerializeField] GameObject myMob = null;
    [SerializeField] GameObject myGun = null;
    //[SerializeField] mob_script mob = null;
    NetworkView netview { get { return GetComponent<NetworkView>(); } }
    private Vector3 m_movingVector;
    private Vector2 m_rotationVector;
    float yVelocity = 0;
    private bool m_isDead = false;

    void Start()
    {
        m_movingVector = new Vector3(0, 0, 0);
        m_rotationVector = new Vector2(0, 0);
        myGun.transform.parent = m_camera.transform;
        if (networkView && !networkView.isMine)
        {
            myMob.animation.PlayQueued("run", QueueMode.PlayNow);
        }
    }

    void Update()
    {
        if (networkView && networkView.isMine)
        {
            if (GetComponent<mob_script>().health > 0)
            {
                myMob.SetActive(false);
                foreach (Camera cam in GameObject.FindObjectsOfType(typeof(Camera)))
                { if (cam != m_camera.GetComponent<Camera>())cam.enabled = false; }
                m_camera.GetComponent<Camera>().enabled = true;
                //-------------------------------------------------------------------------------------------- 
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
                if (m_joystick.leftVector.magnitude > 10)
                {
                    //Vector3 MovingVector = 
                    Vector3 movement = transform.TransformDirection(new Vector3(m_joystick.leftVector.x, 0, m_joystick.leftVector.y));
                    m_movingVector = new Vector3(movement.x, 0, movement.z);
                    m_movingVector /= 180;
                    if (m_movingVector.x > 0.8f) m_movingVector.x = 0.8f;
                    if (m_movingVector.z > 0.8f) m_movingVector.z = 0.8f;
                    if (m_movingVector.x < -0.8f) m_movingVector.x = -0.8f;
                    if (m_movingVector.z < -0.8f) m_movingVector.z = -0.8f;
                }
                else
                {
                    m_movingVector = new Vector3(0, 0, 0);
                }
                if (!m_characterController.isGrounded)
                {
                    yVelocity += Physics.gravity.y * Time.deltaTime;
                }
                else if (yVelocity < 0)
                {
                    yVelocity = 0;
                }
                m_movingVector.y += yVelocity;

                m_characterController.Move(m_movingVector);
            }
            else
            {
                //Debug.Log("perhepsDead");
            }
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
            Debug.Log("Dead");
            m_isDead = true;
            myMob.animation.PlayQueued("death", QueueMode.PlayNow);
        }
    }

    void OnGUI()
    {
        Rect rectButtonJump = new Rect(Screen.width - 105, Screen.height - 105, 100, 100);
        if (GUI.Button(rectButtonJump, "Jump"))
        {
            Debug.Log("Jump");
            yVelocity = 2;
        }
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
}