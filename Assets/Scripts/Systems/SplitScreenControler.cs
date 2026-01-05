using UnityEngine;

public class SplitScreenControler : MonoBehaviour
{
    [Header("Split Screen Parameters")]
    [SerializeField] private float splitDistance = 0;
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    private bool isSplit;
    
    void FixedUpdate()
    {
        float distance = Vector3.Distance(player1.position, player2.position);
        if(distance > splitDistance)
        {
            Split();
        }
        else
        {
            Merge();
        }
    }

    private void Split()
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
    }
}
