namespace TxCommand1
{
    partial class TxRobotTCPForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._robotPicker = new Tecnomatix.Engineering.Ui.TxObjEditBoxCtrl();
            this._txtTranslation = new System.Windows.Forms.TextBox();
            this._txtRotation = new System.Windows.Forms.TextBox();
            this._btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Robot:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "TCP Translation:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "TCP Rotation:";
            // 
            // _robotPicker
            // 
            this._robotPicker.KeepFaceEmphasizedWhenControlIsNotFocused = true;
            this._robotPicker.ListenToPick = true;
            this._robotPicker.Location = new System.Drawing.Point(171, 14);
            this._robotPicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._robotPicker.Name = "_robotPicker";
            this._robotPicker.Object = null;
            this._robotPicker.PickAndClear = false;
            this._robotPicker.PickLevel = Tecnomatix.Engineering.Ui.TxPickLevel.Component;
            this._robotPicker.PickOnly = false;
            this._robotPicker.ReadOnly = false;
            this._robotPicker.Size = new System.Drawing.Size(149, 20);
            this._robotPicker.TabIndex = 3;
            this._robotPicker.ValidatorType = Tecnomatix.Engineering.Ui.TxValidatorType.Robot;
            this._robotPicker.TypeInvalid += new System.EventHandler(this._robotPicker_TypeInvalid);
            this._robotPicker.TypeValid += new System.EventHandler(this._robotPicker_TypeValid);
            this._robotPicker.Picked += new Tecnomatix.Engineering.Ui.TxObjEditBoxCtrl_PickedEventHandler(this._robotPicker_Picked);
            // 
            // _txtTranslation
            // 
            this._txtTranslation.Location = new System.Drawing.Point(171, 49);
            this._txtTranslation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._txtTranslation.Name = "_txtTranslation";
            this._txtTranslation.ReadOnly = true;
            this._txtTranslation.Size = new System.Drawing.Size(151, 22);
            this._txtTranslation.TabIndex = 4;
            // 
            // _txtRotation
            // 
            this._txtRotation.Location = new System.Drawing.Point(171, 82);
            this._txtRotation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._txtRotation.Name = "_txtRotation";
            this._txtRotation.ReadOnly = true;
            this._txtRotation.Size = new System.Drawing.Size(151, 22);
            this._txtRotation.TabIndex = 5;
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(245, 128);
            this._btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(75, 23);
            this._btnClose.TabIndex = 6;
            this._btnClose.Text = "close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
            // 
            // TxRobotTCPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 208);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._txtRotation);
            this.Controls.Add(this._txtTranslation);
            this.Controls.Add(this._robotPicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TxRobotTCPForm";
            this.ShouldCloseOnDocumentUnloading = true;
            this.ShowIcon = false;
            this.Text = "Robot TCP Location";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Tecnomatix.Engineering.Ui.TxObjEditBoxCtrl _robotPicker;
        private System.Windows.Forms.TextBox _txtTranslation;
        private System.Windows.Forms.TextBox _txtRotation;
        private System.Windows.Forms.Button _btnClose;
    }
}