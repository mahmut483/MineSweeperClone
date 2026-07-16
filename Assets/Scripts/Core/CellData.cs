using UnityEngine;

namespace MineSweeperClone.Core
{
    /// <summary>
    /// Mayın tarlası oyunundaki bir hücrenin verilerini tutan struct.
    /// Struct olarak tanımlanması performans açısından avantajlıdır (stack allocation).
    /// </summary>
    public struct CellData
    {
        public CellType Type;
        public Vector3Int Position;
        public int Number;
        public bool Revealed;
        public bool Flagged;
        public bool Exploded;
    }
}
