using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OneToggleEntry : MonoBehaviour
{
    public Toggle toggleElemRef;  

    public void buttonClick (){
        toggleElemRef.isOn = !toggleElemRef.isOn;
    }

}
// 