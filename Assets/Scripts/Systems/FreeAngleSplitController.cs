using UnityEngine;
using Unity.Cinemachine;

public class FreeAngleSplitController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private CinemachineCamera cam1;
    [SerializeField] private CinemachineCamera cam2;

    [Header("Material")]
    [SerializeField] private Material splitMaterial;

    [Header("Parameters")]
    [SerializeField] private float splitDistance = 8f;
    [SerializeField] private float mergeDistance = 5f;
    [SerializeField] private Vector2 offsetWeights = new Vector2(1f, 1f);
    [SerializeField] private float cameraOffset = 4f;
    [SerializeField] private float followLerp = 5f;

    private bool isSplit;
    private CinemachinePositionComposer composer1;
    private CinemachinePositionComposer composer2;

    void Awake()
    {
        composer1 = cam1.GetComponent<CinemachinePositionComposer>();
        composer2 = cam2.GetComponent<CinemachinePositionComposer>();

        if (!composer1 || !composer2)
        {
            Debug.LogError("Position Composer cannot be found.");
        }
    }

    void Update()
    {
        Vector2 A = player1.position;
        Vector2 B = player2.position;

        float dist = Vector2.Distance(A, B);
        Debug.Log(dist);

        /* if (!isSplit && dist > splitDistance)
            isSplit = true;
        else if (isSplit && dist < mergeDistance)
            isSplit = false;

        if (!isSplit)
        {
            splitMaterial.SetFloat("_Softness", 1f);
            composer1.TargetOffset = Vector3.zero;
            composer2.TargetOffset = Vector3.zero;
            return;
        } */

        Vector2 dir = (B - A).normalized;
        Vector2 mid = (A + B) * 0.5f;
        Vector2 normal = new Vector2(-dir.y, dir.x);
        Vector2 weightedNormal = new Vector2(
            normal.y * offsetWeights.x,
            normal.x * offsetWeights.y
        );

        composer1.TargetOffset = weightedNormal * cameraOffset;
        composer2.TargetOffset = -weightedNormal * cameraOffset;

        splitMaterial.SetVector("_SplitNormal", new Vector4(normal.x, normal.y, 0, 0));
        splitMaterial.SetFloat("_SplitOffset", 0f);
        splitMaterial.SetFloat("_Softness", 0.02f);
    }
}
