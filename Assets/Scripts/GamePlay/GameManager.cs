using UnityEngine;
using MineSweeperClone.Board;
using MineSweeperClone.Core;
using MineSweeperClone.Events;
using MineSweeperClone.Input;
using TMPro;

namespace MineSweeperClone.GamePlay
{
    /// <summary>
    /// Oyun akışını koordine eden ana MonoBehaviour.
    /// Kendisi iş mantığı içermez — ilgili sınıflara delege eder.
    /// Sorumlulukları: Yaşam döngüsü yönetimi ve bileşenler arası koordinasyon.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Board Ayarları")]
        [SerializeField] private int width = 16;
        [SerializeField] private int height = 16;
        [SerializeField] private TMP_Text levelTxt;
        [SerializeField] private TMP_Text minesCountTxt;
        [SerializeField] private TMP_Text levelPanelTitle;
        [SerializeField] private TMP_Text levelPanelLabel;
        [SerializeField] private GameObject levelPanel;
        [SerializeField] private TMP_Text panelButtonTMP;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip revealingClip, explodingClip, floodingClip, flagingClip;
        
        private int mineCount;
        private int firstLevelMineCount = 10;
        private int midLevelMineCount = 20;
        private int lastLevelMineCount = 30;
        
        public int level = 1;

        private const float CameraDepth = -10f;

        private BoardRenderer boardRenderer;
        private Camera mainCamera;
        private GridData grid;
        private GameInputHandler inputHandler;
        private CellRevealer cellRevealer;
        private bool isGameOver;

        private void Awake()
        {
            boardRenderer = GetComponentInChildren<BoardRenderer>();
            cellRevealer = new CellRevealer();
            mineCount = firstLevelMineCount;
        }

        private void Start()
        {
            // Performans optimizasyonu: FPS'i 60'a sabitle ve VSync'i aç
            // Bu sayede oyun saniyede binlerce kare çizmeye çalışıp bilgisayarı ısıtmaz
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;

            mainCamera = Camera.main;
            NewGame();
        }

        private void OnDestroy()
        {
            GameEvents.ClearAll();
        }

        /// <summary>
        /// Yeni bir oyun başlatır. Board üretimi, kamera konumu ve çizimi koordine eder.
        /// </summary>
        public void NewGame()
        {
            grid = new GridData(width, height);
            isGameOver = false;

            BoardGenerator.GenerateCells(grid);
            BoardGenerator.GenerateMines(grid, mineCount);
            BoardGenerator.GenerateNumbers(grid);

            inputHandler = new GameInputHandler(mainCamera, boardRenderer.Tilemap);

            mainCamera.transform.position = new Vector3(width / 2f, height / 2f, CameraDepth);
            mainCamera.orthographicSize = height / 2f + 1f;
            boardRenderer.Draw(grid.GetAllCells());

            GameEvents.RaiseGameStarted();

            levelPanel.SetActive(false);

            levelTxt.text = "Level: " + level.ToString();
            minesCountTxt.text = "Mines: " + mineCount.ToString();
            Debug.Log("NewGame çalıştı, level: " + level.ToString());
        }

        private void Update()
        {
            // if (inputHandler.IsRestartPressed)
            // {
            //     NewGame();
            //     return;
            // }

            if (isGameOver)
            {
                return;
            }

            if (inputHandler.IsFlagPressed)
            {
                HandleFlag();
            }
            else if (inputHandler.IsRevealPressed)
            {
                HandleReveal();
            }
        }

        /// <summary>
        /// Sağ tık ile bayrak koyma/kaldırma işlemini koordine eder.
        /// </summary>
        private void HandleFlag()
        {
            Vector3Int cellPosition = inputHandler.GetCellPositionUnderMouse();
            CellData cell = grid.GetCell(cellPosition.x, cellPosition.y);

            if (cell.Type == CellType.Invalid || cell.Revealed)
            {
                return;
            }

            audioSource.clip = flagingClip;
            audioSource.Play();

            cell.Flagged = !cell.Flagged;
            grid.SetCell(cellPosition.x, cellPosition.y, cell);
            boardRenderer.Draw(grid.GetAllCells());
        }

        /// <summary>
        /// Sol tık ile hücre açma işlemini koordine eder.
        /// Hücre tipine göre patlama, flood-fill veya tekli açma yapar.
        /// </summary>
        private void HandleReveal()
        {
            Vector3Int cellPosition = inputHandler.GetCellPositionUnderMouse();
            CellData cell = grid.GetCell(cellPosition.x, cellPosition.y);

            if (cell.Type == CellType.Invalid || cell.Revealed || cell.Flagged)
            {
                return;
            }

            switch (cell.Type)
            {
                case CellType.Mine:
                    cellRevealer.Explode(grid, cellPosition.x, cellPosition.y);
                    isGameOver = true;
                    GameEvents.RaiseGameOver();
                    level = 1;
                    mineCount = firstLevelMineCount;
                    levelPanelTitle.text = "You failled";
                    levelPanelLabel.text = "Good luck next time!";
                    panelButtonTMP.text = "RESTART";
                    levelPanel.SetActive(true);
                    audioSource.clip = explodingClip;
                    audioSource.Play();
                    break;

                case CellType.Empty:
                    cellRevealer.Flood(grid, cellPosition.x, cellPosition.y);
                    audioSource.clip = floodingClip;
                    audioSource.Play();
                    CheckWinCondition();
                    break;

                default:
                    cellRevealer.RevealCell(grid, cellPosition.x, cellPosition.y);
                    audioSource.clip = revealingClip;
                    audioSource.Play();
                    CheckWinCondition();
                    break;
            }

            boardRenderer.Draw(grid.GetAllCells());
        }

        /// <summary>
        /// Kazanma durumunu kontrol eder.
        /// Kazanıldıysa tüm mayınları bayraklar ve event tetikler.
        /// </summary>
        private void CheckWinCondition()
        {
            if (WinConditionChecker.HasWon(grid))
            {
                isGameOver = true;
                WinConditionChecker.FlagAllMines(grid);
                GameEvents.RaiseGameWon();

                levelPanel.SetActive(true);

                levelPanelTitle.text = "You win!";
                levelPanelLabel.text = "Your looking good";
                panelButtonTMP.text = "NEXT";

                level += 1;
                if (level == 1)
                {
                    mineCount = firstLevelMineCount;
                }else if (level == 2)
                {
                   mineCount = midLevelMineCount; 
                }else if (level == 3)
                {
                    mineCount = lastLevelMineCount;
                }
                else
                {
                    levelPanelTitle.text = "You are amazing";
                    levelPanelLabel.text = "Try again for a better result";
                    panelButtonTMP.text = "TRY AGAİN";

                    level = 1;
                    mineCount = firstLevelMineCount;
                }
                Debug.Log("CheckWinCondition level:" + level.ToString());

            }
        }
    }
}
