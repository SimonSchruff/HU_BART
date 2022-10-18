using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[ExecuteInEditMode]
public class SizePanelScript : MonoBehaviour
{
    public bool isDoppleLegend = false;
    [SerializeField] RectTransform questionText;
    [SerializeField] RectTransform checkboxesPanel;
    [SerializeField] RectTransform secondLegend;
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

        if (isDoppleLegend)
        {
            questionText.anchorMax = new Vector2(rootScriptRef.questionSpace, questionText.anchorMax.y);
            checkboxesPanel.anchorMin = new Vector2(rootScriptRef.questionSpace, checkboxesPanel.anchorMin.y);
            checkboxesPanel.anchorMax= new Vector2(((-1)*(rootScriptRef.questionSpace))+1, checkboxesPanel.anchorMax.y);
            secondLegend.anchorMin = new Vector2((rootScriptRef.questionSpace - 1)*(-1), checkboxesPanel.anchorMin.y);
        }
        else
        {
            questionText.anchorMax = new Vector2(rootScriptRef.questionSpace, questionText.anchorMax.y);
            checkboxesPanel.anchorMin = new Vector2(rootScriptRef.questionSpace, checkboxesPanel.anchorMin.y);
        }
    }
}
