namespace Shrinkr.Client.Desktop.WinForms
{
    partial class ShrinkrForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.grpBxResponseFormat = new System.Windows.Forms.GroupBox();
            this.grpBxOptions = new System.Windows.Forms.GroupBox();
            this.lblLongUrl = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnShrink = new System.Windows.Forms.Button();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.rdoText = new System.Windows.Forms.RadioButton();
            this.rdoXml = new System.Windows.Forms.RadioButton();
            this.rdoJson = new System.Windows.Forms.RadioButton();
            this.grpBxResponseFormat.SuspendLayout();
            this.grpBxOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(68, 47);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(263, 20);
            this.txtApiKey.TabIndex = 0;
            // 
            // lblApiKey
            // 
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Location = new System.Drawing.Point(17, 50);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(45, 13);
            this.lblApiKey.TabIndex = 1;
            this.lblApiKey.Text = "API Key";
            // 
            // grpBxResponseFormat
            // 
            this.grpBxResponseFormat.Controls.Add(this.rdoJson);
            this.grpBxResponseFormat.Controls.Add(this.rdoXml);
            this.grpBxResponseFormat.Controls.Add(this.rdoText);
            this.grpBxResponseFormat.Location = new System.Drawing.Point(9, 73);
            this.grpBxResponseFormat.Name = "grpBxResponseFormat";
            this.grpBxResponseFormat.Size = new System.Drawing.Size(322, 54);
            this.grpBxResponseFormat.TabIndex = 2;
            this.grpBxResponseFormat.TabStop = false;
            this.grpBxResponseFormat.Text = "Response Format";
            // 
            // grpBxOptions
            // 
            this.grpBxOptions.Controls.Add(this.btnShrink);
            this.grpBxOptions.Controls.Add(this.txtUrl);
            this.grpBxOptions.Controls.Add(this.lblLongUrl);
            this.grpBxOptions.Controls.Add(this.grpBxResponseFormat);
            this.grpBxOptions.Controls.Add(this.lblApiKey);
            this.grpBxOptions.Controls.Add(this.txtApiKey);
            this.grpBxOptions.Location = new System.Drawing.Point(12, 12);
            this.grpBxOptions.Name = "grpBxOptions";
            this.grpBxOptions.Size = new System.Drawing.Size(596, 168);
            this.grpBxOptions.TabIndex = 0;
            this.grpBxOptions.TabStop = false;
            this.grpBxOptions.Text = "Shrinkr API Parameters";
            // 
            // lblLongUrl
            // 
            this.lblLongUrl.AutoSize = true;
            this.lblLongUrl.Location = new System.Drawing.Point(6, 20);
            this.lblLongUrl.Name = "lblLongUrl";
            this.lblLongUrl.Size = new System.Drawing.Size(56, 13);
            this.lblLongUrl.TabIndex = 3;
            this.lblLongUrl.Text = "Long URL";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(68, 17);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(522, 20);
            this.txtUrl.TabIndex = 4;
            // 
            // btnShrink
            // 
            this.btnShrink.Location = new System.Drawing.Point(515, 135);
            this.btnShrink.Name = "btnShrink";
            this.btnShrink.Size = new System.Drawing.Size(75, 23);
            this.btnShrink.TabIndex = 5;
            this.btnShrink.Text = "Shrink";
            this.btnShrink.UseVisualStyleBackColor = true;
            this.btnShrink.Click += new System.EventHandler(this.btnShrink_Click);
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(12, 206);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ReadOnly = true;
            this.txtResponse.Size = new System.Drawing.Size(596, 122);
            this.txtResponse.TabIndex = 1;
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(9, 190);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(80, 13);
            this.lblResponse.TabIndex = 2;
            this.lblResponse.Text = "Raw Response";
            // 
            // rdoText
            // 
            this.rdoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.rdoText.AutoSize = true;
            this.rdoText.Checked = true;
            this.rdoText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdoText.Location = new System.Drawing.Point(7, 20);
            this.rdoText.Name = "rdoText";
            this.rdoText.Size = new System.Drawing.Size(52, 18);
            this.rdoText.TabIndex = 0;
            this.rdoText.TabStop = true;
            this.rdoText.Text = "Text";
            this.rdoText.UseVisualStyleBackColor = true;
            // 
            // rdoXml
            // 
            this.rdoXml.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.rdoXml.AutoSize = true;
            this.rdoXml.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdoXml.Location = new System.Drawing.Point(72, 20);
            this.rdoXml.Name = "rdoXml";
            this.rdoXml.Size = new System.Drawing.Size(53, 18);
            this.rdoXml.TabIndex = 1;
            this.rdoXml.Text = "XML";
            this.rdoXml.UseVisualStyleBackColor = true;
            // 
            // rdoJson
            // 
            this.rdoJson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.rdoJson.AutoSize = true;
            this.rdoJson.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdoJson.Location = new System.Drawing.Point(138, 20);
            this.rdoJson.Name = "rdoJson";
            this.rdoJson.Size = new System.Drawing.Size(59, 18);
            this.rdoJson.TabIndex = 2;
            this.rdoJson.Text = "JSON";
            this.rdoJson.UseVisualStyleBackColor = true;
            // 
            // ShrinkrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 367);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.grpBxOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ShrinkrForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shrinkr Client";
            this.grpBxResponseFormat.ResumeLayout(false);
            this.grpBxResponseFormat.PerformLayout();
            this.grpBxOptions.ResumeLayout(false);
            this.grpBxOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblApiKey;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.GroupBox grpBxResponseFormat;
        private System.Windows.Forms.GroupBox grpBxOptions;
        private System.Windows.Forms.Label lblLongUrl;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnShrink;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.RadioButton rdoJson;
        private System.Windows.Forms.RadioButton rdoXml;
        private System.Windows.Forms.RadioButton rdoText;
    }
}

