using BlockBlast.Scripts.Game;
using UnityEditor;
using UnityEngine;

namespace BlockBlast.Scripts.Editor
{
    [CustomEditor(typeof(IntGrid))]
    public class BoolGridEditor : UnityEditor.Editor
    {
        private Texture2D myTexture;
        private GUIStyle textureButtonStyle;
        
        public override void OnInspectorGUI()
        {
            if (!myTexture)
            {
                myTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets\\BlockBlast\\Scripts\\Editor\\block.png");
            }

            textureButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(0, 0, 0, 0)
            };
            
            var buttonContent = new GUIContent(myTexture);
            var gridTarget = (IntGrid)target;

            EditorGUI.BeginChangeCheck();

            gridTarget.width = EditorGUILayout.IntField("Width", gridTarget.width);
            gridTarget.height = EditorGUILayout.IntField("Height", gridTarget.height);

            if (GUILayout.Button("Resize Grid"))
            {
                gridTarget.ResizeGrid();
            }

            GUILayout.Space(10);
            GUILayout.Label("Grid:");

            var grid = gridTarget.Grid;

            int size = 40;

            for (int y = 0; y < grid.Count; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < grid[y].row.Count; x++)
                {
                    bool isPressed = false;
                    if (grid[y].row[x] > 0)
                    {
                        isPressed = GUILayout.Button( buttonContent, GUIStyle.none , GUILayout.Width(size) , GUILayout.Height(size));
                    }
                    else
                    {
                        isPressed = GUILayout.Button( "" , textureButtonStyle , GUILayout.Width(size) , GUILayout.Height(size));
                    }
                    
                    if (isPressed)
                    {
                        if (grid[y].row[x] == 0)
                        {
                            grid[y].row[x] = 1;
                        }
                        else
                        {
                            grid[y].row[x] = 0;
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(gridTarget);
            }
        }
    }
}