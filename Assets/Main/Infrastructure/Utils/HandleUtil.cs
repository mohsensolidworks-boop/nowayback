using Main.Infrastructure.Utils.Extensions;
using UnityEditor;
using UnityEngine;

namespace Main.Infrastructure.Utils
{
    public static class HandleUtil
    {
        private static readonly Color _FORCE_COLOR;
        
        static HandleUtil()
        {
            _FORCE_COLOR = new Color(0.6f, 0.9f, 0.1f, 0.5f);
        }
        
        public static void DrawLine(Vector3 from, Vector3 to, bool isDotted = false)
        {
            DrawLine(from, to, Color.white, isDotted: isDotted);
        }
        
        public static void DrawLine(Vector3 from, Vector3 to, Color color, float thickness = 2f, bool isDotted = false)
        {
#if UNITY_EDITOR
            SetColor(color);
            if (isDotted)
            {
                Handles.DrawDottedLine(from, to, thickness);
            }
            else
            {
                Handles.DrawLine(from, to, thickness);
            }
#endif
        }

        public static void DrawRay(Vector3 from, Vector3 direction, Color color)
        {
            DrawLine(from, from + direction, color);
        }
        
        public static void DrawRayArrow(Vector3 from, Vector3 direction, Color color, float arrowHeight = 1f, float arrowRadius = 1f)
        {
            DrawArrow(from, from + direction, color, arrowHeight, arrowRadius);
        }
        
        public static void DrawArrow(Vector3 from, Vector3 to, Color color, float arrowHeight = 1f, float arrowRadius = 1f)
        {
            DrawLine(from, to, color);
            DrawArc(to, from - to, 0f, 360f, color * 0.8f, arrowHeight, arrowRadius, false);
        }
        
        public static void DrawArrowInverse(Vector3 from, Vector3 to, Color color, float arrowHeight = 1f, float arrowRadius = 1f)
        {
            DrawLine(from, to, color);
            DrawArc(from, from - to, 0f, 360f, color * 0.8f, arrowHeight, arrowRadius, false);
        }
        
        public static void DrawForce(Vector3 center, Vector3 force, float magnifier = 0.03f, string suffix = "")
        {
            DrawForce(center, force, _FORCE_COLOR, magnifier, suffix);
        }
        
        public static void DrawForce(Vector3 center, Vector3 force, Color color, float magnifier = 0.03f, string suffix = "", bool withText = false)
        {
            var textPosition = center + force * magnifier;
            DrawRay(center, force * magnifier, color);
            if (withText)
            {
                DrawText(textPosition, $"F{suffix}: {force.magnitude.Round(1)}", color);
            }
        }
        
        public static void DrawCube(Vector3 center, float size = 0.5f)
        {
            DrawCube(center, Color.white, size);
        }
        
        public static void DrawCube(Vector3 center, Color color, float size = 0.5f)
        {
            DrawCube(center, Quaternion.identity, color, size);
        }
        
        public static void DrawCube(Vector3 center, Quaternion rotation, Color color, float size = 0.5f)
        {
#if UNITY_EDITOR
            SetColor(color);
            Handles.CubeHandleCap(0, center, rotation, size, EventType.Repaint);
#endif
        }
        
        public static void DrawWireCube(Vector3 center, Color color, float size = 0.5f)
        {
#if UNITY_EDITOR
            SetColor(color);
            Handles.DrawWireCube(center, Vector3.one * size);
#endif
        }
        
        public static void DrawRectangle(Vector3 center, Vector3 normal, Color color, float size = 0.5f)
        {
            DrawRectangle(center, normal, color, size, size, color);
        }
        
        public static void DrawRectangle(Vector3 center, Vector3 normal, Color color, float bottomEdgeLength, float rightEdgeLength, Color outlineColor)
        {
#if UNITY_EDITOR
            var perpendicular1 = Vector3.Cross(normal, Vector3.right).normalized;
            var perpendicular2 = Vector3.Cross(normal, perpendicular1).normalized;
            bottomEdgeLength *= 0.5f;
            rightEdgeLength *= 0.5f;
            var verts = new Vector3[4];
            verts[0] = center + perpendicular1 * rightEdgeLength + perpendicular2 * bottomEdgeLength;
            verts[1] = center - perpendicular1 * rightEdgeLength + perpendicular2 * bottomEdgeLength;
            verts[2] = center - perpendicular1 * rightEdgeLength - perpendicular2 * bottomEdgeLength;
            verts[3] = center + perpendicular1 * rightEdgeLength - perpendicular2 * bottomEdgeLength;
            Handles.color = Color.white;
            Handles.DrawSolidRectangleWithOutline(verts, color, outlineColor);
#endif
        }
        
        public static void DrawSphere(Vector3 center, Color color, float radius = 0.05f)
        {
#if UNITY_EDITOR
            RefreshCamera();
            SetColor(color);
            Handles.SphereHandleCap(0, center, Quaternion.identity, radius * 2f, EventType.Repaint);
#endif
        }
        
        public static void DrawDisc(Vector3 center, Vector3 normal, Color color, float radius = 0.5f)
        {
#if UNITY_EDITOR
            SetColor(color);
            Handles.DrawSolidDisc(center, normal, radius);
#endif
        }
        
        public static void DrawDisc(Vector3 center, Vector3 normal, float fromAngle, float toAngle, Color color, float radius = 0.5f)
        {
            DrawArc(center, normal, fromAngle, toAngle, color, 0f, radius, false);
        }
        
        public static void DrawCircle(Vector3 center, Vector3 normal, Color color, float radius = 0.5f)
        {
#if UNITY_EDITOR
            SetColor(color);
            Handles.DrawWireDisc(center, normal, radius, 1.5f);
#endif
        }
        
        public static void DrawCircle(Vector3 center, Vector3 normal, float fromAngle, float toAngle, Color color, float radius = 0.5f, float thickness = 2f)
        {
            DrawArc(center, normal, fromAngle, toAngle, color, 0f, radius, true, thickness);
        }
        
        private static void DrawArc(Vector3 center, Vector3 normal, float fromAngle, float toAngle, Color color, float height, float radius, bool isWired, float thickness = 2f)
        {
#if UNITY_EDITOR
            normal = normal.normalized;
            if (normal.sqrMagnitude < 1e-4f)
            {
                return;
            }
            
            var ortho = Vector3.Cross(normal, Vector3.up);
            if (ortho.sqrMagnitude < 1e-4f)
            {
                ortho = Vector3.Cross(normal, Vector3.right);
            }
            ortho.Normalize();
            var dir = Quaternion.AngleAxis(fromAngle, normal) * ortho;
            var from =  normal * height + dir * radius;
            var angleRange = toAngle - fromAngle;
            var hypotenuse = Mathf.Sqrt(height * height + radius * radius);
        
            SetColor(color);
            if (isWired)
            {
                Handles.DrawWireArc(center, normal, from, angleRange, hypotenuse, thickness);
            }
            else
            {
                Handles.DrawSolidArc(center, normal, from, angleRange, hypotenuse);
            }
#endif
        }

        public static void DrawMesh(Vector3 center, Vector3[] points, Color color)
        {
#if UNITY_EDITOR
            SetColor(color);

            for (var i = 0; i < points.Length; i++)
            {
                points[i] += center;
            }
            
            Handles.DrawPolyLine(points);
#endif
        }
        
        public static void DrawText(Vector3 position, string text, Color color)
        {
#if UNITY_EDITOR
            var style = new GUIStyle
            {
                normal =
                {
                    textColor = color,
                },
            };
            Handles.Label(position, text, style);
#endif
        }
        
        public static void DrawSpring(Vector3 center, Vector3 direction, float distance, float strength)
        {
            var springStartPosition = center;
            var springEndPosition = springStartPosition + direction * distance;
            
            DrawSphere(springStartPosition, new Color(0, 0.5f, 0.9f, 0.9f), 0.03f);
            
            var offset = 0.04f;
            var ratio = (distance + offset) / (offset * 2f);
            var color1 = new Color(1.0f, 0.0f, 0.4f);
            var color2 = new Color(0.5f, 1.0f, 0.1f);
            var color = ratio <= 0.5f
                ? Color.Lerp(color1, color2, ratio * 2f)
                : Color.Lerp(color2, color1, (ratio - 0.5f) * 2f);
            
            var discCount = 3 + (int)(strength * 0.0125f);
            var step = (springEndPosition - springStartPosition) / discCount;
            var discRadius = 0.01f + 0.005f * discCount;
            for (var i = 0; i <= discCount; i++)
            {
                var discPosition = springStartPosition + i * step;
                DrawDisc(discPosition, direction, color, discRadius);
            }
            
            DrawText((springStartPosition + springEndPosition) / 2f, $"L: {(springEndPosition - springStartPosition).magnitude.Round()}", Color.white);
        }
        
        public static void DrawCapsule(Vector3 center, float height, float radius)
        {
            DrawCapsule(center, Quaternion.identity, height, radius, Color.white);
        }
        
        public static void DrawCapsule(Vector3 center, float height, float radius, Color color)
        {
            DrawCapsule(center, Quaternion.identity, height, radius, color);
        }

        public static void DrawCapsule(Vector3 center, Quaternion rotation, float height, float radius, Color color)
        {
#if UNITY_EDITOR
            SetColor(color);
            
            // Kapsül yüksekliği çapından (2 * radius) küçük olamaz, olursa küreye döner.
            height = Mathf.Max(height, radius * 2f);
            
            // Silindir kısmının yarım boyu (küre merkezleri arasındaki mesafe / 2)
            var pointOffset = (height - (radius * 2f)) / 2f;
            
            var up = rotation * Vector3.up;
            var forward = rotation * Vector3.forward;
            var right = rotation * Vector3.right;

            var topCenter = center + up * pointOffset;
            var bottomCenter = center - up * pointOffset;
            
            // TODO: make general solution
            RefreshCamera();
            
            // Yan çizgileri çiz (Silindir iskeleti)
            DrawLine(topCenter + right * radius, bottomCenter + right * radius, color);
            DrawLine(topCenter - right * radius, bottomCenter - right * radius, color);
            DrawLine(topCenter + forward * radius, bottomCenter + forward * radius, color);
            DrawLine(topCenter - forward * radius, bottomCenter - forward * radius, color);
            
            // Üst ve Alt çemberleri çiz (Silindirin başlangıç ve bitişi)
            DrawCircle(topCenter, up, color, radius);
            DrawCircle(bottomCenter, up, color, radius);

            // Alt kapağı (Yarım küre) çiz
            Handles.DrawWireArc(bottomCenter, right, forward * radius, 180f, radius);
            Handles.DrawWireArc(bottomCenter, forward, -right * radius, 180f, radius);

            // Üst kapağı (Yarım küre) çiz
            Handles.DrawWireArc(topCenter, right, -forward * radius, 180f, radius);
            Handles.DrawWireArc(topCenter, forward, right * radius, 180f, radius);
#endif
        }
        
        public static void DrawSolidCapsule(Vector3 center, float height, float radius)
        {
            DrawSolidCapsule(center, Quaternion.identity, height, radius, Color.white);
        }

        public static void DrawSolidCapsule(Vector3 center, float height, float radius, Color color)
        {
            DrawSolidCapsule(center, Quaternion.identity, height, radius, color);
        }

        public static void DrawSolidCapsule(Vector3 center, Quaternion rotation, float height, float radius, Color color)
        {
#if UNITY_EDITOR
            RefreshCamera();
            
            height = Mathf.Max(height, radius * 2f);
            var cylinderHeight = height - radius * 2f;
            var offset = cylinderHeight / 2f;
            
            var topPos = center + rotation * Vector3.up * offset;
            DrawSphere(topPos, color, radius);
            
            var bottomPos = center - rotation * Vector3.up * offset;
            DrawSphere(bottomPos, color, radius);
            
            if (cylinderHeight > 0)
            {
                DrawCylinder(center, rotation, cylinderHeight, radius, color);
            }
#endif
        }
        
        public static void DrawCylinder(Vector3 center, Quaternion rotation, float height, float radius, Color color)
        {
#if UNITY_EDITOR
            // Kürelerin (baş ve son) kaplayacağı alan hariç, aradaki silindirin boyu
            RefreshCamera();
            
            // 3. GÖVDE (SİLİNDİR)
            // Unity'nin CylinderHandleCap'i normalde Z eksenine bakar ve size ile orantılı büyür.
            // Biz sadece boyunu uzatmak istediğimiz için Matrix ile "Scale" hilesi yapıyoruz.
            
            // Silindiri Y eksenine (Up) döndürmek için rotasyona 90 derece X ekliyoruz.
            var cylinderRot = rotation * Quaternion.Euler(90, 0, 0);

            // Varsayılan silindir (size=1) Çap=1 (Yarıçap=0.5) ve Boy=2 birimdir (-1 ile +1 arası).
            // Hedef Yarıçap = radius
            // Hedef Boy = cylinderHeight
            
            var scaleRadius = radius * 2f; // Yarıçapı 0.5'ten 'radius'a çıkarmak için 2 ile çarpıyoruz.
            var cylinderMatrix = Matrix4x4.TRS(center, cylinderRot, new Vector3(scaleRadius, scaleRadius, height));
            var originalMatrix = Handles.matrix;
            SetColor(color);
            Handles.matrix = cylinderMatrix;
            // Size'ı 1 veriyoruz çünkü büyütme işlemini Matrix'te yaptık.
            Handles.CylinderHandleCap(0, Vector3.zero, Quaternion.identity, 1f, EventType.Repaint);
            // Matrisi eski haline getir (yoksa diğer gizmoslar bozulur)
            Handles.matrix = originalMatrix;
#endif
        }
        
        private static void SetColor(Color color)
        {
#if UNITY_EDITOR
            Handles.color = color;
#endif
        }
        
        private static void RefreshCamera()
        {
#if UNITY_EDITOR
            if (Camera.current != null)
            {
                if (Camera.current.cameraType == CameraType.SceneView)
                {
                    Handles.SetCamera(Camera.current);
                }
                else
                {
                    Handles.SetCamera(Camera.main);
                }
            }
#endif
        }
    }
}
