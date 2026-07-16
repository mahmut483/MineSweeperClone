using MineSweeperClone.Core;

namespace MineSweeperClone.GamePlay
{
    /// <summary>
    /// Hücre açma, flood-fill ve patlama mantığından sorumlu sınıf.
    /// Oyun mantığının reveal ile ilgili kısmını kapsüller.
    /// </summary>
    public class CellRevealer
    {
        /// <summary>
        /// Tek bir hücreyi açık olarak işaretler.
        /// </summary>
        public void RevealCell(GridData grid, int x, int y)
        {
            CellData cell = grid.GetCell(x, y);
            cell.Revealed = true;
            grid.SetCell(x, y, cell);
        }

        /// <summary>
        /// Boş hücrelerden başlayarak flood-fill algoritması ile
        /// komşu boş ve sayılı hücreleri otomatik olarak açar.
        /// Recursive implementasyon kullanır.
        /// </summary>
        public void Flood(GridData grid, int x, int y)
        {
            CellData cell = grid.GetCell(x, y);

            if (cell.Revealed)
            {
                return;
            }

            if (cell.Type == CellType.Mine || cell.Type == CellType.Invalid)
            {
                return;
            }

            cell.Revealed = true;
            grid.SetCell(x, y, cell);

            if (cell.Type == CellType.Empty)
            {
                Flood(grid, x - 1, y);
                Flood(grid, x + 1, y);
                Flood(grid, x, y - 1);
                Flood(grid, x, y + 1);
            }
        }

        /// <summary>
        /// Mayına basıldığında patlama efekti uygular.
        /// Tıklanan mayını "patlamış" olarak işaretler ve
        /// tüm mayınları görünür yapar.
        /// </summary>
        public void Explode(GridData grid, int x, int y)
        {
            CellData cell = grid.GetCell(x, y);
            cell.Revealed = true;
            cell.Exploded = true;
            grid.SetCell(x, y, cell);

            RevealAllMines(grid);
        }

        /// <summary>
        /// Tahtadaki tüm mayınları görünür yapar.
        /// Oyun kaybedildiğinde çağrılır.
        /// </summary>
        private void RevealAllMines(GridData grid)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    CellData cell = grid.GetCell(x, y);

                    if (cell.Type == CellType.Mine)
                    {
                        cell.Revealed = true;
                        grid.SetCell(x, y, cell);
                    }
                }
            }
        }
    }
}
