using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace GlobalFunctions
    public static class GlobalFunctions
    {
        /// <summary>
        /// Multiply each member of the vector by the corresponding axis
        /// </summary>
        public static Vector3 VecTimes(Vector3 a, Vector3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;

            return a;
        }
    }

