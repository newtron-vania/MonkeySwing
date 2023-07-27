using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// ? ì§œ : 2021-03-09 AM 1:08:48
// ?‘ì„±??: Rito

/*
    [ê°€ì¤‘ì¹˜ ?œë¤ ë½‘ê¸°]

    - ?œë„¤ë¦?„ ?µí•´ ?„ì´?œì˜ ?€?…ì„ ì§€?•í•´ ê°ì²´?”í•˜???¬ìš©?œë‹¤.
    - ì¤‘ë³µ?˜ëŠ” ?„ì´?œì´ ?†ë„ë¡??•ì…”?ˆë¦¬ë¡?êµ¬í˜„?˜ì???
    - ê°€ì¤‘ì¹˜ê°€ 0ë³´ë‹¤ ?‘ì? ê²½ìš° ?ˆì™¸ë¥??¸ì¶œ?œë‹¤.

    - double SumOfWeights : ?„ì²´ ?„ì´?œì˜ ê°€ì¤‘ì¹˜ ???½ê¸° ?„ìš© ?„ë¡œ?¼í‹°)

    - void Add(T, double) : ?ˆë¡œ???„ì´??ê°€ì¤‘ì¹˜ ?ì„ ì¶”ê??œë‹¤.
    - void Add(params (T, double)[]) : ?ˆë¡œ???„ì´??ê°€ì¤‘ì¹˜ ?ì„ ?¬ëŸ¬ ê°?ì¶”ê??œë‹¤.
    - void Remove(T) : ?€???„ì´?œì„ ëª©ë¡?ì„œ ?œê±°?œë‹¤.
    - void ModifyWeight(T, double) : ?€???„ì´?œì˜ ê°€ì¤‘ì¹˜ë¥?ë³€ê²½í•œ??
    - void ReSeed(int) : ?œë¤ ?œë“œë¥??¬ì„¤?•í•œ??

    - T GetRandomPick() : ?„ì¬ ?„ì´??ëª©ë¡?ì„œ ê°€ì¤‘ì¹˜ë¥?ê³„ì‚°?˜ì—¬ ?œë¤?¼ë¡œ ??ª© ?˜ë‚˜ë¥?ë½‘ì•„?¨ë‹¤.
    - T GetRandomPick(double) : ?´ë? ê³„ì‚°???•ë¥  ê°’ì„ ë§¤ê°œë³€?˜ë¡œ ?£ì–´, ?´ë‹¹?˜ëŠ” ??ª© ?˜ë‚˜ë¥?ë½‘ì•„?¨ë‹¤.
    - double GetWeight(T) : ?€???„ì´?œì˜ ê°€ì¤‘ì¹˜ë¥??»ì–´?¨ë‹¤.
    - double GetNormalizedWeight(T) : ?€???„ì´?œì˜ ?•ê·œ?”ëœ ê°€ì¤‘ì¹˜ë¥??»ì–´?¨ë‹¤.

    - ReadonlyDictionary<T, double> GetItemDictReadonly() : ?„ì²´ ?„ì´??ëª©ë¡???½ê¸°?„ìš© ì»¬ë ‰?˜ìœ¼ë¡?ë°›ì•„?¨ë‹¤.
    - ReadonlyDictionary<T, double> GetNormalizedItemDictReadonly()
      : ?„ì²´ ?„ì´?œì˜ ê°€ì¤‘ì¹˜ ì´í•©??1???˜ë„ë¡??•ê·œ?”ëœ ?„ì´??ëª©ë¡???½ê¸°?„ìš© ì»¬ë ‰?˜ìœ¼ë¡?ë°›ì•„?¨ë‹¤.
*/

namespace Rito
{
    /// <summary> ê°€ì¤‘ì¹˜ ?œë¤ ë½‘ê¸° </summary>
    public class WeightedRandomPicker<T>
    {
        /// <summary> ?„ì²´ ?„ì´?œì˜ ê°€ì¤‘ì¹˜ ??</summary>
        public double SumOfWeights
        {
            get
            {
                CalculateSumIfDirty();
                return _sumOfWeights;
            }
        }

        private System.Random randomInstance;
        private readonly Dictionary<T, double> itemWeightDict;
        private readonly Dictionary<T, double> normalizedItemWeightDict; // ?•ë¥ ???•ê·œ?”ëœ ?„ì´??ëª©ë¡

        /// <summary> ê°€ì¤‘ì¹˜ ?©ì´ ê³„ì‚°?˜ì? ?Šì? ?íƒœ?¸ì? ?¬ë? </summary>
        private bool isDirty;
        private double _sumOfWeights;

        /***********************************************************************
        *                               Constructors
        ***********************************************************************/
        #region .
        public WeightedRandomPicker()
        {
            randomInstance = new System.Random();
            itemWeightDict = new Dictionary<T, double>();
            normalizedItemWeightDict = new Dictionary<T, double>();
            isDirty = true;
            _sumOfWeights = 0.0;
        }

        public WeightedRandomPicker(int randomSeed)
        {
            randomInstance = new System.Random(randomSeed);
            itemWeightDict = new Dictionary<T, double>();
            normalizedItemWeightDict = new Dictionary<T, double>();
            isDirty = true;
            _sumOfWeights = 0.0;
        }

        #endregion
        /***********************************************************************
        *                               Add Methods
        ***********************************************************************/
        #region .

        /// <summary> ?ˆë¡œ???„ì´??ê°€ì¤‘ì¹˜ ??ì¶”ê? </summary>
        public void Add(T item, double weight)
        {
            CheckDuplicatedItem(item);
            CheckValidWeight(weight);

            itemWeightDict.Add(item, weight);
            isDirty = true;
        }

        /// <summary> ?ˆë¡œ???„ì´??ê°€ì¤‘ì¹˜ ?ë“¤ ì¶”ê? </summary>
        public void Add(params (T item, double weight)[] pairs)
        {
            foreach (var pair in pairs)
            {
                CheckDuplicatedItem(pair.item);
                CheckValidWeight(pair.weight);

                itemWeightDict.Add(pair.item, pair.weight);
            }
            isDirty = true;
        }

        #endregion
        /***********************************************************************
        *                               Public Methods
        ***********************************************************************/
        #region .

        /// <summary> ëª©ë¡?ì„œ ?€???„ì´???œê±° </summary>
        public void Remove(T item)
        {
            CheckNotExistedItem(item);

            itemWeightDict.Remove(item);
            isDirty = true;
        }

        /// <summary> ?€???„ì´?œì˜ ê°€ì¤‘ì¹˜ ?˜ì • </summary>
        public void ModifyWeight(T item, double weight)
        {
            CheckNotExistedItem(item);
            CheckValidWeight(weight);

            itemWeightDict[item] = weight;
            isDirty = true;
        }

        /// <summary> ?œë¤ ?œë“œ ?¬ì„¤??</summary>
        public void ReSeed(int seed)
        {
            randomInstance = new System.Random(seed);
        }

        #endregion
        /***********************************************************************
        *                               Getter Methods
        ***********************************************************************/
        #region .

        /// <summary> ?œë¤ ë½‘ê¸° </summary>
        public T GetRandomPick()
        {
            // ?œë¤ ê³„ì‚°
            double chance = randomInstance.NextDouble(); // [0.0, 1.0)
            chance *= SumOfWeights;

            return GetRandomPick(chance);
        }

        /// <summary> ì§ì ‘ ?œë¤ ê°’ì„ ì§€?•í•˜??ë½‘ê¸° </summary>
        public T GetRandomPick(double randomValue)
        {
            if (randomValue < 0.0) randomValue = 0.0;
            if (randomValue > SumOfWeights) randomValue = SumOfWeights - 0.00000001;

            double current = 0.0;
            foreach (var pair in itemWeightDict)
            {
                current += pair.Value;

                if (randomValue < current)
                {
                    return pair.Key;
                }
            }

            throw new Exception($"Unreachable - [Random Value : {randomValue}, Current Value : {current}]");
            //return itemPairList[itemPairList.Count - 1].item; // Last Item
        }

        /// <summary> ?€???„ì´?œì˜ ê°€ì¤‘ì¹˜ ?•ì¸ </summary>
        public double GetWeight(T item)
        {
            return itemWeightDict[item];
        }

        /// <summary> ?€???„ì´?œì˜ ?•ê·œ?”ëœ ê°€ì¤‘ì¹˜ ?•ì¸ </summary>
        public double GetNormalizedWeight(T item)
        {
            CalculateSumIfDirty();
            return normalizedItemWeightDict[item];
        }

        /// <summary> ?„ì´??ëª©ë¡ ?•ì¸(?½ê¸° ?„ìš©) </summary>
        public ReadOnlyDictionary<T, double> GetItemDictReadonly()
        {
            return new ReadOnlyDictionary<T, double>(itemWeightDict);
        }

        /// <summary> ê°€ì¤‘ì¹˜ ?©ì´ 1???˜ë„ë¡??•ê·œ?”ëœ ?„ì´??ëª©ë¡ ?•ì¸(?½ê¸° ?„ìš©) </summary>
        public ReadOnlyDictionary<T, double> GetNormalizedItemDictReadonly()
        {
            CalculateSumIfDirty();
            return new ReadOnlyDictionary<T, double>(normalizedItemWeightDict);
        }

        #endregion
        /***********************************************************************
        *                               Private Methods
        ***********************************************************************/
        #region .

        /// <summary> ëª¨ë“  ?„ì´?œì˜ ê°€ì¤‘ì¹˜ ??ê³„ì‚°?´ë†“ê¸?</summary>
        private void CalculateSumIfDirty()
        {
            if(!isDirty) return;
            isDirty = false;

            _sumOfWeights = 0.0;
            foreach (var pair in itemWeightDict)
            {
                _sumOfWeights += pair.Value;
            }

            // ?•ê·œ???•ì…”?ˆë¦¬???…ë°?´íŠ¸
            UpdateNormalizedDict();
        }

        /// <summary> ?•ê·œ?”ëœ ?•ì…”?ˆë¦¬ ?…ë°?´íŠ¸ </summary>
        private void UpdateNormalizedDict()
        {
            normalizedItemWeightDict.Clear();
            foreach(var pair in itemWeightDict)
            {
                normalizedItemWeightDict.Add(pair.Key, pair.Value / _sumOfWeights);
            }
        }

        /// <summary> ?´ë? ?„ì´?œì´ ì¡´ì¬?˜ëŠ”ì§€ ?¬ë? ê²€??</summary>
        private void CheckDuplicatedItem(T item)
        {
            if(itemWeightDict.ContainsKey(item))
                throw new Exception($"?´ë? [{item}] ?„ì´?œì´ ì¡´ì¬?©ë‹ˆ??");
        }

        /// <summary> ì¡´ì¬?˜ì? ?ŠëŠ” ?„ì´?œì¸ ê²½ìš° </summary>
        private void CheckNotExistedItem(T item)
        {
            if(!itemWeightDict.ContainsKey(item))
                throw new Exception($"[{item}] ?„ì´?œì´ ëª©ë¡??ì¡´ì¬?˜ì? ?ŠìŠµ?ˆë‹¤.");
        }

        /// <summary> ê°€ì¤‘ì¹˜ ê°?ë²”ìœ„ ê²€??0ë³´ë‹¤ ì»¤ì•¼ ?? </summary>
        private void CheckValidWeight(in double weight)
        {
            if (weight < 0f)
                throw new Exception("ê°€ì¤‘ì¹˜ ê°’ì? 0ë³´ë‹¤ ì»¤ì•¼ ?©ë‹ˆ??");
        }

        #endregion
    }
}