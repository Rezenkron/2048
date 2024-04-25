using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField, Header("Board")] private SpriteRenderer _boardPrefab;
    [SerializeField] private float _boardAdditionalSize;

    [SerializeField, Header("Cells")] private Cell _cellPrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private List<BlockType> _types;

    public List<Cell> _cells { get; private set; }

    private Vector3 _center;

    private BlockType GetBlockTypeByValue(int value) => _types.First(t => t.Value == value);

    void Start()
    {
        _center = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);

        GenerateGrid();
        SpawnBlocks(2);
    }

    private void GenerateGrid()
    {
        GenerateCells();
        GenerateBoard();
        CenterCamera();
    }

    private void GenerateCells()
    {
        _cells = new List<Cell>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var cell = Instantiate(_cellPrefab, new Vector3(x, y, 0), Quaternion.identity, gameObject.transform);
                cell.name = $"{nameof(cell)} {x} {y}";
                _cells.Add(cell);
            }
        }
    }

    private void GenerateBoard()
    {
        var board = Instantiate(_boardPrefab, _center, Quaternion.identity, gameObject.transform);

        board.size = new Vector2(_width + _boardAdditionalSize, _height + _boardAdditionalSize);
    }

    private void SpawnBlocks(int amount)
    {
        var freeCells = _cells.Where(n  => n.occupiedBlock == null).OrderBy(b => Random.value).ToList();

        foreach(var cell in freeCells.Take(amount))
        {
            var block = Instantiate(_blockPrefab, cell.Pos, Quaternion.identity);
            block.Init(GetBlockTypeByValue(2));
        }
    }

    private void Shift()
    {

    }

    private void CenterCamera()
    {
        Camera.main.transform.position = new Vector3(_center.x, _center.y, Camera.main.transform.position.z);
    }
}


[Serializable]
public struct BlockType
{
    public int Value;
    public Color Color;
}