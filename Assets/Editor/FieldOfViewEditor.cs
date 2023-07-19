using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlockerScript))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FlockerScript fov = (FlockerScript)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.headPoint.transform.position, Vector3.up, Vector3.forward, 360, fov.FOVRadius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.headPoint.transform.eulerAngles.y, -fov.FOVAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.headPoint.transform.eulerAngles.y, fov.FOVAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.headPoint.transform.position, fov.transform.position + viewAngle01 * fov.FOVRadius);
        Handles.DrawLine(fov.headPoint.transform.position, fov.transform.position + viewAngle02 * fov.FOVRadius);

        if (fov.playerSpotted)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fov.headPoint.transform.position, fov.FlockingTarget.transform.position);
        }

        Handles.color = Color.cyan;
        Handles.DrawWireArc(fov.headPoint.transform.position, Vector3.up, Vector3.forward, 360, fov.DesiredDistanceFromTarget.x);
        Handles.DrawWireArc(fov.headPoint.transform.position, Vector3.up, Vector3.forward, 360, fov.DesiredDistanceFromTarget.y);

        Handles.color = Color.blue;
        Handles.DrawWireArc(fov.headPoint.transform.position, Vector3.up, Vector3.forward, 360, fov.ChaseRange.x);
        Handles.DrawWireArc(fov.headPoint.transform.position, Vector3.up, Vector3.forward, 360, fov.ChaseRange.y);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleDegrees)
    {
        angleDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
