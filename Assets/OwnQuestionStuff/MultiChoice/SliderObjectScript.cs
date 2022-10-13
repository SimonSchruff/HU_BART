using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class SliderObjectScript : MonoBehaviour
{
    [SerializeField] Slider sliderRef;
    [SerializeField] TMP_Text sliderTextRef;
    void Update()
    {
        
    }

    void Start (){
        valueChange();
    }

    public void valueChange (){
        sliderTextRef.text = sliderRef.value.ToString();

        GetComponentInParent<OneQuestionElem>().assignValue(sliderTextRef.text);
    }
}
