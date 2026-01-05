using System;
using UnityEngine;

public class SplitScreenControler : MonoBehaviour
{
    [Header("Split Screen Parameters")]
    [SerializeField] private float split_Distance = 15f;
    [SerializeField] private float smooth = 5f;
    [SerializeField] private bool vertical_Split = true;
    [Header("Players")]
    [SerializeField] private Transform player_1;
    [SerializeField] private Transform player_2;
    [Header("Cameras")]
    [SerializeField] private Camera camera_1;
    [SerializeField] private Camera camera_2;
    private bool isSplit;
    
    void Update()
    {
        float distance = Vector3.Distance(player_1.position, player_2.position);
        bool should_Split = distance > split_Distance;
        
        if(should_Split != isSplit)
        {
            isSplit = should_Split;
        }

        UpdateViewports();
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
