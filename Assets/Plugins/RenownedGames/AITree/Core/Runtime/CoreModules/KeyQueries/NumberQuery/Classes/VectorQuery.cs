/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree
{
    public class VectorQuery : NumberQuery<Vector3Key>
    {
        [SerializeField]
        private Vector3 vector;

        [SerializeField]
        private float distanceThreshold;

        protected override bool CompareNumber(Vector3Key key, NumberComparer comparer)
        {
            float distance = Vector3.Distance(key.GetValue(), vector);

            switch (comparer)
            {
                case NumberComparer.IsEqualTo:
                    return distance == distanceThreshold;
                case NumberComparer.IsNotEqualTo:
                    return distance != distanceThreshold;
                case NumberComparer.IsLessThen:
                    return distance < distanceThreshold;
                case NumberComparer.IsLessThenOrEqualTo:
                    return distance <= distanceThreshold;
                case NumberComparer.IsGreaterThen:
                    return distance > distanceThreshold;
                case NumberComparer.IsGreaterThenOrEqualTo:
                    return distance >= distanceThreshold;
            }

            return false;
        }

        /// <summary>
        /// Detail description of key query.
        /// </summary>
        public override string GetDescription(Key key)
        {
            if (key == null)
            {
                return base.GetDescription(key);
            }

            return $"Distance between {key.name} and {vector} {GetNumberComparer()} then {distanceThreshold}";
        }

        #region [Getter / Setter]
        public Vector3 GetVector()
        {
            return vector;
        }

        public void SetVector(Vector3 value)
        {
            this.vector = value;
        }

        public float GetDistanceThreshold()
        {
            return distanceThreshold;
        }

        public void SetDistanceThreshold(float value)
        {
            this.distanceThreshold = value;
        }
        #endregion
    }
}