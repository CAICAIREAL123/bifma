using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabApp_ServerVersion
{
    class partIndexes
    {
        Database database = new Database();
        string fileRootPath = @"C:\Users\caiyu\Desktop\chuquwan\d3test\partindex\";
        public bool newPart(string partType, string partName, string configName, string paintIndex)
        {
            string directoryMasterPath = fileRootPath/*@"C:\Users\caiyu\Desktop\d3test\master\"*/ + partType.ToLower() + "\\";
            if (File.Exists(directoryMasterPath + partName + ".SLDPRT"))
            {
                List<string> readInfo = readIndexFromSW(configName, partName, partType, directoryMasterPath + partName + ".SLDPRT");
                string sql = "";

                sql = "insert into partindexes(partname, materialType, materialThickness, surfaceArea, cubicBoundingBoxWHD, " +
                "accessory, stockQuant, cutProgram, punch, bendNum, salvagniniBool, type, description, flattenSheetInfo, cutLength, cutOut, paint) " +
                $"value('{partName + "-" + configName}', '{readInfo[1]}', '{readInfo[2]}', '{readInfo[3]}', " +
                $"'{readInfo[4]}', '{readInfo[5]}', '{readInfo[6]}', '{readInfo[9]}', '{readInfo[10]}', '{readInfo[11]}', " +
                $"'{readInfo[12]}', '{readInfo[13]}', '{readInfo[14]}', '{readInfo[7]}', '{readInfo[8]}', '{readInfo[15]}', '{paintIndex}')";

                database.mySqlExecuteQuery(sql);
                database.CloseConnection();
            }
            else
            {
                string sql = $"insert into partindexes(partname, type) value('{partName + "-" + configName}', '{partType}')";
                database.mySqlExecuteQuery(sql);
                database.CloseConnection();
            }
            return true;
        }
        public bool updatePart(string partType, string partName, string configName, string paintIndex)
        {

            string directoryMasterPath = fileRootPath/*@"C:\Users\caiyu\Desktop\d3test\master\"*/ + partType.ToLower() + "\\";
            if (File.Exists(directoryMasterPath + partName + ".SLDPRT"))
            {
                List<string> readInfo = readIndexFromSW(configName, partName, partType, directoryMasterPath + partName + ".SLDPRT");
                List<string> sqls = new List<string>();
                sqls.Add("set sql_safe_updates = 0;");
                sqls.Add($"update partindexes set materialType = '{readInfo[1]}', materialThickness= '{readInfo[2]}', surfaceArea = '{readInfo[3]}', " +
                    $"cubicBoundingBoxWHD ='{readInfo[4]}', accessory = '{readInfo[5]}', stockQuant = '{readInfo[6]}', cutProgram = '{readInfo[9]}'," +
                    $"punch = '{readInfo[10]}', bendNum = '{readInfo[11]}', salvagniniBool = '{readInfo[12]}', description = '{readInfo[14]}'," +
                    $"flattenSheetInfo =  '{readInfo[7]}', cutLength = '{readInfo[8]}', cutOut = '{readInfo[15]}', paint = '{paintIndex}'" +
                    $" where partname = '{partName + "-" + configName}'");
                database.mySqlExecuteSomeQuery(sqls);
                database.CloseConnection();
                
            }
            else
            {
                MessageBox.Show("No file was found.");
            }
            return true;
        }

        public static Dictionary<string, string> boolToInt = new Dictionary<string, string>()
        {
            {"true", "1" }, {"false", "0"}, {"", "0"}
        };
        public static string openPath = @"";
        SldWorks swApp;
        ModelDoc2 swModel;
        Configuration swConf;
        ModelDocExtension swModelDocExt;
        CustomPropertyManager swCustMgr;
        SelectionMgr swSelMgr;
        Feature swFeature;
        bool status;
        int num;
        string configValue;
        object _boxIndex;
        double mtoinch = 100 / 2.54;
        public List<string> readIndexFromSW(string configurationName, string partname, string type, string filePath)
        {
            string materialType = "";
            string materialThickness = "";
            string surfaceArea = "";
            string stockQuant = "0";
            string Programs = null;
            string flattenSheetSize_l = "";
            string flattenSheetSize_w = "";
            string flattenSheetSizeString = "";
            string cutLength_outer = "";
            string cutLength_inner = "";
            string cutLengthString = "";
            string punch = "false";
            string bendNum = "0";
            string salvagnini = "false";
            string description = "";
            string cutOut = "";
            activeApp();
            openPartFile(filePath);
            status = swModel.ShowConfiguration2(configurationName);
            ////configuration specific 
            ///configuration name = partname.split('-')[-1]
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swModelDocExt = swModel.Extension;
            swCustMgr = swModelDocExt.CustomPropertyManager[configurationName];
            status = swCustMgr.Get4("Stock Quant", false, out configValue, out stockQuant);
            status = swCustMgr.Get4("Program", false, out configValue, out Programs);

            status = swCustMgr.Get4("Punch", false, out configValue, out punch);
            punch = boolToInt[punch];
            status = swCustMgr.Get4("Salvagnini", false, out configValue, out salvagnini);
            salvagnini = boolToInt[salvagnini];
            status = swCustMgr.Get4("Description", false, out configValue, out description);
            ////cool
            ////cutlist properties
            ///swModel = (ModelDoc2)swApp.ActiveDoc;
            swModelDocExt = swModel.Extension;
            swSelMgr = swModel.SelectionManager;
            status = false;
            while (status == false)
            {
                status = swModelDocExt.SelectByID2("Sheet<1>", "SUBWELDFOLDER", 0, 0, 0, true, 0, null, 0);
            }
            swFeature = swSelMgr.GetSelectedObject5(1);
            swCustMgr = swFeature.CustomPropertyManager;
            status = swCustMgr.Get4("MATERIAL", false, out configValue, out materialType);
            if (materialType == "AISI 1020 Steel, Cold Rolled")
            {
                materialType = "CRS";
            }
            else if (materialType == "AISI 304")
            {
                materialType = "304SS";
            }
            status = swCustMgr.Get4("Sheet Metal Thickness", false, out configValue, out materialThickness);
            status = swCustMgr.Get4("Bounding Box Area-Blank", false, out configValue, out surfaceArea);
            status = swCustMgr.Get4("Bends", false, out configValue, out bendNum);
            status = swCustMgr.Get4("Bounding Box Length", false, out configValue, out flattenSheetSize_l);
            status = swCustMgr.Get4("Bounding Box Width", false, out configValue, out flattenSheetSize_w);
            flattenSheetSizeString = flattenSheetSize_l + "," + flattenSheetSize_w;
            status = swCustMgr.Get4("Cutting Length-Outer", false, out configValue, out cutLength_outer);
            status = swCustMgr.Get4("Cutting Length-Inner", false, out configValue, out cutLength_inner);
            status = swCustMgr.Get4("Cut Outs", false, out configValue, out cutOut);
            cutLengthString = cutLength_outer + "," + cutLength_inner;

            ////cool
            ////boundingbox
            swSelMgr = swModel.SelectionManager;
            status = swModelDocExt.SelectByID2("Bounding Box", "SKETCH", 0, 0, 0, false, 0, null, 0);
            swFeature = swSelMgr.GetSelectedObject6(1, -1);
            status = swFeature.GetBox(ref _boxIndex);
            double[] boxIndex = (double[])_boxIndex;
            List<double> boxWHD = new List<double>()
            {
                (boxIndex[3]-boxIndex[0])*mtoinch, (boxIndex[4]-boxIndex[1])*mtoinch, (boxIndex[5]-boxIndex[2])*mtoinch
            };
            string cubicBoxString = boxWHD[0].ToString() + "," + boxWHD[1].ToString() + "," + boxWHD[2].ToString();
            ////cool
            ///////no accessory here 他说他还没想好
            //string sql = "";
            //if (Programs != null)
            //{
            //    sql = "insert into partindexes(partname, materialType, materialThickness, surfaceArea, cubicBoundingBoxWHD, " +
            //        "accessory, stockQuant, cutProgram, punch, bendNum, salvagniniBool, type, description) " +
            //        $"value('{partname}', '{materialType}', '{materialThickness.ToString()}', '{surfaceArea}', " +
            //        $"'{cubicBoxString}', '', '{stockQuant}', '{Programs}', '{punch}', '{bendNum}', '{salvagnini}', '{type}', '{description}')";
            //}
            //else
            //{
            //    sql = "insert into partindexes(partname, materialType, materialThickness, surfaceArea, cubicBoundingBoxWHD, " +
            //       "accessory, stockQuant, flattenSheetInfo, cutLength, punch, bendNum, salvagniniBool, type, description) " +
            //       $"value('{partname}', '{materialType}', '{materialThickness.ToString()}', '{surfaceArea}', " +
            //       $"'{cubicBoxString}', '', '{stockQuant}', '{flattenSheetSizeString}', '{cutLengthString}', '{punch}', " +
            //       $"'{bendNum}', '{salvagnini}', '{type}', '{description}')";
            //}
            //database.mySqlExecuteQuery(sql);
            //database.CloseConnection();

            killSW();
            if (stockQuant == "")
            {
                stockQuant = "0";
            }
            if (punch == "")
            {
                punch = "0";
            }
            if (bendNum == "")
            {
                bendNum = "0";
            }
            if (salvagnini == "")
            {
                salvagnini = "false";
            }

            if (Convert.ToDouble(materialThickness) >= 0.1)
            {
                materialThickness = "0.105";
            }
            else if (Convert.ToDouble(materialThickness) >= 0.07)
            {
                materialThickness = "0.075";
            }
            else if (Convert.ToDouble(materialThickness) >= 0.059)
            {
                materialThickness = "0.060";
            }
            else if (Convert.ToDouble(materialThickness) >= 0.047)
            {
                materialThickness = "0.048";
            }
            else if (Convert.ToDouble(materialThickness) >= 0.035)
            {
                materialThickness = "0.036";
            }
            else
            {
                materialThickness = "0.03";
            }

            return new List<string>() { partname, materialType, materialThickness, surfaceArea, cubicBoxString, "", stockQuant, flattenSheetSizeString, cutLengthString, 
                ////////////////////////0         1             2                  3            4               5   6           7                       8
                Programs, punch, bendNum, salvagnini, type, description, cutOut};
            ////9         10     11       12          13    14           15
        }

        
        public void activeApp()
        {
            swApp = Activator.CreateInstance(Type.GetTypeFromProgID("sldworks.application")) as SldWorks;
            swApp.Visible = true;
        }
        public void openPartFile(string filePath)
        {
            swModel = (ModelDoc2)(swApp.OpenDoc6(filePath, 1, 0, "", 0, 0));
            swModelDocExt = swModel.Extension;
        }
        public void killSW()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("SLDWORKS");
                foreach (Process process in processes)
                {
                    process.CloseMainWindow();
                    process.Kill();
                }
            }
            catch { }
            swApp = null;
            swModel = null;
            swModelDocExt = null;
            swFeature = null;
            swConf = null;
            swCustMgr = null;
            swSelMgr = null;
            status = false;
            num = 0;
            configValue = null;
            _boxIndex = null;
        }
    }
}
