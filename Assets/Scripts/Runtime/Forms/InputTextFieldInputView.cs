using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Obert.UI.Runtime.Forms
{
    public class InputTextFieldInputView : FieldView
    {
        [Header("Attributes")] [SerializeField]
        private TMP_InputField inputField;

        [SerializeField] private bool isRequired;
        [SerializeField] private string isRequiredErrorMessage = "This field is required";
        [SerializeField] private ScriptableFieldValidator[] fieldValidators;

        private IFieldValidator[] _validators;

        protected override void Awake()
        {
            _validators = fieldValidators?.OfType<IFieldValidator>().ToArray() ?? Array.Empty<IFieldValidator>();

            if (isRequired)
            {
                _validators = _validators.Append(new FieldIsRequiredValidator(() => isRequiredErrorMessage)).ToArray();
            }
            base.Awake();
        }

        protected override Func<IFieldPresenter> PresenterFactory => () =>
            new TextFieldInputPresenter(inputField.onValueChanged, FieldName, _validators);
    }
}