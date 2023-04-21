using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Obert.Common.Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;
using Component = UnityEngine.Component;

namespace Obert.UI.Runtime.Forms
{
    /// <summary>
    /// Form Input View
    /// </summary>
    /// <seealso cref="FormPresenter"/>
    public class FormView : FieldView
    {
        [SerializeField] private UnityEvent<bool> isFormValidChanged;

        [SerializeField] private bool validateOnInit = true;

        private IEnumerable<IFieldPresenter> _inputs;

        protected override Func<IFieldPresenter> PresenterFactory =>
            () => new FormPresenter(_inputs.ToArray(), FieldName);

        protected override void Start()
        {
            base.Start();

            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForEndOfFrame();

            // TODO: replace by GetChildrenInterfacesOfType once fixed
            _inputs = transform.GetComponentsInChildren<Component>().OfType<IInputView>()
                .Select(x => x.FieldPresenter)
                .Where(x => x != null && x != FieldPresenter);

            BindPresenter(PresenterFactory());

            if (!validateOnInit) yield break;

            FieldPresenter.Validate();
        }

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