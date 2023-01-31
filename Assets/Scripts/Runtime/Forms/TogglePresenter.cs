using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Obert.Common.Runtime.Extensions;
using UnityEngine.Events;

namespace Obert.UI.Runtime.Forms
{
    public class TogglePresenter : IFieldPresenter
    {
        private class Validator : IFieldValidator
        {
            private readonly bool _validValue;
            private readonly Func<string, bool> _parser;
            private readonly string _errorMessage;
            public Validator(bool validValue, string errorMessage, Func<string, bool> parser)
            {
                _errorMessage = errorMessage;
                _validValue = validValue;
                _parser = parser;
            }
            public string Validate(string value)
            {
                var boolValue = _parser(value);
                return boolValue == _validValue ? null : _errorMessage;
            }
        }
        private readonly IFieldValidator[] _validators;
        private readonly IDisposable _subscription;
        private readonly bool _validValue;
        private readonly Func<bool, string> _formatter;
        private string _fieldValue;
        private bool _isValid = true;
        private string[] _validationErrors;
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
       
        public TogglePresenter(bool validValue, UnityEvent<bool> valueChanges, string validationErrorMessage, string fieldName,
            Func<string, bool> parser = null, Func<bool, string> formatter = null)
        {
            _validators = new IFieldValidator[] { new Validator(validValue, validationErrorMessage, parser) };
            _formatter = formatter;
            FieldName = fieldName;
            _subscription = valueChanges.Subscribe(currentValue =>
            {
                FieldValue = _formatter(currentValue);
                Validate();
            });
        }

        public void Dispose()
        {
            _subscription?.Dispose();
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

        public void Validate()
        {
            ValidationErrors = _validators.Select(x => x.Validate(FieldValue)).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            IsValid = ValidationErrors.IsNullOrEmpty();
        }

        public string[] ValidationErrors
        {
            get => _validationErrors;
            private set
            {
                if (Equals(value, _validationErrors)) return;
                _validationErrors = value;
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
                OnPropertyChanged();
            }
        }

        public string FieldName { get; }
    }
}