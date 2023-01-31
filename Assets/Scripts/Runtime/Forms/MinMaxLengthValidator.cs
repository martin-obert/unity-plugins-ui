using System;
using UnityEngine;

namespace Obert.UI.Runtime.Forms
{
    [CreateAssetMenu(menuName = "Forms/Field Validators/Min Max Length", fileName = "Min Max Length Validator", order = 0)]
    public class MinMaxLengthValidator : ScriptableFieldValidator
    {
        [SerializeField, Tooltip("Set to negative value, to disable check")]
        private int minLength = -1;
        
        [SerializeField, Tooltip("Set to negative value, to disable check")]
        private int maxLength = -1;
       
        [SerializeField, Tooltip("You can use string formatting: {0} = minLength, {1} = maxLength")]
        private string errorMessage = "Value has to be between: {0} and {1} chars long";
        
        protected override string GetErrorMessage() => string.Format(errorMessage, minLength, maxLength);

        protected override bool IsValid(string value)
        {
            if (maxLength >= 0 && minLength > maxLength)
                throw new ArgumentOutOfRangeException($"Ranges doesn't match rule: max > min ({minLength} : {maxLength})");

            var valueLength = value?.Length ?? 0;
            var doMinLenCheck = minLength >= 0;
            var doMaxLenCheck = maxLength >= 0;

            if (doMinLenCheck && valueLength < minLength)
                return false;

            if (doMaxLenCheck && valueLength > maxLength)
                return false;

            return true;
        }
    }
}