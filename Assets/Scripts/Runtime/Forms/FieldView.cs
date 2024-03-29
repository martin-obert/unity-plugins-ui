﻿using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Obert.UI.Runtime.Forms
{
    public abstract class FieldView : MonoBehaviour, IInputView, IDisposable
    {
        [Header("Attributes")] [SerializeField]
        private string fieldName;

        [Header("Events")] [SerializeField] private UnityEvent<bool> onIsValid;
        [SerializeField] private UnityEvent<bool> onIsInvalid;
        [SerializeField] private UnityEvent<string> onValidationError;

        public IFieldPresenter FieldPresenter { get; private set; }
        protected string FieldName => fieldName ?? gameObject.name;

        protected virtual void Awake()
        {
            Assert.IsNotNull(PresenterFactory);

        }

        protected virtual void Start()
        {

        }

        protected abstract Func<IFieldPresenter> PresenterFactory { get; }

        protected void BindPresenter(IFieldPresenter fieldPresenter)
        {
            if (FieldPresenter != null) throw new Exception("Presenter is already bound");

            FieldPresenter = fieldPresenter ?? throw new ArgumentNullException(nameof(fieldPresenter));
            FieldPresenter.PropertyChanged += PresenterOnPropertyChanged;
        }

        protected virtual void PresenterOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IFieldPresenter.IsValid):
                    onIsValid?.Invoke(FieldPresenter.IsValid);
                    onIsInvalid?.Invoke(!FieldPresenter.IsValid);
                    if (!FieldPresenter.IsValid) onValidationError?.Invoke(FieldPresenter.ValidationErrors?.Last());
                    return;
            }
        }

        private void OnDestroy()
        {
            if (FieldPresenter == null) return;

            FieldPresenter.PropertyChanged -= PresenterOnPropertyChanged;
            FieldPresenter.Dispose();
        }

        public string Value
        {
            get => FieldPresenter.FieldValue;
            set => FieldPresenter.FieldValue = value;
        }

        public void Dispose()
        {
            FieldPresenter?.Dispose();
            FieldPresenter = null;
        }
    }
}