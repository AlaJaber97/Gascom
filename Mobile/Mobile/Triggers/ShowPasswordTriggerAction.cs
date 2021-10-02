using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace Mobile.Triggers
{
    public class ShowPasswordTriggerAction : TriggerAction<ImageButton>, INotifyPropertyChanged
    {
        public string ShowIcon { get; set; } = "ic_eye";
        public string HideIcon { get; set; } = "ic_eye_hide";
        public bool HidePassword { get; set; } = false;
        public MaterialTextFieldInputType InputType { get; set; } = MaterialTextFieldInputType.Password;

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void Invoke(ImageButton sender)
        {
            sender.Source = !HidePassword ? ShowIcon : HideIcon;
            InputType = HidePassword ? MaterialTextFieldInputType.Password : MaterialTextFieldInputType.Text;
            HidePassword = !HidePassword;
            OnPropertyChanged(nameof(InputType));
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
