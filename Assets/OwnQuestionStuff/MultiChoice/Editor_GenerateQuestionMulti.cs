
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestionCheckMulti))]
public class Editor_GenerateQuestionMulti : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestionCheckMulti questionCheckRef = (QuestionCheckMulti)target;
        if (GUILayout.Button("Redraw Questions"))
        {
            questionCheckRef.redrawQuestions(); // how do i call this?
        }
     //   DrawDefaultInspector();
    }
}
