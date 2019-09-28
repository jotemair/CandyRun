﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Water Level")]
public class WaterLevel : MonoBehaviour
{
    #region Public Properties

    #endregion

    #region Private Variables

    [SerializeField]
    Shader _shader = null;

    Material _material = null;

    [SerializeField]
    private Texture2D _water = null;

    private Camera _cam = null;

    [SerializeField]
    private float _waterLevel = 10f;

    #endregion

    #region MonoBehaviour functions

    private void Start()
    {
        _cam = GetComponent<Camera>();
        _cam.depthTextureMode = DepthTextureMode.DepthNormals;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (null != _shader)
        {
            if (null == _material)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            if (null == _water)
            {
                _water = new Texture2D(1, 1);
            }

            _material.SetTexture("_WaterTexture", _water);

            {
                _cam = GetComponent<Camera>();

                float Angle;
                float Hipo;

                Vector3 Sc_Center;
                Vector3 X_Vector;
                Vector3 Y_Vector;

                Ray ray_x = _cam.ScreenPointToRay(new Vector3(Screen.width, 0));
                Ray ray_y = _cam.ScreenPointToRay(new Vector3(0, Screen.height));
                Ray ray_0 = _cam.ScreenPointToRay(new Vector3(0, 0));
                Ray ray_2 = _cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

                Angle = Vector3.Angle(ray_x.direction, ray_2.direction);

                Hipo = _cam.farClipPlane / Mathf.Cos(Mathf.PI * Angle / 180);

                X_Vector = Hipo * ray_x.direction - Hipo * ray_0.direction;
                Y_Vector = Hipo * ray_y.direction - Hipo * ray_0.direction;

                Debug.DrawRay(transform.position + Hipo * ray_0.direction, X_Vector, Color.red);
                Debug.DrawRay(transform.position + Hipo * ray_0.direction, Y_Vector, Color.green);

                Sc_Center = transform.position + Hipo * ray_0.direction;

                _material.SetVector("_Vector_X", new Vector4(X_Vector.x, X_Vector.y, X_Vector.z, 0));
                _material.SetVector("_Vector_Y", new Vector4(Y_Vector.x, Y_Vector.y, Y_Vector.z, 0));
                _material.SetVector("_Screen_Center", new Vector4(Sc_Center.x, Sc_Center.y, Sc_Center.z, 0));

                _material.SetFloat("_WaterLevel", _waterLevel);
            }

            Graphics.Blit(source, destination, _material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    #endregion
}