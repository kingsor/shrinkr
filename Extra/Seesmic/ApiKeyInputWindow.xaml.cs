namespace Shrinkr.Client.Seemic
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public partial class ApiKeyInputWindow : ChildWindow
    {
        public string ApiKey
        {
            get { return txtApiKey.Text; }
        }

        public ApiKeyInputWindow()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler ApiKeyReceived;

        private void RaiseApiKeyReceived()
        {
            if (ApiKeyReceived != null)
            {
                ApiKeyReceived(this, new RoutedEventArgs());
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Guid apiKey;
            if (Guid.TryParse(txtApiKey.Text, out apiKey))
            {
                DialogResult = true;
                RaiseApiKeyReceived();
            }
            else
            {
                lblErrorPrompt.Visibility = Visibility.Visible;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

