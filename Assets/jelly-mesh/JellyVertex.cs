using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UX.JellyMesh
{
    public struct JellyVertex
    {
        public int index;
        public Vector3 originalPosition;
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 force;

        public JellyVertex(int index, Vector3 position)
        {
            this.index = index;
            this.position = position;
            this.velocity = Vector3.zero;
            this.force = Vector3.zero;
            this.originalPosition = Vector3.zero;
        }

        public JellyVertex(Vector3 position, Vector3 originalPosition)
        {
            this.index = 0;
            this.position = position;
            this.velocity = Vector3.zero;
            this.force = Vector3.zero;
            this.originalPosition = originalPosition;
        }

        public void Shake(Vector3 target, float mass, float stiffness, float dampen)
        {
            force = (target - position) * stiffness;
            velocity = (velocity + force / mass) * dampen;
            position += velocity;
            if ((velocity + force + force / mass).sqrMagnitude < 0.00001f)
            {
                position = target;
            }
        }
    }
}
