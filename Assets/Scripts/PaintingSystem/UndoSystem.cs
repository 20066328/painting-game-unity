using UnityEngine;
using System.Collections.Generic;

namespace PaintingGame
{
    public class UndoSystem : MonoBehaviour
    {
        [SerializeField] private int maxUndoSteps = 20;
        private Stack<Texture2D> undoStack = new Stack<Texture2D>();
        private PaintManager paintManager;

        private void Start()
        {
            paintManager = GetComponent<PaintManager>();
        }

        public void SaveState(Texture2D currentTexture)
        {
            if (undoStack.Count >= maxUndoSteps)
            {
                // Remove oldest state
                var tempList = new List<Texture2D>(undoStack);
                tempList.RemoveAt(tempList.Count - 1);
                undoStack = new Stack<Texture2D>(tempList);
            }

            // Create a copy of current texture
            Texture2D stateCopy = new Texture2D(currentTexture.width, currentTexture.height, currentTexture.format, false);
            Graphics.CopyTexture(currentTexture, stateCopy);
            undoStack.Push(stateCopy);
        }

        public void Undo()
        {
            if (undoStack.Count <= 0) return;

            Texture2D previousState = undoStack.Pop();
            paintManager.SetTexture(previousState);
        }

        public bool CanUndo()
        {
            return undoStack.Count > 0;
        }

        public void ClearHistory()
        {
            undoStack.Clear();
        }
    }
}
