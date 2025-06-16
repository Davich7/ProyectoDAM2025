using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    Waypoint WaypointTarget => target as Waypoint;

    //Método para poder mover los puntos de recorrido de los NPCs.
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        if (WaypointTarget.Puntos == null)
        {
            return;
        }
        //Ciclo que recorra todos los puntos de la clase Waypoints.
        for (int i = 0; i < WaypointTarget.Puntos.Length; i++)
        {
            //CREAMOS HANDLE.
            //Verificamos cualquier cambio del editor para actualizar la posición de los puntos de la ruta cuando los intentamos mover.
            EditorGUI.BeginChangeCheck();
            //Puntos de la ruta del NPC.
            Vector3 puntoActual = WaypointTarget.PosicionActual + WaypointTarget.Puntos[i];
            //Handle se posicionará en cada punto de la ruta creada y se guardará la información de cada punto en la variable nuevoPunto.
            Vector3 nuevoPunto = Handles.FreeMoveHandle(puntoActual, Quaternion.identity,
                0.7f, new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);

            //CREAMOS TEXTO.
            GUIStyle texto = new GUIStyle();
            texto.fontStyle = FontStyle.Bold;
            texto.fontSize = 16;
            texto.normal.textColor = Color.black;
            //Posicionamos en la parte inferior derecha de cada handle.
            Vector3 alineamiento = Vector3.down * 0.3f + Vector3.right * 0.3f;
            
            //Facilitamiento de creaciones de rutas con el handle.
            Handles.Label(WaypointTarget.PosicionActual + WaypointTarget.Puntos[i] + alineamiento
                , $"{i + 1}", texto);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Move Handle");
                WaypointTarget.Puntos[i] = nuevoPunto - WaypointTarget.PosicionActual;
            }
        }
    }
}
