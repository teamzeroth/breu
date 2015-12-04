using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerMovement : MonoBehaviour {

    public float speed = 5;
    [Range(1, 10)]
    public float friction = 5;
    [Range(1, 10)]
    public float spin = 4;

    public float touchNormal = 100;
    public float maxTouchForce = 1;
    public float minMagnetudeSpin = 0.4f;

    public bool usingTouch = true;

    Rigidbody _rigidbody;

    Vector3 lastVector = Vector3.zero;
    Vector3 updateVector = Vector3.zero;

    public WallSoundCheck wallSound;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_feetAudio = transform.Find("Feets").GetComponent<FMOD_CustonEmitter>();
    }

    void Update()
    {
        Vector3 vector = usingTouch ? GetTouchAxis() : GetAxis();

        if (vector == Vector3.zero) {
            lastVector = updateVector;
            PlayerFriction();

            if (wallSound.HasPlayed()) {
                //_feetAudio.SetParameter("velocidade", 0);
                wallSound.Stop(0.1f);
            }

        } else {
            if (!wallSound.HasPlayed() && !wallSound.HasStarted()) {
                wallSound.SetParameter("velocidade", 0);
                wallSound.Play();
            }

            PlayerInput(vector);
        }
    }

    Vector3 GetAxis()
    {
        Vector3 temp = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        return temp.magnitude >= 0.2f ? temp : Vector3.zero;
    }

    Vector2 initialTouchPos;

    Vector3 GetTouchAxis()
    {
        if (Input.touchCount != 0) {
            if (initialTouchPos == Vector2.zero)
                initialTouchPos = Input.GetTouch(0).position;

            Vector2 temp = (Input.GetTouch(0).position - initialTouchPos) / touchNormal;
            return new Vector3(temp.x, 0, temp.y);
        }

        initialTouchPos = Vector2.zero;
        return Vector3.zero;
    }

    void PlayerInput(Vector3 vector)
    {
        float oldAngle = Mathf.Atan2(lastVector.x, lastVector.z);
        float curAngle = Mathf.Atan2(vector.x, vector.z);

        float angle = oldAngle + curAngle;
        float force = Mathf.Min(vector.magnitude, maxTouchForce) * speed;

        wallSound.SetParameter("velocidade", Mathf.Min(vector.magnitude, 0.8f));

        Vector3 rotation = new Vector3(0, angle * Mathf.Rad2Deg, 0);

        _rigidbody.velocity = updateVector = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * force;

        if (vector.magnitude > minMagnetudeSpin)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotation), Time.deltaTime * spin);
    }

    void PlayerFriction()
    {
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, Vector3.zero, Time.deltaTime * friction);
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.ArrowCap(0,
            transform.position,
            transform.rotation,
            1
        );
    }
#endif

}
