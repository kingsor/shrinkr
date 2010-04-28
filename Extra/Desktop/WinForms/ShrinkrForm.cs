using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Shrinkr.Client.Desktop.WinForms
{
    public partial class ShrinkrForm : Form
    {
        public ShrinkrForm()
        {
            InitializeComponent();
        }

        private string ApiKey
        {
            get
            {
                return txtApiKey.Text;
            }
        }

        private string LongUrl
        {
            get
            {
                return txtUrl.Text;
            }
        }

        private string Format
        {
            get
            {
                return grpBxResponseFormat.Controls.OfType<RadioButton>().Single(r => r.Checked).Text.ToLower();
            }
        }

        private string ValidateDataEntry()
        {
            string errorMsg = String.Empty;

            if (String.IsNullOrWhiteSpace(LongUrl))
            {
                errorMsg += @"Please enter URL to shrink."+Environment.NewLine;
            }

            if(String.IsNullOrWhiteSpace(ApiKey))
            {
                errorMsg += @"Please enter API Key.";
            }
            
            return errorMsg;
        }

        private void ShrinkIt()
        {
            btnShrink.Enabled = false;
            string rdirApiUrl = String.Format("http://rdir.in/api?url={0}&apikey={1}&format={2}", LongUrl, ApiKey, Format);

            WebRequest request = WebRequest.Create(new Uri(rdirApiUrl));
            request.BeginGetResponse(asyncState =>
                                         {
                                             WebResponse response = request.EndGetResponse(asyncState);
                                             
                                             using (var sr = new StreamReader(response.GetResponseStream()))
                                             {
                                                 txtResponse.Text = sr.ReadToEnd();
                                             }
                                             btnShrink.Enabled = true;
                                         }, null);
        }

        private void btnShrink_Click(object sender, EventArgs e)
        {
            string validationMsg = ValidateDataEntry();

            if(!String.IsNullOrEmpty(validationMsg))
            {
                MessageBox.Show(validationMsg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                ShrinkIt();
            }
        }
    }
}
