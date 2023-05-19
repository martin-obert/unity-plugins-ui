using System;

namespace Obert.UI.Runtime.Repeaters
{
    public interface IObservableReadOnlyDataSource<TData> : IReadOnlyDataSource<TData>, IObservable<TData>
    {
        
    }
}