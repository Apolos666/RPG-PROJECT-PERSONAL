using UnityEngine;

namespace RPG.Core
{
    public class ActionSchedular : MonoBehaviour
    {
        private IAction _currentAction;

        public void StartAction(IAction action)
        {
            if (_currentAction == action) return;

            _currentAction?.Cancel();

            _currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}

