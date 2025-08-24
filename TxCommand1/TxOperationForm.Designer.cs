namespace TxCommand1
{
    partial class TxOperationForm
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
            this._operationPicker = new Tecnomatix.Engineering.Ui.TxObjEditBoxCtrl();
            this._btnClose = new System.Windows.Forms.Button();
            this._txtOperationName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._txtOperationType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Operation:";
            // 
            // _operationPicker
            // 
            this._operationPicker.KeepFaceEmphasizedWhenControlIsNotFocused = true;
            this._operationPicker.ListenToPick = true;
            this._operationPicker.Location = new System.Drawing.Point(171, 14);
            this._operationPicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._operationPicker.Name = "_operationPicker";
            this._operationPicker.Object = null;
            this._operationPicker.PickAndClear = false;
            this._operationPicker.PickLevel = Tecnomatix.Engineering.Ui.TxPickLevel.Component;
            this._operationPicker.PickOnly = false;
            this._operationPicker.ReadOnly = false;
            this._operationPicker.Size = new System.Drawing.Size(149, 20);
            this._operationPicker.TabIndex = 3;
            this._operationPicker.ValidatorType = Tecnomatix.Engineering.Ui.TxValidatorType.Operation;
            this._operationPicker.TypeInvalid += new System.EventHandler(this._operationPicker_TypeInvalid);
            this._operationPicker.TypeValid += new System.EventHandler(this._operationPicker_TypeValid);
            this._operationPicker.Picked += new Tecnomatix.Engineering.Ui.TxObjEditBoxCtrl_PickedEventHandler(this._operationPicker_Picked);
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(245, 116);
            this._btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(75, 23);
            this._btnClose.TabIndex = 6;
            this._btnClose.Text = "close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
            // 
            // _txtOperationName
            // 
            this._txtOperationName.Location = new System.Drawing.Point(171, 57);
            this._txtOperationName.Name = "_txtOperationName";
            this._txtOperationName.Size = new System.Drawing.Size(149, 22);
            this._txtOperationName.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Name";
            // 
            // _txtOperationType
            // 
            this._txtOperationType.Location = new System.Drawing.Point(171, 85);
            this._txtOperationType.Name = "_txtOperationType";
            this._txtOperationType.Size = new System.Drawing.Size(149, 22);
            this._txtOperationType.TabIndex = 9;
            this._txtOperationType.ReadOnly = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Type";
            // 
            // TxOperationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 167);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._txtOperationType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._txtOperationName);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._operationPicker);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TxOperationForm";
            this.ShouldCloseOnDocumentUnloading = true;
            this.ShowIcon = false;
            this.Text = "Path Energy Optimization";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.TextBox _txtOperationName;
        private System.Windows.Forms.TextBox _txtOperationType;

        #endregion

        private System.Windows.Forms.Label label1;
        private Tecnomatix.Engineering.Ui.TxObjEditBoxCtrl _operationPicker;
        private System.Windows.Forms.Button _btnClose;
    }
}