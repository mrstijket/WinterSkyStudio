using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Unity.Burst.CompilerServices;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] EnemyWatchAI enemyWatchAI;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask hideMask;
    [SerializeField] float viewDistance = 10f;
    private Mesh mesh;
    private float fov;
    private float angle;
    private Vector3 origin;
    private float startingAngle;

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        fov = 40f;
    }
    private void LateUpdate() {
        int rayCount = 50;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        if (startingAngle != angle) {
            angle = startingAngle;
        }

        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, layerMask);
            //if(raycastHit2D == GameObject.FindGameObjectWithTag("Player")) //   IsTouchingLayers(LayerMask.GetMask("Player")))
            //{
            //    enemyWatchAI.gotCaught = true;
            //}
            //if (Physics.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), out Hit, 5))
            //{
            //    if (Hit.collider.CompareTag("Enemy"))
            //    {
            //        Debug.Log(Hit);
            //        enemyWatchAI.gotCaught = true;
            //    }
            //}

            if (raycastHit2D.collider == null) {
                //No hit
                vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
            }
            else {
                //Hit object
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;
            RaycastHit2D hideHit = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, hideMask);
            RaycastHit2D targetHit = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, targetMask);
            if (targetHit.collider == null) {
                //no action needed here for now
            }
            else {
                if (raycastHit2D.collider != null) {
                    Debug.Log("hit");
                    enemyWatchAI.gotCaught = true;
                }
            }

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }
    public void SetAimDirection(float aimDirection) {
        startingAngle = aimDirection - fov / 2f;//UtilsClass.GetAngleFromVectorFloat(aimDirection) - fov / 2f;
    }

}
