using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 Pos => transform.position;
    public Block occupiedBlock;
}
