using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Obert.Common.Runtime.Logging;
using Obert.UI.Runtime.Forms;

namespace Obert.UI.Runtime.Serializers
{
    public static class FormSerializer
    {
        public static IDictionary<string, IDictionary<string, string>> ToDictionary(FormPresenter presenter)
        {
            if (presenter == null) throw new ArgumentNullException(nameof(presenter));

            return new Dictionary<string, IDictionary<string, string>>
                { { presenter.FieldName, presenter.GetFieldsAsDictionary() } };
        }

        public static string ToJsonString(FormPresenter presenter)
        {
            if (presenter == null) throw new ArgumentNullException(nameof(presenter));
            var result = ToDictionary(presenter);
            return JsonConvert.SerializeObject(result);
        }

        public static void SetInternalFieldsValues(string value, Func<string, IEnumerable<IFieldPresenter>> fieldFilter)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            try
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);

                foreach (var (key, val) in dict)
                {
                    var matchingFields = fieldFilter(key);
                    foreach (var matchingField in matchingFields)
                    {
                        matchingField.FieldValue = val;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Instance.LogException(e);
            }
        }
    }
}