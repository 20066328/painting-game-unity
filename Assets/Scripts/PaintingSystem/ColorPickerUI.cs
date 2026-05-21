using UnityEngine;
using UnityEngine.UI;

namespace PaintingGame
{
    public class ColorPickerUI : MonoBehaviour
    {
        [SerializeField] private PaintManager paintManager;
        [SerializeField] private Color[] colorPalette = new Color[]
        {
            Color.black,
            Color.white,
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Color.magenta,
            new Color(1f, 0.5f, 0f), // Orange
            new Color(0.5f, 0f, 0.5f) // Purple
        };

        private Image currentColorDisplay;
        private Color currentColor = Color.black;

        private void Start()
        {
            CreateColorButtons();
            SetColor(Color.black);
        }

        private void CreateColorButtons()
        {
            Transform parent = transform.Find("ColorPanel");
            if (parent == null) return;

            for (int i = 0; i < colorPalette.Length; i++)
            {
                GameObject buttonObj = new GameObject($"ColorButton_{i}");
                buttonObj.transform.SetParent(parent);
                RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(50, 50);

                Image buttonImage = buttonObj.AddComponent<Image>();
                buttonImage.color = colorPalette[i];

                Button button = buttonObj.AddComponent<Button>();
                Color colorAtIndex = colorPalette[i];
                button.onClick.AddListener(() => SetColor(colorAtIndex));
            }
        }

        public void SetColor(Color color)
        {
            currentColor = color;
            paintManager.SetColor(color);

            Image display = transform.Find("CurrentColorDisplay")?.GetComponent<Image>();
            if (display != null)
            {
                display.color = color;
            }
        }

        public Color GetCurrentColor()
        {
            return currentColor;
        }
    }
}
