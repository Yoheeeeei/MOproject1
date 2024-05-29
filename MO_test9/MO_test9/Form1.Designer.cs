namespace MO_test9
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_nonmedia = new System.Windows.Forms.Button();
            this.button_mediameasure = new System.Windows.Forms.Button();
            this.textBox_Hmax = new System.Windows.Forms.TextBox();
            this.textBox_dH = new System.Windows.Forms.TextBox();
            this.textBox_dtheta = new System.Windows.Forms.TextBox();
            this.Hmax = new System.Windows.Forms.Label();
            this.dH = new System.Windows.Forms.Label();
            this.dtheta = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_nonmedia
            // 
            this.button_nonmedia.Location = new System.Drawing.Point(594, 285);
            this.button_nonmedia.Name = "button_nonmedia";
            this.button_nonmedia.Size = new System.Drawing.Size(167, 69);
            this.button_nonmedia.TabIndex = 0;
            this.button_nonmedia.Text = "Nonmedia-measure";
            this.button_nonmedia.UseVisualStyleBackColor = true;
            this.button_nonmedia.Click += new System.EventHandler(this.button_nonmedia_Click);
            // 
            // button_mediameasure
            // 
            this.button_mediameasure.Location = new System.Drawing.Point(594, 113);
            this.button_mediameasure.Name = "button_mediameasure";
            this.button_mediameasure.Size = new System.Drawing.Size(167, 66);
            this.button_mediameasure.TabIndex = 1;
            this.button_mediameasure.Text = "media-measure";
            this.button_mediameasure.UseVisualStyleBackColor = true;
            this.button_mediameasure.Click += new System.EventHandler(this.button_mediameasure_Click);
            // 
            // textBox_Hmax
            // 
            this.textBox_Hmax.Location = new System.Drawing.Point(242, 66);
            this.textBox_Hmax.Name = "textBox_Hmax";
            this.textBox_Hmax.Size = new System.Drawing.Size(227, 25);
            this.textBox_Hmax.TabIndex = 2;
            this.textBox_Hmax.Text = "600";
            // 
            // textBox_dH
            // 
            this.textBox_dH.Location = new System.Drawing.Point(242, 134);
            this.textBox_dH.Name = "textBox_dH";
            this.textBox_dH.Size = new System.Drawing.Size(227, 25);
            this.textBox_dH.TabIndex = 3;
            this.textBox_dH.Text = "100";
            // 
            // textBox_dtheta
            // 
            this.textBox_dtheta.Location = new System.Drawing.Point(242, 200);
            this.textBox_dtheta.Name = "textBox_dtheta";
            this.textBox_dtheta.Size = new System.Drawing.Size(226, 25);
            this.textBox_dtheta.TabIndex = 4;
            // 
            // Hmax
            // 
            this.Hmax.AutoSize = true;
            this.Hmax.Location = new System.Drawing.Point(103, 66);
            this.Hmax.Name = "Hmax";
            this.Hmax.Size = new System.Drawing.Size(86, 18);
            this.Hmax.TabIndex = 5;
            this.Hmax.Text = "Hmax[mT]";
            this.Hmax.Click += new System.EventHandler(this.label1_Click);
            // 
            // dH
            // 
            this.dH.AutoSize = true;
            this.dH.Location = new System.Drawing.Point(103, 141);
            this.dH.Name = "dH";
            this.dH.Size = new System.Drawing.Size(65, 18);
            this.dH.TabIndex = 6;
            this.dH.Text = "dH[mT]";
            // 
            // dtheta
            // 
            this.dtheta.AutoSize = true;
            this.dtheta.Location = new System.Drawing.Point(103, 207);
            this.dtheta.Name = "dtheta";
            this.dtheta.Size = new System.Drawing.Size(94, 18);
            this.dtheta.TabIndex = 7;
            this.dtheta.Text = "dtheta[deg]";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dtheta);
            this.Controls.Add(this.dH);
            this.Controls.Add(this.Hmax);
            this.Controls.Add(this.textBox_dtheta);
            this.Controls.Add(this.textBox_dH);
            this.Controls.Add(this.textBox_Hmax);
            this.Controls.Add(this.button_mediameasure);
            this.Controls.Add(this.button_nonmedia);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_nonmedia;
        private System.Windows.Forms.Button button_mediameasure;
        private System.Windows.Forms.TextBox textBox_Hmax;
        private System.Windows.Forms.TextBox textBox_dH;
        private System.Windows.Forms.TextBox textBox_dtheta;
        private System.Windows.Forms.Label Hmax;
        private System.Windows.Forms.Label dH;
        private System.Windows.Forms.Label dtheta;
    }
}

