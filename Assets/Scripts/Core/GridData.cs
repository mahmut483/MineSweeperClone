namespace MineSweeperClone.Core
{
    /// <summary>
    /// Oyun tahtasının hücre verilerini yöneten sınıf.
    /// Grid boyutları, hücre erişimi ve sınır kontrolünden sorumludur.
    /// </summary>
    public class GridData
    {
        public int Width { get; }
        public int Height { get; }

        private readonly CellData[,] cells;

        public GridData(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new CellData[width, height];
        }

        /// <summary>
        /// Belirtilen koordinattaki hücreyi döndürür.
        /// Geçersiz koordinatlar için Type = Invalid olan boş bir CellData döner.
        /// </summary>
        public CellData GetCell(int x, int y)
        {
            if (IsValid(x, y))
            {
                return cells[x, y];
            }

            return new CellData { Type = CellType.Invalid };
        }

        /// <summary>
        /// Belirtilen koordinattaki hücreyi günceller.
        /// CellData struct olduğu için set işlemi zorunludur.
        /// </summary>
        public void SetCell(int x, int y, CellData cell)
        {
            if (IsValid(x, y))
            {
                cells[x, y] = cell;
            }
        }

        /// <summary>
        /// Koordinatların grid sınırları içinde olup olmadığını kontrol eder.
        /// </summary>
        public bool IsValid(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        /// <summary>
        /// Tüm hücre verisini döndürür. BoardRenderer tarafından çizim için kullanılır.
        /// </summary>
        public CellData[,] GetAllCells()
        {
            return cells;
        }
    }
}
