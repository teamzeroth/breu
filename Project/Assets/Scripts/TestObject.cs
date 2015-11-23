using UnityEngine;
using System.Collections;

public class TestObject : MonoBehaviour {

    void Start() {

        Invoke("BeforeStart", 0.1f);
    }

    void BeforeStart() {
        FMOD_CustonEmitter ce = GetComponent<FMOD_CustonEmitter>();
        ce.SetParameter("near", 10f);
    }
}
