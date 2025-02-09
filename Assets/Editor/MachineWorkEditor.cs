#if UNITY_EDITOR
using UnityEditor;
using InteractObjects.Work;

[CustomEditor(typeof(MachineWork))]
public class MachineWorkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawPropertiesExcluding(serializedObject, "workConfig");
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
