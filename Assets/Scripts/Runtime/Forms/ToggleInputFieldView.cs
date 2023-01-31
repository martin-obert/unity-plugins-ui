using System;
using UnityEngine;
using UnityEngine.UI;

namespace Obert.UI.Runtime.Forms
{
    public class ToggleInputFieldView : FieldView
    {
        [SerializeField] private BooleanValueHandler handler;
        [SerializeField] private Toggle toggle;
        [SerializeField] private bool validValue;
        [SerializeField] private string validationErrorMessage = "Invalid state";

        protected override Func<IFieldPresenter> PresenterFactory => () => new TogglePresenter(validValue,
            toggle.onValueChanged, validationErrorMessage, FieldName,
            handler.Parse, handler.Format);
    }
}