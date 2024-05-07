using System;
using System.Collections.Generic;
using System.Linq;
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

    public List<Cell> Cells { get; private set; }
    public List<Block> Blocks { get; private set; }

    private Vector3 _center;

    private BlockType GetBlockTypeByValue(int value) => _types.First(t => t.Value == value);
    private Cell GetCellAtPosition(Vector2 pos)
    {
        return Cells.FirstOrDefault(n => n.Pos == pos);
    }

    void Start()
    {
        _center = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);

        GenerateGrid();
        SpawnBlocks(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Shift(Vector2.left);
            SpawnBlocks(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Shift(Vector2.right);
            SpawnBlocks(1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Shift(Vector2.up);
            SpawnBlocks(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shift(Vector2.down);
            SpawnBlocks(1);
        }
    }

    private void GenerateGrid()
    {
        GenerateCells();
        GenerateBoard();
        CenterCamera();
    }

    private void GenerateCells()
    {
        Cells = new List<Cell>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var cell = Instantiate(_cellPrefab, new Vector3(x, y, 0), Quaternion.identity, gameObject.transform);
                cell.name = $"{nameof(cell)} {x} {y}";
                Cells.Add(cell);
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
        Blocks = new List<Block>();
        var freeCells = Cells.Where(n  => n.OccupiedBlock == null).OrderBy(b => Random.value).ToList();

        foreach(var cell in freeCells.Take(amount))
        {
            SpawnBlock(cell, Random.value > 0.8f ? 4 : 2);
        }
    }

    private void SpawnBlock(Cell cell, int value)
    {
        var block = Instantiate(_blockPrefab, cell.Pos, Quaternion.identity);
        block.SetBlock(cell);
        block.Init(GetBlockTypeByValue(value));
        Blocks.Add(block);
    }

    private void Shift(Vector2 dir)
    {
        var orderedBlocks = Blocks.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();
        if(dir == Vector2.right || dir  == Vector2.up) orderedBlocks.Reverse();
        
        foreach(var block in orderedBlocks)
        {
            var next = block.Cell;
            do
            {
                block.SetBlock(next);

                var possibleCell = GetCellAtPosition(next.Pos + dir);
                if (possibleCell != null)
                {
                    if(possibleCell.OccupiedBlock != null && possibleCell.OccupiedBlock.CanMerge(block.Value))
                    {
                        block.MergeBlock(possibleCell.OccupiedBlock);
                    }

                    else if (possibleCell.OccupiedBlock == null)
                    {
                        next = possibleCell;
                    }
                }
            } while (next != block.Cell);

            block.transform.position = block.Cell.Pos;
        }

        foreach (var block in orderedBlocks)
        {
            var movePoint = block.MergingBlock != null ? block.MergingBlock.Cell.Pos : block.Cell.Pos;
        }

        foreach(var block in orderedBlocks.Where(b => b.MergingBlock != null))
        {
            MergeBlocks(block.MergingBlock , block);
        }
    }

    private void MergeBlocks(Block baseBlock, Block mergingBlock)
    {
        SpawnBlock(baseBlock.Cell, baseBlock.Value * 2);
        RemoveBlock(baseBlock);
        RemoveBlock(mergingBlock);
    }

    private void RemoveBlock(Block block)
    {
        Blocks.Remove(block);
        Destroy(block.gameObject);
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