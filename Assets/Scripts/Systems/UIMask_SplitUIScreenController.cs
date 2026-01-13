using UnityEngine;

public class SplitUIScreenController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float splitDistance = 10f;
    [SerializeField] private float mergeDistance = 7f;
    [SerializeField] private float maskOffset = 200f;

    [Header("UI")]
    [SerializeField] private RectTransform leftPart;
    [SerializeField] private RectTransform rightPart;

    [Header("Players")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private Camera camera1;

    private bool isSplit = false;

    void Update()
    {
        Vector2 A = camera1.WorldToScreenPoint(player1.position);
        Vector2 B = camera1.WorldToScreenPoint(player2.position);

        float dist = Vector2.Distance(A, B);

        if(!isSplit && dist > splitDistance)
        {
            isSplit = true;
        } else if (isSplit && dist < mergeDistance) {
            isSplit = false;
        }

        if (!isSplit)
        {
            leftPart.gameObject.SetActive(false);
            rightPart.gameObject.SetActive(false);
            Debug.Log(
            $"WorldDist = {Vector2.Distance(player1.position, player2.position)}, " +
            $"ScreenDist = {dist}"
            );

            return;
        }

        leftPart.gameObject.SetActive(true);
        rightPart.gameObject.SetActive(true);
        
        Debug.Log(
        $"WorldDist = {Vector2.Distance(player1.position, player2.position)}, " +
        $"ScreenDist = {dist}"
        );


        Vector2 mid = (A + B) * 0.5f;
        Vector2 dir = (B - A).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;

        leftPart.position = mid - perp * maskOffset;
        rightPart.position = mid + perp * maskOffset;

        leftPart.rotation = Quaternion.Euler(0, 0, angle);
        rightPart.rotation = Quaternion.Euler(0, 0, angle);

        float w = Screen.width * 2f;
        float h = Screen.height * 2f;

        leftPart.sizeDelta = new Vector2(w, h);
        rightPart.sizeDelta = new Vector2(w, h);
    }
}
