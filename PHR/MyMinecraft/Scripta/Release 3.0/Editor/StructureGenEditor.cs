using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StructureGenerator))]
public class StructureGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Create a Button
        if (GUILayout.Button("Gen Structure"))
        {
            // Cast the target to your script
            StructureGenerator generator = (StructureGenerator)target;

            // Call the function
            generator.GenStructure();
        }
    }
}