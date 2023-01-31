using System;

namespace Obert.UI.Runtime.Forms
{
    public interface IFieldValidator
    {
        string Validate(string value);
    }

    public sealed class FieldIsRequiredValidator : IFieldValidator
    {
        private readonly Func<string> _messageFactory;

        public FieldIsRequiredValidator(Func<string> messageFactory)
        {
            _messageFactory = messageFactory;
        }
        
        public string Validate(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? _messageFactory.Invoke() : null;
        }
    }
}