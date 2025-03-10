using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CustomInspector.Helpers
{
    public static class Common
    {
        /// <summary>
        /// The width of the scrollbar that appears when using GUI.ScrollViewScope
        /// </summary>
        public const float scrollbarThickness = 15;
        /// <summary>
        /// Gets the first item that matches the predicate
        /// </summary>
        /// <returns>If any item was found</returns>
        public static bool TryGetFirst<T>(this IEnumerable<T> list, Func<T, bool> predicate, out T match)
        {
            foreach (T item in list)
            {
                if (predicate(item))
                {
                    match = item;
                    return true;
                }
            }
            match = default;
            return false;
        }
        public static IEnumerable<Transform> GetBaseChildren(this Transform parent)
        {
            foreach (Transform child in parent)
            {
                yield return child;
            }
        }
        public static IEnumerable<Transform> GetAllChildren(this Transform parent)
        {
            foreach (Transform child in parent)
            {
                yield return child;
                foreach (Transform innerChild in child.GetAllChildren())
                    yield return innerChild;
            }
        }
        /// <summary>
        /// Preserve percentage from one range A to another range B
        /// </summary>
        public static float FromRangeToRange(float value, float minA, float maxA, float minB, float maxB)
        {
            if (minA == maxA)
            {
                if (minB == maxB)
                    return value + (minB - minA); //just shift the number
                else //just get a point in the middle
                    return (minB + maxB) / 2f;
            }
            float percentage = (value - minA) / (maxA - minA);
            return percentage * (maxB - minB) + minB;
        }
        public static bool SequenceEqual(this IList list1, IList list2)
        {
            if (list1.Count != list2.Count)
                return false;

            try
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i] == null)
                    {
                        if (list2[i] != null)
                            return false;
                    }
                    else if (!list1[i].Equals(list2[i]))
                        return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
            return true;
        }
    }
}
