using MineSweeperClone.Core;

namespace MineSweeperClone.GamePlay
{
    /// <summary>
    /// Oyunun kazanılıp kazanılmadığını kontrol eden statik sınıf.
    /// Kazanma koşulu: Mayın olmayan tüm hücreler açılmış olmalı.
    /// </summary>
    public static class WinConditionChecker
    {
        /// <summary>
        /// Tüm güvenli hücrelerin açılıp açılmadığını kontrol eder.
        /// </summary>
        /// <returns>Oyuncu kazandıysa true, aksi halde false.</returns>
        public static bool HasWon(GridData grid)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    CellData cell = grid.GetCell(x, y);

                    if (cell.Type != CellType.Mine && !cell.Revealed)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Kazanma durumunda tüm mayınları bayraklı olarak işaretler.
        /// Görsel geri bildirim için kullanılır.
        /// </summary>
        public static void FlagAllMines(GridData grid)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    CellData cell = grid.GetCell(x, y);

                    if (cell.Type == CellType.Mine)
                    {
                        cell.Flagged = true;
                        grid.SetCell(x, y, cell);
                    }
                }
            }
        }
    }
}
