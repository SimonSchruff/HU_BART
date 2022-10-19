using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class GetCheckTextSizeScript : MonoBehaviour
{
    QuestionCheckMulti rootRef;
    TMP_Text selfText;

    void Update()
    {
        if (rootRef == null)
        {
            rootRef = GetComponentInParent<QuestionCheckMulti>();
            selfText = GetComponent<TMP_Text>();
        }
        selfText.fontSize = rootRef.checkboxTextSize;
    }
}
