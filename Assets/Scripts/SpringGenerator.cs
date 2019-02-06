using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringGenerator : MonoBehaviour {
    [Header("Величина шага")]
    [SerializeField] private float stepSize = 1f;
    [Header("Радиус пружины")]
    [SerializeField] private float radius = 1f;
    [Header("Длина пружины")]
    [SerializeField] private float length = 5f;
    public float Length {
        get {
            return length;
        }
        set {
            if (length != value) {
                length = value;
                GenerateSpring();
            }
        }
    }

    public float Strength
    {
        get
        {
            return strength;
        }

        set
        {
            if (strength != value)
            {
                strength = value;
                GenerateSpring();
                OnStrengthChanged.Invoke(value);
            }
        }
    }

    public Action<float> OnStrengthChanged = delegate { };
    [Header("Радиус сечения пружины")]
    [SerializeField] float wireRadius = 0.1f;
    [Header("Количество сегментов сечения пружины")]
    [SerializeField] int wireSegmentsCount = 10;
    [Header("Угол сегментации витка")]
    [SerializeField]float stepSegmentAngle = 30;
    [Header("Сила сжатия")]
    [SerializeField] float strength = 0f;
    [Header("Кривая сжатия пружины")]
    [SerializeField] AnimationCurve strengthImpact;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh mesh;
    void Start () {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        mesh = new Mesh();
        GenerateSpring();	
	}

    public void GenerateSpring() {
        var points = GeneratePoints();
        var triangles = GenerateTriangles(points);
        mesh.Clear();
        mesh.SetVertices(points);
        mesh.SetTriangles(triangles, 0);
        skinnedMeshRenderer.sharedMesh = mesh;
    }

    public List<Vector3> GeneratePoints() {
        List<Vector3> points = new List<Vector3>();
        float currentLength = 0f;
        float compressedLength = 0f;
        int segmentId = 0;
        float lengthPerSegment = stepSize * (stepSegmentAngle / 360f);
        while (currentLength < length)
        {
            currentLength += lengthPerSegment;
            float compressedSegmentLength = lengthPerSegment - strength * strengthImpact.Evaluate(currentLength / length) * lengthPerSegment;
            compressedLength += (compressedSegmentLength<0f)?0f:compressedSegmentLength;
            for (int i = 0; i < wireSegmentsCount; i++)
            {
                Vector3 p = new Vector3(wireRadius, 0f, 0f);
                p = Quaternion.Euler(0f, 0f, i * 360f / (float)wireSegmentsCount) * p;
                p += Vector3.right * radius;
                p = Quaternion.Euler(0f, segmentId * stepSegmentAngle, 0f) * p;
                p += Vector3.up * (compressedLength);
                points.Add(p);
            }
                segmentId++;
        }
        return points;
    }

    public List<int> GenerateTriangles(List<Vector3> points) {
        List<int> triangles = new List<int>();
        int segmentId = 0;
        while ((segmentId+1) * wireSegmentsCount < points.Count)
        {
            for (int i = 0; i < wireSegmentsCount; i++) {
                triangles.Add(i + segmentId * wireSegmentsCount);
                triangles.Add(i + (segmentId +1 ) * wireSegmentsCount);
                triangles.Add((i+1)%wireSegmentsCount + segmentId * wireSegmentsCount);

                triangles.Add((i + 1) % (wireSegmentsCount) + (segmentId + 1) * wireSegmentsCount);
                triangles.Add((i + 1) % (wireSegmentsCount) + segmentId * wireSegmentsCount);
                triangles.Add(i + (segmentId + 1) * wireSegmentsCount);
            }
            segmentId++;
        }
        return triangles;
    }

  }
