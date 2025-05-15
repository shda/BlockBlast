using System;
using System.Collections.Generic;
using BlockBlast.Scripts.Common;
using TriInspector;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.ChangeShape
{
    public class BonusChangeShapeUi : MonoBehaviour
    {
        [SerializeField] private PoolGameObjectsComponent<ButtonsChangeShape> poolButtons;
        private readonly List<ButtonsChangeShape> _buttonsRotates = new();
        
        public Action<Shape> OnPressChangeShape;

        private void Awake()
        {
            poolButtons.Init();
        }

        [Button]
        public void ShowBonusUi(IEnumerable<Shape> shapes)
        {
            RemoveOldButtons();
            foreach (var shape in shapes)
            {
                var buttonsRotate = poolButtons.Get();
                buttonsRotate.PressChangeShape = () =>
                {
                    OnPressChangeShape?.Invoke(shape);
                };
                
                buttonsRotate.transform.parent = transform;
                buttonsRotate.SetFallowTransform(shape.transform);
                
                _buttonsRotates.Add(buttonsRotate);
            }
        }
        
        [Button]
        public void HideBonusUi()
        {
            RemoveOldButtons();
        }

        private void RemoveOldButtons()
        {
            foreach (var rotate in _buttonsRotates)
            {
                poolButtons.Release(rotate);
            }

            _buttonsRotates.Clear();
        }
    }
}