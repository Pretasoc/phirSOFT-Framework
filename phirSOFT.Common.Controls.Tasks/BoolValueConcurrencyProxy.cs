using System;
using JetBrains.Annotations;

namespace phirSOFT.Common.Controls.Tasks
{
    [PublicAPI]
    internal sealed class BoolValueConcurrencyProxy
    {
        private bool _application;
        private bool _user;

        public BoolValueConcurrencyProxy(bool user, bool application = false)
        {
            _user = user;
            _application = application;
        }

        public bool User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnValueChanged();
            }
        }

        public bool Application
        {
            get { return _application; }
            set
            {
                _application = value;
                OnValueChanged();
            }
        }

        public bool Value => User & Application;

        public event EventHandler ValueChanged;

        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}