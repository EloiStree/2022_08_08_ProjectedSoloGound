using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public class E_DrawingUtility
    {
        public static Color fowardColor= Color.blue;
        public static Color rightColor = Color.red;
        public static Color upColor = Color.green;
        public static Color downColor = Color.blue+Color.white*0.5f;
        public static Color leftColor = Color.red + Color.white * 0.5f;
        public static Color backwardColor = Color.green + Color.white * 0.5f;

        public static void DrawAxis(in Transform root, in AxisType type, in float distance = 0.1f, in float time=0) {
            Vector3 p = root.position;
            Quaternion q=root.rotation;
            DrawAxisFrom(in p, in q, in type, in fowardColor,in distance, time <= 0 ? time: Time.deltaTime);

        }

        public static void DrawLines(float deltaTime, Color color, params Vector3 [] worldPosition)
        {
            for (int i = 1; i < worldPosition.Length; i++)
            {
                Debug.DrawLine(worldPosition[i - 1], worldPosition[i], color, deltaTime);
            }
        }

        public static void DrawCartesianOrigine(in Vector3 where, in Quaternion direction, in float distanceOfAxis, in float timeInSeconds)
        {

            DrawAxisFrom(in where, in direction, AxisType.Forward, in fowardColor, distanceOfAxis, timeInSeconds);
            DrawAxisFrom(in where, in direction, AxisType.Up, in upColor, distanceOfAxis, timeInSeconds);
            DrawAxisFrom(in where, in direction, AxisType.Right, in rightColor, distanceOfAxis, timeInSeconds);
        }
      
        public static void DrawAxe(in Vector3 where, in Quaternion direction, in AxisType axisType, in Color color, in float distanceOfAxis, in float timeInSeconds)
        {

            DrawAxisFrom(in where, in direction, AxisType.Down, in color, in distanceOfAxis, in timeInSeconds);
        }

        public enum AxisType { Left, Right, Up ,Down, Forward, Backward}
        private static void DrawAxisFrom(in Vector3 where, in Quaternion direction, in AxisType axisType, in Color color,in float distanceOfAxis, in float timeInSeconds)
        {
            Vector3 d;
            switch (axisType)
            {
                case AxisType.Left:
                    d = -Vector3.right;
                    break;
                case AxisType.Right:
                    d = Vector3.right;
                    break;
                case AxisType.Up:
                    d = Vector3.up;
                    break;
                case AxisType.Down:
                    d = -Vector3.up;
                    break;
                case AxisType.Forward:
                    d = Vector3.forward;
                    break;
                case AxisType.Backward:
                    d = -Vector3.forward;
                    break;
                default:
                    d = Vector3.forward;
                    break;
            }

            Debug.DrawLine(where, where + ((direction * d )* distanceOfAxis), color, timeInSeconds);
        }

        public static void DrawLinesInDirection(in Vector3 point, in Vector3 direction, in float timeInSeconds, in Color color)
        {
            Debug.DrawLine(point, point + direction, color, timeInSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointToRelocate"></param>
        /// <param name="root"></param>
        /// <param name="color"></param>
        /// <param name="deltaTime"></param>
        /// <param name="localDirection"></param>
        public static void ComeOnVincent(Transform pointToRelocate, Transform root, Color color, float deltaTime)
        {

            E_CodeTag.DirtyCode.Info("This code is dirty as shit but it is just to help vincent");
            Eloi.E_RelocationUtility.GetWorldToLocal_DirectionalPoint(pointToRelocate.position,
                pointToRelocate.rotation, root,
                out Vector3 localPosition, out Quaternion localrotation);
            Vector3 localDirection = localrotation * Vector3.forward;
            Debug.DrawLine(localPosition, localPosition + localDirection, color, deltaTime);
            Debug.DrawLine(Vector3.zero, localDirection, color*0.9f, deltaTime);

        }
    }
}