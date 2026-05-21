using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PaintingGame
{
    public class ImageManager : MonoBehaviour
    {
        [SerializeField] private PaintManager paintManager;
        [SerializeField] private UndoSystem undoSystem;
        [SerializeField] private Transform imageListContainer;
        
        private Dictionary<int, Texture2D> images = new Dictionary<int, Texture2D>();
        private int currentImageId = 0;
        private int nextImageId = 1;

        private void Start()
        {
            CreateNewImage();
        }

        public void CreateNewImage()
        {
            Texture2D newTexture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
            newTexture.filterMode = FilterMode.Point;
            
            Color[] clearPixels = new Color[newTexture.width * newTexture.height];
            for (int i = 0; i < clearPixels.Length; i++)
            {
                clearPixels[i] = Color.white;
            }
            newTexture.SetPixels(clearPixels);
            newTexture.Apply();

            int imageId = nextImageId++;
            images[imageId] = newTexture;

            AddImageButton(imageId);
            SelectImage(imageId);
        }

        public void SelectImage(int imageId)
        {
            if (!images.ContainsKey(imageId)) return;

            currentImageId = imageId;
            paintManager.SetTexture(images[imageId]);
            undoSystem.ClearHistory();

            UpdateButtonStates();
        }

        private void AddImageButton(int imageId)
        {
            GameObject buttonObj = new GameObject($"Image_{imageId}");
            buttonObj.transform.SetParent(imageListContainer);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(80, 80);

            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = Color.white;

            Button button = buttonObj.AddComponent<Button>();
            int id = imageId;
            button.onClick.AddListener(() => SelectImage(id));

            Text buttonText = new GameObject("Text").AddComponent<Text>();
            buttonText.transform.SetParent(buttonObj.transform);
            buttonText.text = $"Img {imageId}";
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            buttonText.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }

        private void UpdateButtonStates()
        {
            foreach (Transform child in imageListContainer)
            {
                Button button = child.GetComponent<Button>();
                if (button != null)
                {
                    ColorBlock colors = button.colors;
                    if (child.name == $"Image_{currentImageId}")
                    {
                        colors.normalColor = Color.green;
                    }
                    else
                    {
                        colors.normalColor = Color.white;
                    }
                    button.colors = colors;
                }
            }
        }

        public int GetCurrentImageId()
        {
            return currentImageId;
        }

        public Dictionary<int, Texture2D> GetAllImages()
        {
            return images;
        }
    }
}
