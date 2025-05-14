using System;
using System.IO;
using BlockBlast.Scripts.Game;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace BlockBlast.Scripts.Editor.ShapeGeneratorScripts
{
    [CreateAssetMenu(menuName = "Create " + nameof(ShapeMaker), fileName = nameof(ShapeMaker), order = 0)]
    public class ShapeMaker : ScriptableObject
    {
        [SerializeField] private Shape shapePrefab;
        [SerializeField] private ShapeGeneratorData[] shapeData;
        
        [Button]
        public void Make()
        {
            foreach (var data in shapeData)
            {
                string path = AssetDatabase.GetAssetPath(data.folderSavePrefabs);
                Debug.Log(path);

                DeleteAllPrefabsInFolder(path);
                
                string content = data.parsingText.text;
                // Разделяем по блокам, используя "\n\n" как разделитель
                string[] blocks = content.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                int index = 1;
                
                foreach (var block in blocks)
                {
                    if(string.IsNullOrWhiteSpace(block))
                        continue;
                    
                    string[] lines = block.Split("\r\n");
                    
                    if(lines.Length <= 0)
                        continue;

                    int rows = lines.Length;
                    int cols = lines[0].Length;

                    int[,] matrix = new int[rows, cols];

                    for (int row = 0; row < rows; row++)
                    {
                        for (int col = 0; col < cols; col++)
                        {
                            char c = lines[row][col];
                            if (c == 'x')
                                matrix[row, col] = 1;
                            else if (c == 'o')
                                matrix[row, col] = 0;
                            else
                                throw new FormatException($"Неизвестный символ '{c}' на [{row},{col}]");
                        }
                    }

                    CreateShape(matrix , path , index);
                    index++;
                    /*
                    // Выводим матрицу
                    Debug.Log("Новый блок:");
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            Debug.Log(matrix[i, j]);
                        }
                        Debug.Log("");
                    }
                    */
                }
            }
        }


        private void CreateShape(int[,] matrix , string pathToFolder , int index)
        {
            string variantPrefabPath = pathToFolder + "/" + index + ".prefab";
            
            var newShape = PrefabUtility.InstantiatePrefab(shapePrefab, null) as Shape;
            PrefabUtility.ApplyPrefabInstance(newShape.gameObject, InteractionMode.AutomatedAction);
            
            newShape.SetMatrix(matrix);
            PrefabUtility.SaveAsPrefabAssetAndConnect(
                newShape.gameObject,
                variantPrefabPath,
                InteractionMode.AutomatedAction
            );
            
            DestroyImmediate(newShape.gameObject);
        }

        private static void DeleteAllPrefabsInFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError($"Папка не найдена: {folderPath}");
                return;
            }

            // Получаем все .prefab файлы в папке
            string[] prefabPaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.TopDirectoryOnly);

            foreach (string path in prefabPaths)
            {
                string assetPath = path.Replace('\\', '/').Replace(Application.dataPath, "Assets");

                // Удаляем префаб
                AssetDatabase.DeleteAsset(assetPath);
                Debug.Log($"Префаб удален: {assetPath}");
            }

            AssetDatabase.Refresh();
            Debug.Log($"Очистка завершена. Удалено префабов: {prefabPaths.Length}");
        }
    }
}