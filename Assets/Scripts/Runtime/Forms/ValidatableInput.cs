using UnityEngine.UIElements;

namespace Obert.UI.Runtime.Forms
{
    public class ValidatableInput : TextField 
    {
        public new class UxmlFactory : UxmlFactory<ValidatableInput, UxmlTraits>
        {
            
        }

        public ValidatableInput()
        {
            
        }
        
        // Add the two custom UXML attributes.
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_String = new() { name = "my-string", defaultValue = "default_value" };
            UxmlIntAttributeDescription m_Int = new() { name = "my-int", defaultValue = 2 };
            
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ValidatableInput;

                ate.myString = m_String.GetValueFromBag(bag, cc);
                ate.myInt = m_Int.GetValueFromBag(bag, cc);
            }
        }
        
        // Must expose your element class to a { get; set; } property that has the same name 
        // as the name you set in your UXML attribute description with the camel case format
        public string myString { get; set; }
        public int myInt { get; set; }
    }
}