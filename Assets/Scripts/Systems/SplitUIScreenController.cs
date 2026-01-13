using UnityEngine;

public class SplitUIScreenController : MonoBehaviour
{
    [Header("UI")]
    public RectTransform leftPart;
    public RectTransform rightPart;

    [Header("Players")]
    public Transform player1;
    public Transform player2;
    public Camera camera1;

    void Update()
    {
        Vector2 A = camera1.WorldToScreenPoint(player1.position);
        Vector2 B = camera1.WorldToScreenPoint(player2.position);

        Vector2 mid = (A + B) * 0.5f;
        Vector2 dir = (B - A).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        leftPart.position = mid;
        rightPart.position = mid;

        leftPart.rotation = Quaternion.Euler(0, 0, angle + 90f);
        rightPart.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}
