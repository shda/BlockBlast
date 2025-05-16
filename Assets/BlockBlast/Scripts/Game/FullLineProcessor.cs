using System.Collections.Generic;
using BlockBlast.Scripts.Game.Bonus;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class FullLineProcessor : MonoBehaviour
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private GameBoardGridSettings boardGridSettings;
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private LineExplosionAnimationPool lineExplosionAnimationPool;
        [SerializeField] private ScoreLabes scoreLabes;
        [SerializeField] private BonusHandler bonusHandler;
        
        public void CheckFullLines(Shape dragShape)
        {
            var size = boardGridSettings.GridSize;
            
            var crystalsHide = new HashSet<Crystal>();
            
            var lines = CheckLines(true); // Проверка строк
            lines += CheckLines(false); // Проверка столбцов
            bonusHandler.FullLineRemove(lines);
            AnimatedRemoveCrystalsToLine(crystalsHide);
            
            
            int  CheckLines(bool isRow)
            { 
                int countLines = 0;
                for (var i = 0; i < (isRow ? size.x : size.y); i++)
                {
                    var crystals = new List<Crystal>();
                    for (var j = 0; j < (isRow ? size.y : size.x); j++)
                    {
                        var x = isRow ? i : j;
                        var y = isRow ? j : i;
                        var point = gameBoard.Crystals[x, y];
                        if (point)
                        {
                            crystals.Add(gameBoard.Crystals[x, y]);
                        }
                        else
                        {
                            crystals.Clear();
                            break;
                        }
                    }

                    if (crystals.Count > 0)
                    {
                        foreach (var crystal in crystals)
                        {
                            crystalsHide.Add(crystal);
                        }

                        countLines++;
                        PlayExplosionLine(crystals , dragShape);
                    }
                }

                return countLines;
            }
        }
        
        private void AnimatedRemoveCrystalsToLine(IEnumerable<Crystal> crystals)
        {
            foreach (var crystal in crystals)
            {
                scoreLabes.AddScore(1);

                crystal.PlayAnimationHide(() =>
                {
                    crystal.transform.localScale = Vector3.one;
                    crystalPool.Release(crystal);
                });
                
                gameBoard.ClearPoint(crystal.Coordinates);
            }
        }

        private void PlayExplosionLine(List<Crystal> crystals , Shape dragShape)
        {
            var isVertical = !Mathf.Approximately(crystals[0].transform.position.y, crystals[1].transform.position.y);
            var firstPosition = crystals[0].transform.position;

            foreach (var crystal in crystals)
            {
                if (crystal.transform.position.x < firstPosition.x || 
                    crystal.transform.position.y < firstPosition.y)
                {
                    firstPosition = crystal.transform.position;
                }
            }

            var explosion = lineExplosionAnimationPool.Get();
            explosion.transform.position = firstPosition;
            explosion.SetColor(dragShape.CrystalColor);
            if (isVertical)
            {
                explosion.PlayVerticalAnimation();
            }
            else
            {
                explosion.PlayHorizontalAnimation();
            }

            explosion.OnComplete = () =>
            {
                lineExplosionAnimationPool.Release(explosion); 
            };
        }
    }
}