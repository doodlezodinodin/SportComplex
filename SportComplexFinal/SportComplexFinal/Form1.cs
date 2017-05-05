using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SportComplexFinal
{
    public partial class Form1 : Form
    {
        SqlConnection connect = null;  
        SqlCommand command = null;  
        SqlDataReader dataReader = null;

        private String fileAddress = @"C:\Users\alex\Documents\Visual Studio 2015\Projects\SportComplexFinal\SportComplexFinal\DbSportComplex.mdf";
        private String tableString = "TableInstructions";
        private String buttonIUD;
        private String login = "a";
        private String pass = "a";

        
        private bool admin = false;

        public Form1()
        {
            InitializeComponent();
        }

        public async void loadSql(String table)
        {
            string connectString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + fileAddress  + "; Integrated Security=True"; ;
            connect = new SqlConnection(connectString);

            await connect.OpenAsync();

            command = new SqlCommand("SELECT * FROM [" + table + "]", connect);

            try
            {
                dataReader = await command.ExecuteReaderAsync();

                

                switch (tableString)
                {
                    case "TableInstructions":
                        {
                            listBox.Items.Add("загружаю базу данных DbSportComplex.mdf/TableInstructions...");
                            listBox.Items.Add("загрузка прошла успешно...");
                            listBox.Items.Add("[INFO] В данной базе данных хранится список ИНСТРУКТОРОВ нашего Спорт Комплекса");
                            listBox.Items.Add("******************************************************************************************************************************");
                            listBox.Items.Add("");
                            listBox.Items.Add("[id]  [Фамилия  инструктора]        [Имя  инструктора]       [Отчество  инструктора]     ");
                            listBox.Items.Add("");
                        }
                        break;
                    case "TableClients":
                        {
                            listBox.Items.Add("загружаю базу данных DbSportComplex.mdf/TableClients...");
                            listBox.Items.Add("загрузка прошла успешно...");
                            listBox.Items.Add("[INFO] В данной базе данных хранится список КЛИЕНТОВ нашего Спорт Комплекса");
                            listBox.Items.Add("******************************************************************************************************************************");
                            listBox.Items.Add("");                
                            listBox.Items.Add("id  Фамилия клиента    Имя клиента     Отчество клиента    Дата рождения    Телефон   Адрес");
                            listBox.Items.Add("");
                        }
                        break;
                    case "TableSchedule":
                        {
                            listBox.Items.Add("загружаю базу данных DbSportComplex.mdf/TableSchedule...");
                            listBox.Items.Add("загрузка прошла успешно...");
                            listBox.Items.Add("[INFO] В данной базе данных хранится РАСПИСАНИЕ ТРЕНИРОВОК нашего Спорт Комплекса");
                            listBox.Items.Add("******************************************************************************************************************************");
                            listBox.Items.Add("");
                            listBox.Items.Add("id        Группа        Дата проведения        Время начала        Время окончания");
                            listBox.Items.Add("");
                        }
                        break;
                    case "TableCost":
                        {
                            listBox.Items.Add("загружаю базу данных DbSportComplex.mdf/TableCost...");
                            listBox.Items.Add("загрузка прошла успешно...");
                            listBox.Items.Add("[INFO] В данной базе данных хранится СТОИМОСТЬ ЗАНЯТИЙ нашего Спорт Комплекса");
                            listBox.Items.Add("******************************************************************************************************************************");
                            listBox.Items.Add("");
                            listBox.Items.Add("id        Название занятия        Стоимость        Кол-во занятий");
                            listBox.Items.Add("");
                        }
                        break;
                }

                

                while (await dataReader.ReadAsync())
                {
                    switch (table)
                    {
                        case "TableInstructions":
                            tableInstructionsMethod();
                            break;
                        case "TableClients":
                            tableClientsMethod();
                            break;
                        case "TableSchedule":
                            tableScheduleMethod();
                            break;
                        case "TableCost":
                            tableCostMethod();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
            }
        }

        // *** Close *******************************************************************************************************

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (connect != null && connect.State != ConnectionState.Closed)
                connect.Close();
        }

        // *** button ***************************************************************************************************************************************************
        //***************************************************************************************************************************************************************
        //***************************************************************************************************************************************************************

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (connect != null && connect.State != ConnectionState.Closed)
                connect.Close();
            this.Close();
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            tableString = "TableInstructions";
            loadSql(tableString);          
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            tableString = "TableClients";
            loadSql(tableString);
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            tableString = "TableSchedule";
            loadSql(tableString);
        }

        private void btnCost_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            tableString = "TableCost";
            loadSql(tableString);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            btnClose.Visible = false;
            buttonIUD = "btnInsert";
            switchBtnICSC(false);
            switchBtnIUD(0, false);          
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnClose.Visible = false;
            label1.Text = "Не забудьте ввести код строки, которую хотите изменить";
            label1.Visible = true;
            buttonIUD = "btnUpdate";
            switchBtnICSC(false);
            switchBtnIUD(1, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnClose.Visible = false;
            label1.Text = "Введите код строки которую хотите удалить";
            label1.Visible = true;
            buttonIUD = "btnDelete";
            switchBtnICSC(false);
            switchBtnIUD(2, false);
        }

        // *** Close Insert ***

        private void btnIIClose_Click(object sender, EventArgs e)
        {
            btnClose.Visible = true;
            label1.Visible = false;
            lbPanelError.Visible = false;

            switchBtnICSC(true);
            switchBtnIUD(switchCloseIUD(), true);

            switchPanelIUD(false);
        }

        private void btnICClose_Click(object sender, EventArgs e)
        {
            btnClose.Visible = true;
            label1.Visible = false;
            lbPanelError.Visible = false;

            switchBtnICSC(true);
            switchBtnIUD(switchCloseIUD(), true);

            switchPanelIUD(false);
        }

        private void btnISClose_Click(object sender, EventArgs e)
        {
            btnClose.Visible = true;
            label1.Visible = false;
            lbPanelError.Visible = false;

            switchBtnICSC(true);
            switchBtnIUD(switchCloseIUD(), true);

            switchPanelIUD(false);
        }

        private void btnICostClose_Click(object sender, EventArgs e)
        {
            btnClose.Visible = true;
            label1.Visible = false;
            lbPanelError.Visible = false;

            switchBtnICSC(true);
            switchBtnIUD(switchCloseIUD(), true);

            switchPanelIUD(false);
        }

        // *** Registr ***
        private void btnOk_Click(object sender, EventArgs e)
        {
                if (tbLogin.Text == login && tbPass.Text == pass)
                {
                    panelRegistr.Visible = false;
                    btnOk.Visible = false;
                    tbLogin.Text = "";
                    tbPass.Text = "";

                    lableAdmin(true);
                    btnIUD(true);

                    admin = true;
                }
                else
                {
                    if (tbLogin.Text == "" || tbPass.Text == "")
                    {
                        lbError3.Visible = true;
                        lbError3.Text = "не все поля заполнены*";
                        tbColor(Color.Black);
                    }
                    else
                    {
                        lbError3.Visible = true;
                        lbError3.Text = "пароль неверный*";
                        tbColor(Color.Red);
                    }
                }
        }

        //Panel Instructions Ok
        private void btnIIOk_Click(object sender, EventArgs e)
        {
            switchButton();
        }
        //Panel Cost
        private void btnICostOk_Click(object sender, EventArgs e)
        {
            switchButton();
        }

        //PanelClients
        private void btnICOk_Click(object sender, EventArgs e)
        {
            switchButton();
        }

        //Panel Schedule
        private void btnISOk_Click(object sender, EventArgs e)
        {
            switchButton();
        }

        private void btnOkClose_Click(object sender, EventArgs e)
        {
            tbColor(Color.Black);

            panelRegistr.Visible = true;
            btnOk.Visible = true;

            lableAdmin(false);
            btnIUD(false);

            admin = false;
        }

        // **************************************************************************************************************************************************************
        //***************************************************************************************************************************************************************
        //***************************************************************************************************************************************************************

        //**** Fill Database ****************************************

        public void tableInstructionsMethod()
        {
            listBox.Items.Add(Convert.ToString(dataReader["id"]) + "    " +
                              Convert.ToString(dataReader["Фамилия"]) + "    " +
                              Convert.ToString(dataReader["Имя"]) + "    " +
                              Convert.ToString(dataReader["Отчество"]));
        }

        public void tableClientsMethod()
        {
            listBox.Items.Add(Convert.ToString(dataReader["id"]) + "    " +
                              Convert.ToString(dataReader["Фамилия"]) + "    " +
                              Convert.ToString(dataReader["Имя"]) + "    " +
                              Convert.ToString(dataReader["Отчество"]) + "    " +
                              Convert.ToString(dataReader["Дата"]) + "    " +
                              Convert.ToString(dataReader["Телефон"]) + "    " +
                              Convert.ToString(dataReader["Адрес"]));
            
        }

        public void tableScheduleMethod()
        {
            listBox.Items.Add(Convert.ToString(dataReader["id"]) + "    " +
                              Convert.ToString(dataReader["Группа"]) + "    " +
                              Convert.ToString(dataReader["Дата"]) + "    " +
                              Convert.ToString(dataReader["ВремяНачала"]) + "    " +
                              Convert.ToString(dataReader["ВремяОкончания"]));
            
        }

        public void tableCostMethod()
        {
            listBox.Items.Add(Convert.ToString(dataReader["id"]) + "    " +
                              Convert.ToString(dataReader["Название"]) + "    " +
                              Convert.ToString(dataReader["Стоимость"]) + "    " +
                              Convert.ToString(dataReader["Количество"]));

        }
        //************************************************

        public void btnIUD(bool f)
        {
            btnInsert.Visible = f;
            btnUpdate.Visible = f;
            btnDelete.Visible = f;
            btnOkClose.Visible = f;
        }

        public void tbColor(Color color)
        {
            tbLogin.ForeColor = color;
            tbPass.ForeColor = color;
        }

        public void lableAdmin(bool f)
        {
            if (f == true)
            {
                lbError.ForeColor = Color.Green;
                lbError.Text = "Вы вошли как администратор";
                lbError2.Text = "(вы можете добавлять, изменять и удалять информацию)";
                lbError3.Visible = false;
            }
            else
            {
                lbError.ForeColor = Color.Red;
                lbError.Text = "Вы находитесь в режиме пользователя*";
                lbError2.Text = "(вы можете только просматривать файлы)";
            }
        }

        public void outLbPanelError(bool f)
        {
            String text = "Информация успешно ";

            if (f)
            {              
                switch (buttonIUD)
                {
                    case "btnInsert": text = text + "добавлена в Базу данных: ";
                        break;
                    case "btnUpdate":
                        text = text + "изменена в Базе данных: ";
                        break;
                    case "btnDelete":
                        text = text + "удаленна из Базы данных: ";
                        break;
                }

                switch (tableString)
                {
                    case "TableInstructions": text = text + "[DBSportComplex.mdf/TableInstructions]";
                        break;
                    case "TableClients":
                        text = text + "[DBSportComplex.mdf/TableClients]";
                        break;
                    case "TableSchedule":
                        text = text + "[DBSportComplex.mdf/TableSchedule]";
                        break;
                    case "TableCost":
                        text = text + "[DBSportComplex.mdf/TableCost]";
                        break;
                }

                lbPanelError.Visible = true;
                lbPanelError.ForeColor = Color.Green;
                lbPanelError.Text = text;
            }
            else
            {
                lbPanelError.Visible = true;
                lbPanelError.ForeColor = Color.Red;
                lbPanelError.Text = "Не все поля заполнены*";
            }
        }

        //--  Switch  ----------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void switchBtnICSC(bool f)
        {
            switch (tableString)
            {
                case "TableInstructions":
                    {
                        btnInstructions.Enabled = f;
                        btnClients.Visible = f;
                        btnSchedule.Visible = f;
                        btnCost.Visible = f;
                    }
                    break;
                case "TableClients":
                    {
                        btnInstructions.Visible = f;
                        btnClients.Enabled = f;
                        btnSchedule.Visible = f;
                        btnCost.Visible = f;
                    }
                    break;
                case "TableSchedule":
                    {
                        btnInstructions.Visible = f;
                        btnClients.Visible = f;
                        btnSchedule.Enabled = f;
                        btnCost.Visible = f;
                    }
                    break;
                case "TableCost":
                    {
                        btnInstructions.Visible = f;
                        btnClients.Visible = f;
                        btnSchedule.Visible = f;
                        btnCost.Enabled = f;
                    }
                    break;
            }
        }

        public void switchBtnIUD(int i, bool f)
        {
            switch (i)
            {
                case 0:  //but Insert
                    {
                        btnInsert.Enabled = f;
                        btnUpdate.Visible = f;
                        btnDelete.Visible = f;
                        btnOkClose.Enabled = f;

                        switchPanelIUD(true);               
                    }
                    break;
                case 1:  //but Update
                    {
                        btnInsert.Visible = f;
                        btnUpdate.Enabled = f;
                        btnDelete.Visible = f;
                        btnOkClose.Enabled = f;

                        switchPanelIUD(true);
                    }
                    break;
                case 2:  //but Delete
                    {
                        btnInsert.Visible = f;
                        btnUpdate.Visible = f;
                        btnDelete.Enabled = f;
                        btnOkClose.Enabled = f;

                        switchPanelIUD(true);
                    }
                    break;
            }
        }

        public void switchPanelIUD(bool f)
        {
            switch (tableString)
            {
                case "TableInstructions":
                    {
                        panelInstructions.Visible = f; 
                        if (buttonIUD == "btnInsert")
                        {
                            tbIId.Visible = false;
                            lbIId.Visible = false;

                            tbIISurname.Visible = true;
                            tbIIName.Visible = true;
                            tbIIPat.Visible = true;

                            lbIISurname.Visible = true;
                            lbIIName.Visible = true;
                            lbIIPat.Visible = true;
                        }
                        if (buttonIUD == "btnUpdate")
                        {
                            tbIId.Visible = true;
                            lbIId.Visible = true;

                            tbIISurname.Visible = true;
                            tbIIName.Visible = true;
                            tbIIPat.Visible = true;

                            lbIISurname.Visible = true;
                            lbIIName.Visible = true;
                            lbIIPat.Visible = true;
                        }
                        if (buttonIUD == "btnDelete")
                        {
                            tbIId.Visible = true;
                            lbIId.Visible = true;

                            tbIISurname.Visible = false;
                            tbIIName.Visible = false;
                            tbIIPat.Visible = false;

                            lbIISurname.Visible = false;
                            lbIIName.Visible = false;
                            lbIIPat.Visible = false;
                        }
                    }
                    break;
                case "TableClients":
                    {
                        panelClients.Visible = f;
                        if (buttonIUD == "btnInsert")
                        {
                            tbCId.Visible = false;
                            lbCId.Visible = false;

                            tbICSurname.Visible = true;
                            tbICName.Visible = true;
                            tbICPat.Visible = true;
                            tbICDate.Visible = true;
                            tbICMobile.Visible = true;
                            tbICAddress.Visible = true;

                            lbICSurname.Visible = true;
                            lbICName.Visible = true;
                            lbICPat.Visible = true;
                            lbICDate.Visible = true;
                            lbICMobile.Visible = true;
                            lbICAddress.Visible = true;
                        }
                        if (buttonIUD == "btnUpdate")
                        {
                            tbCId.Visible = true;
                            lbCId.Visible = true;

                            tbICSurname.Visible = true;
                            tbICName.Visible = true;
                            tbICPat.Visible = true;
                            tbICDate.Visible = true;
                            tbICMobile.Visible = true;
                            tbICAddress.Visible = true;

                            lbICSurname.Visible = true;
                            lbICName.Visible = true;
                            lbICPat.Visible = true;
                            lbICDate.Visible = true;
                            lbICMobile.Visible = true;
                            lbICAddress.Visible = true;
                        }
                        if (buttonIUD == "btnDelete")
                        {
                            tbCId.Visible = true;
                            lbCId.Visible = true;

                            tbICSurname.Visible = false;
                            tbICName.Visible = false;
                            tbICPat.Visible = false;
                            tbICDate.Visible = false;
                            tbICMobile.Visible = false;
                            tbICAddress.Visible = false;

                            lbICSurname.Visible = false;
                            lbICName.Visible = false;
                            lbICPat.Visible = false;
                            lbICDate.Visible = false;
                            lbICMobile.Visible = false;
                            lbICAddress.Visible = false;
                        }
                    } 
                    
                    break;
                case "TableSchedule":
                    {
                        panelSchedule.Visible = f;
                        if (buttonIUD == "btnInsert")
                        {
                            tbSId.Visible = false;
                            lbSId.Visible = false;
                           
                            tbISId.Visible = true;
                            tbISDate.Visible = true;
                            tbISTimeStart.Visible = true;
                            tbISFinish.Visible = true;

                            lbISId.Visible = true;
                            lbISDate.Visible = true;
                            lbISTimeStart.Visible = true;
                            lbISTimeFinish.Visible = true;
                        } else
                        if (buttonIUD == "btnUpdate")
                        {
                            tbSId.Visible = true;
                            lbSId.Visible = true;

                            tbISId.Visible = true;
                            tbISDate.Visible = true;
                            tbISTimeStart.Visible = true;
                            tbISFinish.Visible = true;

                            lbISId.Visible = true;
                            lbISDate.Visible = true;
                            lbISTimeStart.Visible = true;
                            lbISTimeFinish.Visible = true;
                        } else
                        if (buttonIUD == "btnDelete")
                        {
                            tbSId.Visible = true;
                            lbSId.Visible = true;

                            tbISId.Visible = false;
                            tbISDate.Visible = false;
                            tbISTimeStart.Visible = false;
                            tbISFinish.Visible = false;

                            lbISId.Visible = false;
                            lbISDate.Visible = false;
                            lbISTimeStart.Visible = false;
                            lbISTimeFinish.Visible = false;
                        }
                    } 
                    break;
                case "TableCost":
                    {
                        panelCost.Visible = f;
                        if (buttonIUD == "btnInsert")
                        {
                            tbCostId.Visible = false;
                            lbCostId.Visible = false;

                            tpICostName.Visible = true;
                            tpICostCost.Visible = true;
                            tpICostCount.Visible = true;

                            lbICostName.Visible = true;
                            lbICostCost.Visible = true;
                            lbICostCount.Visible = true;
                        }
                        if (buttonIUD == "btnUpdate")
                        {
                            tbCostId.Visible = true;
                            lbCostId.Visible = true;

                            tpICostName.Visible = true;
                            tpICostCost.Visible = true;
                            tpICostCount.Visible = true;

                            lbICostName.Visible = true;
                            lbICostCost.Visible = true;
                            lbICostCount.Visible = true;
                        }
                        if (buttonIUD == "btnDelete")
                        {
                            tbCostId.Visible = true;
                            lbCostId.Visible = true;

                            tpICostName.Visible = false;
                            tpICostCost.Visible = false;
                            tpICostCount.Visible = false;

                            lbICostName.Visible = false;
                            lbICostCost.Visible = false;
                            lbICostCount.Visible = false;
                        }

                    }
                    break;
            }
        }

        public void switchClearPanelIUD()
        {
            switch (tableString)
            {
                case "TableInstructions":
                    {
                        tbIId.Text = "";
                        tbIISurname.Text = "";
                        tbIIName.Text = "";
                        tbIIPat.Text = "";
                    }
                    break;
                case "TableClients":
                    {
                        tbCId.Text = "";
                        tbICSurname.Text = "";
                        tbICName.Text = "";
                        tbICPat.Text = "";
                        tbICDate.Text = "";
                        tbICMobile.Text = "";
                        tbICAddress.Text = "";
                    }
                    break;
                case "TableSchedule":
                    {
                        tbSId.Text = "";
                        tbISId.Text = "";
                        tbISDate.Text = "";
                        tbISTimeStart.Text = "";
                        tbISFinish.Text = "";
                    }
                    break;
                case "TableCost":
                    {
                        tbCostId.Text = "";
                        tpICostName.Text = "";
                        tpICostCost.Text = "";
                        tpICostCount.Text = "";
                    }
                    break;
            }
        }  

        public void switchButton()
        {
            switch (buttonIUD)
            {
                case "btnInsert":
                    {
                        switchInsert();
                    }
                    break;
                case "btnUpdate":
                    {
                        switchUpdate();
                    }
                    break;
                case "btnDelete":
                    {
                        switchDelete();
                    }
                    break;
            }
        }

        public void switchInsert()
        {
            switch (tableString)
            {
                case "TableInstructions":
                    {
                        insertInstructions();
                    }
                break;
                case "TableClients":
                    {
                        insertClients();
                    }
                    break;
                case "TableSchedule":
                    {
                        insertSchedule();
                    }
                    break;
                case "TableCost":
                    {
                        insertCost();
                    }
                    break;

            }
        }

        public void switchUpdate()
        {
            switch (tableString)
            {
                case "TableInstructions":
                    {
                        updateInstructions();
                    }
                    break;
                case "TableClients":
                    {
                        updateClients();
                    }
                    break;
                case "TableSchedule":
                    {
                        updateSchedule();
                    }
                    break;
                case "TableCost":
                    {
                        updateCost();
                    }
                    break;
            }
        }

        public void switchDelete()
        {
            switch (tableString)
            {
                case "TableInstructions":
                    {
                        deleteInstructions();
                    }
                    break;
                case "TableClients":
                    {
                        deleteClients();
                    }
                    break;
                case "TableSchedule":
                    {
                        deleteSchedule();
                    }
                    break;
                case "TableCost":
                    {
                        deleteCost();
                    }
                    break;
            }
        }

        public int switchCloseIUD()
        {
            int i = 0;
            switch (buttonIUD)
            {
                case "btnInsert":
                    i = 0;
                    break;
                case "btnUpdate":
                    i = 1;
                    break;
                case "btnDelete":
                    i = 2;
                    break;
            }
            return i;
        }
        //*** Insert ***

        public async void insertInstructions()
        {
            if (!string.IsNullOrEmpty(tbIISurname.Text) && !string.IsNullOrEmpty(tbIISurname.Text) &&
                !string.IsNullOrEmpty(tbIIName.Text) && !string.IsNullOrEmpty(tbIIName.Text) &&
                !string.IsNullOrEmpty(tbIIPat.Text) && !string.IsNullOrEmpty(tbIIPat.Text))
            {
                command = new SqlCommand("INSERT INTO [TableInstructions] (Фамилия, Имя, Отчество)VALUES(@Фамилия, @Имя, @Отчество)", connect);

                command.Parameters.AddWithValue("Фамилия", tbIISurname.Text);
                command.Parameters.AddWithValue("Имя", tbIIName.Text);
                command.Parameters.AddWithValue("Отчество", tbIIPat.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }          
        }

        public async void insertClients()
        {

            if (!string.IsNullOrEmpty(tbICSurname.Text) && !string.IsNullOrEmpty(tbICSurname.Text) &&
                !string.IsNullOrEmpty(tbICName.Text) && !string.IsNullOrEmpty(tbICName.Text) &&
                !string.IsNullOrEmpty(tbICPat.Text) && !string.IsNullOrEmpty(tbICPat.Text) &&
                !string.IsNullOrEmpty(tbICDate.Text) && !string.IsNullOrEmpty(tbICDate.Text) &&
                !string.IsNullOrEmpty(tbICMobile.Text) && !string.IsNullOrEmpty(tbICMobile.Text) &&
                !string.IsNullOrEmpty(tbICAddress.Text) && !string.IsNullOrEmpty(tbICAddress.Text))
            {
                command = new SqlCommand("INSERT INTO [TableClients] (Фамилия, Имя, Отчество, Дата, Телефон, Адрес)VALUES(@Фамилия, @Имя, @Отчество, @Дата, @Телефон, @Адрес)", connect);

                command.Parameters.AddWithValue("Фамилия", tbICSurname.Text);
                command.Parameters.AddWithValue("Имя", tbICName.Text);
                command.Parameters.AddWithValue("Отчество", tbICPat.Text);
                command.Parameters.AddWithValue("Дата", tbICDate.Text);
                command.Parameters.AddWithValue("Телефон", tbICMobile.Text);
                command.Parameters.AddWithValue("Адрес", tbICAddress.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void insertSchedule()
        {
            if (!string.IsNullOrEmpty(tbISId.Text) && !string.IsNullOrEmpty(tbISId.Text) &&
                !string.IsNullOrEmpty(tbISDate.Text) && !string.IsNullOrEmpty(tbISDate.Text) &&
                !string.IsNullOrEmpty(tbISTimeStart.Text) && !string.IsNullOrEmpty(tbISTimeStart.Text) &&
                !string.IsNullOrEmpty(tbISFinish.Text) && !string.IsNullOrEmpty(tbISFinish.Text))
            {
                command = new SqlCommand("INSERT INTO [TableSchedule] (Группа, Дата, ВремяНачала, ВремяОкончания)VALUES(@Группа, @Дата, @ВремяНачала, @ВремяОкончания)", connect);

                command.Parameters.AddWithValue("Группа", tbISId.Text);
                command.Parameters.AddWithValue("Дата", tbISDate.Text);
                command.Parameters.AddWithValue("ВремяНачала", tbISTimeStart.Text);
                command.Parameters.AddWithValue("ВремяОкончания", tbISFinish.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void insertCost()
        {
            if (!string.IsNullOrEmpty(tpICostName.Text) && !string.IsNullOrEmpty(tpICostName.Text) &&
                !string.IsNullOrEmpty(tpICostCost.Text) && !string.IsNullOrEmpty(tpICostCost.Text) &&
                !string.IsNullOrEmpty(tpICostCount.Text) && !string.IsNullOrEmpty(tpICostCount.Text))
            {
                command = new SqlCommand("INSERT INTO [TableCost] (Название, Стоимость, Количество)VALUES(@Название, @Стоимость, @Количество)", connect);

                command.Parameters.AddWithValue("Название", tpICostName.Text);
                command.Parameters.AddWithValue("Стоимость", tpICostCost.Text);
                command.Parameters.AddWithValue("Количество", tpICostCount.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        //*** Update ***

        public async void updateInstructions()
        {

            if (!string.IsNullOrEmpty(tbIId.Text) && !string.IsNullOrEmpty(tbIId.Text) &&
                !string.IsNullOrEmpty(tbIISurname.Text) && !string.IsNullOrEmpty(tbIISurname.Text) &&
                !string.IsNullOrEmpty(tbIIName.Text) && !string.IsNullOrEmpty(tbIIName.Text) &&
                !string.IsNullOrEmpty(tbIIPat.Text) && !string.IsNullOrEmpty(tbIIPat.Text))
            {
                command = new SqlCommand("UPDATE [TableInstructions] SET [Фамилия]=@Фамилия, [Имя]=@Имя, [Отчество]=@Отчество WHERE [Id]=@Id", connect);

                command.Parameters.AddWithValue("Id", tbIId.Text);
                command.Parameters.AddWithValue("Фамилия", tbIISurname.Text);
                command.Parameters.AddWithValue("Имя", tbIIName.Text);
                command.Parameters.AddWithValue("Отчество", tbIIPat.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void updateClients()
        {

            if (!string.IsNullOrEmpty(tbCId.Text) && !string.IsNullOrEmpty(tbCId.Text) &&
                !string.IsNullOrEmpty(tbICSurname.Text) && !string.IsNullOrEmpty(tbICSurname.Text) &&
                !string.IsNullOrEmpty(tbICName.Text) && !string.IsNullOrEmpty(tbICName.Text) &&
                !string.IsNullOrEmpty(tbICPat.Text) && !string.IsNullOrEmpty(tbICPat.Text) &&
                !string.IsNullOrEmpty(tbICDate.Text) && !string.IsNullOrEmpty(tbICDate.Text) &&
                !string.IsNullOrEmpty(tbICMobile.Text) && !string.IsNullOrEmpty(tbICMobile.Text) &&
                !string.IsNullOrEmpty(tbICAddress.Text) && !string.IsNullOrEmpty(tbICAddress.Text))
            {
                command = new SqlCommand("UPDATE [TableClients] SET [Фамилия]=@Фамилия, [Имя]=@Имя, [Отчество]=@Отчество, [Дата]=@Дата, [Телефон]=@Телефон, [Адрес]=@Адрес WHERE [Id]=@Id", connect);

                command.Parameters.AddWithValue("Id", tbCId.Text);
                command.Parameters.AddWithValue("Фамилия", tbICSurname.Text); 
                command.Parameters.AddWithValue("Имя", tbICName.Text);
                command.Parameters.AddWithValue("Отчество", tbICPat.Text);
                command.Parameters.AddWithValue("Дата", tbICDate.Text);
                command.Parameters.AddWithValue("Телефон", tbICMobile.Text);
                command.Parameters.AddWithValue("Адрес", tbICAddress.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void updateSchedule()
        {

            if (!string.IsNullOrEmpty(tbSId.Text) && !string.IsNullOrEmpty(tbSId.Text) &&
                !string.IsNullOrEmpty(tbISId.Text) && !string.IsNullOrEmpty(tbISId.Text) &&
                !string.IsNullOrEmpty(tbISDate.Text) && !string.IsNullOrEmpty(tbISDate.Text) &&
                !string.IsNullOrEmpty(tbISTimeStart.Text) && !string.IsNullOrEmpty(tbISTimeStart.Text) &&
                !string.IsNullOrEmpty(tbISFinish.Text) && !string.IsNullOrEmpty(tbISFinish.Text))
            {
                command = new SqlCommand("UPDATE [TableSchedule] SET [Группа]=@Группа, [Дата]=@Дата, [ВремяНачала]=@ВремяНачала, [ВремяОкончания]=@ВремяОкончания WHERE [Id]=@Id", connect);

                command.Parameters.AddWithValue("Id", tbSId.Text);
                command.Parameters.AddWithValue("Группа", tbISId.Text);
                command.Parameters.AddWithValue("Дата", tbISDate.Text);
                command.Parameters.AddWithValue("ВремяНачала", tbISTimeStart.Text);
                command.Parameters.AddWithValue("ВремяОкончания", tbISFinish.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void updateCost()
        {

            if (!string.IsNullOrEmpty(tbCostId.Text) && !string.IsNullOrEmpty(tbCostId.Text) &&
                !string.IsNullOrEmpty(tpICostName.Text) && !string.IsNullOrEmpty(tpICostName.Text) &&
                !string.IsNullOrEmpty(tpICostCost.Text) && !string.IsNullOrEmpty(tpICostCost.Text) &&
                !string.IsNullOrEmpty(tpICostCount.Text) && !string.IsNullOrEmpty(tpICostCount.Text))
            {
                command = new SqlCommand("UPDATE [TableCost] SET [Название]=@Название, [Стоимость]=@Стоимость, [Количество]=@Количество WHERE [Id]=@Id", connect);

                command.Parameters.AddWithValue("Id", tbCostId.Text);
                command.Parameters.AddWithValue("Название", tpICostName.Text);
                command.Parameters.AddWithValue("Стоимость", tpICostCost.Text);
                command.Parameters.AddWithValue("Количество", tpICostCount.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void deleteInstructions()
        {
            if (!string.IsNullOrEmpty(tbIId.Text) && !string.IsNullOrEmpty(tbIId.Text))
            {
                command = new SqlCommand("DELETE FROM [TableInstructions] WHERE [ID]=@Id", connect);

                command.Parameters.AddWithValue("Id", tbIId.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void deleteClients()
        {
            if (!string.IsNullOrEmpty(tbCId.Text) && !string.IsNullOrEmpty(tbCId.Text))
            {
                command = new SqlCommand("DELETE FROM [TableClients] WHERE [ID]=@Id", connect);

                command.Parameters.AddWithValue("Id", tbCId.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void deleteSchedule()
        {
            if (!string.IsNullOrEmpty(tbSId.Text) && !string.IsNullOrEmpty(tbSId.Text))
            {
                command = new SqlCommand("DELETE FROM [TableSchedule] WHERE [ID]=@Id", connect);

                command.Parameters.AddWithValue("Id", tbSId.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        public async void deleteCost()
        {
            if (!string.IsNullOrEmpty(tbCostId.Text) && !string.IsNullOrEmpty(tbCostId.Text))
            {
                command = new SqlCommand("DELETE FROM [TableCost] WHERE [ID]=@Id", connect);

                command.Parameters.AddWithValue("Id", tbCostId.Text);

                await command.ExecuteNonQueryAsync();

                listBox.Items.Clear();
                loadSql(tableString);

                switchClearPanelIUD();
                outLbPanelError(true);
            }
            else
            {
                outLbPanelError(false);
            }
        }

        private void сменитьПарольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (admin == true) MessageBox.Show("Чтоб сменить пароль, нужно выйти с режима администратора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                visibleAll(false);
             
                panelLP.Visible = true;

             }
        }

        public void visibleAll(bool f)
        {
            listBox.Visible = f;
            panelRegistr.Visible = f;
            btnInstructions.Visible = f;
            btnClients.Visible = f;
            btnSchedule.Visible = f;
            btnCost.Visible = f;
            btnOk.Visible = f;
            lbError.Visible = f;
            lbError2.Visible = f;
        }

        private void btnLPOk_Click(object sender, EventArgs e)
        {
            if (tbOldLogin.Text == login && tbOldPass.Text == pass)
            {
                login = tbNewLogin.Text;
                pass = tbNewPass.Text;

                tbOldLogin.Text = "";
                tbOldPass.Text = "";

                tbNewLogin.Text = "";
                tbNewPass.Text = "";

                MessageBox.Show("Новый пароль сохранён. ", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                panelLP.Visible = false;
                visibleAll(true);
            }
            else
            {
                MessageBox.Show("Старый пароль не верный.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLPClose_Click(object sender, EventArgs e)
        {
            tbOldLogin.Text = "";
            tbOldPass.Text = "";

            tbNewLogin.Text = "";
            tbNewPass.Text = "";

            panelLP.Visible = false;
            visibleAll(true);
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа подключенна к базе данных [DBSportComplex.mdf]\n"
                + "Данная база данных имеет четыре таблицы написаных на языке SQL: \n" 
                + "[TableInstructions] - в этой таблице находится список инструкторов. \n" 
                + "[TableClients] - в этой таблице находится список клиентов. \n"
                + "[TableSchedule] - в этой таблице находится расписание занятий. \n"
                + "[TableCost] - в этой таблице находится стоимость тренировок\n\n"
                + "Выполнила проект Дарья Зюзина(с)\n"
                + "05.05.2017  ", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void расположениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelOne.Visible = true;
            visibleAll(false);         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            panelOne.Visible = false;
            visibleAll(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                fileAddress = textBox1.Text;
                loadSql(tableString);

                textBox1.Text = "";
                panelOne.Visible = false;
                visibleAll(true);
            }
            else
            {
                MessageBox.Show("Поле не должно быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void открытьСтандартнуюБазуДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadSql(tableString);
        }
    }
}
