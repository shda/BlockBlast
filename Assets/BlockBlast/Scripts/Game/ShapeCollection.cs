using System.Collections.Generic;
using System.Linq;
using BlockBlast.Scripts.Common.Extensions;
using UnityEngine;
using UnityEngine.Pool;

namespace BlockBlast.Scripts.Game
{
    [CreateAssetMenu(menuName = "BlockBlast/Create " + nameof(ShapeCollection), fileName = nameof(ShapeCollection), order = 0)]
    public class ShapeCollection : ScriptableObject
    {
        [SerializeField] private Shape[] sampleBlocks;
        [SerializeField] private Shape[] hardBlocks;
        
        public IEnumerable<Shape> GetPrefabShapes(int tour)
        {
            var shapes = ListPool<Shape>.Get();

            //С первого раунда по четвертый раунд игроку всегда выдается три простых блока;
            if (tour <= 4)
            {
                Debug.Log($"Step 1: {tour}");
                shapes.AddRange(GetRandomUniqueShapes(sampleBlocks, 3));
            }
            // С пятого по девятый раунд игроку всегда выдается один сложный блок и два простых блока;
            else if (tour <= 10)
            {
                Debug.Log($"Step 2: {tour}");
                shapes.AddRange(GetRandomUniqueShapes(hardBlocks, 1));
                shapes.AddRange(GetRandomUniqueShapes(sampleBlocks, 2));
            }
            //С десятого по четырнадцатый раунд игроку выдается два сложных блока и один простой;
            else if (tour <= 14)
            {
                Debug.Log($"Step 3: {tour}");
                shapes.AddRange(GetRandomUniqueShapes(hardBlocks, 2));
                shapes.AddRange(GetRandomUniqueShapes(sampleBlocks, 1));
            }
            //Начиная с пятнадцатого раунда и до окончания игровой сессии игроку выдаются случайные блоки
            else
            {
                Debug.Log($"Step 4: {tour}");
                shapes.AddRange(GetRandomUniqueShapes(sampleBlocks.Concat(hardBlocks), 3));
            }

            var retArray = shapes.ToArray();
            ListPool<Shape>.Release(shapes);

            return retArray;
        }

        private IEnumerable<Shape> GetRandomUniqueShapes(IEnumerable<Shape> shapes, int count)
        {
            var tempList = ListPool<Shape>.Get();
            tempList.AddRange(shapes);
            tempList.Shuffle();
            
            return tempList.GetRange(0, count);
        }
    }
}