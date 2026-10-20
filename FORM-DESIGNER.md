# Show and edit the Windows Form

Open **BaobabRidge.sln** in Visual Studio, not Open Folder. In Solution Explorer,
double-click **MainForm.cs** to open **MainForm.cs [Design]**. The form is a
System.Windows.Forms.Form. Its .Designer.cs, .resx and Member3 partial files are
nested beneath it. The project explicitly records that MainForm is a Form.

## Demonstrate the visual tools

1. Open **View > Toolbox** (Ctrl+Alt+X). This contains Button, Label, TextBox,
   DataGridView and other Windows Forms controls.
2. Select an existing control on the form. Selection handles show it is editable.
3. Press **F4** for Properties. Show **(Name)**, **Text**, **Location**, **Size**
   and **TabIndex**. A button's name is used by code; its text is the visible caption.
4. Select the lightning-bolt Events button in Properties. For btnAdd, the Click
   event points to btnAdd_Click. Double-click that entry to open the existing handler.
5. To learn how adding a form works, use **Project > Add > Form (Windows Forms)**
   and give the practice form a different name in a separate practice project.
   Visual Studio creates its .cs, .Designer.cs and optional .resx files.
6. Drag a Button from the Toolbox onto the practice form, set its Name and Text,
   then double-click the button to create its Click handler. Practise this yourself
   before presenting; do not claim a drag-and-drop history you did not perform.

Visual Studio saves control creation, properties and event connections into
InitializeComponent in MainForm.Designer.cs. Keep validation and file operations
in the existing form handlers/services. Runtime grid columns are configured in
CreateAnimalColumns; do not add duplicate columns through the designer.

The original layout was prepared in C# with Codex assistance. Its project setup
was corrected and the form was opened, edited and saved through Visual Studio's
Windows Forms Designer. This does not change the assistance disclosure.

ForceDesignerDPIUnaware keeps the designer's layout measurements consistent across
different Windows display scaling settings. It does not change application DPI mode.
