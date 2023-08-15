using System.Collections;
using UnityEngine;

namespace TMPTAS
{
    public class DebugLine
    {
        private LineRenderer lr;
        private float width = 0.1f;

        public DebugLine(float width = 0.1f)
        {
            this.width = width;
        }

        public void CreateLine(Material m)
        {
            lr = new GameObject().AddComponent<LineRenderer>();
            lr.material = m;
            lr.positionCount = 2;

            lr.startWidth = width;
            lr.endWidth = width;

            lr.sortingOrder = 1000;
        }

        public void SetPositions(Vector2 start, Vector2 end)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        public void SetPositions(Vector3[] positions)
        {
            lr.positionCount = positions.Length;
            lr.SetPositions(positions);
        }

        public void SetPositions(Vector2[] positions)
        {
            lr.positionCount = positions.Length;
            for (int i = 0; i < positions.Length; i++)
            {
                lr.SetPosition(i, positions[i]);
            }
        }

        public void SetColor(Color c)
        {
            lr.startColor = c;
            lr.endColor = c;
        }
    }
}