using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[ExecuteInEditMode]
public class SizePanelScript : MonoBehaviour
{
    [SerializeField] RectTransform questionText;
    [SerializeField] RectTransform checkboxesPanel;
    [SerializeField] QuestionCheckMulti rootScriptRef;

    private void Update()
    {
        if(questionText == null)
        {
            questionText = transform.GetChild(0).GetComponent<RectTransform>();
            checkboxesPanel = transform.GetChild(1).GetComponent<RectTransform>();      // Assign Refs
        }
        if(rootScriptRef == null)
        {
            rootScriptRef = transform.GetComponentInParent<QuestionCheckMulti>();
        }
        questionText.anchorMax = new Vector2(rootScriptRef.questionSpace, questionText.anchorMax.y);
        checkboxesPanel.anchorMin = new Vector2(rootScriptRef.questionSpace, checkboxesPanel.anchorMin.y);

    }
}
