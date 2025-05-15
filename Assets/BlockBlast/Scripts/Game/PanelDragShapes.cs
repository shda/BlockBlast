using System.Collections.Generic;
using BlockBlast.Scripts.Common.Extensions;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class PanelDragShapes : MonoBehaviour
    {
        [SerializeField] private ShapeCollection shapeCollection;
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameBoardGridSettings  boardGridSettings;
        
        [SerializeField] private float scaleOnPanel = 0.3f;
        [SerializeField] private int maxCountShapes = 3;
        [SerializeField] private Transform[] shapePoint;

        private int _tour = 0;
        public List<Shape> Shapes { get; set; } = new();
        
        public void CreateNewShapes()
        {
            _tour++;
            var prefabShapes = shapeCollection.GetPrefabShapes(_tour);

            foreach (var prefab in prefabShapes)
            {
                var newShape = Instantiate(prefab);
                newShape.CreateCrystals(crystalPool , boardGridSettings , Random.Range(0, 5));
                AddShape(newShape);
            }
        }
        
        public void RemoveShape(Shape shape)
        {
            Shapes.Remove(shape);
            shape.ReleaseCrystals(crystalPool);
            
            Destroy(shape.gameObject);

            if (Shapes.EmptyOnNull())
            {
                CreateNewShapes();
            }
        }
        
        private void AddShape(Shape shape)
        {
            if(Shapes.Count >= maxCountShapes)
                return;
            
            Shapes.Add(shape);
            shape.transform.position = shapePoint[Shapes.Count - 1].position; // - (shape.ColliderSize * (0.2f));
            shape.transform.localScale = new Vector3(scaleOnPanel , scaleOnPanel , 1);
        }

        //Debug
        public void DebugInitShapes(Shape[] setShapes)
        {
            foreach (var shape in Shapes)
            {
                shape.ReleaseCrystals(crystalPool);
                Destroy(shape.gameObject);
            }
            
            Shapes.Clear();

            foreach (var shape in setShapes)
            {
                var newShape = Instantiate(shape);
                newShape.CreateCrystals(crystalPool , boardGridSettings);
                AddShape(Instantiate(shape));
            }
        }

        public void ChangeShape(Shape shape)
        { 
            Shapes.Remove(shape);
            shape.ReleaseCrystals(crystalPool);
            
            Destroy(shape.gameObject);
            
            var newShape = Instantiate(shapeCollection.GetRandomShape(shape));
            newShape.CreateCrystals(crystalPool , boardGridSettings , Random.Range(0, 5));
            Shapes.Add(newShape);
            
            newShape.transform.position = shape.transform.position;
            newShape.transform.localScale = new Vector3(scaleOnPanel , scaleOnPanel , 1);
        }
    }
}