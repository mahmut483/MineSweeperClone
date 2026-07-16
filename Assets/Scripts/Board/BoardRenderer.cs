using UnityEngine;
using UnityEngine.Tilemaps;
using MineSweeperClone.Core;

namespace MineSweeperClone.Board
{
    /// <summary>
    /// Oyun tahtasını Tilemap üzerinde görselleştiren MonoBehaviour.
    /// Sadece çizim sorumluluğunu taşır (Single Responsibility).
    /// </summary>
    public class BoardRenderer : MonoBehaviour
    {
        public Tilemap Tilemap { get; private set; }

        [SerializeField] private Tile tileUnknown;
        [SerializeField] private Tile tileMine;
        [SerializeField] private Tile tileEmpty;
        [SerializeField] private Tile tileExploded;
        [SerializeField] private Tile tileFlag;

        [Tooltip("Sırasıyla 1-8 arası sayı tile'ları. Index 0 = sayı 1, Index 7 = sayı 8.")]
        [SerializeField] private Tile[] tileNumbers = new Tile[8];

        private void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
        }

        /// <summary>
        /// Verilen hücre durumuna göre tüm tahtayı yeniden çizer.
        /// </summary>
        public void Draw(CellData[,] state)
        {
            int width = state.GetLength(0);
            int height = state.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CellData cell = state[x, y];
                    Tilemap.SetTile(cell.Position, GetTile(cell));
                }
            }
        }

        private Tile GetTile(CellData cell)
        {
            if (cell.Revealed)
            {
                return GetRevealedTile(cell);
            }

            if (cell.Flagged)
            {
                return tileFlag;
            }

            return tileUnknown;
        }

        private Tile GetRevealedTile(CellData cell)
        {
            switch (cell.Type)
            {
                case CellType.Empty:
                    return tileEmpty;
                case CellType.Mine:
                    return cell.Exploded ? tileExploded : tileMine;
                case CellType.Number:
                    return GetNumberTile(cell);
                default:
                    return null;
            }
        }

        private Tile GetNumberTile(CellData cell)
        {
            int index = cell.Number - 1;

            if (index >= 0 && index < tileNumbers.Length)
            {
                return tileNumbers[index];
            }

            return null;
        }
    }
}
