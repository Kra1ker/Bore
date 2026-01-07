using System;
using UnityEngine;
using Unity.Cinemachine;

public class SplitScreenControler : MonoBehaviour
{
    [Header("Split Screen Parameters")]
    [SerializeField] private float split_Distance = 15f;
    [SerializeField] private float smooth = 5f;
    [SerializeField] private bool vertical_Split = true;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 8f;
    [SerializeField] private float zoomStartDistance = 3f;
    [SerializeField] private float zoomEndDistance = 6f;
    [SerializeField] private float zoomSmooth = 3f;

    [Header("Players")]
    [SerializeField] private Transform player_1;
    [SerializeField] private Transform player_2;

    [Header("Cameras")]
    [SerializeField] private Camera camera_1;
    [SerializeField] private Camera camera_2;
    [SerializeField] private CinemachineCamera cinemachineCamera_1;
    [SerializeField] private CinemachineCamera cinemachineCamera_2;
    private bool isSplit;
    
    void Update()
    {
        float distance = Vector3.Distance(player_1.position, player_2.position);
        bool should_Split = distance > split_Distance;
        
        if(should_Split != isSplit)
        {
            isSplit = should_Split;
        }
        if(!should_Split)
        UpdateZoom(distance);

        UpdateViewports();
    }

    void UpdateZoom(float distance)
    {
        float t = Mathf.InverseLerp(zoomStartDistance, zoomEndDistance, distance);
        float zoom = Mathf.Lerp(minZoom, maxZoom, t);

        cinemachineCamera_1.Lens.OrthographicSize = 
            Mathf.Lerp(cinemachineCamera_1.Lens.OrthographicSize, zoom, Time.deltaTime * zoomSmooth);

        cinemachineCamera_2.Lens.OrthographicSize =
            Mathf.Lerp(cinemachineCamera_2.Lens.OrthographicSize, zoom, Time.deltaTime * zoomSmooth);
    }

    void UpdateViewports()
    {
        if (!isSplit)
        {
            camera_1.rect = LerpRect(camera_1.rect, new Rect(0, 0, 1, 1));
            camera_2.rect = LerpRect(camera_2.rect, new Rect(0, 0, 0, 0));
        }
        else
        {
            if(vertical_Split)
            {
                camera_1.rect = LerpRect(camera_1.rect, new Rect(0f, 0f, 0.5f, 1f));
                camera_2.rect = LerpRect(camera_2.rect, new Rect(0.5f, 0f, 0.5f, 1f));
            } else
            {
                camera_1.rect = LerpRect(camera_1.rect, new Rect(0f, 0.5f, 1f, 0.5f));
                camera_2.rect = LerpRect(camera_2.rect, new Rect(0f, 0f, 1f, 0.5f));
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

    /* private void Split()
    {
        if (isSplit) return;
        isSplit = true;
        Debug.Log("Split ON");
    }

    private void Merge()
    {
        if (!isSplit) return;
        isSplit = false;
        Debug.Log("Split OFF");
    } */ // SplitScreen version: Concept
}
