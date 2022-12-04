using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UX.JellyMesh
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class JellySkinMesh : MonoBehaviour
    {
        public float intensity = 1f;
        public float mass = 1f;
        public float stiffness = 1f;
        public float damping = 0.75f;

        Mesh clonedMesh;
        JellyVertex[] jellyVertices;
        Vector3[] modifiedVertices;
        int verticesCount;

        Vector3 boundsMax;
        Vector3 boundsSize;

        void Start()
        {
            var renderer = GetComponent<SkinnedMeshRenderer>();

            //swap the original mesh with one that will be modified
            var originalMesh = renderer.sharedMesh;
            clonedMesh = Instantiate(originalMesh);
            renderer.sharedMesh = clonedMesh;

            //create jelly vertices
            verticesCount = clonedMesh.vertices.Length;
            modifiedVertices = new Vector3[verticesCount];
            jellyVertices = new JellyVertex[verticesCount];

            for (int i = 0; i < verticesCount; i++)
            {
                modifiedVertices[i] = clonedMesh.vertices[i];
                jellyVertices[i] = new JellyVertex(transform.TransformPoint(clonedMesh.vertices[i]), clonedMesh.vertices[i]);
            }

            //save the bounds
            boundsMax = renderer.bounds.max;
            boundsSize = renderer.bounds.size;
        }

        void FixedUpdate()
        {
            for (int i = 0; i < verticesCount; i++)
            {
                modifiedVertices[i] = jellyVertices[i].originalPosition;
                Vector3 target = transform.TransformPoint(modifiedVertices[i]);
                float intensity = (1 - (boundsMax.y - target.y) / boundsSize.y) * this.intensity;
                jellyVertices[i].Shake(target, mass, stiffness, damping);
                target = transform.InverseTransformPoint(jellyVertices[i].position);
                modifiedVertices[i] = Vector3.Lerp(modifiedVertices[i], target, intensity);

            }
            clonedMesh.vertices = modifiedVertices;
        }
    }

    // public class JellySkinMesh : MonoBehaviour
    // {
    //     public float intensity = 1f;
    //     public float mass = 1f;
    //     public float stiffness = 1f;
    //     public float dampen = 0.75f;

    //     Mesh originalMesh;
    //     Mesh clonedMesh;
    //     SkinnedMeshRenderer renderer;
    //     JellyVertex[] vertices;
    //     Vector3[] vertexArray;

    //     void Start()
    //     {
    //         renderer = GetComponent<SkinnedMeshRenderer>();

    //         originalMesh = renderer.sharedMesh;
    //         clonedMesh = Instantiate(originalMesh);
    //         renderer.sharedMesh = clonedMesh;

    //         int count = clonedMesh.vertices.Length;
    //         vertices = new JellyVertex[count];
    //         for (int i = 0; i < count; i++)
    //         {
    //             vertices[i] = new JellyVertex(i, transform.TransformPoint(clonedMesh.vertices[i]));
    //         }

    //     }

    //     void FixedUpdate()
    //     {
    //         vertexArray = originalMesh.vertices;
    //         for (int i = 0; i < vertices.Length; i++)
    //         {
    //             Vector3 target = transform.TransformPoint(vertexArray[vertices[i].index]);
    //             float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * this.intensity;
    //             vertices[i].Shake(target, mass, stiffness, dampen);
    //             target = transform.InverseTransformPoint(vertices[i].position);
    //             vertexArray[vertices[i].index] = Vector3.Lerp(vertexArray[vertices[i].index], target, intensity);

    //         }
    //         clonedMesh.vertices = vertexArray;
    //     }
    // }
}