using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Utils
{
    [ContentProperty(nameof(Text))]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        public string Text { get; set; }
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                return ProvideValue(serviceProvider);
            }
            catch (Exception)
            {
                return Text;
            }
        }
        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding
            {
                Path = $"[{Text}]",
                Source = LocalizationResourceManager.Instance,
            };
            return binding;
        }
    }
}
