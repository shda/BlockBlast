using System.Collections.Generic;
using BlockBlast.Scripts.Common.Extensions;
using Unity.Collections;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class ShapesPanel : MonoBehaviour
    {
        [SerializeField] private ShapeCollection shapeCollection;
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameBoardGridSettings  boardGridSettings;
        
        [SerializeField] private float scaleToPanel = 0.3f;
        [SerializeField] private int maxShapes = 3;
        [SerializeField] private Transform[] shapePoint;
        
        private List<Shape> _shapes = new();
        
        public void AddShape(Shape shape)
        {
            if(_shapes.Count >= maxShapes)
               return;
            
            _shapes.Add(shape);
            shape.transform.position = shapePoint[_shapes.Count - 1].position;
            shape.transform.localScale = new Vector3(scaleToPanel , scaleToPanel , 1);
        }

        public void RemoveShape(Shape shape)
        {
            _shapes.Remove(shape);
            shape.ReleaseCrystals(crystalPool);
            
            Destroy(shape.gameObject);

            if (_shapes.EmptyOnNull())
            {
                CreateShapes();
            }
        }

        public void CreateShapes()
        {
            for (int i = 0; i < maxShapes; i++)
            {
                var newShape = shapeCollection.CrateRandomShape();
                newShape.CreateCrystals(crystalPool , boardGridSettings);
                AddShape(newShape);
            }
        }
    }
}