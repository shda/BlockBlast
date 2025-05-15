using BlockBlast.Scripts.Game.Bonus.Rotate;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.ChangeShape
{
    public class BonusChangeShape : BonusBase
    {
        [SerializeField] private PanelDragShapes panelDragShapes;
        [SerializeField] private DragShapeHandler dragShapeHandler;
        [SerializeField] private BonusChangeShapeUi  changeShapeUi;
        
        public override void StartBonus()
        {
            dragShapeHandler.IsDragDisable = true;
            
            changeShapeUi.ShowBonusUi(panelDragShapes.Shapes);
            changeShapeUi.OnPressChangeShape = shape =>
            {
                panelDragShapes.ChangeShape(shape);
                
                dragShapeHandler.IsDragDisable = false;
                changeShapeUi.HideBonusUi();
                
                OnComplete?.Invoke();
            };
        }

        public override void StopBonus()
        {
            dragShapeHandler.IsDragDisable = false;
            changeShapeUi.HideBonusUi();
        }
    }
}