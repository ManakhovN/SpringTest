using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LengthChanger : MonoBehaviour {
    [SerializeField] SpringGenerator springGenerator;
    Slider slider;
    void Start () {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(LengthChanged);
        LengthChanged(springGenerator.Length);
	}

    private void LengthChanged(float arg0)
    {
        springGenerator.Length = arg0;
    }
}
