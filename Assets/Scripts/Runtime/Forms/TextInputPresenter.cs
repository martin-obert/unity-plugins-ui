using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Obert.Common.Runtime.Extensions;
using UnityEngine.Events;

namespace Obert.UI.Runtime.Forms
{
    public class TextInputPresenter : IFieldPresenter
    {
        private readonly Action _unsubscribe;
        private readonly IFieldValidator[] _validators;
        private string _fieldValue;
        private bool _isValid = true;
        private bool _isDirty = true;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public bool IsValid
        {
            get => _isValid;
            private set
            {
                if (value == _isValid) return;
                _isValid = value;
                OnPropertyChanged();
            }
        }

        public string FieldValue
        {
            get => _fieldValue;
            set
            {
                if (value == _fieldValue) return;
                _fieldValue = value;
                _isDirty = true;
                OnPropertyChanged();
                Validate();
            }
        }

        public string FieldName { get; }

        public void Validate()
        {
            if(!_isDirty) return;

            ValidationErrors = _validators.Select(x => x.Validate(_fieldValue))
                .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            IsValid = ValidationErrors.IsNullOrEmpty();

            _isDirty = false;
        }

        public string[] ValidationErrors { get; private set; }

        public TextInputPresenter(UnityEvent<string> valueProvider, string fieldName,
            params IFieldValidator[] validators)
        {
            FieldName = fieldName;
            valueProvider.AddListener(x => FieldValue = x);
            _unsubscribe = valueProvider.RemoveAllListeners;
            _validators = validators?.Where(x => x != null).ToArray() ?? Array.Empty<IFieldValidator>();
        }

        public void Dispose()
        {
            _unsubscribe?.Invoke();
        }
    }
}