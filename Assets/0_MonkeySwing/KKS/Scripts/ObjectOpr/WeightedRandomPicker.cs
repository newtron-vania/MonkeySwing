using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// ?�짜 : 2021-03-09 AM 1:08:48
// ?�성??: Rito

/*
    [가중치 ?�덤 뽑기]

    - ?�네�?�� ?�해 ?�이?�의 ?�?�을 지?�해 객체?�하???�용?�다.
    - 중복?�는 ?�이?�이 ?�도�??�셔?�리�?구현?��???
    - 가중치가 0보다 ?��? 경우 ?�외�??�출?�다.

    - double SumOfWeights : ?�체 ?�이?�의 가중치 ???�기 ?�용 ?�로?�티)

    - void Add(T, double) : ?�로???�이??가중치 ?�을 추�??�다.
    - void Add(params (T, double)[]) : ?�로???�이??가중치 ?�을 ?�러 �?추�??�다.
    - void Remove(T) : ?�???�이?�을 목록?�서 ?�거?�다.
    - void ModifyWeight(T, double) : ?�???�이?�의 가중치�?변경한??
    - void ReSeed(int) : ?�덤 ?�드�??�설?�한??

    - T GetRandomPick() : ?�재 ?�이??목록?�서 가중치�?계산?�여 ?�덤?�로 ??�� ?�나�?뽑아?�다.
    - T GetRandomPick(double) : ?��? 계산???�률 값을 매개변?�로 ?�어, ?�당?�는 ??�� ?�나�?뽑아?�다.
    - double GetWeight(T) : ?�???�이?�의 가중치�??�어?�다.
    - double GetNormalizedWeight(T) : ?�???�이?�의 ?�규?�된 가중치�??�어?�다.

    - ReadonlyDictionary<T, double> GetItemDictReadonly() : ?�체 ?�이??목록???�기?�용 컬렉?�으�?받아?�다.
    - ReadonlyDictionary<T, double> GetNormalizedItemDictReadonly()
      : ?�체 ?�이?�의 가중치 총합??1???�도�??�규?�된 ?�이??목록???�기?�용 컬렉?�으�?받아?�다.
*/

namespace Rito
{
    /// <summary> 가중치 ?�덤 뽑기 </summary>
    public class WeightedRandomPicker<T>
    {
        /// <summary> ?�체 ?�이?�의 가중치 ??</summary>
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
        private readonly Dictionary<T, double> normalizedItemWeightDict; // ?�률???�규?�된 ?�이??목록

        /// <summary> 가중치 ?�이 계산?��? ?��? ?�태?��? ?��? </summary>
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

        /// <summary> ?�로???�이??가중치 ??추�? </summary>
        public void Add(T item, double weight)
        {
            CheckDuplicatedItem(item);
            CheckValidWeight(weight);

            itemWeightDict.Add(item, weight);
            isDirty = true;
        }

        /// <summary> ?�로???�이??가중치 ?�들 추�? </summary>
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

        /// <summary> 목록?�서 ?�???�이???�거 </summary>
        public void Remove(T item)
        {
            CheckNotExistedItem(item);

            itemWeightDict.Remove(item);
            isDirty = true;
        }

        /// <summary> ?�???�이?�의 가중치 ?�정 </summary>
        public void ModifyWeight(T item, double weight)
        {
            CheckNotExistedItem(item);
            CheckValidWeight(weight);

            itemWeightDict[item] = weight;
            isDirty = true;
        }

        /// <summary> ?�덤 ?�드 ?�설??</summary>
        public void ReSeed(int seed)
        {
            randomInstance = new System.Random(seed);
        }

        #endregion
        /***********************************************************************
        *                               Getter Methods
        ***********************************************************************/
        #region .

        /// <summary> ?�덤 뽑기 </summary>
        public T GetRandomPick()
        {
            // ?�덤 계산
            double chance = randomInstance.NextDouble(); // [0.0, 1.0)
            chance *= SumOfWeights;

            return GetRandomPick(chance);
        }

        /// <summary> 직접 ?�덤 값을 지?�하??뽑기 </summary>
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

        /// <summary> ?�???�이?�의 가중치 ?�인 </summary>
        public double GetWeight(T item)
        {
            return itemWeightDict[item];
        }

        /// <summary> ?�???�이?�의 ?�규?�된 가중치 ?�인 </summary>
        public double GetNormalizedWeight(T item)
        {
            CalculateSumIfDirty();
            return normalizedItemWeightDict[item];
        }

        /// <summary> ?�이??목록 ?�인(?�기 ?�용) </summary>
        public ReadOnlyDictionary<T, double> GetItemDictReadonly()
        {
            return new ReadOnlyDictionary<T, double>(itemWeightDict);
        }

        /// <summary> 가중치 ?�이 1???�도�??�규?�된 ?�이??목록 ?�인(?�기 ?�용) </summary>
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

        /// <summary> 모든 ?�이?�의 가중치 ??계산?�놓�?</summary>
        private void CalculateSumIfDirty()
        {
            if(!isDirty) return;
            isDirty = false;

            _sumOfWeights = 0.0;
            foreach (var pair in itemWeightDict)
            {
                _sumOfWeights += pair.Value;
            }

            // ?�규???�셔?�리???�데?�트
            UpdateNormalizedDict();
        }

        /// <summary> ?�규?�된 ?�셔?�리 ?�데?�트 </summary>
        private void UpdateNormalizedDict()
        {
            normalizedItemWeightDict.Clear();
            foreach(var pair in itemWeightDict)
            {
                normalizedItemWeightDict.Add(pair.Key, pair.Value / _sumOfWeights);
            }
        }

        /// <summary> ?��? ?�이?�이 존재?�는지 ?��? 검??</summary>
        private void CheckDuplicatedItem(T item)
        {
            if(itemWeightDict.ContainsKey(item))
                throw new Exception($"?��? [{item}] ?�이?�이 존재?�니??");
        }

        /// <summary> 존재?��? ?�는 ?�이?�인 경우 </summary>
        private void CheckNotExistedItem(T item)
        {
            if(!itemWeightDict.ContainsKey(item))
                throw new Exception($"[{item}] ?�이?�이 목록??존재?��? ?�습?�다.");
        }

        /// <summary> 가중치 �?범위 검??0보다 커야 ?? </summary>
        private void CheckValidWeight(in double weight)
        {
            if (weight < 0f)
                throw new Exception("가중치 값�? 0보다 커야 ?�니??");
        }

        #endregion
    }
}