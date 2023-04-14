using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Obert.Common.Runtime.Extensions;
using Obert.UI.Runtime.Serializers;

namespace Obert.UI.Runtime.Forms
{
    public class FormPresenter : IFieldPresenter
    {
        private readonly IFieldPresenter[] _fields;
        private bool _isValid = true;
        private string[] _validationErrors;
        private string _value;

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


        public FormPresenter(IFieldPresenter[] inputs, string fieldName)
        {
            inputs.ThrowIfEmptyOrNull();

            _fields = inputs;
            FieldName = fieldName;

            foreach (var unityInput in inputs)
            {
                unityInput.PropertyChanged += UnityInputOnPropertyChanged;
            }
        }

        private void UnityInputOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IFieldPresenter.IsValid):
                    Validate();
                    break;
            }
        }

        public void Validate()
        {
            foreach (var field in _fields)
            {
                field.Validate();
            }
            IsValid = _fields.All(x => x.IsValid);
            if (!IsValid)
            {
                ValidationErrors = _fields.SelectMany(x => x.ValidationErrors).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            }
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
            get => FormSerializer.ToJsonString(this);
            set
            {
                if (value == _value) return;

                if (string.IsNullOrWhiteSpace(value))
                {
                    Clear();
                }
                else
                {
                    FormSerializer.SetInternalFieldsValues(value, key => _fields.Where(x => x.FieldName.Equals(key)));
                }

                Validate();
                OnPropertyChanged();
            }
        }

        public void Clear()
        {
            foreach (var presenter in _fields)
            {
                presenter.FieldValue = null;
            }
            _value = null;
        }

        public Dictionary<string, string> GetFieldsAsDictionary() => _fields.ToDictionary(x => x.FieldName, x => x.FieldValue);

        public string FieldName { get; }

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

        public void Dispose()
        {
            foreach (var unityInput in _fields)
            {
                unityInput.PropertyChanged -= UnityInputOnPropertyChanged;
            }
        }
    }
}