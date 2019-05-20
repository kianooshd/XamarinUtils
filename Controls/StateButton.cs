using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Controls
{
    public enum ExtendedState
    {
        Normal,
        Correct,
        Wrong
    }

    public class StateButton : Button
    {
        ExtendedState extendedState;
        public ExtendedState ExtendedState
        {
            get
            {
                return extendedState;
            }
            set
            {
                extendedState = value;
                OnPropertyChanged();
            }
        }

        string tag;
        public string Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
                OnPropertyChanged();
            }
        }

        public StateButton()
        {
            BackgroundColor = !IsEnabled ? Color.FromHex("#c4c4c4") : ExtendedState == ExtendedState.Correct ? Color.FromHex("#53b266") : ExtendedState == ExtendedState.Wrong ? Color.FromHex("#ff1b33") : Color.FromHex("#3e97c4");

            TextColor = !IsEnabled ? Color.DarkGray : Color.White;

            BorderColor = Color.Transparent;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "ExtendedState" || propertyName == "IsEnabled")
            {
                BackgroundColor = !IsEnabled ? Color.FromHex("#c4c4c4") : ExtendedState == ExtendedState.Correct ? Color.FromHex("#53b266") : ExtendedState == ExtendedState.Wrong ? Color.FromHex("#ff1b33") : Color.FromHex("#3e97c4");

                TextColor = !IsEnabled ? Color.DarkGray : Color.White;
            }
        }
    }
}
