using System;
using System.Collections.Generic;
using System.Linq;
using BlockBlast.Scripts.Common;
using TriInspector;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.Rotate
{
    public class BonusRotateUi : MonoBehaviour
    {
        [SerializeField] private PanelDragShapes panelDragShapes;
        [SerializeField] private DragShapeHandler dragShapeHandler;
        [SerializeField] private PoolGameObjectsComponent<ButtonsRotate> poolButtons;

        private readonly List<ButtonsRotate> _buttonsRotates = new();
        
        public Action OnComplete;

        private void Awake()
        {
            poolButtons.Init();
        }

        [Button]
        public void ShowBonusUi()
        {
            dragShapeHandler.IsDragDisable = true;
            
            RemoveOldButtons();
            foreach (var shape in panelDragShapes.Shapes)
            {
                var buttonsRotate = poolButtons.Get();
                buttonsRotate.PressApply = rotate =>
                {
                    HideBonusUi();
                    OnComplete?.Invoke();
                };
                buttonsRotate.PressRotate = rotate =>
                {
                    foreach (var button in _buttonsRotates.
                                 Where(b => b != buttonsRotate).ToArray())
                    {
                        poolButtons.Release(button);
                        _buttonsRotates.Remove(button);
                    }
                    
                    shape.RotateShapeCounterClockwise();
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
            dragShapeHandler.IsDragDisable = false;
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