using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Obert.UI.Runtime.Forms
{
    public class ToggleInputFieldView : FieldView
    {
        [SerializeField] private BooleanValueHandler handler;
        [SerializeField] private Toggle toggle;
        [SerializeField] private bool validValue;
        [SerializeField] private string validationErrorMessage = "Invalid state";

        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(toggle, "toggle != null");
        }

        protected override void Start()
        {
            base.Start();
            BindPresenter(PresenterFactory());
        }

        protected override Func<IFieldPresenter> PresenterFactory => () => new TogglePresenter(validValue,
            toggle.onValueChanged, validationErrorMessage, FieldName,
            handler.Parse, handler.Format);
    }
}