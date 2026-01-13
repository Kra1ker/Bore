using UnityEngine;

public class FreeAngleSplitController : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Material")]
    [SerializeField] private Material splitMaterial;

    [Header("Distances")]
    [SerializeField] private float splitWorldDistance = 8f;
    [SerializeField] private float mergeWorldDistance = 5f;

    private bool isSplit;

    void Update()
    {
        Vector2 A = player1.position;
        Vector2 B = player2.position;

        float worldDist = Vector2.Distance(A, B);

        if (!isSplit && worldDist > splitWorldDistance)
            isSplit = true;
        else if (isSplit && worldDist < mergeWorldDistance)
            isSplit = false;

        if (!isSplit)
        {
            splitMaterial.SetFloat("_Softness", 1f); // почти без деления
            return;
        }

        Vector2 dir = (B - A).normalized;
        Vector2 normal = new Vector2(-dir.y, dir.x);

        splitMaterial.SetVector("_SplitNormal", new Vector4(normal.x, normal.y, 0, 0));
        splitMaterial.SetFloat("_SplitOffset", 0f);
        splitMaterial.SetFloat("_Softness", 0.02f);
    }
}
