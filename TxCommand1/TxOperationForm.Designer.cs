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
            this._btnOptimize = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txNumericEditBoxCtrl1 = new Tecnomatix.Engineering.Ui.TxNumericEditBoxCtrl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Operation:";
            // 
            // _operationPicker
            // 
            this._operationPicker.KeepFaceEmphasizedWhenControlIsNotFocused = true;
            this._operationPicker.ListenToPick = true;
            this._operationPicker.Location = new System.Drawing.Point(147, 14);
            this._operationPicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._operationPicker.Name = "_operationPicker";
            this._operationPicker.Object = null;
            this._operationPicker.PickAndClear = false;
            this._operationPicker.PickLevel = Tecnomatix.Engineering.Ui.TxPickLevel.Component;
            this._operationPicker.PickOnly = false;
            this._operationPicker.ReadOnly = false;
            this._operationPicker.Size = new System.Drawing.Size(173, 20);
            this._operationPicker.TabIndex = 3;
            this._operationPicker.ValidatorType = Tecnomatix.Engineering.Ui.TxValidatorType.Operation;
            this._operationPicker.TypeInvalid += new System.EventHandler(this._operationPicker_TypeInvalid);
            this._operationPicker.TypeValid += new System.EventHandler(this._operationPicker_TypeValid);
            this._operationPicker.Picked += new Tecnomatix.Engineering.Ui.TxObjEditBoxCtrl_PickedEventHandler(this._operationPicker_Picked);
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(245, 108);
            this._btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(75, 23);
            this._btnClose.TabIndex = 6;
            this._btnClose.Text = "close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
            // 
            // _btnOptimize
            // 
            this._btnOptimize.Location = new System.Drawing.Point(147, 108);
            this._btnOptimize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._btnOptimize.Name = "_btnOptimize";
            this._btnOptimize.Size = new System.Drawing.Size(75, 23);
            this._btnOptimize.TabIndex = 7;
            this._btnOptimize.Text = "optimize";
            this._btnOptimize.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Target duration:";
            // 
            // txNumericEditBoxCtrl1
            // 
            this.txNumericEditBoxCtrl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txNumericEditBoxCtrl1.Location = new System.Drawing.Point(147, 51);
            this.txNumericEditBoxCtrl1.LowerBound = -7.92281525142643E+18D;
            this.txNumericEditBoxCtrl1.Name = "txNumericEditBoxCtrl1";
            this.txNumericEditBoxCtrl1.ReadOnly = false;
            this.txNumericEditBoxCtrl1.Size = new System.Drawing.Size(173, 22);
            this.txNumericEditBoxCtrl1.StepSize = 1D;
            this.txNumericEditBoxCtrl1.TabIndex = 9;
            this.txNumericEditBoxCtrl1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txNumericEditBoxCtrl1.ThousandsSeparator = false;
            this.txNumericEditBoxCtrl1.UpperBound = 7.92281525142643E+18D;
            this.txNumericEditBoxCtrl1.UseDecimalPlacesFromOption = true;
            this.txNumericEditBoxCtrl1.Value = 0D;
            this.txNumericEditBoxCtrl1.ValueType = Tecnomatix.Engineering.Ui.TxNumericEditValueType.Linear;
            // 
            // TxOperationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 155);
            this.Controls.Add(this.txNumericEditBoxCtrl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._btnOptimize);
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

        private Tecnomatix.Engineering.Ui.TxNumericEditBoxCtrl txNumericEditBoxCtrl1;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Button _btnOptimize;

        #endregion

        private System.Windows.Forms.Label label1;
        private Tecnomatix.Engineering.Ui.TxObjEditBoxCtrl _operationPicker;
        private System.Windows.Forms.Button _btnClose;
    }
}