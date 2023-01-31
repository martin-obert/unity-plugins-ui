using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Obert.Common.Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Obert.UI.Runtime.Forms
{
    public class InputFormView : FieldView
    {
        [SerializeField] private UnityEvent<bool> isFormValidChanged;

        [SerializeField] private bool validateOnInit = true;

        private IEnumerable<IFieldPresenter> _inputs;

        protected override void Awake()
        {
            // base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            
            _inputs = this.GetChildrenInterfacesOfType<IInputView>()
                .Select(x => x?.FieldPresenter)
                .Where(x => x != null && x != FieldPresenter);
            
            BindPresenter(PresenterFactory());
            
            if (validateOnInit)
            {
                foreach (var fieldPresenter in _inputs)
                {
                    fieldPresenter.Validate();
                }

                FieldPresenter.Validate();
            }
        }

        protected override Func<IFieldPresenter> PresenterFactory =>
            () => new FormPresenter(_inputs.ToArray(), FieldName);

        protected override void PresenterOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(FormPresenter.IsValid):
                    isFormValidChanged?.Invoke(FieldPresenter.IsValid);
                    return;
            }
        }
    }
}