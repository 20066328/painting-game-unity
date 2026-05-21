using UnityEngine;
using UnityEngine.UI;

namespace PaintingGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PaintManager paintManager;
        [SerializeField] private UndoSystem undoSystem;
        [SerializeField] private ImageManager imageManager;
        [SerializeField] private Button undoButton;
        [SerializeField] private Button newImageButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private Slider brushSizeSlider;

        private void Start()
        {
            SetupButtons();
            SetupSliders();
        }

        private void SetupButtons()
        {
            if (undoButton != null)
                undoButton.onClick.AddListener(() => UndoLastStroke());

            if (newImageButton != null)
                newImageButton.onClick.AddListener(() => imageManager.CreateNewImage());

            if (clearButton != null)
                clearButton.onClick.AddListener(() => ClearCurrentImage());
        }

        private void SetupSliders()
        {
            if (brushSizeSlider != null)
            {
                brushSizeSlider.minValue = 1f;
                brushSizeSlider.maxValue = 50f;
                brushSizeSlider.value = 5f;
                brushSizeSlider.onValueChanged.AddListener((value) => 
                {
                    paintManager.SetBrushSize(value);
                });
            }
        }

        private void UndoLastStroke()
        {
            if (undoSystem.CanUndo())
            {
                undoSystem.Undo();
            }
        }

        private void ClearCurrentImage()
        {
            paintManager.ClearTexture();
        }

        private void Update()
        {
            if (undoButton != null)
                undoButton.interactable = undoSystem.CanUndo();
        }
    }
}
