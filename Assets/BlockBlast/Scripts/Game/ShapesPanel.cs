using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class ShapesPanel : MonoBehaviour
    {
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
    }
}