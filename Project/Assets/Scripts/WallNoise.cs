using UnityEngine;
using System.Collections;
using System;

public class WallNoise : MonoBehaviour {

    static public WallNoise self;

    public int EmittersNumber = 2;
    public FMODAsset asset;

    private FMOD_CustonEmitter[] Emitters;

    void Awake() {
        self = this;

        transform.position = Vector3.zero;
        Emitters = new FMOD_CustonEmitter[EmittersNumber];

        for (int i = 0; i < EmittersNumber; i++) {
            GameObject audioEmitterGO = new GameObject("wallEmitter", typeof(FMOD_CustonEmitter));
            audioEmitterGO.transform.parent = transform;

            FMOD_CustonEmitter audioEmitter = audioEmitterGO.GetComponent<FMOD_CustonEmitter>();
            audioEmitter.asset = asset;
            audioEmitter.startEventOnAwake = false;

            Emitters[i] = audioEmitter;
        }

        Invoke("afterStart", 0.3f);
    }

    public void afterStart() {
        foreach (FMOD_CustonEmitter emitter in Emitters)
            emitter.SetParameter("closeToWalls", 1f);
    }

    public void setPositions(Vector3[] v) {
        int j = Mathf.Min(v.Length, EmittersNumber);

        for (int i = 0; i < j; i++) {
            if (!Emitters[i].HasPlayed()) Emitters[i].Play();
            Emitters[i].transform.position = v[i];
        }

        for (int i = j; i < EmittersNumber; i++)
            Emitters[i].Stop();
    }

    public void Desactive() {
        for (int i = 0; i < EmittersNumber; i++)
            Emitters[i].Stop();
    }
}
