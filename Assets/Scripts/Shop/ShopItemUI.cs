using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeWorks
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private GameObject _unavailablePanel;
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
            _amountText.SetText($"x{item.Amount}");
            _purchaseButton.onClick.AddListener(() => 
            {
                ShopManager.Instance.Purchase(item);
                _amountText.SetText($"x{item.Amount}");
            });

            UpdateAvailability(item);

            PlayerManager.Instance.PointsChanged += () =>
            {
                UpdateAvailability(item);
            };
        }

        void UpdateAvailability(ShopItem item)
        {
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
