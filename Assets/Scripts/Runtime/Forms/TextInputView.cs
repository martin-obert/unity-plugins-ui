using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Obert.UI.Runtime.Forms
{
    public class TextInputView : FieldView
    {
        [Header("Attributes")] [SerializeField]
        private TMP_InputField inputField;

        [SerializeField] private bool isRequired;
        [SerializeField] private string isRequiredErrorMessage = "This field is required";
        [SerializeField] private ScriptableFieldValidator[] fieldValidators;

        private IFieldValidator[] _validators;

        protected override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(inputField, "inputField != null");

        }

        protected override void Start()
        {
            _validators = fieldValidators?.OfType<IFieldValidator>().ToArray() ?? Array.Empty<IFieldValidator>();
            if (isRequired)
            {
                _validators = _validators.Append(new FieldIsRequiredValidator(() => isRequiredErrorMessage)).ToArray();
            }

            BindPresenter(PresenterFactory());
        }

        protected override Func<IFieldPresenter> PresenterFactory => () =>
            new TextInputPresenter(inputField.onValueChanged, FieldName, _validators);
    }
}