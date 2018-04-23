' Developer Express Code Central Example:
' How to replace the cursor with a thumbnail image while dragging a row in GridView
' 
' This example demonstrates how to replace the standard cursor with a transparent
' image of the dragged row during a drag-and-drop operation in GridView
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E2735


Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid.Drawing

Namespace SwapRows
	''' <summary>
	''' Summary description for Form1.
	''' </summary>
	Public Class Form1
		Inherits System.Windows.Forms.Form
		Private gridControl1 As DevExpress.XtraGrid.GridControl
		Private gridView1 As DevExpress.XtraGrid.Views.Grid.GridView
		Private dataSet1 As System.Data.DataSet
		Private dataTable1 As System.Data.DataTable
		Private dataColumn1 As System.Data.DataColumn
		Private dataColumn2 As System.Data.DataColumn
		Private dataColumn3 As System.Data.DataColumn
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.Container = Nothing

		Public Sub New()
			'
			' Required for Windows Form Designer support
			'
			InitializeComponent()

			'
			' TODO: Add any constructor code after InitializeComponent call
			'
		End Sub

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If components IsNot Nothing Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"
		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.gridControl1 = New DevExpress.XtraGrid.GridControl()
			Me.gridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.dataSet1 = New System.Data.DataSet()
			Me.dataTable1 = New System.Data.DataTable()
			Me.dataColumn1 = New System.Data.DataColumn()
			Me.dataColumn2 = New System.Data.DataColumn()
			Me.dataColumn3 = New System.Data.DataColumn()
			CType(Me.gridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dataTable1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' gridControl1
			' 
			Me.gridControl1.AllowDrop = True
			Me.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.gridControl1.Location = New System.Drawing.Point(0, 0)
			Me.gridControl1.MainView = Me.gridView1
			Me.gridControl1.Name = "gridControl1"
			Me.gridControl1.Size = New System.Drawing.Size(499, 310)
			Me.gridControl1.TabIndex = 0
			Me.gridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.gridView1})
			' 
			' gridView1
			' 
			Me.gridView1.GridControl = Me.gridControl1
			Me.gridView1.Name = "gridView1"
			Me.gridView1.OptionsBehavior.Editable = False
			Me.gridView1.OptionsView.EnableAppearanceEvenRow = True
			Me.gridView1.OptionsView.EnableAppearanceOddRow = True
			Me.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
			' 
			' dataSet1
			' 
			Me.dataSet1.DataSetName = "NewDataSet"
			Me.dataSet1.Locale = New System.Globalization.CultureInfo("en-US")
			Me.dataSet1.Tables.AddRange(New System.Data.DataTable() { Me.dataTable1})
			' 
			' dataTable1
			' 
			Me.dataTable1.Columns.AddRange(New System.Data.DataColumn() { Me.dataColumn1, Me.dataColumn2, Me.dataColumn3})
			Me.dataTable1.TableName = "Table1"
			' 
			' dataColumn1
			' 
			Me.dataColumn1.ColumnName = "ID"
			Me.dataColumn1.DataType = GetType(Integer)
			' 
			' dataColumn2
			' 
			Me.dataColumn2.ColumnName = "Keyword"
			' 
			' dataColumn3
			' 
			Me.dataColumn3.ColumnName = "Order"
			Me.dataColumn3.DataType = GetType(Decimal)
			' 
			' Form1
			' 
			Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
			Me.ClientSize = New System.Drawing.Size(499, 310)
			Me.Controls.Add(Me.gridControl1)
			Me.Name = "Form1"
			Me.Text = "Form1"
'			Me.Load += New System.EventHandler(Me.Form1_Load);
			CType(Me.gridControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dataSet1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dataTable1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		#End Region

		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		<STAThread> _
		Shared Sub Main()
			Application.Run(New Form1())
		End Sub

		Private Sub FillDataSet()
			dataTable1.Rows.Add(New Object() { 1, "abstract", 1 })
			dataTable1.Rows.Add(New Object() { 2, "event", 2 })
			dataTable1.Rows.Add(New Object() { 3, "new", 3 })
			dataTable1.Rows.Add(New Object() { 4, "struct", 4 })
			dataTable1.Rows.Add(New Object() { 5, "as", 5 })
			dataTable1.Rows.Add(New Object() { 6, "explicit", 6 })
			dataTable1.Rows.Add(New Object() { 7, "null", 7 })
			dataTable1.Rows.Add(New Object() { 8, "switch", 8 })
			dataTable1.Rows.Add(New Object() { 9, "base", 9 })
			dataTable1.Rows.Add(New Object() { 10, "extern", 10 })
			dataTable1.Rows.Add(New Object() { 11, "object", 11 })
			dataTable1.Rows.Add(New Object() { 12, "this", 12 })
			dataTable1.Rows.Add(New Object() { 13, "bool", 13 })
			dataTable1.Rows.Add(New Object() { 14, "false", 14 })
			dataTable1.Rows.Add(New Object() { 15, "operator", 15 })
			dataTable1.Rows.Add(New Object() { 16, "throw", 16 })
			dataTable1.Rows.Add(New Object() { 17, "break", 17 })
			dataTable1.AcceptChanges()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
			FillDataSet()
			gridControl1.DataSource = dataTable1
			gridView1.PopulateColumns()
			gridView1.OptionsCustomization.AllowSort = False
			gridView1.OptionsView.ShowGroupPanel = False
			Dim TempDragHelper As DragHelper = New DragHelper(gridView1)
		End Sub


	End Class
End Namespace