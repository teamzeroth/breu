using UnityEngine;
using System.Collections;
using UnityEditor;

public class WallSoundCheck : MonoBehaviour {

    public FMOD_CustonEmitter feetEmitter;
    public FMOD_CustonEmitter eastEmitter;
    public FMOD_CustonEmitter westEmitter;
    public FMOD_CustonEmitter northEmitter;
    public FMOD_CustonEmitter southEmitter;

    public float raycastRange = 5.0f;

    public float speedOfSound = 340.29f;

    private Transform myTransform;
    private bool isPlaying = false;

    void Awake()
    {
        myTransform = transform;
    }

    public void Play()
    {
        isPlaying = true;
        feetEmitter.Play();

        eastEmitter.Play();
        westEmitter.Play();
        northEmitter.Play();
        southEmitter.Play();


        StartCoroutine(TrackWall());
    }

    private IEnumerator TrackWall()
    {
        while(isPlaying)
        {
            Vector3 myPosition = myTransform.position;
            WallRaycast(Vector3.forward, northEmitter, myPosition);
            WallRaycast(Vector3.back, southEmitter, myPosition);
            WallRaycast(Vector3.right, eastEmitter, myPosition);
            WallRaycast(Vector3.left, westEmitter, myPosition);
            yield return null;
        }
    }

    private void WallRaycast(Vector3 direction, FMOD_CustonEmitter emitter, Vector3 myPosition)
    {
        Transform emitterTransform = emitter.transform;

        RaycastHit hit;
        if (Physics.Raycast(myPosition, direction, out hit, raycastRange))
        {
            emitterTransform.position = hit.point;// + 10.0f * direction;
            //if(emitter.HasStoped())
            //    emitter.Play();
        }
        else
        {
            emitterTransform.position = new Vector3(10000,10000,10000);
            //emitter.Stop();
        }
    }

    IEnumerator PlayDelayedSound(FMOD_CustonEmitter emitter, float delay)
    {
        yield return new WaitForSeconds(delay);
        emitter.Play();
    }

    public bool HasPlayed()
    {
        return feetEmitter.HasPlayed();
    }

    public bool HasStarted()
    {
        return feetEmitter.HasStarted();
    }

    public void SetParameter(string parameter, float value)
    {
        feetEmitter.SetParameter(parameter, value);
        eastEmitter.SetParameter(parameter, value);
        westEmitter.SetParameter(parameter, value);
        northEmitter.SetParameter(parameter, value);
        southEmitter.SetParameter(parameter, value);
    }

    public void Stop(float delay)
    {
        isPlaying = false;
        feetEmitter.Stop(delay);
        eastEmitter.Stop(delay);
        westEmitter.Stop(delay);
        northEmitter.Stop(delay);
        southEmitter.Stop(delay);
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Vector3 pos = transform.position;
        Handles.DrawLine(pos, pos + Vector3.forward * raycastRange);
        Handles.DrawLine(pos, pos + Vector3.back * raycastRange);
        Handles.DrawLine(pos, pos + Vector3.right * raycastRange);
        Handles.DrawLine(pos, pos + Vector3.left * raycastRange);
    }
#endif
}
