using System;
using UnityEngine;
using UnityEngine.UI;

namespace ErrorPopup
{
    public class ErrorPopupComponent : MonoBehaviour
    {
        public GameObject errorViewPrefab;

        public ErrorPosition errorPosition;

        public float distanceFromCenterPercent = 100f;

        private RectTransform OriginalContainerTransform;

        private GameObject errorView;

        public void ShowError(string errorText, bool vibrate = false)
        {
            if (errorView == null)
            {
                CreateErrorWindow();
                Button componentInChildren = errorView.GetComponentInChildren<Button>();
                componentInChildren.onClick.AddListener(delegate
                {
                    HideError();
                });
            }
            else
            {
                errorView.SetActive(value: true);
            }
            errorView.GetComponent<RectTransform>().SetParent(OriginalContainerTransform, worldPositionStays: false);
            SetErrorWindowPosition();
            SetErrorArrow();
            ReparentErrorViewToRootRectTransform();
            Text componentInChildren2 = errorView.GetComponentInChildren<Text>();
            componentInChildren2.text = errorText;

#if UNITY_ANDROID
            if (vibrate)
            {
                Handheld.Vibrate();
            }
#else
            if (vibrate)
            {
                Debug.Log("Vibration is only supported on Android. Skipping vibration.");
            }
#endif
        }

        public void HideError()
        {
            if (errorView != null)
            {
                errorView.SetActive(value: false);
            }
        }

        private void CreateErrorWindow()
        {
            errorView = (UnityEngine.Object.Instantiate(errorViewPrefab, base.gameObject.transform.position, base.gameObject.transform.rotation) as GameObject);
            if (errorView.GetComponentInChildren<Text>() == null)
            {
                throw new Exception("ErrorViewPrefab must have a Text component.");
            }
            OriginalContainerTransform = base.gameObject.GetComponent<RectTransform>();
        }

        private void SetErrorWindowPosition()
        {
            RectTransform component = errorView.GetComponent<RectTransform>();
            RectTransform rectTransform = (RectTransform)base.gameObject.transform;
            Vector2 zero = Vector2.zero;
            float num = distanceFromCenterPercent / 100f;
            switch (errorPosition)
            {
                case ErrorPosition.DOWN:
                    zero.y = (0f - rectTransform.rect.height) * num / 2f - component.rect.height / 2f;
                    zero.x = 0f;
                    break;
                case ErrorPosition.RIGHT:
                    zero.x = rectTransform.rect.width * num / 2f + component.rect.width / 2f;
                    zero.y = 0f;
                    break;
                case ErrorPosition.LEFT:
                    zero.x = (0f - rectTransform.rect.width) * num / 2f - component.rect.width / 2f;
                    zero.y = 0f;
                    break;
                case ErrorPosition.UP:
                    zero.y = rectTransform.rect.height * num / 2f + component.rect.height / 2f;
                    zero.x = 0f;
                    break;
            }
            component.anchoredPosition = zero;
        }

        private void ReparentErrorViewToRootRectTransform()
        {
            Vector3 localScale = errorView.GetComponent<RectTransform>().localScale;
            Vector3 localScale2 = new Vector3(localScale.x, localScale.y, localScale.z);
            Transform transform = base.transform;
            while (transform != null && transform.parent != null && transform.parent.GetComponent<RectTransform>() != null)
            {
                transform = transform.parent;
            }
            errorView.GetComponent<RectTransform>().SetParent(transform, worldPositionStays: true);
            errorView.GetComponent<RectTransform>().localScale = localScale2;
        }

        private void SetErrorArrow()
        {
            SetErrorArrow component = errorView.GetComponent<SetErrorArrow>();
            if (component != null)
            {
                errorView.GetComponent<SetErrorArrow>().SetArrowByDirection(errorPosition);
                return;
            }
            throw new Exception("The error prefab is missing a SetErrorArrow script component.");
        }

        private void OnDestroy()
        {
            UnityEngine.Object.Destroy(errorView);
        }
    }
}
