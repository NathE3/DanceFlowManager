using System.Windows;
using System.Windows.Controls;

namespace InfoManager.Helpers
{
        public static class PasswordBoxHelper
        {
            public static readonly DependencyProperty BoundPasswordProperty =
                DependencyProperty.RegisterAttached(
                    "BoundPassword",
                    typeof(string),
                    typeof(PasswordBoxHelper),
                    new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged)
                );

            public static string GetBoundPassword(DependencyObject obj)
                => (string)obj.GetValue(BoundPasswordProperty);

            public static void SetBoundPassword(DependencyObject obj, string value)
                => obj.SetValue(BoundPasswordProperty, value);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                if (e.NewValue == null)
                {
                    passwordBox.Password = string.Empty;
                }
                else if (passwordBox.Password != (string)e.NewValue)
                {
                    passwordBox.Password = (string)e.NewValue;
                }

                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            SetBoundPassword(passwordBox, passwordBox.Password);
        }
    }
}


