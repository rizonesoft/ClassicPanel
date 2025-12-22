namespace ClassicPanel.UI;

partial class MainWindow
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.menuStrip = new ClassicPanel.UI.Controls.ModernMenuStrip();
        this.toolStrip = new ClassicPanel.UI.Controls.ModernToolStrip();
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.SuspendLayout();
        // 
        // menuStrip
        // 
        this.menuStrip.Location = new System.Drawing.Point(0, 0);
        this.menuStrip.Name = "menuStrip";
        this.menuStrip.Size = new System.Drawing.Size(800, 24);
        this.menuStrip.TabIndex = 0;
        this.menuStrip.Text = "menuStrip";
        // 
        // toolStrip
        // 
        this.toolStrip.Location = new System.Drawing.Point(0, 24);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Size = new System.Drawing.Size(800, 52);
        this.toolStrip.TabIndex = 1;
        this.toolStrip.Text = "toolStrip";
        // 
        // statusStrip
        // 
        this.statusStrip.Location = new System.Drawing.Point(0, 428);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(800, 22);
        this.statusStrip.TabIndex = 2;
        this.statusStrip.Text = "statusStrip";
        // 
        // MainWindow
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.statusStrip);
        this.Controls.Add(this.toolStrip);
        this.Controls.Add(this.menuStrip);
        this.MainMenuStrip = this.menuStrip;
        this.Name = "MainWindow";
        this.Text = "ClassicPanel";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private ClassicPanel.UI.Controls.ModernMenuStrip menuStrip;
    private ClassicPanel.UI.Controls.ModernToolStrip toolStrip;
    private System.Windows.Forms.StatusStrip statusStrip;

    #endregion
}

