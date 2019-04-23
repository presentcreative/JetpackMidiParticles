/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.FlowControl.Emitters
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SplineEmitter :MonoBehaviour
    {
        private float _lastSentinel;

        private void Start()
        {
#if UNITY_EDITOR
            GenerateEmitter();
#endif
        }

        private void GenerateEmitter()
        {
            Mesh mesh = new Mesh();
            mesh.name = "Spline " + gameObject.name;

            Spline[] splines = GetComponentsInChildren<Spline>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> tris = new List<int>();
            for (int i = 0; i < splines.Length; i++)
            {
                Spline s = splines[i];
                Vector3 shear = new Vector3(0.02f, 0.02f, 0.02f);
                s.CrawlSpline((a, b) =>
                {
                    b = transform.InverseTransformPoint(b);
                    vertices.Add(b + shear);
                    vertices.Add(b - shear);
                }, 60);
                for (int j = 0; j < vertices.Count - 3; j++)
                {
                    tris.Add(j);
                    tris.Add(j + 1);
                    tris.Add(j + 2);

                    tris.Add(j + 1);
                    tris.Add(j + 3);
                    tris.Add(j + 2);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = tris.ToArray();

            ParticleSystem ps = GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Mesh;
            shape.mesh = mesh;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (RegenerateRequired())
            {
                GenerateEmitter();
            }
        }

        private bool RegenerateRequired()
        {
            Sentinel sentinel = new Sentinel();

            Spline[] splines = GetComponentsInChildren<Spline>();
            for (int i = 0; i < splines.Length; i++)
            {
                sentinel = splines[i].AddToSentinel(sentinel);
            }

            if (_lastSentinel != sentinel.checksum)
            {
                _lastSentinel = sentinel.checksum;
                return true;
            }

            return false;
        }
#endif

    }
}
