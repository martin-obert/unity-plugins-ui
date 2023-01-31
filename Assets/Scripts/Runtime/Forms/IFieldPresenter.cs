using System;
using System.ComponentModel;

namespace Obert.UI.Runtime.Forms
{
    public interface IFieldPresenter : INotifyPropertyChanged, IDisposable
    {
        bool IsValid { get; }
        void Validate();
        string[] ValidationErrors { get; }
        string FieldValue { get; set; }
        string FieldName { get; }
    }
}