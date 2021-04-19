using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_GizmosExstension : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("GizmosExstension",
                "Gizmos拓展绘制物体边框等",
                "Gizmos",
                "using System;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public static class GizmosExtensions\n    {\n        #region 进一步封装的绘制方法\n\n        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]\n        public static void DrawWireCube(BoxCollider boxCollider, GizmoType gizmosType)\n        {\n            var originColor = GUI.color;\n            GUI.color = Color.white;\n            DrawWireCube(boxCollider.center, boxCollider.size, boxCollider.transform.rotation);\n            GUI.color = originColor;\n        }\n\n        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]\n        public static void DrawWireSphere(SphereCollider sphereCollider, GizmoType gizmosType)\n        {\n            var originColor = GUI.color;\n            GUI.color = Color.white;\n            DrawWireSphere(sphereCollider.center, sphereCollider.radius, sphereCollider.transform.rotation);\n            GUI.color = originColor;\n        }\n\n        #endregion\n\n        #region 基本绘制方法\n\n        /// <summary>\n        /// Draws a wire cube with a given rotation \n        /// </summary>\n        /// <param name=\"center\"></param>\n        /// <param name=\"size\"></param>\n        /// <param name=\"rotation\"></param>\n        public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation = default(Quaternion))\n        {\n            var old = Gizmos.matrix;\n            if (rotation.Equals(default(Quaternion)))\n                rotation = Quaternion.identity;\n            Gizmos.matrix = Matrix4x4.TRS(center, rotation, size);\n            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);\n            Gizmos.matrix = old;\n        }\n\n        public static void DrawArrow(Vector3 from, Vector3 to, float arrowHeadLength = 0.25f,\n            float arrowHeadAngle = 20.0f)\n        {\n            Gizmos.DrawLine(from, to);\n            var direction = to - from;\n            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *\n                        new Vector3(0, 0, 1);\n            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *\n                       new Vector3(0, 0, 1);\n            Gizmos.DrawLine(to, to + right * arrowHeadLength);\n            Gizmos.DrawLine(to, to + left * arrowHeadLength);\n        }\n\n        public static void DrawWireSphere(Vector3 center, float radius, Quaternion rotation = default(Quaternion))\n        {\n            var old = Gizmos.matrix;\n            if (rotation.Equals(default(Quaternion)))\n                rotation = Quaternion.identity;\n            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);\n            Gizmos.DrawWireSphere(Vector3.zero, radius);\n            Gizmos.matrix = old;\n        }\n\n\n        /// <summary>\n        /// Draws a flat wire circle (up)\n        /// </summary>\n        /// <param name=\"center\"></param>\n        /// <param name=\"radius\"></param>\n        /// <param name=\"segments\"></param>\n        /// <param name=\"rotation\"></param>\n        public static void DrawWireCircle(Vector3 center, float radius, int segments = 20,\n            Quaternion rotation = default(Quaternion))\n        {\n            DrawWireArc(center, radius, 360, segments, rotation);\n        }\n\n        /// <summary>\n        /// Draws an arc with a rotation around the center\n        /// </summary>\n        /// <param name=\"center\">center point</param>\n        /// <param name=\"radius\">radiu</param>\n        /// <param name=\"angle\">angle in degrees</param>\n        /// <param name=\"segments\">number of segments</param>\n        /// <param name=\"rotation\">rotation around the center</param>\n        public static void DrawWireArc(Vector3 center, float radius, float angle, int segments = 20,\n            Quaternion rotation = default(Quaternion))\n        {\n            var old = Gizmos.matrix;\n\n            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);\n            Vector3 from = Vector3.forward * radius;\n            var step = Mathf.RoundToInt(angle / segments);\n            for (int i = 0; i <= angle; i += step)\n            {\n                var to = new Vector3(radius * Mathf.Sin(i * Mathf.Deg2Rad), 0, radius * Mathf.Cos(i * Mathf.Deg2Rad));\n                Gizmos.DrawLine(from, to);\n                from = to;\n            }\n\n            Gizmos.matrix = old;\n        }\n\n\n        /// <summary>\n        /// Draws an arc with a rotation around an arbitraty center of rotation\n        /// </summary>\n        /// <param name=\"center\">the circle's center point</param>\n        /// <param name=\"radius\">radius</param>\n        /// <param name=\"angle\">angle in degrees</param>\n        /// <param name=\"segments\">number of segments</param>\n        /// <param name=\"rotation\">rotation around the centerOfRotation</param>\n        /// <param name=\"centerOfRotation\">center of rotation</param>\n        public static void DrawWireArc(Vector3 center, float radius, float angle, int segments, Quaternion rotation,\n            Vector3 centerOfRotation)\n        {\n            var old = Gizmos.matrix;\n            if (rotation.Equals(default(Quaternion)))\n                rotation = Quaternion.identity;\n            Gizmos.matrix = Matrix4x4.TRS(centerOfRotation, rotation, Vector3.one);\n            var deltaTranslation = centerOfRotation - center;\n            Vector3 from = deltaTranslation + Vector3.forward * radius;\n            var step = Mathf.RoundToInt(angle / segments);\n            for (int i = 0; i <= angle; i += step)\n            {\n                var to = new Vector3(radius * Mathf.Sin(i * Mathf.Deg2Rad), 0, radius * Mathf.Cos(i * Mathf.Deg2Rad)) +\n                         deltaTranslation;\n                Gizmos.DrawLine(from, to);\n                from = to;\n            }\n\n            Gizmos.matrix = old;\n        }\n\n        /// <summary>\n        /// Draws an arc with a rotation around an arbitraty center of rotation\n        /// </summary>\n        /// <param name=\"matrix\">Gizmo matrix applied before drawing</param>\n        /// <param name=\"radius\">radius</param>\n        /// <param name=\"angle\">angle in degrees</param>\n        /// <param name=\"segments\">number of segments</param>\n        public static void DrawWireArc(Matrix4x4 matrix, float radius, float angle, int segments)\n        {\n            var old = Gizmos.matrix;\n            Gizmos.matrix = matrix;\n            Vector3 from = Vector3.forward * radius;\n            var step = Mathf.RoundToInt(angle / segments);\n            for (int i = 0; i <= angle; i += step)\n            {\n                var to = new Vector3(radius * Mathf.Sin(i * Mathf.Deg2Rad), 0, radius * Mathf.Cos(i * Mathf.Deg2Rad));\n                Gizmos.DrawLine(from, to);\n                from = to;\n            }\n\n            Gizmos.matrix = old;\n        }\n\n        /// <summary>\n        /// Draws a wire cylinder face up with a rotation around the center\n        /// </summary>\n        /// <param name=\"center\"></param>\n        /// <param name=\"radius\"></param>\n        /// <param name=\"height\"></param>\n        /// <param name=\"rotation\"></param>\n        public static void DrawWireCylinder(Vector3 center, float radius, float height,\n            Quaternion rotation = default(Quaternion))\n        {\n            var old = Gizmos.matrix;\n            if (rotation.Equals(default(Quaternion)))\n                rotation = Quaternion.identity;\n            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);\n            var half = height / 2;\n\n            //draw the 4 outer lines\n            Gizmos.DrawLine(Vector3.right * radius - Vector3.up * half, Vector3.right * radius + Vector3.up * half);\n            Gizmos.DrawLine(-Vector3.right * radius - Vector3.up * half, -Vector3.right * radius + Vector3.up * half);\n            Gizmos.DrawLine(Vector3.forward * radius - Vector3.up * half, Vector3.forward * radius + Vector3.up * half);\n            Gizmos.DrawLine(-Vector3.forward * radius - Vector3.up * half,\n                -Vector3.forward * radius + Vector3.up * half);\n\n            //draw the 2 cricles with the center of rotation being the center of the cylinder, not the center of the circle itself\n            DrawWireArc(center + Vector3.up * half, radius, 360, 20, rotation, center);\n            DrawWireArc(center + Vector3.down * half, radius, 360, 20, rotation, center);\n            Gizmos.matrix = old;\n        }\n\n        /// <summary>\n        /// Draws a wire capsule face up\n        /// </summary>\n        /// <param name=\"center\"></param>\n        /// <param name=\"radius\"></param>\n        /// <param name=\"height\"></param>\n        /// <param name=\"rotation\"></param>\n        public static void DrawWireCapsule(Vector3 center, float radius, float height,\n            Quaternion rotation = default(Quaternion))\n        {\n            if (rotation.Equals(default(Quaternion)))\n                rotation = Quaternion.identity;\n            var old = Gizmos.matrix;\n            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);\n            var half = height / 2 - radius;\n\n            //draw cylinder base\n            DrawWireCylinder(center, radius, height - radius * 2, rotation);\n\n            //draw upper cap\n            //do some cool stuff with orthogonal matrices\n            var mat = Matrix4x4.Translate(center + rotation * Vector3.up * half) *\n                      Matrix4x4.Rotate(rotation * Quaternion.AngleAxis(90, Vector3.forward));\n            DrawWireArc(mat, radius, 180, 20);\n            mat = Matrix4x4.Translate(center + rotation * Vector3.up * half) * Matrix4x4.Rotate(\n                rotation * Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(90, Vector3.forward));\n            DrawWireArc(mat, radius, 180, 20);\n\n            //draw lower cap\n            mat = Matrix4x4.Translate(center + rotation * Vector3.down * half) * Matrix4x4.Rotate(\n                rotation * Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.forward));\n            DrawWireArc(mat, radius, 180, 20);\n            mat = Matrix4x4.Translate(center + rotation * Vector3.down * half) *\n                  Matrix4x4.Rotate(rotation * Quaternion.AngleAxis(-90, Vector3.forward));\n            DrawWireArc(mat, radius, 180, 20);\n\n            Gizmos.matrix = old;\n        }\n\n        #endregion\n    }\n}",
                "Assets/Editor/Examples/Example_13_GizmosExstension",
                typeof(Example_GizmosExstension),
                picPath : "Assets/Editor/Examples/Example_13_GizmosExstension/QQ截图20210419154927.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
