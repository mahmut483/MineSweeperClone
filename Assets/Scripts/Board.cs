using System.IO.Compression;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    // Diğer class'lardan erişilebilir fakat değiştirilemez
    public Tilemap tilemap { get; private set; }

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int heigth = state.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                Cell cell = state[x, y];
            }
        }

    }

    // private Tile GetTile(Cell cell)
    // {
    //     if (cell.revealed)
    //     {
    //         //...
    //     }else if (cell.flagged)
    //     {
    //         //...
    //     }
    //     else
    //     {
    //         //...
    //     }
    // }
}
