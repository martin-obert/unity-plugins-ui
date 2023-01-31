using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Obert.UI.Runtime.Forms
{
    [CreateAssetMenu(menuName = "Forms/Field Validators/RegEx", fileName = "RegEx Validator", order = 0)]
    public class RegexValidator : ScriptableFieldValidator
    {
        [SerializeField]
        private string regExpression;

        [SerializeField] private string errorMessage = "This field has to match validation pattern";

        [SerializeField] private RegexOptions options = RegexOptions.None;

        [SerializeField] private int matchTimeoutSeconds = 2;

        protected override string GetErrorMessage() => errorMessage;

        protected override bool IsValid(string value)
        {
            return  Regex.IsMatch(value ?? string.Empty, regExpression, matchTimeout: TimeSpan.FromSeconds(matchTimeoutSeconds), options: options);
        }
    }
}