using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeWorks
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private GameObject _unavailablePanel;
        [SerializeField] private GameObject _maxAmountReachedPanel;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private Button _purchaseButton;

        public void Init(ShopItem item)
        {
            _iconImage.sprite = item.Icon;
            _nameText.SetText(item.Name);
            _descriptionText.SetText(item.Description);
            _priceText.SetText(item.Price.ToString("N0"));
            _maxAmountReachedPanel.SetActive(false);

            UpdateAmount(item);
            _purchaseButton.onClick.AddListener(() => 
            {
                ShopManager.Instance.Purchase(item);
                UpdateAmount(item);
            });

            UpdateAvailability(item);
            PlayerManager.Instance.PointsChanged += () =>
            {
                UpdateAvailability(item);
            };
        }

        void UpdateAmount(ShopItem item)
        {
            _amountText.SetText($"{item.Amount}/{item.MaxAmount}");
            if (item.Amount >= item.MaxAmount)
            {
                _purchaseButton.interactable = false;
                _maxAmountReachedPanel.SetActive(true);
            }
        }

        void UpdateAvailability(ShopItem item)
        {
            if (item.Amount >= item.MaxAmount)
            {
                return;
            }

            if (PlayerManager.Instance.Points >= item.Price)
            {
                _unavailablePanel.SetActive(false);
                _purchaseButton.interactable = true;
            }
            else
            {
                _unavailablePanel.SetActive(true);
                _purchaseButton.interactable = false;
            }
        }
    }
}
