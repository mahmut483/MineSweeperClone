using System;

namespace MineSweeperClone.Events
{
    /// <summary>
    /// Oyun genelindeki olayları yöneten statik event sınıfı.
    /// Observer Pattern uygulaması ile loose coupling sağlar.
    /// </summary>
    public static class GameEvents
    {
        /// <summary>
        /// Yeni bir oyun başladığında tetiklenir.
        /// </summary>
        public static event Action OnGameStarted;

        /// <summary>
        /// Oyuncu mayına basıp oyunu kaybettiğinde tetiklenir.
        /// </summary>
        public static event Action OnGameOver;

        /// <summary>
        /// Oyuncu tüm güvenli hücreleri açarak kazandığında tetiklenir.
        /// </summary>
        public static event Action OnGameWon;

        public static void RaiseGameStarted() => OnGameStarted?.Invoke();
        public static void RaiseGameOver() => OnGameOver?.Invoke();
        public static void RaiseGameWon() => OnGameWon?.Invoke();

        /// <summary>
        /// Tüm event aboneliklerini temizler.
        /// Sahne geçişlerinde memory leak önlemek için çağrılmalıdır.
        /// </summary>
        public static void ClearAll()
        {
            OnGameStarted = null;
            OnGameOver = null;
            OnGameWon = null;
        }
    }
}
