using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _text;
    public int Value;
    public Cell Cell;
    public Block MergingBlock;
    public bool Merging;
    public Vector2 Pos => transform.position;
    public void Init(BlockType type)
    {
        Value = type.Value;
        _spriteRenderer.color = type.Color;
        _text.text = type.Value.ToString();
    }

    public void SetBlock(Cell cell)
    {
        if(Cell != null)
        {
            Cell.OccupiedBlock = null;
        }
        Cell = cell;
        Cell.OccupiedBlock = this;
    }

    public void MergeBlock(Block blockToMergeWith)
    {
        MergingBlock = blockToMergeWith;

        Cell.OccupiedBlock = null;

        blockToMergeWith.Merging = true;

    }

    public bool CanMerge(int value) => value == Value && !Merging && MergingBlock == null;
}
