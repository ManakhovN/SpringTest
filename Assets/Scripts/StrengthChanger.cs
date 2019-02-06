using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class StrengthChanger : MonoBehaviour {
    [SerializeField] SpringGenerator springGenerator;
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(StrengthChanged);
        StrengthChanged(springGenerator.Strength);
    }

    private void StrengthChanged(float arg0)
    {
        springGenerator.Strength = arg0;
    }
}
