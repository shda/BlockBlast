using TMPro;
using UniRx;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus
{
    public class LabelCountBonus : MonoBehaviour
    {
        [SerializeField] private BonusCountData bonusCountData;
        [SerializeField] private TextMeshProUGUI labelCountBonus;

        [SerializeField] private bool isRotateBonus;
        [SerializeField] private bool isDestroyBonus;
        [SerializeField] private bool isChangeShape;
        
        private void Awake()
        {
            if(isRotateBonus)
                bonusCountData.CountRotateBonus.Subscribe(SetValue).AddTo(this);
            
            if(isDestroyBonus)
                bonusCountData.CountDestroyBonus.Subscribe(SetValue).AddTo(this);
            
            if(isChangeShape)
                bonusCountData.CountChangeShapeBonus.Subscribe(SetValue).AddTo(this);
        }

        private void SetValue(int value)
        {
            labelCountBonus.text = value.ToString();
        }
    }
}