using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEditor.Rendering;

public class SplitScreenControler : MonoBehaviour
{
    [Header("Split Screen Parameters")]
    [SerializeField] private float splitDistance = 7.5f;
    [SerializeField] private float smooth = 5f;
    [SerializeField] private bool verticalSplit = true;
    [Header("")]
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 8f;
    [SerializeField] private float zoomStartDistance = 3f;
    [SerializeField] private float zoomEndDistance = 6f;
    [SerializeField] private float zoomSmooth = 3f;

    [Header("Players")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Cameras")]
    [SerializeField] private Camera camera1;
    [SerializeField] private Camera camera2;
    [SerializeField] private CinemachineCamera cinemachineCamera1;
    [SerializeField] private CinemachineCamera cinemachineCamera2;
    private bool isSplit;
    
    void Update()
    {
        float distance = Vector3.Distance(player1.position, player2.position);
        
        UpdateZoom(distance);
        UpdateViewports(distance);
    }

    void UpdateZoom(float distance)
    {
        float t = Mathf.InverseLerp(zoomStartDistance, zoomEndDistance, distance);
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, t);

        cinemachineCamera1.Lens.OrthographicSize = 
            Mathf.Lerp(cinemachineCamera1.Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSmooth);

        cinemachineCamera2.Lens.OrthographicSize =
            Mathf.Lerp(cinemachineCamera2.Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSmooth);
    }

    void UpdateViewports(float distance)
    {
        bool shouldSplit = distance > splitDistance;
        isSplit = shouldSplit;

        if (distance <= splitDistance)
        {
            camera1.rect = LerpRect(camera1.rect, new Rect(0, 0, 1, 1));
            camera2.rect = LerpRect(camera2.rect, new Rect(0.5f, 0, 0, 1));
            return;
        }
        
        Vector2 dir = player2.position - player1.position;
        verticalSplit = Mathf.Abs(dir.x) > Mathf.Abs(dir.y);

        {
            if(verticalSplit)
            {
                camera1.rect = LerpRect(camera1.rect, new Rect(0f, 0f, 0.5f, 1f));
                camera2.rect = LerpRect(camera2.rect, new Rect(0.5f, 0f, 0.5f, 1f));
            } else
            {
                camera1.rect = LerpRect(camera1.rect, new Rect(0f, 0.5f, 1f, 0.5f));
                camera2.rect = LerpRect(camera2.rect, new Rect(0f, 0f, 1f, 0.5f));
            }
        }
    }

    Rect LerpRect(Rect current, Rect target)
    {
        return new Rect(
            Mathf.Lerp(current.x, target.x, Time.deltaTime * smooth),
            Mathf.Lerp(current.y, target.y, Time.deltaTime * smooth),
            Mathf.Lerp(current.width, target.width, Time.deltaTime * smooth),
            Mathf.Lerp(current.height, target.height, Time.deltaTime * smooth)
        );
    }
}
