using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// ? μ§ : 2021-03-09 AM 1:08:48
// ?μ±??: Rito

/*
    [κ°μ€μΉ ?λ€ λ½κΈ°]

    - ?λ€λ¦? ?΅ν΄ ?μ΄?μ ??μ μ§?ν΄ κ°μ²΄?ν???¬μ©?λ€.
    - μ€λ³΅?λ ?μ΄?μ΄ ?λλ‘??μ?λ¦¬λ‘?κ΅¬ν?μ???
    - κ°μ€μΉκ° 0λ³΄λ€ ?μ? κ²½μ° ?μΈλ₯??ΈμΆ?λ€.

    - double SumOfWeights : ?μ²΄ ?μ΄?μ κ°μ€μΉ ???½κΈ° ?μ© ?λ‘?Όν°)

    - void Add(T, double) : ?λ‘???μ΄??κ°μ€μΉ ?μ μΆκ??λ€.
    - void Add(params (T, double)[]) : ?λ‘???μ΄??κ°μ€μΉ ?μ ?¬λ¬ κ°?μΆκ??λ€.
    - void Remove(T) : ????μ΄?μ λͺ©λ‘?μ ?κ±°?λ€.
    - void ModifyWeight(T, double) : ????μ΄?μ κ°μ€μΉλ₯?λ³κ²½ν??
    - void ReSeed(int) : ?λ€ ?λλ₯??¬μ€?ν??

    - T GetRandomPick() : ?μ¬ ?μ΄??λͺ©λ‘?μ κ°μ€μΉλ₯?κ³μ°?μ¬ ?λ€?Όλ‘ ??ͺ© ?λλ₯?λ½μ?¨λ€.
    - T GetRandomPick(double) : ?΄λ? κ³μ°???λ₯  κ°μ λ§€κ°λ³?λ‘ ?£μ΄, ?΄λΉ?λ ??ͺ© ?λλ₯?λ½μ?¨λ€.
    - double GetWeight(T) : ????μ΄?μ κ°μ€μΉλ₯??»μ΄?¨λ€.
    - double GetNormalizedWeight(T) : ????μ΄?μ ?κ·?λ κ°μ€μΉλ₯??»μ΄?¨λ€.

    - ReadonlyDictionary<T, double> GetItemDictReadonly() : ?μ²΄ ?μ΄??λͺ©λ‘???½κΈ°?μ© μ»¬λ ?μΌλ‘?λ°μ?¨λ€.
    - ReadonlyDictionary<T, double> GetNormalizedItemDictReadonly()
      : ?μ²΄ ?μ΄?μ κ°μ€μΉ μ΄ν©??1???λλ‘??κ·?λ ?μ΄??λͺ©λ‘???½κΈ°?μ© μ»¬λ ?μΌλ‘?λ°μ?¨λ€.
*/

namespace Rito
{
    /// <summary> κ°μ€μΉ ?λ€ λ½κΈ° </summary>
    public class WeightedRandomPicker<T>
    {
        /// <summary> ?μ²΄ ?μ΄?μ κ°μ€μΉ ??</summary>
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
        private readonly Dictionary<T, double> normalizedItemWeightDict; // ?λ₯ ???κ·?λ ?μ΄??λͺ©λ‘

        /// <summary> κ°μ€μΉ ?©μ΄ κ³μ°?μ? ?μ? ?ν?Έμ? ?¬λ? </summary>
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

        /// <summary> ?λ‘???μ΄??κ°μ€μΉ ??μΆκ? </summary>
        public void Add(T item, double weight)
        {
            CheckDuplicatedItem(item);
            CheckValidWeight(weight);

            itemWeightDict.Add(item, weight);
            isDirty = true;
        }

        /// <summary> ?λ‘???μ΄??κ°μ€μΉ ?λ€ μΆκ? </summary>
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

        /// <summary> λͺ©λ‘?μ ????μ΄???κ±° </summary>
        public void Remove(T item)
        {
            CheckNotExistedItem(item);

            itemWeightDict.Remove(item);
            isDirty = true;
        }

        /// <summary> ????μ΄?μ κ°μ€μΉ ?μ  </summary>
        public void ModifyWeight(T item, double weight)
        {
            CheckNotExistedItem(item);
            CheckValidWeight(weight);

            itemWeightDict[item] = weight;
            isDirty = true;
        }

        /// <summary> ?λ€ ?λ ?¬μ€??</summary>
        public void ReSeed(int seed)
        {
            randomInstance = new System.Random(seed);
        }

        #endregion
        /***********************************************************************
        *                               Getter Methods
        ***********************************************************************/
        #region .

        /// <summary> ?λ€ λ½κΈ° </summary>
        public T GetRandomPick()
        {
            // ?λ€ κ³μ°
            double chance = randomInstance.NextDouble(); // [0.0, 1.0)
            chance *= SumOfWeights;

            return GetRandomPick(chance);
        }

        /// <summary> μ§μ  ?λ€ κ°μ μ§?ν??λ½κΈ° </summary>
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

        /// <summary> ????μ΄?μ κ°μ€μΉ ?μΈ </summary>
        public double GetWeight(T item)
        {
            return itemWeightDict[item];
        }

        /// <summary> ????μ΄?μ ?κ·?λ κ°μ€μΉ ?μΈ </summary>
        public double GetNormalizedWeight(T item)
        {
            CalculateSumIfDirty();
            return normalizedItemWeightDict[item];
        }

        /// <summary> ?μ΄??λͺ©λ‘ ?μΈ(?½κΈ° ?μ©) </summary>
        public ReadOnlyDictionary<T, double> GetItemDictReadonly()
        {
            return new ReadOnlyDictionary<T, double>(itemWeightDict);
        }

        /// <summary> κ°μ€μΉ ?©μ΄ 1???λλ‘??κ·?λ ?μ΄??λͺ©λ‘ ?μΈ(?½κΈ° ?μ©) </summary>
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

        /// <summary> λͺ¨λ  ?μ΄?μ κ°μ€μΉ ??κ³μ°?΄λκΈ?</summary>
        private void CalculateSumIfDirty()
        {
            if(!isDirty) return;
            isDirty = false;

            _sumOfWeights = 0.0;
            foreach (var pair in itemWeightDict)
            {
                _sumOfWeights += pair.Value;
            }

            // ?κ·???μ?λ¦¬???λ°?΄νΈ
            UpdateNormalizedDict();
        }

        /// <summary> ?κ·?λ ?μ?λ¦¬ ?λ°?΄νΈ </summary>
        private void UpdateNormalizedDict()
        {
            normalizedItemWeightDict.Clear();
            foreach(var pair in itemWeightDict)
            {
                normalizedItemWeightDict.Add(pair.Key, pair.Value / _sumOfWeights);
            }
        }

        /// <summary> ?΄λ? ?μ΄?μ΄ μ‘΄μ¬?λμ§ ?¬λ? κ²??</summary>
        private void CheckDuplicatedItem(T item)
        {
            if(itemWeightDict.ContainsKey(item))
                throw new Exception($"?΄λ? [{item}] ?μ΄?μ΄ μ‘΄μ¬?©λ??");
        }

        /// <summary> μ‘΄μ¬?μ? ?λ ?μ΄?μΈ κ²½μ° </summary>
        private void CheckNotExistedItem(T item)
        {
            if(!itemWeightDict.ContainsKey(item))
                throw new Exception($"[{item}] ?μ΄?μ΄ λͺ©λ‘??μ‘΄μ¬?μ? ?μ΅?λ€.");
        }

        /// <summary> κ°μ€μΉ κ°?λ²μ κ²??0λ³΄λ€ μ»€μΌ ?? </summary>
        private void CheckValidWeight(in double weight)
        {
            if (weight < 0f)
                throw new Exception("κ°μ€μΉ κ°μ? 0λ³΄λ€ μ»€μΌ ?©λ??");
        }

        #endregion
    }
}