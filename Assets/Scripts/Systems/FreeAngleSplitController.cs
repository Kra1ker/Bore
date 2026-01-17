using UnityEngine;
using Unity.Cinemachine;

public class FreeAngleSplitController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private Camera cam1;
    [SerializeField] private Camera cam2;
    [SerializeField] private CinemachineCamera cinemachineCam1;
    [SerializeField] private CinemachineCamera cinemachineCam2;
    [SerializeField] private CinemachineCamera cinemachineCamGroup;

    [Header("Material")]
    [SerializeField] private Material splitMaterial;

    [Header("Parameters")]
    [SerializeField] private float splitDistance = 8f;
    [SerializeField] private float mergeDistance = 5f;
    [SerializeField] private Vector2 offsetWeights = new Vector2(1f, 1f);
    [SerializeField] private float cameraOffset = 4f;

    private bool isSplit;
    private CinemachinePositionComposer _composer1;
    private CinemachinePositionComposer _composer2;
    private CinemachineBrain _cinemaBrainCam1;
    private CinemachineBrain _cinemaBrainCam2;

    void Awake()
    {
        _composer1 = cinemachineCam1.GetComponent<CinemachinePositionComposer>();
        _composer2 = cinemachineCam2.GetComponent<CinemachinePositionComposer>();
        _cinemaBrainCam1 = cam1.GetComponent<CinemachineBrain>();
        _cinemaBrainCam2 = cam2.GetComponent<CinemachineBrain>();

        if (!_composer1 || !_composer2)
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

        if (!isSplit && dist > splitDistance)
            isSplit = true;
        else if (isSplit && dist < mergeDistance)
            isSplit = false;

        if (!isSplit)
        {
            splitMaterial.SetFloat("_Softness", 1f);
            splitMaterial.SetFloat("_SplitOffset", Mathf.Lerp(splitMaterial.GetFloat("_SplitOffset"), -2f, 0.1f));
            splitMaterial.SetFloat("_SplitLineWidth", Mathf.Lerp(splitMaterial.GetFloat("_SplitLineWidth"), 0f, Time.deltaTime * 5f));
            _composer1.TargetOffset = Vector3.zero;
            _composer2.TargetOffset = Vector3.zero;
            _cinemaBrainCam1.ChannelMask = OutputChannels.Channel03;
            _cinemaBrainCam2.ChannelMask = OutputChannels.Channel03;
            return;
        }

        Vector2 dir = (B - A).normalized;
        Vector2 mid = (A + B) * 0.5f;
        Vector2 normal = new Vector2(-dir.y, dir.x);
        Vector2 weightedNormal = new Vector2(
            normal.y * offsetWeights.x,
            normal.x * -offsetWeights.y
        );

        _composer1.TargetOffset = weightedNormal * cameraOffset;
        _composer2.TargetOffset = -weightedNormal * cameraOffset;
        _cinemaBrainCam1.ChannelMask = OutputChannels.Channel01;
        _cinemaBrainCam2.ChannelMask = OutputChannels.Channel02;

        splitMaterial.SetVector("_SplitDir", new Vector4(-normal.y, normal.x, 0, 0));
        splitMaterial.SetFloat("_SplitOffset", Mathf.Lerp(splitMaterial.GetFloat("_SplitOffset"), 0, 0.1f));
        splitMaterial.SetFloat("_SplitLineWidth", Mathf.Lerp(splitMaterial.GetFloat("_SplitLineWidth"), 0.002f, Time.deltaTime * 5f));
    }
}
