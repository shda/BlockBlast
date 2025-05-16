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
        [SerializeField] private PoolGameObjectsComponent<ButtonsRotate> poolButtons;
        private readonly List<ButtonsRotate> _buttonsRotates = new();
        
        public Action<Shape> OnPressRotate;
        public Action<Shape> OnPressApply;

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
                buttonsRotate.PressApply = rotate =>
                {
                   OnPressApply?.Invoke(shape);
                };
                buttonsRotate.PressRotate = rotate =>
                {
                    foreach (var button in _buttonsRotates.
                                 Where(b => b != buttonsRotate).ToArray())
                    {
                        poolButtons.Release(button);
                        _buttonsRotates.Remove(button);
                    }
                    
                    OnPressRotate?.Invoke(shape);
                };
                
                buttonsRotate.transform.parent = transform;
                buttonsRotate.transform.localScale = Vector3.one;
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