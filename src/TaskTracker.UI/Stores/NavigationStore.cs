using System;
using System.Collections.Generic;
using System.Windows;
using TaskTracker.UI.ViewModels;

namespace TaskTracker.UI.Stores
{
    public class NavigationStore
    {
        private readonly Stack<ViewModelBase> _backStack = new();
        private readonly Stack<ViewModelBase> _forwardStack = new();

        public ViewModelBase CurrentViewModel { get; private set; }

        public event Action? StateChanged;

        public NavigationStore() { }

        public void Initialize(ViewModelBase initialVm)
        {
            CurrentViewModel = initialVm;
            StateChanged?.Invoke();
        }

        public void Navigate(ViewModelBase newVm)
        {
            _backStack.Push(CurrentViewModel);
            _forwardStack.Clear();
            CurrentViewModel = newVm;
            StateChanged?.Invoke();
        }

        public void GoBack()
        {
            if (!_backStack.TryPop(out var prev)) return;
            _forwardStack.Push(CurrentViewModel);
            CurrentViewModel = prev;
            StateChanged?.Invoke();
        }

        public void GoForward()
        {
            if (!_forwardStack.TryPop(out var next)) return;
            _backStack.Push(CurrentViewModel);
            CurrentViewModel = next;
            StateChanged?.Invoke();
        }

        public bool CanGoBack => _backStack.Count > 0;
        public bool CanGoForward => _forwardStack.Count > 0;
    }
}
