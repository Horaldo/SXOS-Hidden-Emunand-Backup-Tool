<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.OutputTextBox = New System.Windows.Forms.TextBox()
        Me.InputTextBox = New System.Windows.Forms.TextBox()
        Me.ExecuteButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'OutputTextBox
        '
        Me.OutputTextBox.Location = New System.Drawing.Point(50, 53)
        Me.OutputTextBox.Multiline = True
        Me.OutputTextBox.Name = "OutputTextBox"
        Me.OutputTextBox.ReadOnly = True
        Me.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.OutputTextBox.Size = New System.Drawing.Size(547, 240)
        Me.OutputTextBox.TabIndex = 0
        '
        'InputTextBox
        '
        Me.InputTextBox.Location = New System.Drawing.Point(107, 327)
        Me.InputTextBox.Name = "InputTextBox"
        Me.InputTextBox.Size = New System.Drawing.Size(100, 20)
        Me.InputTextBox.TabIndex = 0
        '
        'ExecuteButton
        '
        Me.ExecuteButton.Location = New System.Drawing.Point(693, 145)
        Me.ExecuteButton.Name = "ExecuteButton"
        Me.ExecuteButton.Size = New System.Drawing.Size(75, 23)
        Me.ExecuteButton.TabIndex = 2
        Me.ExecuteButton.Text = "Execute"
        Me.ExecuteButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.ExecuteButton)
        Me.Controls.Add(Me.InputTextBox)
        Me.Controls.Add(Me.OutputTextBox)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents OutputTextBox As TextBox
    Friend WithEvents InputTextBox As TextBox
    Friend WithEvents ExecuteButton As Button
End Class
