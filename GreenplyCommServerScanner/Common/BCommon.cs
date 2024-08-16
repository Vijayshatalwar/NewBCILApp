using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlClient;


namespace GreenplyScannerCommServer.Common
{
    public class BCommon
    {

        ///Function enabled/disabled group details data according to event chosen
        ///grpDetails : name of groupbox of details
        ///grpActions : name of groupbox of action buttons
        ///strTable : name of event performing on the form
        public void EnabledControls(PCommon clsPC)
        {
            bool b = true;
            if (clsPC.Event == "LOAD" || clsPC.Event == "SAVE" || clsPC.Event == "CANCEL")
            {
                clsPC.dgvDetails.Enabled = b;
                foreach (Control ctrlDetails in clsPC.grpDetails.Controls)
                {
                    if (ctrlDetails.GetType() == typeof(TextBox) || ctrlDetails.GetType() == typeof(RadioButton) || ctrlDetails.GetType() == typeof(Button) || ctrlDetails.GetType() == typeof(DateTimePicker) || ctrlDetails.GetType() == typeof(DataGridView) || ctrlDetails.GetType() == typeof(CheckBox) || ctrlDetails.GetType() == typeof(ComboBox) || ctrlDetails.GetType() == typeof(ListBox))
                    {
                        if (ctrlDetails.GetType() != typeof(Button))
                        {
                            if (ctrlDetails.GetType() == typeof(CheckBox))
                            {
                                CheckBox myChkBx = (CheckBox)ctrlDetails;
                                myChkBx.Checked = false;
                            }
                            else if (ctrlDetails.GetType() == typeof(RadioButton))
                            {
                                RadioButton myrbtn = (RadioButton)ctrlDetails;
                                myrbtn.Checked = false;
                            }
                            else if (ctrlDetails.GetType() == typeof(ComboBox))
                            {
                                ComboBox cmb = (ComboBox)ctrlDetails;

                                if (cmb.Items.Count > 0)
                                {
                                    cmb.SelectedIndex = 0;
                                    cmb.SelectionLength = 0;
                                }

                            }
                            else if (ctrlDetails.GetType() == typeof(ListBox))
                            {
                                ListBox lstBox = (ListBox)ctrlDetails;
                                if (lstBox.Items.Count > 0)
                                {
                                    lstBox.DataSource = null;
                                }
                            }
                            else if (ctrlDetails.GetType() == typeof(DataGridView))
                            {
                                DataGridView dgv = (DataGridView)ctrlDetails;
                                if (dgv.Rows.Count > 0)
                                {
                                    DataTable dt = (DataTable)dgv.DataSource;
                                    if (dt != null)
                                    {
                                        dgv.DataSource = null;
                                    }
                                    else
                                    {
                                        dgv.Rows.Clear();
                                    }

                                }
                            }
                            else
                            {
                                ctrlDetails.Text = "";
                            }
                        }
                        ctrlDetails.Enabled = !b;
                    }

                }
                if (clsPC.grpSearch != null)
                {
                    foreach (Control ctrlSearch in clsPC.grpSearch.Controls)
                    {
                        if (ctrlSearch.GetType() == typeof(TextBox) || ctrlSearch.GetType() == typeof(Button) || ctrlSearch.GetType() == typeof(CheckBox) || ctrlSearch.GetType() == typeof(ComboBox) || ctrlSearch.GetType() == typeof(DateTimePicker))
                        {
                            ctrlSearch.Enabled = b;
                        }

                    }
                }

                foreach (Control ctrlActions in clsPC.grpActions.Controls)
                {
                    if (ctrlActions.Name == "btnAdd")
                    {
                        ctrlActions.Enabled = b;
                    }
                    else if (ctrlActions.Name == "btnEdit" || ctrlActions.Name == "btnDelete")
                    {
                        if (clsPC.dgvDetails.Rows.Count == 0)
                        {
                            ctrlActions.Enabled = !b;
                        }
                        else
                        {
                            ctrlActions.Enabled = b;
                        }
                    }
                    else if (ctrlActions.Name == "btnSave" || ctrlActions.Name == "btnCancel" || ctrlActions.Name == "btnDelOrder")
                    {
                        ctrlActions.Enabled = !b;
                    }
                }
                clsPC.dgvDetails.Focus();
            }




            else if (clsPC.Event == "ADD" || clsPC.Event == "EDIT")
            {
                clsPC.dgvDetails.Enabled = !b;
                foreach (Control ctrlDetails in clsPC.grpDetails.Controls)
                {
                    if (ctrlDetails.GetType() == typeof(TextBox) || ctrlDetails.GetType() == typeof(RadioButton) || ctrlDetails.GetType() == typeof(Button) || ctrlDetails.GetType() == typeof(DateTimePicker) || ctrlDetails.GetType() == typeof(DataGridView) || ctrlDetails.GetType() == typeof(CheckBox) || ctrlDetails.GetType() == typeof(ComboBox) || ctrlDetails.GetType() == typeof(ListBox))
                    {
                        if (clsPC.Event == "ADD")
                        {
                            if (ctrlDetails.GetType() != typeof(Button))
                            {
                                if (ctrlDetails.GetType() == typeof(CheckBox))
                                {
                                    CheckBox myChkBx = (CheckBox)ctrlDetails;
                                    myChkBx.Checked = false;
                                }
                                else if (ctrlDetails.GetType() == typeof(RadioButton))
                                {
                                    RadioButton myrbtn = (RadioButton)ctrlDetails;
                                    myrbtn.Checked = false;
                                }
                                else if (ctrlDetails.GetType() == typeof(ComboBox))
                                {
                                    ComboBox cmb = (ComboBox)ctrlDetails;

                                    if (cmb.Items.Count > 0)
                                    {
                                        cmb.SelectedIndex = 0;
                                        cmb.SelectionLength = 0;
                                    }
                                }
                                else if (ctrlDetails.GetType() == typeof(DataGridView))
                                {
                                    DataGridView dgv = (DataGridView)ctrlDetails;
                                    if (dgv.Rows.Count > 0)
                                    {
                                        DataTable dt = (DataTable)dgv.DataSource;
                                        if (dt != null)
                                        {
                                            dgv.DataSource = null;
                                        }
                                        else
                                        {
                                            dgv.Rows.Clear();
                                        }
                                    }
                                }
                                else
                                {
                                    ctrlDetails.Text = "";
                                }
                            }
                        }
                        ctrlDetails.Enabled = b;
                    }

                }
                if (clsPC.grpSearch != null)
                {
                    foreach (Control ctrlSearch in clsPC.grpSearch.Controls)
                    {
                        if (ctrlSearch.GetType() == typeof(TextBox) || ctrlSearch.GetType() == typeof(Button) || ctrlSearch.GetType() == typeof(CheckBox) || ctrlSearch.GetType() == typeof(ComboBox) || ctrlSearch.GetType() == typeof(DateTimePicker))
                        {
                            ctrlSearch.Enabled = !b;
                        }
                    }
                }

                foreach (Control ctrl in clsPC.grpActions.Controls)
                {
                    if (ctrl.Name == "btnAdd" || ctrl.Name == "btnEdit" || ctrl.Name == "btnDelete")
                    {
                        ctrl.Enabled = !b;
                    }
                    else if (ctrl.Name == "btnSave" || ctrl.Name == "btnCancel" || ctrl.Name == "btnDelOrder")
                    {
                        ctrl.Enabled = b;
                    }
                }
            }

        }

        /*
         * highlight control on got focus by changing backcolor and font boldness
         */
        public void OnGotFocusFormControl(Form frmSource, Control ctrlSource)
        {
            foreach (Control ctrl in frmSource.Controls)
            {
                if (ctrl.GetType() == typeof(ComboBox))
                {
                    ComboBox cmb = (ComboBox)ctrl;

                    if (cmb.Items.Count > 0)
                    {
                        cmb.SelectionLength = 0;
                    }
                }

                if ((ctrl.Enabled == true) && (ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(ComboBox) || ctrl.GetType() == typeof(CheckBox)))
                {
                    if (ctrl.GetType() != typeof(CheckBox))
                    {
                        ctrl.BackColor = Color.White;
                    }
                    else
                    {
                        ctrl.BackColor = frmSource.BackColor;
                    }
                    ctrl.Font = new Font(ctrl.Font, FontStyle.Regular);
                }
            }
            if (ctrlSource.GetType() == typeof(TextBox) || ctrlSource.GetType() == typeof(ComboBox) || ctrlSource.GetType() == typeof(CheckBox))
            {
                ctrlSource.BackColor = Color.FromArgb(255, 255, 192);
                ctrlSource.Font = new Font(ctrlSource.Font, FontStyle.Bold);
                ctrlSource.Focus();
            }
        }

        public void OnGotFocusGroupBoxControl(GroupBox grpSource, Control ctrlSource)
        {
            foreach (Control ctrl in grpSource.Controls)
            {
                if (ctrl.GetType() == typeof(ComboBox))
                {
                    ComboBox cmb = (ComboBox)ctrl;

                    if (cmb.Items.Count > 0)
                    {
                        cmb.SelectionLength = 0;
                    }
                }

                if ((ctrl.Enabled == true) && (ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(ComboBox)) || ctrl.GetType() == typeof(CheckBox) || ctrl.GetType() == typeof(ListBox))
                {
                    ctrl.BackColor = Color.White;
                    ctrl.Font = new Font(ctrl.Font, FontStyle.Regular);
                }
            }
            if (ctrlSource.GetType() == typeof(TextBox) || ctrlSource.GetType() == typeof(ComboBox) || ctrlSource.GetType() == typeof(CheckBox) || ctrlSource.GetType() == typeof(ListBox))
            {
                ctrlSource.BackColor = Color.FromArgb(255, 255, 192);
                ctrlSource.Font = new Font(ctrlSource.Font, FontStyle.Bold);
                ctrlSource.Focus();
            }
        }

        /*
         * move to next control on enter excluding buttons, 
         * and jumping control should'nt be '0' because it use as default while calling method
         * also while calling method to jump any particular control passed it's TabIndex instead of '0'
         */
        public void MoveToNextFormControl(Form frmSource, Control ctrlSource, int LastTabIndex, int JumpIndex)
        {
            Int32 Index;
            if ((ctrlSource.TabIndex == LastTabIndex) && (JumpIndex == 0))
            {
                Index = 0;
            }
            else
            {
                if (JumpIndex == 0)
                    Index = ctrlSource.TabIndex;
                else
                    Index = JumpIndex;
            }
            foreach (Control ctrlDestination in frmSource.Controls)
            {
                if ((ctrlDestination.TabIndex == Index + 1) && (ctrlDestination.GetType() == typeof(TextBox) || ctrlDestination.GetType() == typeof(ComboBox) || ctrlDestination.GetType() == typeof(DateTimePicker) || ctrlDestination.GetType() == typeof(RadioButton)))
                {
                    if (ctrlDestination.GetType() == typeof(TextBox))
                    {
                        TextBox txtDestination = (TextBox)ctrlDestination;
                        SelectText(txtDestination);
                    }
                    else if (ctrlDestination.GetType() == typeof(DateTimePicker))
                    {
                        DateTimePicker dtDestination = (DateTimePicker)ctrlDestination;
                        SelectTextDateTimePicker(dtDestination);
                    }
                    else if (ctrlDestination.GetType() == typeof(ComboBox))
                    {
                        ComboBox cmb = (ComboBox)ctrlDestination;

                        if (cmb.Items.Count > 0)
                        {
                            cmb.SelectionLength = 0;
                        }
                    }
                    // change control backcolor and font boldness
                    ctrlDestination.BackColor = Color.FromArgb(255, 255, 192);
                    ctrlDestination.Font = new Font(ctrlDestination.Font, FontStyle.Bold);
                    ctrlDestination.Focus();
                }
                else
                {
                    if ((ctrlDestination.Enabled == true) && (ctrlDestination.GetType() == typeof(TextBox) || ctrlDestination.GetType() == typeof(ComboBox) || ctrlDestination.GetType() == typeof(DateTimePicker) || ctrlDestination.GetType() == typeof(RadioButton)))
                    {
                        if (ctrlDestination.GetType() != typeof(RadioButton))
                        {
                            ctrlDestination.BackColor = Color.White;
                        }
                        ctrlDestination.Font = new Font(ctrlDestination.Font, FontStyle.Regular);
                    }
                }
            }
        }

        public void MoveToNextGroupBoxControl(GroupBox grpSource, Control ctrlSource, int LastTabIndex, int JumpIndex)
        {
            Int32 Index;
            if ((ctrlSource.TabIndex == LastTabIndex) && (JumpIndex == 0 || grpSource != null))
            {
                Index = JumpIndex;
            }
            else
            {
                if (JumpIndex == 0 || grpSource != null)
                    Index = ctrlSource.TabIndex;
                else
                    Index = JumpIndex;
            }
            foreach (Control ctrlDestination in grpSource.Controls)
            {
                if ((ctrlDestination.TabIndex == Index + 1) && (ctrlDestination.GetType() == typeof(TextBox) || ctrlDestination.GetType() == typeof(DateTimePicker) || ctrlDestination.GetType() == typeof(ComboBox) || ctrlDestination.GetType() == typeof(CheckBox) || ctrlDestination.GetType() == typeof(ListBox)))
                {
                    if (ctrlDestination.GetType() == typeof(TextBox))
                    {
                        TextBox txtDestination = (TextBox)ctrlDestination;
                        SelectText(txtDestination);
                    }
                    else if (ctrlDestination.GetType() == typeof(DateTimePicker))
                    {
                        DateTimePicker dtDestination = (DateTimePicker)ctrlDestination;
                        SelectTextDateTimePicker(dtDestination);
                    }
                    else if (ctrlDestination.GetType() == typeof(ComboBox))
                    {
                        ComboBox cmb = (ComboBox)ctrlDestination;

                        if (cmb.Items.Count > 0)
                        {
                            cmb.SelectionLength = 0;
                        }
                    }
                    // change control backcolor and font boldness
                    ctrlDestination.BackColor = Color.FromArgb(255, 255, 192);
                    ctrlDestination.Font = new Font(ctrlDestination.Font, FontStyle.Bold);
                    ctrlDestination.Focus();
                }
                else
                {
                    if ((ctrlDestination.Enabled == true) && (ctrlDestination.GetType() == typeof(TextBox) || ctrlDestination.GetType() == typeof(DateTimePicker) || ctrlDestination.GetType() == typeof(ComboBox) || ctrlDestination.GetType() == typeof(CheckBox) || ctrlDestination.GetType() == typeof(ListBox)))
                    {
                        if (ctrlDestination.GetType() == typeof(ComboBox))
                        {
                            ComboBox cmb = (ComboBox)ctrlDestination;

                            if (cmb.Items.Count > 0)
                            {
                                cmb.SelectionLength = 0;
                            }
                        }
                        ctrlDestination.BackColor = Color.White;
                        ctrlDestination.Font = new Font(ctrlDestination.Font, FontStyle.Regular);
                    }
                }
            }
        }

        /*
         * change the default color of each control
         */
        public void GroupBoxControlsDefaultColor(PCommon clsPC)
        {
            foreach (Control ctrlDetails in clsPC.grpDetails.Controls)
            {
                if ((ctrlDetails.Enabled == true) && (ctrlDetails.GetType() == typeof(TextBox) || ctrlDetails.GetType() == typeof(ComboBox) || ctrlDetails.GetType() == typeof(ListBox)))
                {
                    ctrlDetails.BackColor = SystemColors.MenuBar;
                    ctrlDetails.Font = new Font(ctrlDetails.Font, FontStyle.Regular);
                }
            }
            if (clsPC.grpSearch != null)
            {
                foreach (Control ctrlSearch in clsPC.grpSearch.Controls)
                {
                    if ((ctrlSearch.Enabled == true) && (ctrlSearch.GetType() == typeof(TextBox) || ctrlSearch.GetType() == typeof(ComboBox)))
                    {
                        ctrlSearch.BackColor = SystemColors.MenuBar;
                        ctrlSearch.Font = new Font(ctrlSearch.Font, FontStyle.Regular);
                    }
                }
            }
        }

        /*
        * select whole text of textbox
        */
        public void SelectText(TextBox txt)
        {
            txt.Select(0, txt.Text.Length);
            txt.Focus();
        }

        /*
        * close and dispose passed form
        */
        public void CloseForm(Form frm)
        {
            frm.Close();
            frm.Dispose();
        }

        /*
        * select whole text of datetimepicker
        */
        public void SelectTextDateTimePicker(DateTimePicker dtpicker)
        {
            dtpicker.Select();
            dtpicker.Focus();
        }

        public void ProperAlpha(TextBox txt)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            txt.Text = textInfo.ToTitleCase(txt.Text);
            txt.SelectionStart = txt.Text.Length;
        }

        /*
         * Only allow to key Symbol on selected control
         * c - key character passed value from control
         */
        public bool Symbol(char c)
        {
            if (char.IsDigit(c) == false && char.IsLetter(c) == false)
            {
                return true;
            }
            return false;
        }
        public bool SymbolNew(char c)
        {
            char a = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (a == c)
            {
                return false;
            }
            if (char.IsDigit(c) == false && char.IsLetter(c) == false)
            {
                return true;
            }
            return false;
        }
        /*
         * Only allow to key numbers on selected control
         * c - key character passed value from control
         * decimalPoint - to allow decimal or not with number
         */
        public bool Num(char c, bool decimalPoint)
        {
            if (decimalPoint == true && c.ToString() == ".")
            {
                return true;
            }
            else if (char.IsDigit(c) == true)
            {
                return true;
            }
            return false;
        }

        public bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        public bool IsValidPhone(string strPhoneNumber)
        {
            // Return true if strIn is in valid Phone format.

            string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

            if (strPhoneNumber != null) return Regex.IsMatch(strPhoneNumber, MatchPhoneNumberPattern);

            else return false;

        }

        public void SelectAll(DataGridView dgv, bool bSelect)
        {
            string strChk = "true";
            if (bSelect == false) strChk = "false";
            for (int iCnt = 0; iCnt < dgv.RowCount; iCnt++)
            {
                dgv.Rows[iCnt].Cells[0].Value = strChk;
            }
        }

        public string ReplaceDateFormat(string strDate)
        {
            string[] str = strDate.Split('-');
            strDate = str[1] + "/" + str[0] + "/" + str[2];
            return strDate;
        }

        public void ShowDataInComboBox(PCommon clsPC)
        {
            try
            {
                DataTable dt = clsPC.Datatable;
                if (clsPC.SelectInCombo)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "";
                    dr[1] = "--Select--";
                    dt.Rows.InsertAt(dr, 0);
                }
                clsPC.Combo.ValueMember = dt.Columns[0].ToString();
                clsPC.Combo.DisplayMember = dt.Columns[1].ToString();
                clsPC.Combo.DataSource = dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clsPC = null;
            }
        }

        public string EncryptPassword(string lPass, string Ltype)
        {
            string encName = string.Empty;
            string LCompname = string.Empty;
            int i;
            int j;
            int LLen;
            char c1;
            char c2;

            LLen = lPass.Length;
            LCompname = "BCILBCILBCILBCILBCILBCIL";
            for (i = 0; i < LLen; i++)
            {
                c1 = Convert.ToChar(lPass.Substring(i, 1));
                c2 = Convert.ToChar(LCompname.Substring(i, 1));

                if (Ltype == "E")
                {
                    var e1 = Encoding.GetEncoding(1252);
                    j = (int)c1 + (int)c2 + i;
                    var s = e1.GetString(new byte[] { Convert.ToByte(j) });

                    encName = encName + s;
                }
                else
                {
                    var e1 = Encoding.GetEncoding(1252);
                    var r = e1.GetBytes(new char[] { Convert.ToChar(c1) });
                    j = (int)r[0] - (int)c2 - i;

                    encName = encName + (char)j;
                }
            }
            return encName;
        }

        public string EncryptSecurityLevel(string Ltype, string LRights, int Lcode, string LUserID)
        {
            string lEncrypt;
            string ESL = string.Empty;
            char x;
            char y;
            char z;
            string b;
            int i;
            int j = 0;
            char c1;
            char c2;
            lEncrypt = LUserID + "BCIL";
            //lEncrypt = lEncrypt.Substring(0, 3);
            lEncrypt = lEncrypt.Substring(0, 3).ToUpper();
            //b = Lcode + "479";
            b = "000" + Lcode.ToString();
            b = b.Substring(b.Length - 3, 3);
            if (Ltype == "E")
            {
                for (i = 0; i < lEncrypt.Length; i++)
                {
                    var e1 = Encoding.GetEncoding(1252);
                    c1 = Convert.ToChar(lEncrypt.Substring(i, 1));
                    c2 = Convert.ToChar(b.Substring(i, 1));
                    j = (int)c1 + (int)c2 + (int)Convert.ToChar(LRights);
                    var s = e1.GetString(new byte[] { Convert.ToByte(j) });
                    ESL = ESL + s;
                }
            }
            else if (Ltype == "D")
            {
                var e1 = Encoding.GetEncoding(1252);
                var r = e1.GetBytes(new char[] { Convert.ToChar(LRights.Substring(0, 1)) });
                var r1 = e1.GetBytes(new char[] { Convert.ToChar(LRights.Substring(1, 1)) });
                var r2 = e1.GetBytes(new char[] { Convert.ToChar(LRights.Substring(2, 1)) });
                j = (int)r[0] - ((int)Convert.ToChar(lEncrypt.Substring(0, 1)) + (int)Convert.ToChar(b.Substring(0, 1)));
                x = (char)j;
                j = (int)r1[0] - ((int)Convert.ToChar(lEncrypt.Substring(1, 1)) + (int)Convert.ToChar(b.Substring(1, 1)));
                y = (char)j;
                j = (int)r2[0] - ((int)Convert.ToChar(lEncrypt.Substring(2, 1)) + (int)Convert.ToChar(b.Substring(2, 1)));
                z = (char)j;
                if (x != y || x != z)
                {
                    ESL = "N";
                }
                else
                {
                    ESL = x.ToString();
                }
            }
            return ESL;
        }

        public enum TypeCode
        {
            LOAD,
            SAVE,
            CANCEL,
            ADD,
            EDIT,
            ALLITEMS,
            ALLSLOTS,
            BULK,
            CATEGORY,
            CUSTOMER,
            ITEM,
            LOCATION,
            MULTI,
            PLANT,
            PO,
            SORETURN,
            SINGLE,
            PORETURN,
            SO,
            STO,
            DAMAGE,
            VENDOR,
            VEHICLE,
            WAREHOUSE,
            PICKLIST,
            PICKER,
            VEHICLETYPE,
            DISPATCH_DESTINATION,
            DISPATCHED_DESTINATION,
            INVOICE,
            DISPATCHED_INVOICE,
            STN,
            STN_DESTINATION,
            STN_PICKLISTID,
            DISPATCH,
            DESTINATION,
            ORDER,
            LINENO, SUBLINENO,
            ERP,
            ERPITEMLOC,
            GRN,
            GRNSTO,
            REMOVE,
            NOEDIT,
            CLOSE,
            CLOSED,
            RETAIN,
            FCLOSE,
            CLS, COUNTRY, STATE, CITY, UOM, LOCATIONTYPE, ITEMLOCATION,
            INWARD, OUTWARD, MENU,
            INVENTORYITEMWISE,
            LOCATIONSTOCK,
            RPTRECEIVING,
            RPTDISPATCH,
            SOURCECODE, SOURCENAME,
            DEL,
            CARTON,
            USB, LAN, DONE,
            ADMIN, USER,
            AGEING, PACKINGLIST, PUT, PUTAWAY, GRNPRODOC, ITEMPRINT,
            REJECTED, QC, DEMO, LIVE, ORDERNO, PICKSHEET, OK, LOTNO,
            COLOR, SIZE, BRAND, GROUP, FGITEMPRINT, FGITEM, FGCOLOR, FGSIZE, PROD, RMITEM, PRODNEW, ADC, PR, FGD, ORDERTYPE,
        }

        public enum Modules
        {
            QC,
            PACKING,
            LOADING
        }

        /// <summary>
        /// Common method to get datatable from string response
        /// </summary>
        /// <param name="strResponse"></param>
        /// <param name="chRowDel"></param>
        /// <param name="chColDel"></param>
        /// 
        public DataTable getTable(string strResponse, char chRowDel, char chColDel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DispVal");
            dt.Columns.Add("ValMember");

            if (strResponse != string.Empty)
            {
                string[] strRow = strResponse.Split(chRowDel);

                for (int i = 0; i < strRow.Length; i++)
                {
                    string[] strCol = strRow[i].Split(chColDel);
                    DataRow dr = dt.NewRow();
                    dr[0] = strCol[0].ToString();
                    dr[1] = strCol[1].ToString();
                    dt.Rows.InsertAt(dr, i + 1);
                }
            }

            return dt;
        }

        public DataTable GetDataTable(string Response, string datacolumn, string datatype, char RowDeli, char ColDeli)
        {
            DataTable dt = new DataTable();
            string[] DataColumns = datacolumn.Split(ColDeli);
            string[] DataTypes = datatype.Split(ColDeli);
            for (int i = 0; i < DataColumns.Length; i++)
            {
                dt.Columns.Add(DataColumns[i].ToString(), Type.GetType(DataTypes[i]));
            }
            //dt.Columns.Add("DispVal");
            //dt.Columns.Add("ValMember");

            if (Response != string.Empty)
            {
                string[] strRow = Response.Split(RowDeli);

                for (int i = 0; i < strRow.Length; i++)
                {
                    string[] strCol = strRow[i].Split(ColDeli);
                    DataRow dr = dt.NewRow();
                    for (int x = 0; x < DataColumns.Length; x++)
                    {
                        dr[DataColumns[x].ToString()] = strCol[x];
                    }
                    dt.Rows.InsertAt(dr, i + 1);
                }
            }

            return dt;
        }

        public bool readConnectionSetting(bool isLocal)
        {
            string strFileName = string.Empty;
            strFileName = PCommon.strLocalConfigPath + PCommon.strLocalConfigFile;
            string[] strArr = null;
            FileInfo oDbFile = new FileInfo(strFileName);
            StreamReader oSrRead = null;
            string strline = string.Empty;
            bool isFileExist = false;
            bool _flage = false;

            try
            {
                if (oDbFile.Exists)
                {
                    oSrRead = new StreamReader(strFileName);
                    do
                    {
                        strline = oSrRead.ReadLine();
                        if (strline.Trim().ToUpper() == "</LOCAL_SETTING>")
                        {
                            break; // TODO: might not be correct. Was : Exit Do 
                        }
                        strArr = strline.Split('=');
                        if (strArr[0].Trim().ToUpper() == "DATABASENAME")
                        {
                            GlobalVariable.mSqlDb = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "USERNAME")
                        {
                            GlobalVariable.mSqlUser = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "PASSWORD")
                        {
                            GlobalVariable.mSqlPwd = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "SERVERNAME")
                        {
                            GlobalVariable.mSqlServer = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "PRINTERIP")
                        {
                            GlobalVariable.mPrinterIp = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "PRINTERPORT")
                        {
                            GlobalVariable.mSockPort = Convert.ToInt32(strArr[1].Trim());
                        }
                        if (strArr[0].Trim().ToUpper() == "LOCATIONCODE")
                        {
                            GlobalVariable.mSiteCode = strArr[1].Trim();
                        }
                    }
                    while (!(strline == null));
                    GlobalVariable.mSqlConString = string.Empty;
                    GlobalVariable.mSqlConString = "Data Source=" + GlobalVariable.mSqlServer + ";" + "Initial Catalog=" + GlobalVariable.mSqlDb + ";" + "User ID=" + GlobalVariable.mSqlUser + ";" + "Password=" + GlobalVariable.mSqlPwd;
                    oSrRead.Close();
                    oSrRead = null;
                    oDbFile = null;
                    SqlConnection con = new SqlConnection(GlobalVariable.mSqlConString);
                    try
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                            _flage = true;
                        else
                            _flage = false;

                        return _flage;

                    }
                    catch (Exception)
                    {
                        _flage = false;
                    }
                    return _flage;

                }
                else
                {
                    oDbFile = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (oSrRead != null)
                {
                    oSrRead.Close();
                }
                oSrRead = null;
                oDbFile = null;
            }
        }

        public bool readSiteSetting()
        {
            FileInfo oSiteFile = new FileInfo(PCommon.strSiteConfig);
            StreamReader oSrRead = null;
            string strline = string.Empty;
            bool isFileExist = false;

            try
            {
                if (oSiteFile.Exists)
                {
                    oSrRead = new StreamReader(PCommon.strSiteConfig);
                    while (!oSrRead.EndOfStream)
                    {
                        strline = oSrRead.ReadLine();
                    }

                    oSrRead.Close();
                    oSrRead = null;

                    if (strline == string.Empty)
                    {
                        isFileExist = false;
                    }
                    else
                    {
                        PCommon.strSiteData = strline;
                        isFileExist = true;
                    }
                    return isFileExist;
                }
                else
                {
                    oSiteFile = null;
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (oSrRead != null)
                {
                    oSrRead.Close();
                }
                oSrRead = null;
                oSiteFile = null;
            }
        }

        public int splitSiteFile()
        {
            //ALL~0#TypeCode~0#SiteName~0#SiteCode~0#SiteId~0
            int iReturn = 0;
            string strAllSite = string.Empty;

            string[] strHash = PCommon.strSiteData.Split('#');

            if (strHash.Length == 5)
            {
                //All Sites Or Not  0-All Sites, 1--One Site
                strAllSite = strHash[0].Split('~')[1];

                if (strAllSite == "1")
                {
                    //TypeCode
                    PCommon.LoginSiteType = strHash[1].Split('~')[1];
                    //SiteName
                    PCommon.LoginSiteName = strHash[2].Split('~')[1];
                    //SiteCode
                    PCommon.LoginSiteCode = strHash[3].Split('~')[1];
                    //SiteId
                    PCommon.LoginSiteId = strHash[4].Split('~')[1];

                    iReturn = 2;
                }
                else if (strAllSite == "0")
                {
                    iReturn = 1;
                }
            }
            else
            {
                iReturn = 0;
            }

            return iReturn;
        }

        public bool readDemoSetting()
        {
            FileInfo oDemoFile = new FileInfo(PCommon.strDemoConfig);
            StreamReader oSrRead = null;
            string strline = string.Empty;
            bool isFileExist = false;

            try
            {
                if (oDemoFile.Exists)
                {
                    oSrRead = new StreamReader(PCommon.strDemoConfig);
                    while (!oSrRead.EndOfStream)
                    {
                        strline = oSrRead.ReadLine();
                    }

                    oSrRead.Close();
                    oSrRead = null;

                    if (strline == string.Empty)
                    {
                        isFileExist = false;
                    }
                    else
                    {
                        PCommon.strApplicationType = strline;
                        isFileExist = true;
                    }
                    return isFileExist;
                }
                else
                {
                    oDemoFile = null;
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (oSrRead != null)
                {
                    oSrRead.Close();
                }
                oSrRead = null;
                oDemoFile = null;
            }
        }

        public void FillComboBox(PCommon clsPC)
        {
            try
            {
                DataTable dt = clsPC.Datatable;
                if (clsPC.SelectInCombo)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "--Select--";
                    dr[1] = "";
                    dt.Rows.InsertAt(dr, 0);
                }
                clsPC.Combo.DisplayMember = dt.Columns[0].ToString();
                clsPC.Combo.ValueMember = dt.Columns[1].ToString();
                clsPC.Combo.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                clsPC = null;
            }
        }

        //public bool EscapeModule(string strModule)
        //{
        //    //DCommon oDC = new DCommon();
        //    //try
        //    //{
        //    //    return oDC.EscapeModule(strModule);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex;
        //    //}
        //    //finally
        //    //{
        //    //    oDC = null;
        //    //}

        //}

        public static string DtToString(DataTable dt)
        {
            string sRow = string.Empty;
            string sDTString = string.Empty;
            if (dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sCol = string.Empty;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sCol = sCol + dt.Rows[i][j].ToString() + "#";
                    }
                    sRow = sRow + sCol.Substring(0, sCol.Length - 1) + "~";
                }
                sDTString = sRow.Substring(0, sRow.Length - 1);
            }
            return sDTString;
        }

        public static string getDtColumns(DataTable dt)
        {
            string strResponse = string.Empty;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strResponse = strResponse + "#" + dt.Columns[i].ColumnName;
            }
            strResponse = strResponse.Substring(1, strResponse.Length - 1);
            return strResponse;
        }

        public static string getDtString(DataTable dt)
        {
            string strResponse = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].ToString() == string.Empty)
                    {
                        strResponse = strResponse + "#" + "NA";
                    }
                    else
                    {
                        strResponse = strResponse + "#" + dt.Rows[i][j].ToString();
                    }
                }
                strResponse = strResponse + "$";
            }

            return strResponse;
        }

        public static string getString(DataTable dt)
        {
            string strResponse = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strResponse == string.Empty)
                {
                    strResponse = dt.Rows[i][0].ToString(); //+ "#" + dt.Rows[i][1].ToString();
                }
                else
                {
                    strResponse = strResponse + "$" + dt.Rows[i][0].ToString() + "#" + dt.Rows[i][1].ToString();
                }
            }

            return strResponse;
        }

        public static string gettableString(DataTable dt)
        {
            string strResponse = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strResponse == string.Empty)
                {
                    strResponse = dt.Rows[i][0].ToString() + "#" + dt.Rows[i][1].ToString() + "#" + dt.Rows[i][2].ToString() + "#" + dt.Rows[i][3].ToString();
                }
                else
                {
                    strResponse = strResponse + "$" + dt.Rows[i][0].ToString() + "#" + dt.Rows[i][1].ToString() + "#" + dt.Rows[i][2].ToString() + "#" + dt.Rows[i][3].ToString();
                }
            }

            return strResponse;
        }
    }
}
