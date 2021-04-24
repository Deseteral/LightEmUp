using UnityEngine;

namespace DefaultNamespace {
    public static class Utils {
        public static Vector3 MouseInWorld() {
            var v3 = Input.mousePosition;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            v3.z = 0f;
            return v3;
        }
    }
}