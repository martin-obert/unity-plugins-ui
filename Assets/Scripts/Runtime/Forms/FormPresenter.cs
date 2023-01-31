using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Obert.Common.Runtime.Extensions;
using UnityEngine;

namespace Obert.UI.Runtime.Forms
{
    public class FormPresenter : IFieldPresenter
    {
        private readonly IFieldPresenter[] _fields;
        private bool _isValid = true;

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
            IsValid = _fields.All(x => x.IsValid);
        }

        public string[] ValidationErrors { get; private set; }

        public string FieldValue
        {
            get
            {
                var result = new Dictionary<string, Dictionary<string, string>>
                    { { FieldName, ToDictionary() } };
                return JsonConvert.SerializeObject(result);
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    foreach (var field in _fields)
                    {
                        field.FieldValue = null;
                    }
                    return;
                }

                try
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
                    foreach (var (key, val) in dict)
                    {
                        var matchingFields = _fields.Where(x => x.FieldName.Equals(key)).ToArray();
                        foreach (var matchingField in matchingFields)
                        {
                            matchingField.FieldValue = val;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public Dictionary<string, string> ToDictionary() => _fields.ToDictionary(x => x.FieldName, x => x.FieldValue);

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