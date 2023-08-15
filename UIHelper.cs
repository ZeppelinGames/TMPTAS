using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TMPTAS
{
    public enum AnchorPreset
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        Stretch
    }
    public static class UIHelper
    {

        public static Canvas CreateCanvas()
        {
            GameObject canvasGO = new GameObject("Canvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
            canvasGO.AddComponent<CanvasGroup>();

            return canvas;
        }

        public static RectTransform CreatePanel(Canvas parentCanvas, Color color, float width = 200f, float height = 100f, AnchorPreset anchorPreset = AnchorPreset.MiddleCenter)
        {
            GameObject panelGO = new GameObject("Panel");
            RectTransform panelRect = panelGO.AddComponent<RectTransform>();
            panelRect.SetParent(parentCanvas.transform);

            SetAnchorPreset(panelRect, anchorPreset);

            panelRect.sizeDelta = new Vector2(width, height);

            Image panelImage = panelGO.AddComponent<Image>();
            panelImage.color = color;

            return panelRect;
        }

        public static Text CreateTextBox(RectTransform parentPanel, string text, Color textColour, int fontSize = 16, TextAnchor textAnchor = TextAnchor.MiddleLeft, AnchorPreset anchorPreset = AnchorPreset.MiddleCenter)
        {
            GameObject textBoxGO = new GameObject("TextBox");
            RectTransform textBoxRect = textBoxGO.AddComponent<RectTransform>();
            textBoxRect.SetParent(parentPanel.transform);

            SetAnchorPreset(textBoxRect, anchorPreset);

            textBoxRect.sizeDelta = Vector2.zero;

            Text textBoxText = textBoxGO.AddComponent<Text>();
            textBoxText.text = text;
            textBoxText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textBoxText.fontSize = fontSize;
            textBoxText.alignment = textAnchor;
            textBoxText.color = textColour;

            return textBoxText;
        }

        private static void SetAnchorPreset(RectTransform rectTransform, AnchorPreset anchorPreset)
        {
            rectTransform.anchoredPosition = Vector2.zero;

            switch (anchorPreset)
            {
                case AnchorPreset.TopLeft:
                    rectTransform.anchorMin = new Vector2(0f, 1f);
                    rectTransform.anchorMax = new Vector2(0f, 1f);
                    rectTransform.pivot = new Vector2(0f, 1f);
                    break;
                case AnchorPreset.TopCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    rectTransform.pivot = new Vector2(0.5f, 1f);
                    break;
                case AnchorPreset.TopRight:
                    rectTransform.anchorMin = new Vector2(1f, 1f);
                    rectTransform.anchorMax = new Vector2(1f, 1f);
                    rectTransform.pivot = new Vector2(1f, 1f);
                    break;
                case AnchorPreset.MiddleLeft:
                    rectTransform.anchorMin = new Vector2(0f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0f, 0.5f);
                    rectTransform.pivot = new Vector2(0f, 0.5f);
                    break;
                case AnchorPreset.MiddleCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    break;
                case AnchorPreset.MiddleRight:
                    rectTransform.anchorMin = new Vector2(1f, 0.5f);
                    rectTransform.anchorMax = new Vector2(1f, 0.5f);
                    rectTransform.pivot = new Vector2(1f, 0.5f);
                    break;
                case AnchorPreset.BottomLeft:
                    rectTransform.anchorMin = new Vector2(0f, 0f);
                    rectTransform.anchorMax = new Vector2(0f, 0f);
                    rectTransform.pivot = new Vector2(0f, 0f);
                    break;
                case AnchorPreset.BottomCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0f);
                    rectTransform.pivot = new Vector2(0.5f, 0f);
                    break;
                case AnchorPreset.BottomRight:
                    rectTransform.anchorMin = new Vector2(1f, 0f);
                    rectTransform.anchorMax = new Vector2(1f, 0f);
                    rectTransform.pivot = new Vector2(1f, 0f);
                    break;

                case AnchorPreset.Stretch:
                    rectTransform.anchorMin = new Vector2(0f, 0f);
                    rectTransform.anchorMax = new Vector2(1f, 1f);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    break;
                default:
                    Debug.LogError("Invalid anchor preset!");
                    break;
            }
        }
    }
}