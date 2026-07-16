using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace MineSweeperClone.Input
{
    /// <summary>
    /// Oyuncu girdilerini yöneten sınıf.
    /// Input okuma ve dünya koordinatına çevirme sorumluluğunu taşır.
    /// MonoBehaviour değildir — GameManager tarafından oluşturulur ve kullanılır.
    /// </summary>
    public class GameInputHandler
    {
        private readonly Camera mainCamera;
        private readonly Tilemap tilemap;

        public GameInputHandler(Camera mainCamera, Tilemap tilemap)
        {
            this.mainCamera = mainCamera;
            this.tilemap = tilemap;
        }

        /// <summary>
        /// Oyunu yeniden başlatma tuşuna basılıp basılmadığını kontrol eder.
        /// </summary>
        public bool IsRestartPressed =>
            Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame;

        /// <summary>
        /// Hücre açma (sol tık) girdisini kontrol eder.
        /// </summary>
        public bool IsRevealPressed =>
            Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        /// <summary>
        /// Bayrak koyma (sağ tık) girdisini kontrol eder.
        /// </summary>
        public bool IsFlagPressed =>
            Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame;

        /// <summary>
        /// Fare imlecinin altındaki hücrenin grid koordinatını hesaplar.
        /// Ekran koordinatını → dünya koordinatına → grid koordinatına dönüştürür.
        /// </summary>
        public Vector3Int GetCellPositionUnderMouse()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            return tilemap.WorldToCell(worldPosition);
        }
    }
}
