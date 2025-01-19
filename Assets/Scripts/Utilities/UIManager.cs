using UnityEngine;

namespace SnakeWorks
{
    public static class UIManager
    {
        private static GameObject _currentScreen;

        public static void OpenScreen(GameObject screen)
        {
            if (screen == null || _currentScreen == screen)
            {
                return;
            }

            if (_currentScreen != null)
            {
                _currentScreen.SetActive(false);
            }
            _currentScreen = screen;
            _currentScreen.SetActive(true);
        }
    }
}