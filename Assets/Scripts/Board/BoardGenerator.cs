using UnityEngine;
using MineSweeperClone.Core;

namespace MineSweeperClone.Board
{
    /// <summary>
    /// Oyun tahtasının hücre, mayın ve sayı üretiminden sorumlu statik sınıf.
    /// Sadece board üretim mantığını içerir (Single Responsibility).
    /// </summary>
    public static class BoardGenerator
    {
        /// <summary>
        /// Grid'deki tüm hücreleri boş olarak başlatır.
        /// </summary>
        public static void GenerateCells(GridData grid)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    var cell = new CellData
                    {
                        Position = new Vector3Int(x, y, 0),
                        Type = CellType.Empty
                    };

                    grid.SetCell(x, y, cell);
                }
            }
        }

        /// <summary>
        /// Grid'e belirtilen sayıda mayın yerleştirir.
        /// Çakışma durumunda bir sonraki boş hücreye kaydırır.
        /// </summary>
        public static void GenerateMines(GridData grid, int mineCount)
        {
            // Olası bir sonsuz döngüyü engellemek için mayın sayısını güvenli sınıra çekiyoruz
            mineCount = Mathf.Min(mineCount, grid.Width * grid.Height);

            for (int i = 0; i < mineCount; i++)
            {
                int x = Random.Range(0, grid.Width);
                int y = Random.Range(0, grid.Height);

                while (grid.GetCell(x, y).Type == CellType.Mine)
                {
                    x++;

                    if (x >= grid.Width)
                    {
                        x = 0;
                        y++;

                        if (y >= grid.Height)
                        {
                            y = 0;
                        }
                    }
                }

                CellData cell = grid.GetCell(x, y);
                cell.Type = CellType.Mine;
                grid.SetCell(x, y, cell);
            }
        }

        /// <summary>
        /// Her boş hücrenin etrafındaki mayın sayısını hesaplar ve
        /// sayı > 0 ise hücre tipini Number olarak günceller.
        /// </summary>
        public static void GenerateNumbers(GridData grid)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    CellData cell = grid.GetCell(x, y);

                    if (cell.Type == CellType.Mine)
                    {
                        continue;
                    }

                    cell.Number = CountAdjacentMines(grid, x, y);

                    if (cell.Number > 0)
                    {
                        cell.Type = CellType.Number;
                    }

                    grid.SetCell(x, y, cell);
                }
            }
        }

        /// <summary>
        /// Belirtilen hücrenin 8 komşusundaki mayın sayısını hesaplar.
        /// </summary>
        private static int CountAdjacentMines(GridData grid, int cellX, int cellY)
        {
            int count = 0;

            for (int offsetX = -1; offsetX <= 1; offsetX++)
            {
                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    if (offsetX == 0 && offsetY == 0)
                    {
                        continue;
                    }

                    int x = cellX + offsetX;
                    int y = cellY + offsetY;

                    if (!grid.IsValid(x, y))
                    {
                        continue;
                    }

                    if (grid.GetCell(x, y).Type == CellType.Mine)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
