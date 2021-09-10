using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace Mobile.Triggers
{
    public class ShowPasswordTriggerAction : TriggerAction<ImageButton>
    {
        public string ShowIcon { get; set; } = "ic_eye";
        public string HideIcon { get; set; } = "ic_eye_hide";
        public bool HidePassword { get; set; } = true;
        public MaterialTextFieldInputType InputType { get; set; } = MaterialTextFieldInputType.Password;
        protected override void Invoke(ImageButton sender)
        {
            sender.Source = HidePassword ? ShowIcon : HideIcon;
            InputType = HidePassword ? MaterialTextFieldInputType.Password : MaterialTextFieldInputType.Text;
            HidePassword = !HidePassword;
        }
    }
}
