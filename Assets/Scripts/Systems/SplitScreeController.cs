using UnityEngine;

public class SplitScreenController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private Camera mainCamera;

    [Header("Parameters")]
    [SerializeField] private float lineLenght = 50f;

    void OnDrawGizmos()
    {
        if (!player1 || !player2 || !mainCamera) return;

        Vector2 A = player1.position;
        Vector2 B = player2.position;

        Vector2 mid = (A + B) * 0.5f;
        Vector2 dir = (B - A).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x);

        Vector2 worldP1 = mid - perp * lineLenght;
        Vector2 worldP2 = mid + perp * lineLenght;

        Vector2 screenP1 = mainCamera.WorldToScreenPoint(worldP1);
        Vector2 screenP2 = mainCamera.WorldToScreenPoint(worldP2);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(worldP1, worldP2);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(A, B);

        Debug.Log($"Split Screen Line: {screenP1} -> {screenP2}");
    }
}
