using Microsoft.Data.SqlClient;
using PROGPOEP2_Library_ST10083450;
using PROGPOEP2_Library_ST10083450.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROGPOEP2_ST10083450
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   ListUtil ListUtil = new ListUtil();
        St10083450ProgPoeContext db = new St10083450ProgPoeContext();
        public static string modName;
        public string modCode;
        public int numCreds;
        public int numClassHrs;
        public static int numOfWeeks;
        public DateTime semDate;
        public double numSelfStudyHrs;
        public DateTime selfStudyDate;
        public String mCode;
        public float rStudyHrs;
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            foreach (Module modList in db.Modules.Where(m => m.UserId == ListUtil.usersLoggedIn[0].UserId).ToList())
            {
                cbModuleCode.Items.Add(modList.ModCode);
            } 
           
        }

        public SqlConnection sqlConnection;

        private void btnAddSemester_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                Semester semester = new Semester();
                numOfWeeks = Convert.ToInt32(tbNumberOfWeeks.Text);
                semDate = Convert.ToDateTime(dpSemesterStartDate.Text);

               db.Semesters.Add(new Semester { UserId =  ListUtil.usersLoggedIn[0].UserId,NumWeeks = numOfWeeks, SemesterStartDate = semDate });
                db.SaveChanges();
                MessageBox.Show("Semester Info recorded");
            }
            catch
            {
                MessageBox.Show("Please enter the values in correctly");
                tbNumberOfWeeks.Clear();
            }
        }




            

        private void btnAddModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                modName = tbModuleName.Text;
                modCode = tbModuleCode.Text;
                numCreds = Convert.ToInt32(tbNumberOfCredits.Text);
                numClassHrs = Convert.ToInt32(tbClassHours.Text);
                rStudyHrs = Calculations.requiredStudyHrs(numCreds, numClassHrs, numOfWeeks);

                db.Modules.Add(new Module { UserId = ListUtil.usersLoggedIn[0].UserId, ModName = modName, ModCode = modCode, ModCredits = numCreds, ClassHours = numClassHrs, RemainingStudyHrs = rStudyHrs });
                db.SaveChanges();
                cbModuleCode.Items.Clear();
                foreach (Module modList in db.Modules.Where(m => m.UserId == ListUtil.usersLoggedIn[0].UserId).ToList())
                {
                    cbModuleCode.Items.Add(modList.ModCode);
                }
                
               //Module modList = db.Modules.Where(m => m.UserId == ListUtil.usersLoggedIn[0].UserId).ToList();
               
                MessageBox.Show("Module Added!!");
                tbModuleName.Clear();
                tbModuleCode.Clear();
                tbClassHours.Clear();
                tbNumberOfCredits.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbModuleName.Clear();
                tbModuleCode.Clear();
                tbClassHours.Clear();
                tbNumberOfCredits.Clear();
            }
        }

        private void btnAddSelfStudyHours_Click(object sender, RoutedEventArgs e)
        {
            Calculations calculations = new Calculations();
            try
            {
                numSelfStudyHrs = Convert.ToDouble(tbNumberOfSelfStudyHours.Text);
                selfStudyDate = Convert.ToDateTime(dpDateOfSelfStudy.Text);
                mCode = cbModuleCode.Text;

                //code attribution
                //this method was taken from CodeLikeaDev
                //https://codelikeadev.com/blog/find-item-in-list-csharp
                //Sameer Saini

                //LINQs used to find Module Code
                
                List<Module> currentUserMod = db.Modules.Where(m => m.UserId == ListUtil.usersLoggedIn[0].UserId).ToList();
                Module findMod = currentUserMod.FirstOrDefault(m => m.ModCode == mCode);
              findMod.RemainingStudyHrs = Calculations.SelfStudyCalc(numSelfStudyHrs, findMod.RemainingStudyHrs);
                 
                MessageBox.Show("Self Study Hours Recorded");
                tbNumberOfSelfStudyHours.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                tbNumberOfSelfStudyHours.Clear();
            }
        }
        //this method is used to update the datagrid when the button is pressed
        private void btnUpdateDisplay_Click(object sender, RoutedEventArgs e)
        {
            //code attribution
            //this method was taken from AppsLoveWorld
            //https://www.appsloveworld.com/csharp/100/1671/how-to-combine-multiple-lists-and-use-as-a-gridview-datasource?expand_article=1
            //Norberto Escobar
            //https://stackoverflow.com/users/4783509/norberto-escobar
            int currentUserID = ListUtil.usersLoggedIn[0].UserId;

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("moduleName");
            dataTable.Columns.Add("moduleCode");
            dataTable.Columns.Add("numberCredits");
            dataTable.Columns.Add("numClassHours");
            dataTable.Columns.Add("remainingStudyHours");

            foreach (Module module in db.Modules.Where(m => m.UserId == currentUserID))
            {
                dataTable.Rows.Add(module.ModName, module.ModCode, module.ModCredits, module.ClassHours, module.RemainingStudyHrs);
            }

            myDataGrid.ItemsSource = dataTable.DefaultView;

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        { 
            ListUtil listUtil = new ListUtil();
            listUtil.ClearList();

            Registration_and_Login registration_And_Login = new Registration_and_Login();

            this.Visibility = Visibility.Hidden;
            registration_And_Login.ShowDialog();
            this.Visibility = Visibility.Visible;
            
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          
            Application.Current.Shutdown();
            
        }

        private void btnCheckSemester_Click(object sender, RoutedEventArgs e)
        {
            Semester checkWeeks = db.Semesters.FirstOrDefault(s => s.UserId == ListUtil.usersLoggedIn[0].UserId);
            if (checkWeeks != null)
            {
                numOfWeeks = checkWeeks.NumWeeks;
                
                MessageBox.Show("Semester Data is already recorded");
               

                gridCheckSemester.Visibility = Visibility.Hidden;
                string tabtoswitchto = "Add Module";
                foreach  (TabItem tab in myTabControl.Items)
                {
                    if (tab.Header.ToString() == tabtoswitchto)
                    {
                        myTabControl.SelectedItem = tab;
                    }
                }


            }
        }
    }
}




