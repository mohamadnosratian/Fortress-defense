﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float limitLeft = -6;
    public float limitRight = 1000;

    public float moveSpeed = 2;
    public float distanceScale = 1;
    float beginX;
    float beginCamreaPosX;
    bool isDragging = false;
    Vector3 target = new Vector3(-1, 0, 0);
    bool allowWorking = false;
    public Vector3 target_right = new Vector3(-10, 0, 0);
    public Vector3 target_left = new Vector3(-1, 0, 0);
    public bool allowTouch = false, is_left = true;
    public Image CameraMove;
    public Sprite Left, Right;

    IEnumerator Start()
    {
        yield return null;
        beginCamreaPosX = transform.position.x;
        target = transform.position;
        target.x = Mathf.Clamp(transform.position.x, limitLeft + CameraHalfWidth, limitRight - CameraHalfWidth);
        allowWorking = true;
    }

    public void Move()
    {
        if (is_left)
        {
            target = target_right;
            CameraMove.sprite = Left;
            is_left = false;
        }
        else
        {
            target = target_left;
            CameraMove.sprite = Right;
            is_left = true;
        }
    }
    void Update()
    {
        if (!allowTouch)
        {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.fixedDeltaTime);
            return;
        }
        if (!allowWorking)
            return;
        transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.fixedDeltaTime);
        if (GameManager.Instance.State != GameManager.GameState.Playing)
            return;
        if (!isDragging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                beginX = Input.mousePosition.x;
                beginCamreaPosX = transform.position.x;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            else
            {
                target = new Vector3(beginCamreaPosX + (beginX - Input.mousePosition.x) * distanceScale * 0.01f, transform.position.y, transform.position.z);

                target.x = Mathf.Clamp(target.x, limitLeft + CameraHalfWidth, limitRight - CameraHalfWidth);
            }
        }
    }

    public float CameraHalfWidth
    {
        get { return (Camera.main.orthographicSize * ((float)Screen.width / (float)Screen.height)); }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.color = Color.yellow;
        Vector2 boxSize = new Vector2(limitRight - limitLeft, Camera.main.orthographicSize * 2);
        Vector2 center = new Vector2((limitRight + limitLeft) * 0.5f, transform.position.y);
        Gizmos.DrawWireCube(center, boxSize);
    }
}
