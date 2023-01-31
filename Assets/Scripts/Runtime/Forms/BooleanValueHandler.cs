using System;
using UnityEngine;

namespace Obert.UI.Runtime.Forms
{
    [Serializable]
    public sealed class BooleanValueHandler
    {
        [SerializeField] private string trueValue = "1";
        [SerializeField] private string falseValue = "0";
        [SerializeField] private bool nullOrWhitespaceAllowed;
        [SerializeField] private StringComparison stringComparison = StringComparison.Ordinal;
        
        public Func<string, bool> Parse =>
            value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return nullOrWhitespaceAllowed;
                if (value.Equals(trueValue, stringComparison))
                    return true;
                if (value.Equals(falseValue, stringComparison))
                    return false;
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    $"Cannot parse boolean value {value}. Allowed variants are {trueValue} for {true} and {falseValue} for {false}");
            };

        public Func<bool, string> Format =>
            value => value ? trueValue : falseValue;
    }
}