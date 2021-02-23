using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    
    public Camera cam;
    private Vector3 Velocity = Vector3.zero;
    private Vector3 Rotate = Vector3.zero;
    private float cameraTiltX = 0f;
    private Vector3 dashForce = Vector3.zero;
    private float teleport = 0.1f;
    [SerializeField]
    private float maxCameraAngle = 90f;
    private float currentCameraRot = 0f;
    private Rigidbody rb;
    private float addRecoil;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Recoil(float R)
    {
        addRecoil += R;
    }
    public void StopRecoil()
    {
        //addRecoil = 0;
    }
    //movement method
    public void Move(Vector3 velocity)
    {
        Velocity = velocity;
        
    }
    public void Rotation(Vector3 rotate)
    {
        Rotate = rotate;
        
    }
    public void CameraTilt(float tilt)
    {
        cameraTiltX = tilt;

    }
    public void TailWind(float _xMov,float _yMov )
    {
        rb.transform.Translate(_xMov*teleport,0f,_yMov*teleport);
    }
    public void dash(Vector3 _dashForce)
    {
        dashForce = _dashForce;

    }
    public void FixedUpdate()
    {
        
        PerformMovement();
        PerformRotation();
    }
    void PerformMovement()
    {
        //Debug.Log(Velocity);
        if (Velocity != Vector3.zero)
        {
            
            rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);//will our rigid body
        }
        if (dashForce != Vector3.zero)
        {

            rb.AddForce(dashForce*Time.fixedDeltaTime,ForceMode.Acceleration);//will our rigid body
        }
    }
    void PerformRotation()
    {
       rb.MoveRotation(rb.rotation * Quaternion.Euler(Rotate));
        if (cam != null)
        {
            //new rotation code
            currentCameraRot -= cameraTiltX;
            currentCameraRot = Mathf.Clamp(currentCameraRot, -maxCameraAngle, maxCameraAngle);
            cam.transform.localEulerAngles = new Vector3(currentCameraRot-addRecoil, 0f, 0f);
            

        }
    }
    
    
}