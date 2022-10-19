using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[ExecuteInEditMode]
public class GetLegendSizeScript : MonoBehaviour
{
    QuestionCheckMulti rootRef;
    TMP_Text selfText;
    [SerializeField] bool isFirstLegendEntry = false;

    void Update()
    {
        if(rootRef == null || selfText == null)
        {
            rootRef = GetComponentInParent<QuestionCheckMulti>();
            selfText = GetComponent<TMP_Text>();
        }
        try
        {
            selfText.fontSize = rootRef.legendTextSize;
        }
        catch
        {

        }
        if (isFirstLegendEntry)
        {
            if (GetComponentInParent<QuestionCheckMulti>().hasLegendStartText)
            {
                gameObject.SetActive(true);
                TMP_Text text = GetComponent<TMP_Text>();
                text.text = GetComponentInParent<QuestionCheckMulti>().legendStartText;
            }
            else
            {
               gameObject.SetActive(false);
            }
           
        }
    }

    private void Start()
    {
    }
}
