using UnityEngine;

namespace Obert.UI.Runtime.Forms
{
    public abstract class ScriptableFieldValidator : ScriptableObject, IFieldValidator
    {
        public string Validate(string value) => IsValid(value) ? null : GetErrorMessage();

        protected abstract bool IsValid(string value);

        protected virtual string GetErrorMessage() => "This field has validation error";
    }
}