using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int Value;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _text;
    public void Init(BlockType type)
    {
        Value = type.Value;
        _spriteRenderer.color = type.Color;
        _text.text = type.Value.ToString();
    }
}
