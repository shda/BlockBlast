using System.Collections.Generic;
using BlockBlast.Scripts.Common.Extensions;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class SelectShapesPanel : MonoBehaviour
    {
        [SerializeField] private ShapeCollection shapeCollection;
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameBoardGridSettings  boardGridSettings;
        
        [SerializeField] private float scaleToPanel = 0.3f;
        [SerializeField] private int maxCountShapes = 3;
        [SerializeField] private Transform[] shapePoint;
        
        private readonly List<Shape> _shapes = new();

        private void AddShape(Shape shape)
        {
            if(_shapes.Count >= maxCountShapes)
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
                CreateNewShapes();
            }
        }

        public void CreateNewShapes()
        {
            for (int i = 0; i < maxCountShapes; i++)
            {
                var newShape = shapeCollection.CreateRandomShape();
                newShape.CreateCrystals(crystalPool , boardGridSettings);
                AddShape(newShape);
            }
        }
    }
}