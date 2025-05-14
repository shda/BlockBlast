using BlockBlast.Scripts.Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace BlockBlast.Scripts.Editor.ShapeGeneratorScripts
{
    [System.Serializable]
    public class ShapeGeneratorData
    {
        public TextAsset parsingText;
        public DefaultAsset folderSavePrefabs;
    }
}