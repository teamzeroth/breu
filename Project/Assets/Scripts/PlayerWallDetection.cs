using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWallDetection : MonoBehaviour {

    SphereCollider _collider;

    private List<wallPoints> wallEmitters = new List<wallPoints>();

    public void Start() {
        _collider = GetComponent<SphereCollider>();
    }

    struct wallPoints {
        public Vector3 point;
        public float distance;
    }

    public void Update() {
        wallEmitters.Clear();

        int layer = 1 << LayerMask.NameToLayer("Wall");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _collider.radius, layer);

        if (hitColliders.Length == 0) {
            WallNoise.self.Desactive();
            return;
        }

        foreach (Collider collider in hitColliders) {
            wallPoints wp = new wallPoints();

            wp.point = collider.ClosestPointOnBounds(transform.position);
            wp.distance = Vector3.Distance(transform.position, wp.point);

            if (collider.tag == "Quoin") wp.distance *= 0.75f;

            wallEmitters.Add(wp);
        }

        wallEmitters.Sort((x, y) => {
            return x.distance != y.distance ? x.distance < y.distance ? -1 : 1 : 0;
        });

        Vector3[] points = new Vector3[WallNoise.self.EmittersNumber];

        for (int i = 0; i < Mathf.Min(points.Length, wallEmitters.Count); i++) {
            points[i] = wallEmitters[i].point;
            Debug.DrawLine(transform.position, points[i], Color.red);
        }

        WallNoise.self.setPositions(points);
    }
}
