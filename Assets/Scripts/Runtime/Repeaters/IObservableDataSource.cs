using System;

namespace Obert.UI.Runtime.Repeaters
{
    public interface IObservableDataSource<TData> : IDataSource<TData>, IObservable<TData>
    {
        
    }
}