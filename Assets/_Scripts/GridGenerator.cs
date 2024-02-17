using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [Header("Board"), SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private float _boardAdditionalSize;

    [Header("Cells") ,SerializeField] private Cell _cellPrefab;

    public List<Cell> cells = new();

    private Vector3 _center;

    void Start()
    {
        _center = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        GenerateCells();
        GenerateBoard();
        CenterCamera();
    }

    private void GenerateCells()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var cell = Instantiate(_cellPrefab, new Vector3(x, y, 0), Quaternion.identity, gameObject.transform);
                cell.name = $"Cell {x + 1} {y + 1}";
                cells.Add(cell);
            }
        }
    }

    private void GenerateBoard()
    {
        var board = Instantiate(_boardPrefab, _center, Quaternion.identity, gameObject.transform);

        board.size = new Vector2(_width + _boardAdditionalSize, _height + _boardAdditionalSize);
    }

    private void CenterCamera()
    {
        Camera.main.transform.position = new Vector3(_center.x, _center.y, Camera.main.transform.position.z);
    }
}
