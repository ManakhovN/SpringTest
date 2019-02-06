using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowChanger : MonoBehaviour {
    [SerializeField] SpringGenerator springGenerator;
    SkinnedMeshRenderer skinnedMeshRenderer;
	void Start () {
        if (springGenerator == null)
            GetComponent<SpringGenerator>();
        skinnedMeshRenderer = springGenerator.GetComponent<SkinnedMeshRenderer>();
        springGenerator.OnStrengthChanged += ChangeGlowIntensity;
    }

    void ChangeGlowIntensity(float intensity) {
        skinnedMeshRenderer.material.SetFloat("_MKGlowPower", intensity/8);
    }
}
