﻿using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float MouseSensitivity = 5f;
    [SerializeField]
    private float dashIntensity = 1000f;
    [SerializeField]
    private float DashFuleBurntSpeed = 1f;
    [SerializeField]
    private float DashFuleRegenSpeed = 1f;
    [SerializeField]
    private float DashFuleAmount = 1f;
    public LayerMask environmentMask;
    public float upRecoil;
    public float sideRecoil;

    public float GetDashFuleAmount()
    {
        return DashFuleAmount;
    }

    [Header("Spring settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxFroce = 40f;

    private PlayerMotor Motor;
    private ConfigurableJoint joint;

    void Start()
    {
        //Debug.LogError("!!!");
        Motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    }
    void Update()
    {
        
        RaycastHit _hit;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f,environmentMask))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);//set the new target position to the new base.
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }
        //calculate movement velocity as a 3D vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float yMov =  Input.GetAxisRaw("Vertical");
        

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * yMov;

        Vector3 Velocity = (moveHorizontal + moveVertical) * speed;// final movement vector


        Motor.Move(Velocity);

        float yRot =  Input.GetAxisRaw("Mouse X");
        Vector3 Rotation = new Vector3(0f, sideRecoil + yRot, 0f) * MouseSensitivity;
        Motor.Rotation(Rotation);

        float xRot =  Input.GetAxisRaw("Mouse Y");
        float CameraTiltX = upRecoil + xRot * MouseSensitivity;
        Motor.CameraTilt(CameraTiltX);

        // sideRecoil = 0; //setting recoil back to zero every frame
        //upRecoil = 0;

        if (Input.GetButton("Fire2"))
        {
            Motor.TailWind(xMov, yMov);
        }

        
        Vector3 VectorDashIntensity = Vector3.zero;
        if (Input.GetButton("Jump") && DashFuleAmount > 0)//implement dash mechanics later here.
        {
            DashFuleAmount -= DashFuleBurntSpeed * Time.deltaTime;
            if (DashFuleAmount > 0.01) 
            {
                VectorDashIntensity = Vector3.up * dashIntensity;
                SetJointSettings(0f);
            } 

        }
        else
        {
            if (DashFuleAmount < 1)
            {
                DashFuleAmount += DashFuleRegenSpeed * Time.deltaTime;
                SetJointSettings(jointSpring);
            }
            
        }
        DashFuleAmount = Mathf.Clamp(DashFuleAmount, 0f, 1f);
        Motor.dash(VectorDashIntensity);
        


    }
    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxFroce };
    }
    public void AddRecoil(float up, float side )
    {
        sideRecoil += side;
        upRecoil += up;
    }
}
