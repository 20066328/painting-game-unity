using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PaintingGame
{
    public class PaintManager : MonoBehaviour
    {
        [SerializeField] private RectTransform canvasRect;
        [SerializeField] private Image paintingImage;
        [SerializeField] private float brushSize = 5f;
        [SerializeField] private Color defaultColor = Color.black;
        
        private Texture2D paintingTexture;
        private bool isDrawing = false;
        private Vector2 lastMousePos;
        private UndoSystem undoSystem;
        private Color currentColor;

        private void Start()
        {
            undoSystem = GetComponent<UndoSystem>();
            currentColor = defaultColor;
            InitializeTexture();
        }

        private void InitializeTexture()
        {
            if (paintingImage == null) return;
            
            paintingTexture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
            paintingTexture.filterMode = FilterMode.Point;
            ClearTexture();
            paintingImage.sprite = Sprite.Create(paintingTexture, 
                new Rect(0, 0, paintingTexture.width, paintingTexture.height), 
                Vector2.one * 0.5f);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RectTransformUtility.RectTransformPointToLocalPointInRectangle(
                    canvasRect, Input.mousePosition, null, out Vector2 localPoint))
                {
                    isDrawing = true;
                    undoSystem.SaveState(paintingTexture);
                    lastMousePos = localPoint;
                }
            }

            if (Input.GetMouseButton(0) && isDrawing)
            {
                if (RectTransformUtility.RectTransformPointToLocalPointInRectangle(
                    canvasRect, Input.mousePosition, null, out Vector2 localPoint))
                {
                    DrawLine(lastMousePos, localPoint, currentColor);
                    lastMousePos = localPoint;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDrawing = false;
            }
        }

        private void DrawLine(Vector2 from, Vector2 to, Color color)
        {
            int x0 = (int)from.x;
            int y0 = (int)from.y;
            int x1 = (int)to.x;
            int y1 = (int)to.y;

            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                DrawPoint(x0, y0, color);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
            paintingTexture.Apply();
        }

        private void DrawPoint(int x, int y, Color color)
        {
            int radius = (int)brushSize;
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        int px = x + dx;
                        int py = y + dy;
                        if (px >= 0 && px < paintingTexture.width && py >= 0 && py < paintingTexture.height)
                        {
                            paintingTexture.SetPixel(px, py, color);
                        }
                    }
                }
            }
        }

        public void SetColor(Color color)
        {
            currentColor = color;
        }

        public void SetBrushSize(float size)
        {
            brushSize = Mathf.Max(1, size);
        }

        public void ClearTexture()
        {
            if (paintingTexture == null) return;
            Color[] clearPixels = new Color[paintingTexture.width * paintingTexture.height];
            for (int i = 0; i < clearPixels.Length; i++)
            {
                clearPixels[i] = Color.white;
            }
            paintingTexture.SetPixels(clearPixels);
            paintingTexture.Apply();
        }

        public Texture2D GetCurrentTexture()
        {
            return paintingTexture;
        }

        public void SetTexture(Texture2D texture)
        {
            paintingTexture = texture;
            paintingImage.sprite = Sprite.Create(paintingTexture,
                new Rect(0, 0, paintingTexture.width, paintingTexture.height),
                Vector2.one * 0.5f);
        }
    }
}
