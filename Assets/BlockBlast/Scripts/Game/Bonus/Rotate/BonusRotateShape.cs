using TriInspector;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.Rotate
{
    public class BonusRotateShape : BonusBase
    {
        [SerializeField] private PanelDragShapes panelDragShapes;
        [SerializeField] private DragShapeHandler dragShapeHandler;
        [SerializeField] private BonusRotateUi  bonusRotateUi;
         
        
        [Button]
        public override void StartBonus()
        {
            dragShapeHandler.IsDragDisable = true;
            bonusRotateUi.ShowBonusUi(panelDragShapes.Shapes);

            bool isChange = false;
            
            bonusRotateUi.OnPressApply = shape =>
            {
                dragShapeHandler.IsDragDisable = false;
                bonusRotateUi.HideBonusUi();
                OnComplete?.Invoke();
            };

            bonusRotateUi.OnPressRotate = shape =>
            {
                shape.RotateShapeCounterClockwise();
                
                if (!isChange)
                {
                    isChange = true;
                    bonusCountData.CountRotateBonus.Value--;
                }
            };
        }

        [Button]
        public override void StopBonus()
        {
            dragShapeHandler.IsDragDisable = false;
            bonusRotateUi.HideBonusUi();
        }

        public override bool IsCanUsingBonus()
        {
            return bonusCountData.CountRotateBonus.Value > 0;
        }
    }
}