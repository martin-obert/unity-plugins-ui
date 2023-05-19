using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    public sealed class RepeaterViewScriptableDataBinder : MonoBehaviour
    {
        [SerializeField] private ScriptableDataSourceBase dataSource;
        [SerializeField] private RepeaterView repeater;

        public void Bind()
        {
            repeater.BindDataSource(dataSource);
        }
    }
}