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
        
        public List<Shape> Shapes { get; set; } = new();

        private void AddShape(Shape shape)
        {
            if(Shapes.Count >= maxCountShapes)
               return;
            
            Shapes.Add(shape);
            shape.transform.position = shapePoint[Shapes.Count - 1].position;
            shape.transform.localScale = new Vector3(scaleToPanel , scaleToPanel , 1);
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

        public void CreateNewShapes()
        {
            for (int i = 0; i < maxCountShapes; i++)
            {
                var newShape = shapeCollection.CreateRandomShape();
                newShape.CreateCrystals(crystalPool , boardGridSettings , Random.Range(0, 5));
                AddShape(newShape);
            }
        }
    }
}