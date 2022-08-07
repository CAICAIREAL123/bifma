using MySql.Data.MySqlClient;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class SWFunction
    {
        Database database = new Database();
        //VisualInfo vsInfo = new VisualInfo();
        partIndexes pind = new partIndexes();
        string masterPath = @"C:\Users\yue\Desktop\d3test\" + "master\\";
        string libraryPath = @"C:\Users\yue\Desktop\d3test\" + "library\\";
        string bifmapath = @"C:\Users\yue\Desktop\d3test\cabinet";
        string pdfPath = @"C:\Users\yue\Dropbox\PDF\";
        string buildMasterPath = @"C:\Users\yue\Desktop\d3test\build\master\";
        string buildLibraryPath = @"C:\Users\yue\Desktop\d3test\build\library\";
        double mToInch = 39.3700787;
        int warning = 0;
        int errors = 0;

        public static string fileName = "";
        SldWorks swApp;
        ModelDoc2 swModel;
        AssemblyDoc swAssembly;
        ModelDocExtension swModelExt;
        Feature swFeature;
        Dimension myDimension;
        PackAndGo swPackandGo;
        DrawingDoc swDrawing;
        TableAnnotation swTable;
        BStrWrapper[] pgSetFileNames;
        bool boolStatus;
        public void activeApp()
        {
            killSW();
            swApp = Activator.CreateInstance(Type.GetTypeFromProgID("sldworks.application")) as SldWorks;
            swApp.Visible = true;
            fileName = "";
        }
        public void newAssembly(double w, double h, string path, string name)
        {
            openAssemblyFile(masterPath + "1.SLDASM");
            swApp.ActivateDoc2("1.SLDASM", false, 0);
            swAssembly = (AssemblyDoc)swModel;
            packandgo(path + name + "\\", name, "1", false);
            killSW();
            activeApp();
            openAssemblyFile(path + name + "\\" + name + ".SLDASM");
            int featCount = swModel.GetFeatureCount();
            for (int i = featCount; i > 0; i--)
            {
                swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - i);
                if ((swFeature) != null)
                {
                    string temp_featName_ori = swFeature.Name;
                    string temp_featName = swFeature.Name;
                    if (temp_featName_ori == "widthHeight")
                    {
                        changeDimension("width@widthHeight", Convert.ToDouble(w / mToInch));
                        changeDimension("height@widthHeight", Convert.ToDouble(h / mToInch));
                        reBuild();
                    }
                }
            }
        }
        public void openPartFile(string filePath)
        {
            swModel = (ModelDoc2)(swApp.OpenDoc6(filePath, 1, 0, "", 0, 0));
            string[] filePathSplit = filePath.Split('\\');
            fileName = filePathSplit[filePathSplit.Count() - 1];
        }
        public void openAssemblyFile(string filePath)
        {
            swModel = ((ModelDoc2)(swApp.OpenDoc6(filePath, 2, 0, "", 0, 0)));
            string[] filePathSplit = filePath.Split('/');
            fileName = filePathSplit[filePathSplit.Count() - 1];
        }
        public void openDrawingFile(string filePath)
        {
            swModel = ((ModelDoc2)(swApp.OpenDoc6(filePath, 3, 0, "", 0, 0)));
        }
        public void changeDimension(string sketch, double targetSize)
        {
            swModel.ClearSelection2(true);
            boolStatus = swModel.Extension.SelectByID2(sketch, "SKETCH", 0, 0, 0, false, 0, null, 0);
            swModel.EditSketch();
            myDimension = ((Dimension)(swModel.Parameter(sketch)));
            myDimension.SystemValue = targetSize;
        }
        public void changeDimension_ASM(string sketch, string paraName, double targetSize)
        {
            swModel.ClearSelection2(true);
            boolStatus = swModel.Extension.SelectByID2(sketch, "SKETCH", 0, 0, 0, false, 0, null, 0);
            boolStatus = swModel.Extension.SelectByID2(paraName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            swModel.EditSketch();
            myDimension = ((Dimension)(swModel.Parameter(paraName)));
            myDimension.SystemValue = targetSize;
        }
        public void reBuild()
        {
            swModelExt = swModel.Extension;
            boolStatus = swModelExt.ForceRebuildAll();
        }
        public void reBuildThis()
        {
            //swModelExt = swModel.Extension;
            boolStatus = swModel.EditRebuild3();
        }
        public void savePartAs(string path, string name)
        {
            if (Directory.Exists(path))
            { }
            else
            {
                Directory.CreateDirectory(path);
            }
            swModelExt = (ModelDocExtension)swModel.Extension;
            boolStatus = swModelExt.SaveAs(path + name, 0, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref warning, ref errors);
        }
        public void saveAssemblyAs(string path, string name)
        {
            if (Directory.Exists(path))
            { }
            else
            {
                Directory.CreateDirectory(path);
            }
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swModelExt = (ModelDocExtension)swModel.Extension;
            boolStatus = swModelExt.SaveAs(path + name + ".SLDASM", 0, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref warning, ref errors);
        }
        public void saveDrawingAs(string path, string name)
        {
            if (Directory.Exists(path))
            { }
            else
            {
                Directory.CreateDirectory(path);
            }
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swModelExt = (ModelDocExtension)swModel.Extension;
            boolStatus = swModelExt.SaveAs(path + name + ".SLDDRW", 0, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref warning, ref errors);
        }
        public void insertIntoAssembly_shellPart(string shellName, string path, double cabW, double cabH, double cabD, string type, bool toeBool, List<string> cat, 
            string locStr, string tranStr, List<bool> toespace, int i, List<bool> uppontoespace, bool inserted)
        {
            List<double> xyz = new List<double>();
            List<double> XYZ = new List<double>();
            if (path.Split('.')[1] == "SLDPRT")
            {
                openPartFile(path);
            }
            else
            {
                openAssemblyFile(path);
            }
            xyz = getBoundingBoxXYZ();
            XYZ = CabinetOverview.getPartLocation(xyz[0], xyz[1], xyz[2], cabW, cabH, cabD, locStr, tranStr, toespace, i, uppontoespace);

            swApp.ActivateDoc2(shellName, false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;
            if (inserted == true)
            {
                Component2 swComponent = (Component2)swAssembly.AddComponent4(path, "1", XYZ[0] / mToInch, XYZ[1] / mToInch, XYZ[2] / mToInch);
            }
            else
            {
                Component2 swComponent = (Component2)swAssembly.AddComponent4(path, "0", XYZ[0] / mToInch, XYZ[1] / mToInch, XYZ[2] / mToInch);
            }
            //
            //boolStatus = swAssembly.AddComponent(path, XYZ[0] / mToInch, XYZ[1] / mToInch, XYZ[2] / mToInch);
            closeFile(path);
        }
        
        public void insertIntoAssembly_confPart(string partClass, string partMaster, string partname, List<int> partindex, double cabW, double cabH, double cabD, string cst, string confName, double cabRealH, bool toespaceornot, string model)
        {
            List<double> xyz = new List<double>();
            List<double> XYZ = new List<double>();
            string path = libraryPath + "part\\conf\\" + partClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT";
            openPartFile(path);
            xyz = getBoundingBoxXYZ();
            double temy = 0;
            if (partClass == "hiddenrail")
            {
                //double length = partindex[2] * 31 / 32;
                double cabFullH = CabinetOverview.cabinetFullHeight(Convert.ToInt16(cabH), model);
                double insidehight = CabinetOverview.cabinetInsideHeight(cabFullH, model, cst);
                //double ydif = partindex[0] * 31 / 32;
                //temy = -length / 2 + CabinetOverview.transitInsideToOutside(model)  + insidehight / 2 - ydif;
                ////temy = -drawerh*31/64/mmtoinch
                //    -(c+b+toe)/2 + d/2 + (((cabH-32)  * 31/32+ 31.75) / mmtoinch - c+b+toe)/ 2 - xy_pre[1]*31/32/mmtoinch;
                

                XYZ = CabinetOverview.getHidLocation(partindex, xyz[0], xyz[1], xyz[2], cabW, insidehight, cabD, cst, cabRealH);
                
            }
            else if (partClass == "centerpost")
            {
                if (partname.Substring(0, 1) == "H")
                {
                    cst = "PH";
                }
                double cabFullH = CabinetOverview.cabinetFullHeight(Convert.ToInt16(cabH), model);
                double insidehight = CabinetOverview.cabinetInsideHeight(cabFullH, model, cst);
                XYZ = CabinetOverview.getcenterpostLocation(partindex, xyz[0], xyz[1], xyz[2], cabW, insidehight, cabD, cst, cabRealH);
                //double toe = 0;
                //if (toespaceornot == true)
                //{
                //    toe = -93.73;
                //}
                //double b = -23.88;//bottom frame thickness
                //double c = 23.88;//top frame thickness
                //double drawerhead = xyz[1] / 10 * 31 / 32;
                //double cabFullH = ((cabH - 32) * 31 / 32 + 31.75) * 25.4;
                //double insidehight = cabFullH - c + b + toe;
                //double ydif = partindex[1] * 31 * 25.4 / 32;
                //temy = -drawerhead / 2 - (c + b + toe) / 2 + insidehight / 2 - ydif;
                //temy = -drawerh*31/64/mmtoinch
                //    -(c+b+toe)/2 + d/2 + (((cabH-32)  * 31/32+ 31.75) / mmtoinch - c+b+toe)/ 2 - xy_pre[1]*31/32/mmtoinch;
                //temy /= 25.4;
                //double length = partindex[2] * 31 / 32;
                //double cabFullH = CabinetOverview.cabinetFullHeight(Convert.ToInt16(cabH), model);
                //double insidehight = CabinetOverview.cabinetInsideHeight(cabFullH, model);
                //double ydif = partindex[0] * 31 / 32;
                //temy = -length / 2 + CabinetOverview.transitInsideToOutside(model) + insidehight / 2 - ydif;
            }

            swApp.ActivateDoc2(confName + ".SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;
            boolStatus = swAssembly.AddComponent(path, XYZ[0], XYZ[1], XYZ[2]);
            closeFile(path);
        }
        public void insertIntoAssembly_shell(string path, string cabName)
        {
            openAssemblyFile(path);
            swApp.ActivateDoc2("1.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;
            boolStatus = swAssembly.AddComponent(path, 0, 0, 0);
            closeFile(path);
        }
        public void insertIntoAssembly_conf(string path, double d, string cstyle, string cabName, bool toespaceornot, string model)
        {
            openAssemblyFile(path);
            List<double> xyz = getBoundingBoxXYZ();
            double tempz = (Convert.ToDouble(d) / 2 - xyz[2] / 2);
                tempz /= mToInch;
            swApp.ActivateDoc2("1.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;
            double tempy = 0;
            //double toe = 0;
            //if (toespaceornot == true)
            //{
            //    toe = -93.73;
            //}
            //double b = -23.88;//bottom frame thickness
            //double c = 23.88;//top frame thickness
            tempy = CabinetOverview.transitInsideToOutside(model, cstyle);
            tempy /= mToInch;
            if (cstyle == "PH")
            {
                tempz -= 0.52596 / mToInch;
            }
            boolStatus = swAssembly.AddComponent(path, 0, tempy, tempz);
            closeFile(path);
        }
        public void insertIntoAssembly_drawer(string path, List<double> xy_pre, double cabW, double cabH, double cabD, string cabName, string drawerstyle, int drawerh, bool toespaceornot, int keyornot, string pull, string model)
        {
            openAssemblyFile(path);
            List<double> xyz = getBoundingBoxXYZ();
            double temx = 0; double temy = 0; double temz = 0;
            if (xyz[0] <= cabW / 2)
            {
                if (xy_pre[0] == 0)
                {
                    temx = -cabW / 4;
                }
                else
                {
                    temx = cabW / 4;
                }
            }
            else
            {
                temx = 0;
            }
            double mmtoinch = 1/25.4;
            //double toe = 0;
            double d = 0;
            if (drawerh <= 6)
            {
                d =/* 21.27/25.4 - 1 / 32*/0.80615157;
            }
            
            //if (toespaceornot == true)
            //{
            //    toe = -93.73;
            //}
            //double b = -23.88;//bottom frame thickness
            //double c = 23.88;//top frame thickness
            double drawerhead = Convert.ToDouble(drawerh)*31/32;
            double cabFullH = CabinetOverview.cabinetFullHeight(Convert.ToInt32(cabH), model);
            double insidehight = CabinetOverview.cabinetInsideHeight(cabFullH, model, drawerstyle);
            double ydif = xy_pre[1] * 31  / 32;
            temy = -drawerhead / 2 + CabinetOverview.transitInsideToOutside(model, drawerstyle) + d / 2 + insidehight / 2 - ydif;
            //temy = -drawerh*31/64/mmtoinch
            //    -(c+b+toe)/2 + d/2 + (((cabH-32)  * 31/32+ 31.75) / mmtoinch - c+b+toe)/ 2 - xy_pre[1]*31/32/mmtoinch;

            //temy /= 25.4;

            //temy = (cabH / 2 - Convert.ToDouble(xy_pre[1])) * 31 / 32 - xyz[1] / 2;
            //temy -= 0.23232990 / 2;
            //if (temy - xyz[1] / 2 > 0)
            //{
            //    temy += 0.20861329;
            //}
            //else if (temy - xyz[1] / 2 < 0)
            //{
            //    temy -= 0.20861329;
            //}

            temz = (cabD / 2 - xyz[2] / 2);
            //if (drawerstyle == "PH")
            //{
            //    temz -= 0.69028836 / mToInch;
            //}
            double transz = 0;
            if (pull == "Flush ABS")
            {
                transz += 0.07859598 ;
            }
            else if (pull == "Flush Aluminum")
            {
                
            }
            else if (pull == "Raised Wire 96 mm")
            {
                transz = 0.99985582  + 0.31000000/2 ;  
            }
            else if (pull == "Raised Wire 4\"")
            {
                transz = 0.99985582  + 0.31000000 / 2 ;
            }
            else if (pull == "Raised Wire 128 mm")
            {
                transz = 0.99985582  + 0.31000000 / 2 ;
            }
            if (keyornot == 1)
            {
                if (transz < 0.21685582 )
                {
                    transz = 0.21685582 ;
                }
            }
            if ((drawerstyle == "PH")||(drawerstyle == "WH")||(drawerstyle == "SH"))
            {
                //transz -= 0.87000002  - 0.69000000 ;
            }
            temz += transz;
            swApp.ActivateDoc2("2.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;

            boolStatus = swAssembly.AddComponent(path, temx / mToInch, temy / mToInch, temz / mToInch);
            closeFile(path);
        }
        public void insertIntoAssembly_door(string path, List<double> xy_pre, double cabW, double cabH, double cabD, string cabName, string cabStyle, 
            double doorh, bool pair, bool toespaceornot, int keyornot, string pull, string model)
        {
            openAssemblyFile(path);
            List<double> xyz = getBoundingBoxXYZ();
            double temx = 0; double temy = 0; double temz = 0;
            if (xyz[0] <= cabW / 2)
            {
                if (xy_pre[0] == 0)
                {
                    temx = -cabW / 4;
                }
                else
                {
                    temx = cabW / 4;
                }
            }
            else
            {
                temx = 0;
            }
            if ((cabStyle == "PN") || (cabStyle == "SN"))
            {
                if (pair == true)
                {
                    if (xy_pre[0] == 0)
                    {
                        temx += 0.85/2;
                    }
                    else
                    {
                        temx -= 0.85/2;
                    }
                }
            }
            //temy = (cabH / 2 - Convert.ToDouble(xy_pre[1]) - doorh) * 31 / 32 + xyz[1] / 2 - 0.92921648 + 0.23232990 / 2;
            //if (temy > 0)
            //{
            //    temy += 0.20861329;
            //}
            //else if (temy < 0)
            //{
            //    temy -= 0.20861329;
            //}
            //double toe = 0;
            //if (toespaceornot == true)
            //{
            //    toe = -93.73;
            //}


           

            //double b = -23.88;//bottom frame thickness
            //double c = 23.88;//top frame thickness
            double doorheight = doorh * 31 / 32;
            double cabFullH = CabinetOverview.cabinetFullHeight(Convert.ToInt32(cabH), model);
            double insidehight = CabinetOverview.cabinetInsideHeight(cabFullH, model, cabStyle);
            double ydif = xy_pre[1] * 31 / 32;
            temy = -doorheight / 2 + CabinetOverview.transitInsideToOutside(model, cabStyle) + insidehight / 2 - ydif;
            //temy = -drawerh*31/64/mmtoinch
            //    -(c+b+toe)/2 + d/2 + (((cabH-32)  * 31/32+ 31.75) / mmtoinch - c+b+toe)/ 2 - xy_pre[1]*31/32/mmtoinch;
            //temy /= 25.4;


            temz = (cabD / 2 - 0.393);
            double transz = 0;
            //if ((pull == "Flush ABS")|| (pull == "P1"))
            //{
            //    transz += 0.07859598;
            //}
            //else if ((pull == "Flush Aluminum") || (pull == "P2"))
            //{

            //}
            //else if ((pull == "Raised Wire 96 mm") || (pull == "P3"))
            //{
            //    transz = 0.99985582 + 0.31000000 / 2;
            //}
            //else if ((pull == "Raised Wire 4\"") || (pull == "P4"))
            //{
            //    transz = 0.99985582 + 0.31000000 / 2;
            //}
            //else if ((pull == "Raised Wire 128 mm")|| (pull == "P5"))
            //{
            //    transz = 0.99985582 + 0.31000000 / 2;
            //}
            //if (keyornot == 1)
            //{
            //    if (transz < 0.21685582)
            //    {
            //        transz = 0.21685582;
            //    }
            //}
            //if (transz < 0.3400000)
            //{
            //    transz = 0.3400000;
            //}
            if ((cabStyle == "PH") || (cabStyle == "WH") || (cabStyle == "SH"))
            {
                transz -= 0.87000002 - 0.75600000;
            }
            temz += transz;

            swApp.ActivateDoc2("2.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;

            boolStatus = swAssembly.AddComponent(path, temx / mToInch, temy / mToInch, temz / mToInch);
            closeFile(path);
        }
        public void insertIntoAssembly_pulloutShelf(string path, double y_loc, double cabW, double cabH, double cabD, string cabName, string model, string cstyle)
        {
            openAssemblyFile(path);
            reBuild();
            //List<double> xyz = getBoundingBoxXYZ();
            double x = 0;
            double y = 0;
            double cabFullH = CabinetOverview.cabinetFullHeight(Convert.ToInt16(cabH), model);
            double insidehight = CabinetOverview.cabinetInsideHeight(cabFullH, model, cstyle);
            y = insidehight / 2 - Convert.ToDouble(y_loc) * 31 / 32;
            y += CabinetOverview.transitInsideToOutside(model, cstyle);
            y /= mToInch;
            double z = 0;
            boolStatus = swAssembly.AddComponent(path, x, y, z);
            closeFile(path);
        }

        public void insertIntoAssembly_acc(string model, double nh, double w, double d, string accClass, string accNewName, string xloc, string yloc, string zloc)
        {
            openAssemblyFile(libraryPath + "acc\\" + accClass + "\\" + accNewName + ".SLDASM");
            double temx = 0;
            double temy = 0;
            double temz = 0;
            List<double> xyz = new List<double>();
            xyz = getBoundingBoxXYZ();
            if (xloc == "Left")
            {
                temx = -w / 2 + xyz[0] / 2;
            }
            else if (xloc == "Middle")
            {
                temx = 0;
            }
            else
            {
                temx = w / 2 - xyz[0] / 2;
            }
            if (yloc == "Left")
            {
                temy = -nh / 2 + xyz[1] / 2;
            }
            else if (yloc == "Middle")
            {
                temy = 0;
            }
            else
            {
                temy = nh / 2 - xyz[1] / 2;
            }
            if (zloc == "Left")
            {
                temz = -d / 2 + xyz[2] / 2;
            }
            else if (zloc == "Middle")
            {
                temz = 0;
            }
            else
            {
                temz = d / 2 - xyz[2] / 2;
            }
            swApp.ActivateDoc2("2.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;
            boolStatus = swAssembly.AddComponent(libraryPath + "acc\\" + accClass + "\\" + accNewName + ".SLDASM",temx / mToInch, temy/ mToInch, temz / mToInch);
            closeFile(libraryPath + "acc\\" + accClass + "\\" + accNewName + ".SLDASM");
        }
        public void insertIntoAssembly_otherSub(string cabName, string path, double cabW, double cabH, double cabD, string locStr, string transStr)
        {
            List<double> xyz = new List<double>();
            List<double> XYZ = new List<double>();
            if (path.Split('.')[1] == "SLDPRT")
            {
                openPartFile(path);
            }
            else
            {
                openAssemblyFile(path);
            }
            xyz = getBoundingBoxXYZ();
            XYZ = CabinetOverview.getPartLocation(xyz[0], xyz[1], xyz[2], cabW, cabH, cabD, locStr, transStr, new List<bool>() {false }, 0, new List<bool>() {false });

            swApp.ActivateDoc2(cabName, false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;
            boolStatus = swAssembly.AddComponent(path, XYZ[0] / mToInch, XYZ[1] / mToInch, XYZ[2] / mToInch);
            closeFile(path);
        }

        public List<double> getBoundingBoxXYZ()
        {
            List<double> output = new List<double>();
            object _BoxFeatureArray = null;
            double[] BoxFeatureArray = null;
            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
            boolStatus = false;
            boolStatus = swModel.Extension.SelectByID2("Bounding Box", "SKETCH", 0, 0, 0, false, 0, null, 0);
            if (boolStatus == false)
            {
                int TEMP;
                bool booltemp = swModel.Extension.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
                swModel.FeatureManager.InsertGlobalBoundingBox((int)swGlobalBoundingBoxFitOptions_e.swBoundingBoxType_CustomPlane, true, true, out TEMP);
                boolStatus = swModel.Extension.SelectByID2("Bounding Box", "SKETCH", 0, 0, 0, false, 0, null, 0);
            }
            swFeature = (Feature)swSelMgr.GetSelectedObject6(1, -1);
            boolStatus = swFeature.GetBox(ref _BoxFeatureArray);
            BoxFeatureArray = (double[])_BoxFeatureArray;

            double mToInch = 39.3700787;
            output.Add((Convert.ToDouble(BoxFeatureArray[3]) - Convert.ToDouble(BoxFeatureArray[0])) * mToInch);
            output.Add((Convert.ToDouble(BoxFeatureArray[4]) - Convert.ToDouble(BoxFeatureArray[1])) * mToInch);
            output.Add((Convert.ToDouble(BoxFeatureArray[5]) - Convert.ToDouble(BoxFeatureArray[2])) * mToInch);
            return output;
        }
        public void closeFile(string path)
        {
            swApp.CloseDoc(path);
        }
        public void packandgo(string path, string name, string originalName, bool flattenfolder)
        {
            if (Directory.Exists(path))
            {

            }
            else
            {
                Directory.CreateDirectory(path);
            }
            
            swPackandGo = (PackAndGo)swModel.Extension.GetPackAndGo();
            int namesCount = swPackandGo.GetDocumentNamesCount();
            swPackandGo.IncludeDrawings = true;
            swPackandGo.IncludeSimulationResults = true;
            swPackandGo.IncludeToolboxComponents = true;


            object fileNames;
            object[] pgFileNames = new object[namesCount];
            boolStatus = swPackandGo.GetDocumentNames(out fileNames);
            pgFileNames = (object[])fileNames;
            object pgFileStatus;
            boolStatus = swPackandGo.GetDocumentSaveToNames(out fileNames, out pgFileStatus);
            pgFileNames = (object[])fileNames;




            boolStatus = swPackandGo.SetSaveToName(true, path);
            swPackandGo.FlattenToSingleFolder = flattenfolder;
            swPackandGo.AddPrefix = "";
            swPackandGo.AddSuffix = name;
            int[] statuses = (int[])swModel.Extension.SavePackAndGo(swPackandGo);
            
            string[] newFileNames = new string[namesCount];
            Debug.Print("");
            Debug.Print("  My Pack and Go path and filenames before adding prefix and suffix: ");
            int j = 0;
            for (int i = 0; i <= namesCount - 1; i++)
            {
                string myFileName = (string)pgFileNames[i];
                string myPath = "";
                // Determine type of SolidWorks file based on file extension
                if (myFileName.EndsWith("sldprt"))
                {
                    myFileName = j + ".sldprt";
                }
                else if (myFileName.EndsWith("sldasm"))
                {
                    myFileName = j + ".sldasm";
                }
                else if (myFileName.EndsWith("slddrw"))
                {
                    myFileName = j + ".slddrw";
                }
                else
                {
                    // Only packing up SolidWorks files
                    return;
                }

                myFileName = myPath + myFileName;
                newFileNames[i] = (string)myFileName;
                Debug.Print("    My path and filename is: " + newFileNames[i]);
                j = j + 1;
            }

            // If a drawing document existed for the assembly or part document
            // used in this example, then you have to ensure that the
            // drawing document copied by Pack and Go references the assembly
            // or part document copied by Pack and Go and not the original
            // assembly or part document
            // Calling IPackAndGo::SetSaveToName sets the target for drawings
            // included in Pack and Go and overrides a call to
            // IPackAndGo::SetDocumentSaveToNames
            //status = swPackAndGo.SetSaveToName(true, myPath);


            // Set document paths and filenames for Pack and Go
            BStrWrapper[] pgSetFileNames;
            pgSetFileNames = ObjectArrayToBStrWrapperArray(newFileNames);
            //statuses = swp.SetDocumentSaveToNames(pgSetFileNames);

            // Add a prefix and suffix to the filenames
            //s//wPackAndGo.AddPrefix = "SW";
            //swPackAndGo.AddSuffix = "PackAndGo";

            // Verify document paths and filenames after adding prefix and suffix
            object getFileNames;
            object getDocumentStatus;
            string[] pgGetFileNames = new string[namesCount - 1];

            //bool status = swPackAndGo.GetDocumentSaveToNames(out getFileNames, out getDocumentStatus);
            //pgGetFileNames = (string[])getFileNames;
            Debug.Print("");
            Debug.Print("  My Pack and Go path and filenames after adding prefix and suffix: ");
            for (int i = 0; i <= namesCount - 1; i++)
            {
                Debug.Print("    My path and filename is: " + pgGetFileNames[i]);
            }


            // Pack and Go
            //statuses = (int[])swModelDocExt.SavePackAndGo(swPackAndGo);
        }
        public BStrWrapper[] ObjectArrayToBStrWrapperArray(object[] SwObjects)
        {
            int arraySize;
            arraySize = SwObjects.GetUpperBound(0);
            BStrWrapper[] dispwrap = new BStrWrapper[arraySize + 1];
            int arrayIndex;

            for (arrayIndex = 0; arrayIndex < arraySize + 1; arrayIndex++)
            {
                dispwrap[arrayIndex] = new BStrWrapper((string)(SwObjects[arrayIndex]));
            }

            return dispwrap;

        }
        public void packandgo1(string path, string name, string originalName)
        {
            if (Directory.Exists(path))
            {

            }
            else
            {
                Directory.CreateDirectory(path);
            }


            swPackandGo = (PackAndGo)swModel.Extension.GetPackAndGo();
            int namesCount = swPackandGo.GetDocumentNamesCount();
            swPackandGo.IncludeDrawings = true;
            swPackandGo.IncludeSimulationResults = false;
            swPackandGo.IncludeToolboxComponents = false;
            object fileNames;
            object[] pgFileNames = new object[namesCount];
            boolStatus = swPackandGo.GetDocumentNames(out fileNames);
            pgFileNames = (object[])fileNames;
            object pgFileStatus;
            boolStatus = swPackandGo.GetDocumentSaveToNames(out fileNames, out pgFileStatus);
            pgFileNames = (object[])fileNames;
            boolStatus = swPackandGo.SetSaveToName(true, path);
            swPackandGo.FlattenToSingleFolder = false;
            swPackandGo.AddPrefix = name + "_";
            swPackandGo.AddSuffix = "";
            int[] statuses = (int[])swModel.Extension.SavePackAndGo(swPackandGo);
            
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
            swModelExt = null;
            swFeature = null;
            myDimension = null;
            boolStatus = false;
            swAssembly = null;
        }

        public List<double> readOnePart(string path)
        {
            openPartFile(path);
            int featCount = swModel.GetFeatureCount();
            double w = 0; double h = 0; double d = 0;
            for (int i = featCount; i > 0; i--)
            {
                swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - i);
                if ((swFeature) != null)
                {
                    string temp_featName_ori = swFeature.Name;
                    string temp_featName = swFeature.Name;
                    if (temp_featName_ori == "width")
                    {
                        myDimension = ((Dimension)(swModel.Parameter("width@width")));
                        w = Convert.ToDouble(myDimension.SystemValue);
                    }
                    else if (temp_featName_ori == "height")
                    {
                        myDimension = ((Dimension)(swModel.Parameter("height@height")));
                        h = Convert.ToDouble(myDimension.SystemValue);

                    }
                    else if (temp_featName_ori == "depth")
                    {
                        myDimension = ((Dimension)(swModel.Parameter("depth@depth")));
                        d = Convert.ToDouble(myDimension.SystemValue);

                    }
                    else if (temp_featName_ori == "widthHeight")
                    {
                        myDimension = ((Dimension)(swModel.Parameter("width@widthHeight")));
                        w = Convert.ToDouble(myDimension.SystemValue);
                        myDimension = ((Dimension)(swModel.Parameter("height@widthHeight")));
                        h = Convert.ToDouble(myDimension.SystemValue);

                    }
                    else if (temp_featName_ori == "widthDepth")
                    {
                        myDimension = ((Dimension)(swModel.Parameter("width@widthDepth")));
                        w = Convert.ToDouble(myDimension.SystemValue);
                        myDimension = ((Dimension)(swModel.Parameter("depth@widthDepth")));
                        d = Convert.ToDouble(myDimension.SystemValue);

                    }
                    else if (temp_featName_ori == "heightDepth")
                    {
                        myDimension = ((Dimension)(swModel.Parameter("height@heightDepth")));
                        h = Convert.ToDouble(myDimension.SystemValue);
                        myDimension = ((Dimension)(swModel.Parameter("depth@heightDepth")));
                        d = Convert.ToDouble(myDimension.SystemValue);

                    }
                }
            }
            killSW();
            //closeFile(path);
            return new List<double>() { w, h, d };
        }
        public void insertGeneralTabel(string sheetName, List<List<string>> contentList, double locX, double locY)
        {

            swDrawing = (DrawingDoc)swModel;
            boolStatus = false;
            boolStatus = swDrawing.ActivateSheet(sheetName);
            if (boolStatus == false)
            {
                Debug.Print("Sheet not actived");
            }
            else
            {
               
                swTable = swDrawing.InsertTableAnnotation2(false, locX, locY, 1, "", contentList[0].Count, 5);
                swTable.SetColumnWidth(0, 0.93 / 39.37, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
                swTable.SetColumnWidth(1, 2.7 / 39.37, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
                swTable.SetColumnWidth(2, 4.5 / 39.37, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
                swTable.SetColumnWidth(3, 1.03 / 39.37, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
                swTable.SetColumnWidth(4, 0.78 / 39.37, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
                for (int i = 0; i < contentList.Count; i++)
                {
                    for (int j = 0; j < contentList[i].Count; j++)
                    {
                        swTable.Text[j, i] = contentList[i][j];
                        if (j == 0)
                        {
                            swTable.CellTextHorizontalJustification[j, i] = 2;
                        }
                        else
                        {
                            if ((i == 0) || (i == 4))
                            {
                                swTable.CellTextHorizontalJustification[j, i] = 2;
                            }
                            else
                            {
                                swTable.CellTextHorizontalJustification[j, 1] = 1;
                            }
                        }
                    }
                }
                swModel.Save3(1, errors, warning);
            }
        }

        public bool BuildPart_Shell(string partClass, int w, int h, int d, string partMaster)
        {
            Database database = new Database();
            bool status = false;
           
                int partNameReg = 0;
            List<string> partLoc = new List<string>() ;
                partLoc = CabinetOverview.getPartLocString(partMaster, partClass);
                partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(CabinetOverview.getPartVarString(partMaster, partClass));

            //MySqlDataReader rd = database.getMySqlReader_rd1("select * from partclass where name = '" + partClass + "'");
            //if (rd.Read())
            //{
            //    partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(rd.GetString("variables"));
            //    partLoc = rd.GetString("locations");
            //}
            //database.CloseConnection_rd1();

            string tempClass = "";
            MySqlDataReader rd = database.getMySqlReader_rd1("select * from masterpart_shell where parthead = '" + partMaster + "'");
            if (rd.Read())
            {
                tempClass = rd.GetString("partclass");
            }
            database.CloseConnection_rd1();

            string partname = CabinetOverview.genPartNameByMaster(w, h, d, partMaster, partNameReg);
                if (File.Exists(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT") == true)
                { }
                else
                {
                    Dictionary<string, double> dic = new Dictionary<string, double>() { { "width", w / mToInch }, { "height", h / mToInch }, { "depth", d / mToInch } };
                    Dictionary<string, string> dic1 = new Dictionary<string, string>() { { "width", "width" }, { "height", "height" }, { "depth", "depth" },
                                                                                     { "widthHeight", "widthHeight" },{ "widthDepth", "widthDepth" },{ "heightDepth", "heightDepth" }};

                //activeApp();
               
                if (File.Exists(masterPath + "part\\shell\\" + tempClass + "\\" + partMaster + ".SLDPRT"))
                {
                    openPartFile(masterPath + "part\\shell\\" + tempClass + "\\" + partMaster + ".SLDPRT");
                    packandgo(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster + "\\" + partname + "\\", partname, partMaster, false);
                }
                else if (File.Exists(masterPath + "part\\shell\\" + tempClass + "\\" + partMaster + ".SLDASM"))
                {
                    openAssemblyFile(masterPath + "part\\shell\\" + tempClass + "\\" + partMaster + ".SLDASM");
                    saveAssemblyAs(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster + "\\" + partname + "\\", partname);
                    //packandgo(libraryPath + "part\\shell\\" + partClass + "\\" + partMaster + "\\" + partname + "\\", partname, partMaster, false);
                }
                //else
                //{
                //    MySqlDataReader rd = database.getMySqlReader_rd1("Select * from masterpart_shell where parthead = '" + partMaster + "'");
                //    if (rd.Read())
                //    {
                //        tempClass = rd.GetString("partclass");
                //    }
                //    database.CloseConnection_rd1();
                //    openPartFile(masterPath + "part\\shell\\" + tempClass + "\\" + partMaster + ".SLDPRT");
                //    packandgo(libraryPath + "part\\shell\\" + partClass + "\\" + partMaster + "\\" + partname + "\\", partname, partMaster, false);
                //}
                    
                    killSW();
                    activeApp();
                if (File.Exists(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT"))
                {
                    openPartFile(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT");
                }
                else if (File.Exists(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDASM"))
                {
                    openAssemblyFile(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDASM");
                }
                //else
                //{
                //    openPartFile(libraryPath + "part\\shell\\" + partClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT");
                //}
                
                    int featCount = swModel.GetFeatureCount();
                    for (int i = featCount; i > 0; i--)
                    {
                        swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - i);
                        if ((swFeature) != null)
                        {
                            string temp_featName_ori = swFeature.Name;
                            string temp_featName = swFeature.Name;
                            if (temp_featName_ori == "width")
                            {
                                changeDimension("width@width", Convert.ToDouble(dic["width"]));
                                reBuild();
                            }
                            else if (temp_featName_ori == "height")
                            {
                                changeDimension("height@height", Convert.ToDouble(dic["height"]));
                                reBuild();
                            }
                            else if (temp_featName_ori == "depth")
                            {
                                changeDimension("depth@depth", Convert.ToDouble(dic["depth"]));
                                reBuild();
                            }
                            else if (temp_featName_ori == "widthHeight")
                            {
                                changeDimension("width@widthHeight", Convert.ToDouble(dic["width"]));
                                changeDimension("height@widthHeight", Convert.ToDouble(dic["height"]));
                                reBuild();
                            }
                            else if (temp_featName_ori == "widthDepth")
                            {
                                changeDimension("width@widthDepth", Convert.ToDouble(dic["width"]));
                                changeDimension("depth@widthDepth", Convert.ToDouble(dic["depth"]));
                                reBuild();
                            }
                            else if (temp_featName_ori == "heightDepth")
                            {
                                changeDimension("height@heightDepth", Convert.ToDouble(dic["height"]));
                                changeDimension("depth@heightDepth", Convert.ToDouble(dic["depth"]));
                                reBuild();
                            }
                        }
                    }
                    boolStatus = swModel.Save3(1, errors, warning);
                    //savePartAs(libraryPath + "part\\shell\\" + partClass + "\\" + partMaster + "\\", partname + ".SLDPRT");
                    //string sql = $"insert into 3dlibrary_part(assembly, subassembly, parttype, masterpart, partname) values('cabinet', 'shell', '" +
                    //    $"{partClass}', '{partMaster}', '{partname}')";
                    //database.mySqlExecuteQuery_rd2(sql);
                    //database.CloseConnection_rd2();
                    killSW();
                }
                status = true;
            
            return status;
        }
        public bool BuildPart_Other(string partClass, int w, int h, int d, string partMaster)
        {
            Database database = new Database();
            bool status = false;

            int partNameReg = 0;
            List<string> partLoc = new List<string>();
            partLoc = CabinetOverview.getPartLocString(partMaster, partClass);
            partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(CabinetOverview.getPartVarString(partMaster, partClass));
            //MySqlDataReader rd = database.getMySqlReader_rd1("select * from partclass where name = '" + partClass + "'");
            //if (rd.Read())
            //{
            //    partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(rd.GetString("variables"));
            //    partLoc = rd.GetString("locations");
            //}
            //database.CloseConnection_rd1();
            string partname = CabinetOverview.genPartNameByMaster(w, h, d, partMaster, partNameReg);
            if (File.Exists(libraryPath + "part\\others\\" + partClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT") == true)
            { }
            else
            {
                Dictionary<string, double> dic = new Dictionary<string, double>() { { "width", w / mToInch }, { "height", h / mToInch }, { "depth", d / mToInch } };
                Dictionary<string, string> dic1 = new Dictionary<string, string>() { { "width", "width" }, { "height", "height" }, { "depth", "depth" },
                                                                                     { "widthHeight", "widthHeight" },{ "widthDepth", "widthDepth" },{ "heightDepth", "heightDepth" }};

                activeApp();
                if (File.Exists(masterPath + "part\\others\\" + partClass + "\\" + partMaster + ".SLDPRT"))
                {
                    openPartFile(masterPath + "part\\others\\" + partClass + "\\" + partMaster + ".SLDPRT");
                    packandgo(libraryPath + "part\\others\\" + partClass + "\\" + partMaster + "\\" + partname + "\\", partname, partMaster, false);
                }
                else if (File.Exists(masterPath + "part\\others\\" + partClass + "\\" + partMaster + ".SLDASM"))
                {
                    openAssemblyFile(masterPath + "part\\others\\" + partClass + "\\" + partMaster + ".SLDASM");
                    saveAssemblyAs(libraryPath + "part\\others\\" + partClass + "\\" + partMaster + "\\" + partname + "\\", partname);
                    //packandgo(libraryPath + "part\\shell\\" + partClass + "\\" + partMaster + "\\" + partname + "\\", partname, partMaster, false);
                }

                killSW();
                activeApp();
                if (File.Exists(libraryPath + "part\\others\\" + partClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT"))
                {
                    openPartFile(libraryPath + "part\\others\\" + partClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT");
                }
                else if (File.Exists(libraryPath + "part\\others\\" + partClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDASM"))
                {
                    openAssemblyFile(libraryPath + "part\\others\\" + partClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDASM");
                }

                int featCount = swModel.GetFeatureCount();
                for (int i = featCount; i > 0; i--)
                {
                    swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - i);
                    if ((swFeature) != null)
                    {
                        string temp_featName_ori = swFeature.Name;
                        string temp_featName = swFeature.Name;
                        if (temp_featName_ori == "width")
                        {
                            changeDimension("width@width", Convert.ToDouble(dic["width"]));
                            reBuild();
                        }
                        else if (temp_featName_ori == "height")
                        {
                            changeDimension("height@height", Convert.ToDouble(dic["height"]));
                            reBuild();
                        }
                        else if (temp_featName_ori == "depth")
                        {
                            changeDimension("depth@depth", Convert.ToDouble(dic["depth"]));
                            reBuild();
                        }
                        else if (temp_featName_ori == "widthHeight")
                        {
                            changeDimension("width@widthHeight", Convert.ToDouble(dic["width"]));
                            changeDimension("height@widthHeight", Convert.ToDouble(dic["height"]));
                            reBuild();
                        }
                        else if (temp_featName_ori == "widthDepth")
                        {
                            changeDimension("width@widthDepth", Convert.ToDouble(dic["width"]));
                            changeDimension("depth@widthDepth", Convert.ToDouble(dic["depth"]));
                            reBuild();
                        }
                        else if (temp_featName_ori == "heightDepth")
                        {
                            changeDimension("height@heightDepth", Convert.ToDouble(dic["height"]));
                            changeDimension("depth@heightDepth", Convert.ToDouble(dic["depth"]));
                            reBuild();
                        }
                    }
                }
                boolStatus = swModel.Save3(1, errors, warning);
                //savePartAs(libraryPath + "part\\shell\\" + partClass + "\\" + partMaster + "\\", partname + ".SLDPRT");
                //string sql = $"insert into 3dlibrary_part(assembly, subassembly, parttype, masterpart, partname) values('cabinet', 'shell', '" +
                //    $"{partClass}', '{partMaster}', '{partname}')";
                //database.mySqlExecuteQuery_rd2(sql);
                //database.CloseConnection_rd2();
                killSW();
            }
            status = true;

            return status;
        }
        public void BuildPart_Hiddenrail(string partMaster, List<int> hiddenrailIndex)
        {
            string partname = partMaster + "-" + hiddenrailIndex[2].ToString();
            if (File.Exists(libraryPath + "part\\conf\\hiddenrail\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT"))
            { }
            else
            {
                activeApp();
                openPartFile(masterPath + "part\\conf\\hiddenrail\\" + partMaster + ".SLDPRT");
                packandgo(libraryPath + "part\\conf\\hiddenrail\\" + partMaster + "\\" + partname + "\\", partname, partMaster, false);
                killSW();
                activeApp();
                openPartFile(libraryPath + "part\\conf\\hiddenrail\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT");
                int featCount = swModel.GetFeatureCount();
                for (int i = featCount; i > 0; i--)
                {
                    swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - i);
                    if ((swFeature) != null)
                    {
                        string temp_featName_ori = swFeature.Name;
                        string temp_featName = swFeature.Name;
                        if (temp_featName_ori == "width")
                        {
                            changeDimension("width@width", Convert.ToDouble(hiddenrailIndex[2]) / mToInch);
                            reBuild();
                        }
                    }
                }
                boolStatus = swModel.Save3(1, errors, warning);
                //string sql = $"insert into 3dlibrary_part(assembly, subassembly, parttype, masterpart, partname) values('cabinet', 'conf', '" +
                //    $"hiddenrail', '{partMaster}', '{partname}')";
                //database.mySqlExecuteQuery_rd2(sql);
                //database.CloseConnection_rd2();
                killSW();
            }
        }
        public void BuildPart_Centerpost(string partMaster, List<int> centerpostIndex)
        {
            string partname = partMaster + "-" + centerpostIndex[0].ToString();
            if (File.Exists(libraryPath + "part\\conf\\centerpost\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT"))
            { }
            else
            {
                activeApp();
                openPartFile(masterPath + "part\\conf\\centerpost\\" + partMaster + ".SLDPRT");
                packandgo(libraryPath + "part\\conf\\centerpost\\" + partMaster + "\\" + partname + "\\", partname, partMaster, false);
                killSW();
                activeApp();
                openPartFile(libraryPath + "part\\conf\\centerpost\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT");
                int featCount = swModel.GetFeatureCount();
                for (int i = featCount; i > 0; i--)
                {
                    swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - i);
                    if ((swFeature) != null)
                    {
                        string temp_featName_ori = swFeature.Name;
                        string temp_featName = swFeature.Name;
                        if (temp_featName_ori == "height")
                        {

                            changeDimension("height@height", Convert.ToDouble(centerpostIndex[0])*31/32 / mToInch);
                            reBuild();
                        }
                    }
                }
                boolStatus = swModel.Save3(1, errors, warning);
                //string sql = $"insert into 3dlibrary_part(assembly, subassembly, parttype, masterpart, partname) values('cabinet', 'conf', '" +
                //    $"centerpost', '{partMaster}', '{partname}')";
                //database.mySqlExecuteQuery_rd2(sql);
                //database.CloseConnection_rd2();
                killSW();
            }
        }
        public void BuildPart_DrawerHead(double drawerWidth, double drawerHeight, string drawerHeadName, string drawerStyle, int lockN, int chN, string pullPro)
        {
            activeApp();
            string drawerHeadType = "";
            if ((drawerStyle == "PN")|| (drawerStyle == "IN"))
            {
                drawerHeadType = "_IN";
            }
            else if ((drawerStyle == "SN") || (drawerStyle == "IN & SS"))
            {
                drawerHeadType = "_IN";
            }
            else if ((drawerStyle == "SH") || (drawerStyle == "PH")|| (drawerStyle == "FO") || (drawerStyle == "FO & SS"))
            {
                drawerHeadType = "_FO";
            }
            else
            {
                drawerHeadType = "_FO_HNM";
            }
            string drawerHeadMasterPath = masterPath + "part\\drawer\\drawerhead\\";
            if (drawerHeight < 6)
            {
                drawerHeadMasterPath += "3\\";
            }
            drawerHeadMasterPath += "DRH" + drawerHeadType + ".SLDPRT";
            openPartFile(drawerHeadMasterPath);
            packandgo(libraryPath + "part\\drawer\\drawerhead\\" + drawerHeadName + "\\", drawerHeadName, "DRH" + drawerHeadType, false);
            killSW();
            activeApp();
            openPartFile(libraryPath + "part\\drawer\\drawerhead\\" + drawerHeadName + "\\" + drawerHeadName + ".SLDPRT");
            changeDimension("WIDTH@SIZE", drawerWidth * 25.4 / 1000);
            reBuild();
            changeDimension("HEIGHT@SIZE", drawerHeight * 31 / 32 * 25.4 / 1000);
            reBuild();
            unsuppressLockCHPull_drawer(lockN, chN, pullPro, drawerWidth);
            boolStatus = swModel.Save3(1, errors, warning);
            killSW();
        }
        public void BuildPart_DrawerBody(double drawerWidth, double drawerHeight, string drawerBodyName)
        {
            activeApp();
            string drawerBodyPath = masterPath + "\\part\\drawer\\drawerbody\\";
            if (drawerHeight < 6)
            {
                drawerBodyPath += "3\\";
            }
            drawerBodyPath += "DRBU.SLDPRT";
            openPartFile(drawerBodyPath);
            packandgo(libraryPath + "part\\drawer\\drawerbody\\" + drawerBodyName + "\\", drawerBodyName, "DRBU", false);
            openPartFile(libraryPath + "part\\drawer\\drawerbody\\" + drawerBodyName + "\\" + drawerBodyName + ".SLDPRT");
            changeDimension("WIDTH@SIZE", drawerWidth * 25.4 / 1000);
            reBuild();
            changeDimension("HEIGHT@SIZE", drawerHeight * 31 / 32 * 25.4 / 1000);
            reBuild();
            boolStatus = swModel.Save3(1, errors, warning);
            killSW();
        }
        public void BuildPart_OutDoor(string confi, double actualWidth, double actualHeight, string outDoorName, int lockN, int chN, string pullPro, string doorType, string doorKey)
        {
            activeApp();
            string outDoorMasterPath = masterPath + "part\\door\\outDoor\\" + doorType + "\\" + confi.Substring(0, 1) + "\\LC15.SLDPRT";
            openPartFile(outDoorMasterPath);
            string outDoorLibPath = libraryPath + "part\\door\\outDoor\\" + doorType + "\\" + confi.Substring(0, 1) + "\\" + outDoorName + "";
            packandgo(outDoorLibPath + "\\", outDoorName, "LC15", false);
            killSW();
            activeApp();
            openPartFile(outDoorLibPath + "\\" + outDoorName + ".SLDPRT");
            changeDimension("WIDTH@SIZE", actualWidth * 25.4 / 1000);
            reBuild();
            changeDimension("HEIGHT@SIZE", actualHeight * 25.4 / 1000);
            reBuild();
            unsuppressLockCHPull_door(lockN, chN, pullPro);
            boolStatus = swModel.Save3(1, errors, warning);
            killSW();
        }
        public void BuildPart_InnerDoor(double actualWidth, double actualHeight, string innerDoorName, string doorType, string doorKey)
        {
            activeApp();
            
            string innerDoorMasterPath = masterPath + "part\\door\\innerdoor\\" + doorType + "\\LC15.SLDPRT";
            openPartFile(innerDoorMasterPath);
            string innerDoorLibPath = libraryPath + "part\\door\\innerdoor\\" + doorType + "\\" + innerDoorName + "";
            packandgo(innerDoorLibPath + "\\", innerDoorName, "LC15", false);
            killSW();
            activeApp();
            openPartFile(innerDoorLibPath + "\\" + innerDoorName + ".SLDPRT");
            changeDimension("WIDTH@SIZE", actualWidth * 25.4 / 1000);
            reBuild();
            changeDimension("HEIGHT@SIZE", actualHeight * 25.4 / 1000);
            reBuild();
            boolStatus = swModel.Save3(1, errors, warning);
            killSW();
        }
        public void BuildAcc(string accName, int w, double Nh, int d, int h, bool varw, bool varh, bool vard, string newName)
        {
            openAssemblyFile(masterPath + "acc\\" + accName + "\\" + accName + ".SLDASM");
            
            if ((varw == true)&& (varh == false)&& (vard == false))
            {
                changeDimension("width@width", Convert.ToDouble(w / mToInch));
                reBuild();
            }
            else if ((varw == false) && (varh == true) && (vard == false))
            {
                changeDimension("height@height", Convert.ToDouble(Nh / mToInch));
                reBuild();
            }
            else if ((varw == false) && (varh == false) && (vard == true))
            {
                changeDimension("depth@depth", Convert.ToDouble(d / mToInch));
                reBuild();
            }
            else if ((varw == true) && (varh == true) && (vard == false))
            {
                changeDimension("width@widthHeight", Convert.ToDouble(w/mToInch));
                changeDimension("height@widthHeight", Convert.ToDouble(Nh / mToInch));
                reBuild();
            }
            else if ((varw == true) && (varh == false) && (vard == true))
            {
                changeDimension("width@widthDepth", Convert.ToDouble(w / mToInch));
                changeDimension("depth@widthDepth", Convert.ToDouble(d / mToInch));
                reBuild();
            }
            else if ((varw == false) && (varh == true) && (vard == true))
            {
                changeDimension("height@heightDepth", Convert.ToDouble(Nh / mToInch));
                changeDimension("depth@heightDepth", Convert.ToDouble(d / mToInch));
                reBuild();
            }
            reBuild();
            saveAssemblyAs(libraryPath + "acc\\" + accName + "\\",  newName);
            closeFile(libraryPath + "acc\\" + accName + "\\" + newName);

        }
        public void unsuppressLockCHPull_drawer(int lockN, int chN, string pullPro, double drawerWidth)
        {

            if (lockN == 1)
            {
                boolStatus = swModel.Extension.SelectByID2("K0", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);
            }
            if (chN == 1)
            {
                boolStatus = swModel.Extension.SelectByID2("L", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);
            }

            //pull
            Dictionary<string, string> dic_pullstyle = new Dictionary<string, string>()
            { {"Flush ABS", "P1"},{"Flush Aluminum","P2" }, {"Raised Wire 96 mm","P3" },
                {"Raised Wire 4\"", "P4" }, {"Raised Wire 128 mm", "P5" }, {"P1", "P1"},{"P2","P2" },
                { "P3","P3" },  {"P4", "P4" }, {"P5", "P5" }};
            pullPro = dic_pullstyle[pullPro];
            if (pullPro == "P1")
            {
                if (drawerWidth >= 30)
                {
                    boolStatus = swModel.Extension.SelectByID2("SP2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                }
                else
                {
                    boolStatus = swModel.Extension.SelectByID2("SP1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                }
            }
            else if (pullPro == "P2")
            {
                if (drawerWidth >= 30)
                {
                    boolStatus = swModel.Extension.SelectByID2("AP2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                }
                else
                {
                    boolStatus = swModel.Extension.SelectByID2("AP1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                }
            }
            else if (pullPro == "P3")
            {
                if (drawerWidth >= 30)
                {
                    boolStatus = swModel.Extension.SelectByID2("WIRE2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                    boolStatus = swModel.Extension.SelectByID2("WIRE 2", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swModel.EditSketch();
                    swModel.ClearSelection2(true);
                    myDimension = null;
                    myDimension = ((Dimension)(swModel.Parameter("D1@WIRE 2")));
                    myDimension.SystemValue = 0.096;
                    swModel.ClearSelection2(true);
                    swModel.SketchManager.InsertSketch(true);
                    swModel = ((ModelDoc2)(swApp.ActiveDoc));
                }
                else
                {
                    boolStatus = swModel.Extension.SelectByID2("WIRE1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                    boolStatus = swModel.Extension.SelectByID2("WIRE 1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swModel.EditSketch();
                    swModel.ClearSelection2(true);
                    myDimension = null;
                    myDimension = ((Dimension)(swModel.Parameter("D1@WIRE 1")));
                    myDimension.SystemValue = 0.096;
                    swModel.ClearSelection2(true);
                    swModel.SketchManager.InsertSketch(true);
                    swModel = ((ModelDoc2)(swApp.ActiveDoc));
                }
            }
            else if (pullPro == "P4")
            {
                if (drawerWidth >= 30)
                {
                    boolStatus = swModel.Extension.SelectByID2("WIRE2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                    boolStatus = swModel.Extension.SelectByID2("WIRE 2", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swModel.EditSketch();
                    swModel.ClearSelection2(true);
                    myDimension = null;
                    myDimension = ((Dimension)(swModel.Parameter("D1@WIRE 2")));
                    myDimension.SystemValue = 0.1016;
                    swModel.ClearSelection2(true);
                    swModel.SketchManager.InsertSketch(true);
                    swModel = ((ModelDoc2)(swApp.ActiveDoc));
                }
                else
                {
                    boolStatus = swModel.Extension.SelectByID2("WIRE1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                    boolStatus = swModel.Extension.SelectByID2("WIRE 1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swModel.EditSketch();
                    swModel.ClearSelection2(true);
                    myDimension = null;
                    myDimension = ((Dimension)(swModel.Parameter("D1@WIRE 1")));
                    myDimension.SystemValue = 0.1016;
                    swModel.ClearSelection2(true);
                    swModel.SketchManager.InsertSketch(true);
                    swModel = ((ModelDoc2)(swApp.ActiveDoc));
                }
            }
            else if (pullPro == "P5")
            {
                if (drawerWidth >= 30)
                {
                    boolStatus = swModel.Extension.SelectByID2("WIRE2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                    boolStatus = swModel.Extension.SelectByID2("WIRE 2", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swModel.EditSketch();
                    swModel.ClearSelection2(true);
                    myDimension = null;
                    myDimension = ((Dimension)(swModel.Parameter("D1@WIRE 2")));
                    myDimension.SystemValue = 0.128;
                    swModel.ClearSelection2(true);
                    swModel.SketchManager.InsertSketch(true);
                    swModel = ((ModelDoc2)(swApp.ActiveDoc));
                }
                else
                {
                    boolStatus = swModel.Extension.SelectByID2("WIRE1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swModel.EditUnsuppress2();
                    swModel.ClearSelection2(true);
                    boolStatus = swModel.Extension.SelectByID2("WIRE 1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swModel.EditSketch();
                    swModel.ClearSelection2(true);
                    myDimension = null;
                    myDimension = ((Dimension)(swModel.Parameter("D1@WIRE 1")));
                    myDimension.SystemValue = 0.128;
                    swModel.ClearSelection2(true);
                    swModel.SketchManager.InsertSketch(true);
                    swModel = ((ModelDoc2)(swApp.ActiveDoc));
                }
            }
            bool bRebuild;
            int ret;
            swModel = (ModelDoc2)swApp.ActiveDoc;
            ret = swModel.Extension.NeedsRebuild2;
            Debug.Print("Features need to be rebuilt (1 = needs rebuild)? " + ret);
            if (ret == 1)
            {
                bRebuild = swModel.Extension.ForceRebuildAll();
                Debug.Print("    All features rebuilt in all configurations without activating each configuration? " + bRebuild);
            }
            exportCAD();
        }
        public void unsuppressLockCHPull_door(int lockN, int chN, string pullPro)
        {
            Dictionary<string, string> dic_pullstyle = new Dictionary<string, string>()
            { {"Flush ABS", "P1"},{"Flush Aluminum","P2" }, {"Raised Wire 96 mm","P3" },
                {"Raised Wire 4\"", "P4" }, {"Raised Wire 128 mm", "P5" }, {"P1", "P1"},{"P2","P2" },
                { "P3","P3" },  {"P4", "P4" }, {"P5", "P5" }};
            pullPro = dic_pullstyle[pullPro];
            if (lockN == 1)
            {
                boolStatus = swModel.Extension.SelectByID2("K0", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);
            }
            if (chN == 1)
            {
                boolStatus = swModel.Extension.SelectByID2("L", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);
            }

            //pull
            if (pullPro == "P1")
            {

                boolStatus = swModel.Extension.SelectByID2("SP", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);

            }
            else if (pullPro == "P2")
            {
                boolStatus = swModel.Extension.SelectByID2("AP", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);

            }
            else if (pullPro == "P3")
            {
                boolStatus = swModel.Extension.SelectByID2("WIRE", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);
                boolStatus = swModel.Extension.SelectByID2("WIRE1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swModel.EditSketch();
                swModel.ClearSelection2(true);
                myDimension = null;
                myDimension = ((Dimension)(swModel.Parameter("D1@WIRE1")));
                myDimension.SystemValue = 0.096;
                swModel.ClearSelection2(true);
                swModel.SketchManager.InsertSketch(true);
                swModel = ((ModelDoc2)(swApp.ActiveDoc));
            }
            else if (pullPro == "P4")
            {
                boolStatus = swModel.Extension.SelectByID2("WIRE", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);
                boolStatus = swModel.Extension.SelectByID2("WIRE1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swModel.EditSketch();
                swModel.ClearSelection2(true);
                myDimension = null;
                myDimension = ((Dimension)(swModel.Parameter("D1@WIRE1")));
                myDimension.SystemValue = 0.1016;
                swModel.ClearSelection2(true);
                swModel.SketchManager.InsertSketch(true);
                swModel = ((ModelDoc2)(swApp.ActiveDoc));

            }
            else if (pullPro == "P5")
            {
                boolStatus = swModel.Extension.SelectByID2("WIRE", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditUnsuppress2();
                swModel.ClearSelection2(true);
                boolStatus = swModel.Extension.SelectByID2("WIRE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swModel.EditSketch();
                swModel.ClearSelection2(true);
                myDimension = null;
                myDimension = ((Dimension)(swModel.Parameter("D1@WIRE1")));
                myDimension.SystemValue = 0.128;
                swModel.ClearSelection2(true);
                swModel.SketchManager.InsertSketch(true);
                swModel = ((ModelDoc2)(swApp.ActiveDoc));
            }

            swModel = (ModelDoc2)swApp.ActiveDoc;

            bool bRebuild;
            int ret;

            ret = swModel.Extension.NeedsRebuild2;
            Debug.Print("Features need to be rebuilt (1 = needs rebuild)? " + ret);

            if (ret == 1)
            {
                bRebuild = swModel.Extension.ForceRebuildAll();
                Debug.Print("    All features rebuilt in all configurations without activating each configuration? " + bRebuild);
            }
            exportCAD();



        }
        public void exportCAD()
        {
            swModel = (ModelDoc2)swApp.ActiveDoc;
            string sModelName;
            string sPathName;
            object varAlignment;
            double[] dataAlignment = new double[12];
            object varViews;
            string[] dataViews = new string[2];
            PartDoc swPart;
            sModelName = swModel.GetPathName();
            sPathName = swModel.GetPathName();
            sPathName = sPathName.Substring(0, sPathName.Length - 6);
            sPathName = sPathName + "dwg";

            swPart = (PartDoc)swModel;

            dataAlignment[0] = 0.0;
            dataAlignment[1] = 0.0;
            dataAlignment[2] = 0.0;
            dataAlignment[3] = 1.0;
            dataAlignment[4] = 0.0;
            dataAlignment[5] = 0.0;
            dataAlignment[6] = 0.0;
            dataAlignment[7] = 1.0;
            dataAlignment[8] = 0.0;
            dataAlignment[9] = 0.0;
            dataAlignment[10] = 0.0;
            dataAlignment[11] = 1.0;

            varAlignment = dataAlignment;

            dataViews[0] = "*Current";
            dataViews[1] = "*Front";

            varViews = dataViews;
            int options = 33;  //include flat-pattern geometry and feature
            swPart.ExportToDWG2(sPathName, sModelName, (int)swExportToDWG_e.swExportToDWG_ExportSheetMetal, true, varAlignment, false, false, options, null);

        }

        public bool BuildSubAss_Shell(int w, int h, int d, List<string> shellBomList, List<string> partClassList, string shellName, List<bool> toespace, 
            List<bool> uppontoespace,
            string model, string cstyle, string metalTop, string partMaster, string partClass)
        {
            Database database = new Database();
            bool status = false;

            List<string> partClass_sub = partClassList;
            List<string> partMaster_sub = shellBomList;
            List<int> partNameRegul_sub = new List<int>();
            List < List<string>> partLoc_sub = new List<List<string>>();
            List<string> partnamelist = new List<string>();
            List<List<bool>> partTipCommand = new List<List<bool>>();

            List<string> output = new List<string>();
            //if (shellBomList.Contains("H-LC31"))
            //{
            //    if (w<30)
            //    {
            //        shellBomList.Remove("H-LC31");
            //        partClassList.Remove("31");
            //        partMaster_sub.Remove("H-LC31");
            //        partClass_sub.Remove("31");

            //    }
            //}
            
            for (int i = 0; i < shellBomList.Count; i++)
            {
                if (shellBomList[i] != "")
                {

                    int partNameReg = 0;
                    List<string> partLoc = new List<string>();
                    partLoc = CabinetOverview.getPartLocString(shellBomList[i], partClassList[i]);
                    partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(CabinetOverview.getPartVarString(shellBomList[i], partClassList[i]));
                    //MySqlDataReader rd = database.getMySqlReader_rd1("select * from partclass where name = '" + partClassList[i] + "'");
                    //    if (rd.Read())
                    //    {
                    //        partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(rd.GetString("variables"));
                    //        partLoc = rd.GetString("locations");
                    //    }
                    //    database.CloseConnection_rd1();
                    string partname = CabinetOverview.genPartNameByMaster(w, h, d, shellBomList[i], partNameReg);
                    partNameRegul_sub.Add(partNameReg);
                    partLoc_sub.Add(partLoc);
                    partTipCommand.Add(CabinetOverview.tipConmmendBool(shellBomList[i], w, h, d));
                    partnamelist.Add(partname);
                }
            }
            for (int i = 0; i < partnamelist.Count; i++)
            {
                string tempClass = "";
                MySqlDataReader rd = database.getMySqlReader_rd1("select * from masterpart_shell where parthead = '" + partMaster_sub[i] + "'");
                if (rd.Read())
                {
                    tempClass = rd.GetString("partclass");
                }
                database.CloseConnection_rd1();
                if (File.Exists(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster_sub[i] + "\\" + partnamelist[i] + "\\" + partnamelist[i] + ".SLDPRT"))
                {

                }
                else
                {
                   
                    activeApp();
                    BuildPart_Shell(partClass_sub[i], w, h, d, partMaster_sub[i]);
                    killSW();
                }
            }
            activeApp();
            double nh = CabinetOverview.cabinetFullHeight(h, shellName.Split('_')[0]);
            newAssembly(w, nh, libraryPath + "subassembly\\shell\\", shellName);
            for (int i = 0; i < partnamelist.Count; i++)
            {
                string path = "";
                bool flag = false;
                string tempClass = "";
                MySqlDataReader rd = database.getMySqlReader_rd1("select * from masterpart_shell where parthead = '" + partMaster_sub[i] + "'");
                if (rd.Read())
                {
                    tempClass = rd.GetString("partclass");
                }
                database.CloseConnection_rd1();
                if (File.Exists(libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster_sub[i] + "\\" + partnamelist[i] + "\\" + partnamelist[i] + ".SLDASM"))
                {
                    path = libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster_sub[i] + "\\" + partnamelist[i] + "\\" + partnamelist[i] + ".SLDASM";
                }
                else
                {
                    path = libraryPath + "part\\shell\\" + tempClass + "\\" + partMaster_sub[i] + "\\" + partnamelist[i] + "\\" + partnamelist[i] + ".SLDPRT";

                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (partnamelist[i] == partnamelist[j])
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                insertIntoAssembly_shellPart(shellName, path, Convert.ToDouble(w), nh/*Convert.ToDouble(h)*/, Convert.ToDouble(d), partClass_sub[i], true, 
                    new List<string>() { partMaster_sub[i], w.ToString(), h.ToString(), d.ToString() }, partLoc_sub[i][0], partLoc_sub[i][1], toespace, i, uppontoespace, flag);
            }
            boolStatus = swModel.Save3(1, errors, warning);
            //saveAssemblyAs(libraryPath + "subassembly\\shell\\", shellName);
            killSW();
            status = true;


            //exportDrawing_Shell(model, cstyle, "", metalTop, w, h, d, metalTop, "NSC", "", new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(),
            //new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), shellBomList, libraryPath + "subassembly\\shell\\" + shellName + "\\" + shellName + ".SLDDRW");

            return status;
        }
        public void BuildSubAss_Conf(List<List<int>> hidindex, List<List<int>> centerindex, int partindex, List<List<int>> securindex, List<string> confbom, List<string> partClassList, int w, int h, int d, string confName, bool toespacrornot, string model, string cstyle)
        {
            for (int i = 0; i < hidindex.Count; i++)
            {
                BuildPart_Hiddenrail(confbom[0], hidindex[i]);
            }
            for (int i = 0; i < centerindex.Count; i++)
            {
                BuildPart_Centerpost(confbom[1], centerindex[i]);
            }

            //double mmtoinch = 1 / 25.4;
            //double toe = 0;
            ////double d = 0;
            ////if (drawerh <= 6)
            ////{
            ////    d = 21.27 - 25.4 / 64;
            ////}
            //if (toespacrornot == true)
            //{
            //    toe = -93.73;
            //}
            //double b = -23.88;//bottom frame thickness
            //double c = 23.88;//top frame thickness
            //double drawerhead = drawerh * 25.4 * 31 / 32;
            double cabFullH = CabinetOverview.cabinetFullHeight(h, model);
            double insidehight = CabinetOverview.cabinetInsideHeight(cabFullH, model, cstyle);
            //insidehight /= 25.4;
            //double ydif = xy_pre[1] * 31 * 25.4 / 32;
            //temy = -drawerhead / 2 - (c + b + toe) / 2 + d / 2 + insidehight / 2 - ydif;

            activeApp();
            newAssembly(w, insidehight, libraryPath + "subassembly\\conf\\", confName);
            string cst = "";
            if (confbom[0].Substring(0, 1) == "H")
            {
                cst = "PH";
            }
            for (int i = 0; i < hidindex.Count; i++)
            {
                insertIntoAssembly_confPart("hiddenrail", confbom[0], confbom[0] + "-" + hidindex[i][2].ToString(), hidindex[i], Convert.ToDouble(w), Convert.ToDouble(h), Convert.ToDouble(d), cst, confName, h, toespacrornot, model);
            }
            for (int i = 0; i < centerindex.Count; i++)
            {
                insertIntoAssembly_confPart("centerpost", confbom[1], confbom[1] + "-" + centerindex[i][0].ToString(), centerindex[i], Convert.ToDouble(w), Convert.ToDouble(h), Convert.ToDouble(d), cst, confName, h, toespacrornot, model);
            }
            FeatureManager fm = swModel.FeatureManager;
            AssemblyDoc doc = (AssemblyDoc)swModel;
            int longstatus;
            boolStatus = swModel.Extension.SelectByID2("Top Plane", "PLANE", 0, 0, 0, true, 0, null, 0);
            Feature feature = fm.InsertGlobalBoundingBox((int)swGlobalBoundingBoxFitOptions_e.swBoundingBoxType_CustomPlane, true, true, out longstatus);
            swModel.Save3(1, errors, warning);
            //saveAssemblyAs(libraryPath + "subassembly\\conf\\", confName);
            killSW();
        }
        public bool BuildSubAss_Drawer(double drawerWidth, double drawerHeight, string drawerHeadName, string drawerStyle, string drawerBodyName, string drawerTrackName, string pullPro, int lockN, int chN, string drawerName)
        {
            bool status = false;
            
                if (File.Exists(libraryPath + "part\\drawer\\drawerhead\\" + drawerHeadName + "\\" + drawerHeadName + ".SLDPRT"))
                { }
                else
                {
                    BuildPart_DrawerHead(drawerWidth, drawerHeight, drawerHeadName, drawerStyle, lockN, chN, pullPro); ;
                }
                if (File.Exists(libraryPath + "part\\drawer\\drawerbody\\" + drawerBodyName + "\\" + drawerBodyName + ".SLDPRT"))
                { }
                else
                {
                    BuildPart_DrawerBody(drawerWidth, drawerHeight, drawerBodyName);
                }

                activeApp();
                newAssembly(Convert.ToInt16(drawerWidth), Convert.ToInt16(drawerHeight), libraryPath + "subassembly\\drawer\\", drawerName);

                //DrawerBody
                openPartFile(libraryPath + "part\\drawer\\drawerbody\\" + drawerBodyName + "\\" + drawerBodyName + ".SLDPRT");
                swApp.ActivateDoc2(drawerName + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(libraryPath + "part\\drawer\\drawerbody\\" + drawerBodyName + "\\" + drawerBodyName + ".SLDPRT", 0, 0, 0);
                closeFile(libraryPath + "part\\drawer\\drawerbody\\" + drawerBodyName + "\\" + drawerBodyName + ".SLDPRT");

                //DrawerHead
                openPartFile(libraryPath + "part\\drawer\\drawerhead\\" + drawerHeadName + "\\" + drawerHeadName + ".SLDPRT");
                swApp.ActivateDoc2(drawerName + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(libraryPath + "part\\drawer\\drawerhead\\" + drawerHeadName + "\\" + drawerHeadName + ".SLDPRT", 0, 0, 0.22232620197);
                closeFile(libraryPath + "part\\drawer\\drawerhead\\" + drawerHeadName + "\\" + drawerHeadName + ".SLDPRT");

                //DrawerTrack
                double trackX = 0; double trackY = 0; double trackZ = 0;
                if ((drawerHeight < 6.56) && (drawerHeight >= 6))
                {
                    trackY = drawerHeight * 31 / 32 * 25.4 / 1000 / 2 - 0.0183090058 - 0.00320387391;
                }
                else if (drawerHeight >= 6.56)
                {
                    trackY = -(drawerHeight - 6) * 31 / 32 * 25.4 / 1000 / 2 + 0.07158938;
                }
                else
                {
                    trackY = drawerHeight * 31 / 32 * 25.4 / 1000 / 2 - 0.0043090058 - 0.00320387391 - 0.016;
                }
                trackZ = -0.048968614 - 0.01470435917 - 0.00898510485 - 0.00796655518 + 0.02941696314;
                if (drawerStyle == "PH")
                {
                    trackX = drawerWidth / 2 * 25.4 / 1000 - 0.03068548882 - 0.003564;
                }
                else if (drawerStyle == "PN")
                {
                    trackX = drawerWidth / 2 * 25.4 / 1000 - 0.03068548882 - 0.003564 + 0.00317226088 + 0.001788;
                }
                else if (drawerStyle == "SN")
                {
                    trackX = drawerWidth / 2 * 25.4 / 1000 - 0.03068548882 - 0.003564 + 0.00317226088 + +0.001788;
                }
                else
                {
                    trackX = drawerWidth / 2 * 25.4 / 1000 - 0.03068548882 - 0.003564;
                }
                //Left
                openAssemblyFile(masterPath + "part\\drawer\\drawertrack\\" + drawerTrackName + "-L.SLDASM");
                swApp.ActivateDoc2(drawerName + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(masterPath + "part\\drawer\\drawertrack\\" + drawerTrackName + "-L.SLDASM", -trackX, trackY, trackZ);
                closeFile(masterPath + "part\\drawer\\drawertrack\\" + drawerTrackName + "-L.SLDASM");
                //Right
                openAssemblyFile(masterPath + "part\\drawer\\drawertrack\\" + drawerTrackName + "-R.SLDASM");
                swApp.ActivateDoc2(drawerName + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(masterPath + "part\\drawer\\drawertrack\\" + drawerTrackName + "-R.SLDASM", trackX, trackY, trackZ);
                closeFile(masterPath + "part\\drawer\\drawertrack\\" + drawerTrackName + "-R.SLDASM");

                string acPath = masterPath + "part\\AC\\";

                //Pull
                string pullPath = acPath + "pull\\";
                double pullX = 0; double pullY = 0; double pullZ = 0;
                if (pullPro == "Flush ABS")
                {
                    pullPath += "P2-51.SLDPRT";
                    pullX = 0;
                    pullY = drawerHeight * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000;
                    pullZ = 0.22383920203;
                }
                else if (pullPro == "Flush Aluminum")
                {
                    pullPath += "AEMC-S-22.SLDPRT";
                    pullX = 0;
                    pullY = drawerHeight * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000 + 0.0032131;
                    pullZ = 0.22383920203 - 0.002148;
                }
                else if (pullPro == "Raised Wire 96 mm")
                {
                    pullPath += "RAISED WIRE 96MM.SLDPRT";
                    pullX = 0;
                    pullY = drawerHeight * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000;
                    pullZ = 0.22383920203 + 0.0219185;
                }
                else if (pullPro == "Raised Wire 4\"")
                {
                    pullPath += "RAISED WIRE 4IN.SLDPRT";
                    pullX = 0;
                    pullY = drawerHeight * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000;
                    pullZ = 0.22383920203 + 0.0219185;
                }
                else if (pullPro == "Raised Wire 128 mm")
                {
                    pullPath += "RAISED WIRE 128MM.SLDPRT";
                    pullX = 0;
                    pullY = drawerHeight * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000;
                    pullZ = 0.22383920203 + 0.0219185;
                }
                openPartFile(pullPath);
                swApp.ActivateDoc2(drawerName + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                if (drawerWidth > 24)
                {
                    pullX = drawerWidth / 4 * 25.4 / 1000;
                    boolStatus = swAssembly.AddComponent(pullPath, -pullX, pullY, pullZ);
                    boolStatus = swAssembly.AddComponent(pullPath, pullX, pullY, pullZ);
                    closeFile(pullPath);
                }
                else
                {
                    pullX = 0;
                    boolStatus = swAssembly.AddComponent(pullPath, pullX, pullY, pullZ);
                    closeFile(pullPath);
                }

                //Lock
                if (lockN == 1)
                {
                    openPartFile(acPath + "lock\\K-17.SLDPRT");
                    swApp.ActivateDoc2(drawerName + ".SLDASM", false, 0);
                    swModel = (ModelDoc2)swApp.ActiveDoc;
                    swAssembly = (AssemblyDoc)swModel;
                    boolStatus = swAssembly.AddComponent(acPath + "lock\\K-17.SLDPRT", 0.0917702, (drawerHeight - 3) * 31 / 32 / 2 * 25.4 / 1000 + 0.28625 * 25.4 / 1000 + 0.01017187201, 0.21162010203);
                    closeFile(acPath + "lock\\K-17.SLDPRT");
                }

                //Cardholder
                if (chN == 1)
                {
                    double chX = 0;
                    if (drawerStyle == "PN")
                    {
                        if (drawerWidth >= 18)
                        {
                            chX = 0.16805275 - 0.01914144 + (drawerWidth - 18) / 2 * 25.4 / 1000;
                        }
                        else
                        {
                            chX = -0.16805275 + 0.01914144 + (18 - drawerWidth) / 2 * 25.4 / 1000;
                        }
                    }
                    else
                    {
                        if (drawerWidth >= 18)
                        {
                            chX = 0.16805275 + (drawerWidth - 18) / 2 * 25.4 / 1000;
                        }
                        else
                        {
                            chX = -0.16805275 + (18 - drawerWidth) / 2 * 25.4 / 1000;
                        }
                    }
                    openPartFile(acPath + "cardholder\\LH3.5X1.5NP.SLDPRT");
                    swApp.ActivateDoc2(drawerName + ".SLDASM", false, 0);
                    swModel = (ModelDoc2)swApp.ActiveDoc;
                    swAssembly = (AssemblyDoc)swModel;
                    boolStatus = swAssembly.AddComponent(acPath + "cardholder\\LH3.5X1.5NP.SLDPRT", -chX, drawerHeight * 31 / 32 / 2 * 25.4 / 1000 - 1.25 * 25.4 / 1000, 0.23183850203);
                    closeFile(acPath + "cardholder\\LH3.5X1.5NP.SLDPRT");
                }

                //save
                //saveAssemblyAs(libraryPath + "subassembly\\drawer\\", drawerName);
                boolStatus = swModel.Save3(1, errors, warning);
                killSW();
                status = true;
            
            return status;
        }
        public bool BuildSubAss_Door(List<string> doorlist, string doorKey)
        {
            bool status = false;
            //////0:  material
            ///1:  confi
            ///2:  actualWidth.ToString("0.000000")
            ///3:  actualHeight.ToString("0.000000")
            ///4:  lockN.ToString()
            ///5:  chN.ToString()
            ///6:  pullPro
            ///7:  dic_hinge[hingePro]
            ///8:  d3Name
            ///9:  outDoorName
            ///10: innerDoorName
            ///11: doorName
            ///12: nameWidth
            ///13: nameHeight
            ///14: doorType
            double actualWidth = Convert.ToDouble(doorlist[2]);
            double actualHeight = Convert.ToDouble(doorlist[3]);
            double doorWidth = Convert.ToDouble(doorlist[12]);
            double doorHeight = Convert.ToDouble(doorlist[13]);
            int lockN = Convert.ToInt32(doorlist[4]);
            int chN = Convert.ToInt32(doorlist[5]);
            if (doorlist[14] == "Classic")
            {
                if (File.Exists(libraryPath + "part\\door\\innerdoor\\" + doorlist[14] + "\\" + doorlist[10] + "\\" + doorlist[10] + ".SLDPRT"))
                { }
                else
                {
                    BuildPart_InnerDoor(actualWidth, actualHeight, doorlist[10], doorlist[14], doorKey);
                }
                if (File.Exists(libraryPath + "part\\door\\outdoor\\" + doorlist[14] + "\\" + doorlist[1].Substring(0, 1) + "\\" + doorlist[9] + "\\" + doorlist[9] + ".SLDPRT"))
                { }
                else
                {
                    BuildPart_OutDoor(doorlist[1].Substring(0, 1), actualWidth, actualHeight, doorlist[9], lockN, chN, doorlist[6], doorlist[14], doorKey);
                }
                activeApp();
                newAssembly(actualWidth, actualHeight, libraryPath + "subassembly\\door\\" + doorlist[14] + "\\" + doorlist[1].Substring(0, 1) + "\\", doorlist[11]);
                //swModel.Save3(1, errors, warning);
                //packandgo(libraryPath + "subassembly\\door\\" + doorlist[1].Substring(0,1) + "\\" + doorlist[8], doorlist[8]);
                //killSW();
                //activeApp();
                //openAssemblyFile(libraryPath + "subassembly\\door\\" + doorlist[14] + "\\" + doorlist[1].Substring(0, 1) + "\\" + doorlist[11] + "\\" + doorlist[11] + ".SLDASM");

                //innerDoor
                openPartFile(libraryPath + "part\\door\\innerdoor\\" + doorlist[14] + "\\" + doorlist[10] + "\\" + doorlist[10] + ".SLDPRT");
                swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(libraryPath + "part\\door\\innerdoor\\" + doorlist[14] + "\\" + doorlist[10] + "\\" + doorlist[10] + ".SLDPRT", 0, 0, -(0.018 * 25.4 / 1000));
                closeFile(libraryPath + "part\\door\\innerdoor\\" + doorlist[14] + "\\" + doorlist[10] + "\\" + doorlist[10] + ".SLDPRT");
                boolStatus = swModel.Save3(1, errors, warning);
                if (boolStatus == true)
                {
                    killSW();
                    activeApp();
                    openAssemblyFile(libraryPath + "subassembly\\door\\" + doorlist[14] + "\\" + doorlist[1].Substring(0, 1) + "\\" + doorlist[11] + "\\" + doorlist[11] + ".SLDASM");
                }
                else
                {
                    Debug.Print("Error Saving");
                }
                //outDoor
                openPartFile(libraryPath + "part\\door\\outdoor\\" + doorlist[14] + "\\" + doorlist[1].Substring(0, 1) + "\\" + doorlist[9] + "\\" + doorlist[9] + ".SLDPRT");
                swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(libraryPath + "part\\door\\outdoor\\" + doorlist[14] + "\\" + doorlist[1].Substring(0, 1) + "\\" + doorlist[9] + "\\" + doorlist[9] + ".SLDPRT", 0, 0, 0.018 * 25.4 / 1000);
                closeFile(libraryPath + "part\\door\\outdoor\\" + doorlist[14] + "\\" + doorlist[1].Substring(0, 1) + "\\" + doorlist[9] + "\\" + doorlist[9] + ".SLDPRT");

                //Roller Catch
                double rollerpositionX = 0;
                if (doorlist[1] == "L")
                {
                    rollerpositionX = actualWidth / 2 / mToInch - 0.0613165;
                }
                else if (doorlist[1] == "R")
                {
                    rollerpositionX = 0.0613165 - actualWidth / 2 / mToInch;
                }
                openPartFile(masterPath + "part\\door\\AC\\Roller Catch.SLDPRT");
                swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(masterPath + "part\\door\\AC\\Roller Catch.SLDPRT", rollerpositionX, doorHeight * 31 / 32 * 25.4 / 1000 / 2 - 0.0254635 + 0.009367, -0.022);
                closeFile(masterPath + "part\\door\\AC\\Roller Catch.SLDPRT");

                //CRL#D656(.125X.375)
                openPartFile(masterPath + "part\\door\\AC\\CRL#D656(.125X.375).SLDPRT");
                swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(masterPath + "part\\door\\AC\\CRL#D656(.125X.375).SLDPRT", rollerpositionX, doorHeight * 31 / 32 * 25.4 / 1000 / 2 - 0.0192 + 0.002657475 + 0.009367, -0.02 + 0.00108687 + 0.00978183000);
                closeFile(masterPath + "part\\door\\AC\\CRL#D656(.125X.375).SLDPRT");

                //pull
                double handlepositionX = 0;
                double handlepositionY = 0;
                double handlepositionZ = 0;
                string pullName = "";
                if (doorlist[6] == "P1")
                {
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        handlepositionX = actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000;
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000 - 0.0032385;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        handlepositionX = -(actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000);
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000 - 0.0032385;
                    }
                    handlepositionZ = 0.0027322;
                    pullName = "P2-51-H.SLDPRT";
                }
                else if (doorlist[6] == "P2")
                {
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        handlepositionX = actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000;
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        handlepositionX = -(actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000);
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    handlepositionZ = 0;
                    pullName = "AEMC-S-22-H.SLDPRT";
                }
                else if (doorlist[6] == "P3")
                {
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        handlepositionX = actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000;
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        handlepositionX = -(actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000);
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    handlepositionZ = 0.0246507;
                    pullName = "RAISED WIRE 96MM-H.SLDPRT";
                }
                else if (doorlist[6] == "P5")
                {
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        handlepositionX = actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000;
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        handlepositionX = -(actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000);
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    handlepositionZ = 0.0246507;
                    pullName = "RAISED WIRE 128MM-H.SLDPRT";
                }
                else if (doorlist[6] == "P4")
                {
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        handlepositionX = actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000;
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        handlepositionX = -(actualWidth / 2 / mToInch - 1.455 * 25.4 / 1000);
                        handlepositionY = (doorHeight / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    handlepositionZ = 0.0246507;
                    pullName = "RAISED WIRE 4IN-H.SLDPRT";
                }
                openPartFile(masterPath + "part\\door\\AC\\" + pullName);
                swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(masterPath + "part\\door\\AC\\" + pullName, handlepositionX, handlepositionY, handlepositionZ);
                closeFile(masterPath + "part\\door\\AC\\" + pullName);

                //Lock
                if (lockN == 1)
                {
                    double DLPX = 0;
                    double DLPY = 0;
                    double DLPZ = 0;
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        DLPX = 4.775 * 25.4 / 1000;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        DLPX = -4.775 * 25.4 / 1000;
                    }
                    DLPY = doorHeight * 31 / 32 * 25.4 / 1000 / 2 - 0.01972070889;
                    DLPZ = -0.0094869;
                    openPartFile(masterPath + "part\\door\\AC\\" + "K-17.SLDPRT");
                    swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                    swModel = (ModelDoc2)swApp.ActiveDoc;
                    swAssembly = (AssemblyDoc)swModel;
                    boolStatus = swAssembly.AddComponent(masterPath + "part\\door\\AC\\" + "K-17.SLDPRT", DLPX, DLPY, DLPZ);
                    closeFile(masterPath + "part\\door\\AC\\" + "K-17.SLDPRT");
                }

                //CardHolder
                if (chN == 1)
                {
                    double cardhoderposition_X = 0;
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        cardhoderposition_X = 0.16805275 + (doorWidth - 18) / 2 * 25.4 / 1000;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        cardhoderposition_X = -cardhoderposition_X;
                    }
                    openPartFile(masterPath + "part\\door\\AC\\" + "LH3.5X1.5NP.SLDPRT");
                    swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                    swModel = (ModelDoc2)swApp.ActiveDoc;
                    swAssembly = (AssemblyDoc)swModel;
                    boolStatus = swAssembly.AddComponent(masterPath + "part\\door\\AC\\" + "LH3.5X1.5NP.SLDPRT", cardhoderposition_X, doorHeight * 31 / 32 / 2 * 25.4 / 1000 - 1.25 * 25.4 / 1000, 0.0107315);
                    closeFile(masterPath + "part\\door\\AC\\" + "LH3.5X1.5NP.SLDPRT");
                }

                //Hinge
                double hingepositionX1 = 0;
                double hingepositionY1 = 0;
                double hingepositionZ1 = 0;
                double hingepositionX2 = 0;
                double hingepositionY2 = 0;
                double hingepositionZ2 = 0;
                string hinge1Name = "";
                string hinge2Name = "";
                if ((doorlist[14] == "PN") || (doorlist[14] == "SN"))
                {
                    hingepositionX1 = actualWidth / 2 / mToInch - 0.0313486018;
                    hingepositionY1 = doorHeight * 31 / 32 / 2 * 25.4 / 1000 - 0.001216 - 0.01094422907;
                    hingepositionZ1 = 0.0038227;
                    hingepositionX2 = hingepositionX1;
                    hingepositionY2 = -hingepositionY1;
                    hingepositionZ2 = hingepositionZ1;
                    hinge1Name = "";
                    hinge2Name = "";
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        hinge1Name = "LC1101ZTL.SLDASM";
                        hinge2Name = "LC1101ZBL.SLDASM";
                        hingepositionX1 *= -1;
                        hingepositionX2 *= -1;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        hinge1Name = "LC1101ZTR.SLDASM";
                        hinge2Name = "LC1101ZBR.SLDASM";
                    }
                }
                else if (doorlist[14] == "PH")
                {
                    hingepositionX1 = actualWidth / 2 / mToInch - 0.0338124018;
                    hingepositionY1 = doorHeight * 31 / 32 / 2 * 25.4 / 1000 - 0.001216 - 0.01212847907;
                    hingepositionZ1 = 0;
                    hingepositionX2 = hingepositionX1;
                    hingepositionY2 = -hingepositionY1;
                    hingepositionZ2 = hingepositionZ1;
                    hinge1Name = "";
                    hinge2Name = "";
                    if (doorlist[1].Substring(0, 1) == "L")
                    {
                        hinge1Name = "LC1101HTL.SLDASM";
                        hinge2Name = "LC1101HBL.SLDASM";
                        hingepositionX1 *= -1;
                        hingepositionX2 *= -1;
                    }
                    else if (doorlist[1].Substring(0, 1) == "R")
                    {
                        hinge1Name = "LC1101HTR.SLDASM";
                        hinge2Name = "LC1101HBR.SLDASM";
                    }
                }
                openAssemblyFile(masterPath + "part\\door\\DOOR\\" + hinge1Name);
                swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(masterPath + "part\\door\\DOOR\\" + hinge1Name, hingepositionX1, hingepositionY1, hingepositionZ1);
                closeFile(masterPath + "part\\door\\DOOR\\" + hinge1Name);
                openAssemblyFile(masterPath + "part\\door\\DOOR\\" + hinge2Name);
                swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swAssembly = (AssemblyDoc)swModel;
                boolStatus = swAssembly.AddComponent(masterPath + "part\\door\\DOOR\\" + hinge2Name, hingepositionX2, hingepositionY2, hingepositionZ2);
                closeFile(masterPath + "part\\door\\DOOR\\" + hinge2Name);

                swApp.ActivateDoc2(doorlist[11] + ".SLDASM", false, 0);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModel.Save3(1, errors, warning);
                killSW();
                status = true;
            }

            return status;
        }
        public bool BuildSubAss_PullOutShelf(int w, string masterName, string fileName)
        {
            activeApp();
            openAssemblyFile(masterPath + "subassembly\\pulloutshelf\\" + masterName + "\\" + masterName + ".SLDASM");
            swModel.ClearSelection2(true);
            boolStatus = swModel.Extension.SelectByID2("width@BIFMA_PS-1@SP-LC1630" , "SKETCH", 0, 0, 0, false, 0, null, 0);
            boolStatus = swModel.Extension.SelectByID2("width@width@BIFMA_PS-1@SP-LC1630", "DIMENSION", 0, 0, 0, false, 0, null, 0);
            swModel.EditSketch();
            myDimension = ((Dimension)(swModel.Parameter("width@width@BIFMA_PS.Assembly")));
            myDimension.SystemValue = w/mToInch;
            reBuild();
            swApp.ActivateDoc2("SP-LC1630.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            boolStatus = swModel.Save3(1, errors, warning);
            swApp.ActivateDoc2("BIFMA_PS.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            boolStatus = swModel.Save3(1, errors, warning);
            swApp.ActivateDoc2("SP-LC1630.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            reBuild();
            packAndGo(libraryPath + "subassembly\\pulloutshelf\\" + masterName + "\\" + fileName + "\\", fileName, "-" + w.ToString(), "");
            killSW();
            return true;
        }

        public bool BuildAss_Cabinet(int w, int h, int d, string shellName, string confName, string cabName, List<List<string>> contentList, string pullPro,String TRACKPRO,
            List<string> drawername, List<int> drawerx, List<int> drawery, string drawerStyle,List<int> drawerw,List<int>drawerlock, List<int>drawerh, List<string> drawerheadName, List<string> drawerbodyName, List<string> drawerTrack, List<int> drawerload,
            List<string> doorKey, List<int> doorx, List<int> doory, List<int> doorconf, string doorStyle, List<int> doorh, List<int> doorw, List<int>doorlock, bool pairof, string hinge, string lockstring, string pullOutShelfString, 
            List<string> othersubList)
        {
            bool statuse = false;
            Database database = new Database();
            activeApp();
            string cabLibraryPath = libraryPath + "Assembly\\cabinet\\";
            string shellLibraryPath = libraryPath + "subAssembly\\shell\\" + shellName + "\\" + shellName + ".SLDASM";
            string confLibraryPath = libraryPath + "subAssembly\\conf\\" + confName + "\\" + confName + ".SLDASM";
            List<string> BomForTable = new List<string>();
            List<string> desForTable = new List<string>();
            ///Shell
            string shellname = shellName;
            List<string> shellBomList = new List<string>();
            List<string> partClassList = new List<string>();
            List<string> partDes = new List<string>();
            List<bool> toeboollist = new List<bool>();
            List<bool> uppontoeboollist = new List<bool>();
            List<List<bool>> commentTips = new List<List<bool>>();
            /*partGroup_shell_ori*/
            List<string> partGroup_shell_ori = new List<string>();
            List<string> partGroup_shell_ori_tag = new List<string>();
            List<bool> partGroup_shell_ori_toebool = new List<bool>();
            List<bool> partGroup_shell_ori_uppontoebool = new List<bool>();
            string[] shellNameArray = shellname.Split('_');
            bool toeornot = false;
            string toespaceClass = "";
            string sql = "select * from partclass where toespace = 'True'";
            MySqlDataReader rd = database.getMySqlReader_rd1(sql);
            if (rd.Read())
            {
                toespaceClass = rd.GetString("name");
            }
            database.CloseConnection_rd1();
            sql = "select * from bom_shell where keyvalue = '" + shellNameArray[0] + "_" + shellNameArray[1] + "_" + shellNameArray[2] + "'";
            rd = database.getMySqlReader_rd1(sql);
            if (rd.Read())
            {
                if (rd.GetString(toespaceClass)!="")
                {
                    toeornot = true;
                }
            }
            database.CloseConnection_rd1();
            DataTable dt = database.getMySqlDatatable_rd1("select * from partclass where subass = 'Shell'");
            foreach (DataRow dr in dt.Rows)
            {
                partGroup_shell_ori.Add(dr["name"].ToString());
                partGroup_shell_ori_tag.Add(dr["tag"].ToString());
                partGroup_shell_ori_toebool.Add(Convert.ToBoolean(dr["toespace"]));
                partGroup_shell_ori_uppontoebool.Add(Convert.ToBoolean(dr["uppontoespace"]));
            }
            database.CloseConnection_rd1();
            
            MySqlDataReader rd1 = database.getMySqlReader_rd1("select * from bom_shell where keyvalue = '" + shellNameArray[0] + "_" + shellNameArray[1] + "_" + shellNameArray[2] + "'");
            if (rd1.Read())
            {
                for (int i = 0; i < partGroup_shell_ori.Count; i++)
                {
                    string[] masterindex = rd1.GetString(partGroup_shell_ori[i]).Split('_');
                    for (int j = 0; j < masterindex.Count(); j++)
                    {
                        string parthead = "";
                        string[] contentArray = masterindex[j].Split('|');
                        if (contentArray.Count() == 1)
                        {
                            if ((masterindex != null) || (masterindex[j] != "0"))
                            {
                                MySqlDataReader rd2 = database.getMySqlReader_rd2("select * from masterpart_shell where partclass = '" + partGroup_shell_ori[i] + "' and num = '" + masterindex[j] + "'");
                                if (rd2.Read())
                                {
                                    parthead = rd2.GetString("parthead");
                                }
                                database.CloseConnection_rd2();

                            }
                        }
                        else
                        {
                            MySqlDataReader rd2 = database.getMySqlReader_rd2("select * from masterpart_shell where partclass = '" + contentArray[0] + "' and num = '" + contentArray[1] + "'");
                            if (rd2.Read())
                            {
                                parthead = rd2.GetString("parthead");
                            }
                            database.CloseConnection_rd2();
                        }
                        List<bool> tipCommand = CabinetOverview.tipConmmendBool(parthead, w, h, d);
                        if ((parthead != "") && ((tipCommand == new List<bool>()) || ((tipCommand != new List<bool>()) && (tipCommand[0] == false))))
                        {
                            partClassList.Add(partGroup_shell_ori[i]);
                            partDes.Add(partGroup_shell_ori_tag[i]);
                            shellBomList.Add(parthead);
                            toeboollist.Add(partGroup_shell_ori_toebool[i]);
                            uppontoeboollist.Add(partGroup_shell_ori_uppontoebool[i]);
                        }

                        int partNameReg = 0;
                        List<string> partLoc = new List<string>();
                        partLoc = CabinetOverview.getPartLocString(parthead, partGroup_shell_ori[i]);
                        partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(CabinetOverview.getPartVarString(parthead, partGroup_shell_ori[i]));
                        List<bool> commentCommand = CabinetOverview.tipConmmendBool(parthead, w, h, d);
                        //MySqlDataReader rd2 = database.getMySqlReader_rd1("select * from partclass where name = '" + partClassList[i] + "'");
                        //if (rd2.Read())
                        //{
                        //    partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(rd2.GetString("variables"));
                        //    partLoc = rd2.GetString("locations");
                        //}
                        //database.CloseConnection_rd1();
                        BomForTable.Add(CabinetOverview.genPartNameByMaster(w, h, d, parthead, partNameReg));
                        desForTable.Add(partDes[j]);
                        commentTips.Add(commentCommand);
                    }
                }
            }
            database.CloseConnection_rd1();
            
            if (File.Exists(shellLibraryPath))
            {}
            else
            {
                BuildSubAss_Shell(w, h, d, shellBomList, partClassList, shellName, toeboollist, uppontoeboollist, shellNameArray[0], shellNameArray[1], "M", "", "");
            }
            for (int i = 0; i < shellBomList.Count; i++)
            {
                
            }
            string cst = "";
            if (confName != "")
            {
                
                if (confName.Substring(0, 2) == "PH")
                {
                    cst = "PH";
                }

                int h_tem = CabinetOverview.adjustHforFont(shellname.Split('_')[0], h);

                List<List<int>> hidrailindex = ShellParts.hidrailindex(doorh, doorx, doorw, h_tem, drawerw, drawery, shellName.Split('_')[0], w, drawerlock, drawerx, drawerh);
                List<List<int>> centerpoindex = ShellParts.centerpoindex(hidrailindex, h, drawery, drawerw, w, shellName.Split('_')[0]);
                int partitionindex = ShellParts.partitionindex(shellName.Split('_')[0], doorx, doory, w);
                string locklist = "";
                for (int i = 0; i < drawerlock.Count; i++)
                {
                    locklist += drawerlock[i].ToString() + "_";
                }
                for (int i = 0; i < doorlock.Count; i++)
                {
                    locklist += doorlock[i].ToString() + "_";
                }
                locklist = locklist.Substring(0, locklist.Count() - 1);
                List<List<int>> securitypanelindex = ShellParts.securitypanelindex(locklist, doorx, drawery, w, drawerx, drawerw);
                string flx = "";
                for (int i = 0; i < hidrailindex.Count; i++)
                {
                    flx += hidrailindex[i][0] + "_" + hidrailindex[i][1] + "_" + hidrailindex[i][2];
                    if (i == hidrailindex.Count - 1)
                    {
                        flx += "+";
                    }
                    else
                    {
                        flx += "-";
                    }
                }
                for (int i = 0; i < centerpoindex.Count; i++)
                {
                    flx += centerpoindex[i][0] + "_" + centerpoindex[i][1];
                    if (i == centerpoindex.Count - 1)
                    {
                        flx += "+";
                    }
                    else
                    {
                        flx += "-";
                    }
                }
                flx += partitionindex + "+";
                for (int i = 0; i < securitypanelindex.Count; i++)
                {
                    flx += securitypanelindex[i][0] + "_" + securitypanelindex[i][1] + "_" + securitypanelindex[i][2];
                    if (i == securitypanelindex.Count - 1)
                    {
                        flx += "+";
                    }
                    else
                    {
                        flx += "-";
                    }
                }
                if (File.Exists(confLibraryPath))
                { }
                else
                {
                    confLibraryPath = libraryPath + "subAssembly\\Conf\\" + confName + "\\" + confName + ".SLDASM";
                    string confLibraryPath1 = libraryPath + "subAssembly\\Conf\\";

                    List<string> confBomList = new List<string>();
                    partClassList = new List<string>() { "1280", "13", "1400", "1460" };
                    partDes = new List<string>() { "Hiddenrail", "Centerpost", "Partition", "Security Panel" };
                    /*partGroup_shell_ori*/
                    rd1 = database.getMySqlReader_rd1("select * from bom_conf where keyvalue = '" + shellName.Split('_')[1] + "'");
                    if (rd1.Read())
                    {
                        for (int i = 0; i < partClassList.Count; i++)
                        {
                            string masterindex = rd1.GetString(partClassList[i]);
                            string parthead = "";
                            if ((masterindex != null) || (masterindex != "0"))
                            {
                                MySqlDataReader rd2 = database.getMySqlReader_rd2("select * from masterpart_conf where partclass = '" + partClassList[i] + "' and num = '" + masterindex + "'");
                                if (rd2.Read())
                                {
                                    parthead = rd2.GetString("parthead");
                                }
                                database.CloseConnection_rd2();

                                confBomList.Add(parthead);
                            }
                        }
                    }
                    database.CloseConnection_rd1();
                    for (int i = 0; i < confBomList.Count; i++)
                    {
                        if (i == 0)
                        {
                            for (int j = 0; j < hidrailindex.Count; j++)
                            {
                                BomForTable.Add(confBomList[i] + "-" + hidrailindex[j][2].ToString());
                                desForTable.Add(partDes[i]);
                            }
                        }
                        else if (i == 1)
                        {
                            for (int j = 0; j < centerpoindex.Count; j++)
                            {
                                BomForTable.Add(confBomList[i] + "-" + centerpoindex[j][0].ToString());
                                desForTable.Add(partDes[i]);
                            }
                        }

                    }

                    if (File.Exists(confLibraryPath))
                    {


                    }
                    else
                    {
                        //swfunctions.newAssembly();
                        BuildSubAss_Conf(hidrailindex, centerpoindex, partitionindex, securitypanelindex, confBomList, partClassList, w, h, d, confName, toeornot, shellname.Split('_')[0], shellname.Split('_')[1]);
                        //swfunctions.buildconf(hidrailindex, centerpoindex, partitionindex,  securitypanelindex, confBomList, partClassList, w, h, d);
                        //swfunctions.saveAssemblyAs(confLibraryPath1, cabAssComb_conf.Text + "_" + textBox30.Text + "_" + textBox31.Text + "_" + textBox32.Text + "-" + flx);
                        //swfunctions.closeFile(confLibraryPath);
                    }
                }
            }
            //DRAWER
            Dictionary<string, string> dic_drawerstyle = new Dictionary<string, string>() {
                    {"IN", "PN" }, {"IN & SS", "SN"}, {"OF", "PH"}, {"OF & SS", "SH"}, {"OF & WOOD", "WH"} };
            for (int i = 0; i < drawerx.Count; i++)
            {
                if (File.Exists(libraryPath + "subassembly\\drawer\\" + drawername[i] + "\\" + drawername[i] + ".SLDASM"))
                {

                }
                else
                {
                    BuildSubAss_Drawer(Convert.ToDouble(drawerw[i]), Convert.ToDouble(drawerh[i]), drawerheadName[i], drawerStyle, drawerbodyName[i], drawerTrack[i], pullPro, drawerlock[i], 0, drawername[i]);
                }
            }
            //DOOR
            Dictionary<int, string> dic_doorconf = new Dictionary<int, string>() { { 1, "L" }, { 2, "R" } };
            List<string> doornames = new List<string>();
            List<string> doorType = new List<string>();
            for (int i = 0; i < doorx.Count; i++)
            {
                List<string> doorList = DoorParts.doorList_New_3D(doorStyle, doorconf[i], doorw[i], doorh[i], doorlock[i], 0, pullPro, hinge);
                doornames.Add(doorList[11]);
                doorType.Add(doorList[14]);
                if (File.Exists(libraryPath + "subassembly\\door\\" + doorType[i] + "\\" + dic_doorconf[doorconf[i]] + "\\" + doornames[i] + "\\" + doornames[i] + ".SLDASM"))
                { }
                else
                {
                    List<string> doorlist = DoorParts.doorList_New_3D(doorStyle, doorconf[i], Convert.ToDouble(doorw[i]), Convert.ToDouble(doorh[i]), doorlock[i], 0, pullPro, hinge);
                    BuildSubAss_Door(doorList, doorlist[14]);
                }
            }
            //PULLOUTSHELF
            string[] pulloutshelfArray = pullOutShelfString.Split(',');
         
                List<double> heihei = new List<double>() { 0 };
                string pulloutshelfType = "SP-LC1630";
                int shelfNum = Convert.ToInt16(pulloutshelfArray[0]);
            List<double> shelfLoc = new List<double>();
            if ((pulloutshelfArray.Count() > 1)&&(pulloutshelfArray[0]!="0"))
            {
                for (int i = 1; i < pulloutshelfArray.Count(); i++)
                {
                    heihei.Add(Convert.ToDouble(pulloutshelfArray[i]));
                }
                
                for (int i = 1; i < heihei.Count; i++)
                {
                    shelfLoc.Add(heihei[i]);
                }
            }
                string shelfName = pulloutshelfType + "-" + w.ToString();
                string pullOutShelfLibPath = libraryPath + "subassembly\\pulloutshelf\\" + pulloutshelfType + "\\" + shelfName + "\\" + shelfName + ".SLDASM";
            if (shelfNum != 0)
            {
                if (File.Exists(pullOutShelfLibPath))
                {

                }
                else
                {
                    BuildSubAss_PullOutShelf(w, pulloutshelfType, shelfName);
                }
            }
            //
            List<string> accString = new List<string>();
            List<string> accNewName = new List<string>();
            double nh = CabinetOverview.cabinetFullHeight(h, shellname.Split('_')[0]);
            List<string> otherSubFullName = new List<string>();
            List<string> otherSubClass = new List<string>();
            List<string> otherSubLoc = new List<string>();
            for (int i = 0; i < othersubList.Count; i++)
            {
                if (othersubList[i] != "")
                {
                    int partNameReg = 0;
                    string partLoc = "";
                    string partClass = "";
                    rd = database.getMySqlReader_rd1("select * from masterpart_others where parthead = '" + othersubList[i] + "'");
                    if (rd.Read())
                    {
                        partClass = rd.GetString("partclass");
                    }
                    database.CloseConnection_rd1();
                    rd = database.getMySqlReader_rd1("select * from partclass where name = '" + partClass + "'");
                    if (rd.Read())
                    {
                        partNameReg = CabinetOverview.fromVarToPartClassNameRegulation(rd.GetString("variables"));
                        partLoc = rd.GetString("locations");
                    }
                    database.CloseConnection_rd1();
                    string partname = CabinetOverview.genPartNameByMaster(w, h, d, othersubList[i], partNameReg);
                    otherSubFullName.Add(partname);
                    otherSubClass.Add(partClass);
                    otherSubLoc.Add(partLoc);
                    if (File.Exists(libraryPath + "part\\others\\" + partClass + "\\" + othersubList[i] + "\\" + partname + "\\" + partname + ".SLDASM"))
                    { }
                    else
                    {
                        BuildPart_Other(partClass, w, h, d, othersubList[i]);
                    }
                }
                else
                {
                    otherSubFullName.Add("");
                    otherSubClass.Add("");
                    otherSubLoc.Add("");
                }
            }
            //if (shellname.Split('_')[0] == "Base")
            //{
            //    accString =new string[]{ "K428" };
            //    accNewName = new string[] { "K428" + "-" + w.ToString() + "-" + h.ToString() };
            //    wvar = new bool[] {true };
            //    hvar = new bool[] {true };
            //    dvar = new bool[] { false};
            //    xloc = new string[] {"Middle" };
            //    yloc = new string[] {"Middle" };
            //    zloc = new string[] {"Middle" };
            //    for (int i = 0; i < accString.Count(); i ++)
            //    {
            //        if (File.Exists(libraryPath + "acc\\" + accString[i] + "\\" + accNewName[i] + ".SLDASM"))
            //        {

            //        }
            //        else
            //        {
            //            activeApp();
            //            BuildAcc(accString[i], w, nh, d, h, wvar[i], hvar[i], dvar[i], accNewName[i]);
            //        }
            //    }
            //}



            
            activeApp();
            //newAssembly(w, h, cabLibraryPath + "\\", cabName);
            openAssemblyFile(masterPath + "1.SLDASM");
            
            int featCount = swModel.GetFeatureCount();
            for (int i = featCount; i > 0; i--)
            {
                swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - i);
                if ((swFeature) != null)
                {
                    string temp_featName_ori = swFeature.Name;
                    string temp_featName = swFeature.Name;
                    if (temp_featName_ori == "widthHeight")
                    {
                        changeDimension("width@widthHeight", Convert.ToDouble(w / mToInch));
                        changeDimension("height@widthHeight", Convert.ToDouble(nh / mToInch));
                        reBuild();
                    }
                }
            }
            insertIntoAssembly_shell(shellLibraryPath, cabName);
            if (confName != "")
            {
                insertIntoAssembly_conf(confLibraryPath, Convert.ToDouble(d), cst, cabName, toeornot, shellname.Split('_')[0]);
            }
            
            saveAssemblyAs(masterPath + "drawing\\", "2");

            
            for (int i = 0; i < drawername.Count; i++)
            {
                if (drawername[i] != "")
                {
                    List<double> xyp = new List<double>() { drawerx[i], drawery[i] };
                    insertIntoAssembly_drawer(libraryPath + "subassembly\\drawer\\" + drawername[i] + "\\" + drawername[i] + ".SLDASM", xyp, w, h, d, cabName, drawerStyle, drawerh[i], toeornot, drawerlock[i], pullPro, shellname.Split('_')[0]);
                }
            }

            bool pair = false;
            if (doornames.Count == 2)
            {
                pair = true;  
            }
            for (int i = 0; i < doornames.Count; i++)
            {
                if (doornames[i] != "")
                {
                    List<double> xyp = new List<double>() { doorx[i], doory[i] };
                    insertIntoAssembly_door(libraryPath + "subassembly\\door\\" + doorType[i] + "\\" + dic_doorconf[doorconf[i]] + "\\" + doornames[i] + "\\" + doornames[i] + ".SLDASM", xyp, w, h, d, cabName, shellname.Split('_')[1], Convert.ToDouble(doorh[i]), pair, toeornot, doorlock[i], pullPro, shellname.Split('_')[0]);
                }
            }
            for (int i = 0; i < otherSubFullName.Count; i++)
            {
                if (otherSubFullName[i] != "")
                {
                    insertIntoAssembly_otherSub("2.SLDASM", libraryPath + "part\\others\\" + otherSubClass[i] + "\\" + othersubList[i] + "\\" + otherSubFullName[i] + "\\" + otherSubFullName[i] + ".SLDASM", w, h, d, otherSubLoc[i], "");
                }
            }
            for (int i = 0; i < shelfNum; i++)
            {
                insertIntoAssembly_pulloutShelf(pullOutShelfLibPath, shelfLoc[i], w, h, d, cabName, shellname.Split('_')[0], shellname.Split('_')[1]);
            }
            // boolStatus = swModel.Save3(1, errors, warning);

            //for (int i = 0; i < accString.Count(); i++)
            //{
            //    insertIntoAssembly_acc(shellname.Split('_')[0], nh, w, d, accString[i], accNewName[i], xloc[i], yloc[i], zloc[i]);
            //}

            saveAssemblyAs(masterPath + "drawing\\", "1");
            killSW();
            double cw = 0;
            if ((shellname.Split('_')[0] == "M") || (shellname.Split('_')[0] == "Mobile"))
            {
                List<int> _heihei = new List<int>();
                for (int i = 0; i < heihei.Count; i++)
                {
                    _heihei.Add(Convert.ToInt16(Math.Round(heihei[i])));
                }
                string myname = CabinetOverview.myname(shellname.Split('_')[0], "", shellname.Split('_')[1], "M", w, h, d, "0", 0, drawerx,
                    drawery, drawerw, drawerh, drawerlock, new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, drawerload, doorx, doory, doorw, doorh, doorconf, new List<int>() { 1, 1, 1, 1 },
                    doorlock, new List<int>() { 0, 0, 0, 0 }, pullPro, hinge, TRACKPRO, lockstring, "", "", "NSC", _heihei);
                int ll = myname.Count();
                myname = myname.Substring(0, ll - 2);

                cw = CabinetOverview.readCounterWeight(myname, drawery, w, h);
                if (cw == 0)
                {
                    counterWeightInfo(myname, cabName);
                }
                cw = CabinetOverview.readCounterWeight(myname, drawery, w, h);

                //rd = database.getMySqlReader("select * from counterweightinfo where cabname = '" + myname + "'");
                //if (rd.Read())
                //{
                //    cw = rd.GetDouble("counterweight");
                //    database.CloseConnection();
                //}
                //else
                //{
                    
                //    database.CloseConnection();
                //    rd = database.getMySqlReader("select * from counterweightinfo where cabname = '" + myname + "'");
                //    if (rd.Read())
                //    {
                //        cw = rd.GetDouble("counterweight");
                //    }
                //    database.CloseConnection();
                //}
            }

            VisualInfo vsInfo = new VisualInfo();
            vsInfo.exportDrawing(shellname.Split('_')[0], shellname.Split('_')[1], w, h, d, "M", "NSC", "",cw, doorh, doorw, doorconf, doorx, doory, doorlock, drawerw, drawerh, drawerx, drawery, drawerlock, drawerload, cabName, shellName.Split('_')[2]);

            statuse = true;
            //activeApp();
            //openAssemblyFile(@"C:\Users\caiyu\OneDrive\Desktop\d3test\New folder\1.SLDASM");
            //if (Directory.Exists(cabLibraryPath+ cabName))
            //{ }
            //else
            //{
            //    Directory.CreateDirectory(cabLibraryPath + cabName);
            //}

            ////packandgo(cabLibraryPath + cabLibraryPath + cabName + "\\");
            //packAndGo(cabLibraryPath + cabName + "\\", cabName, "", "");
            //killSW();
            //insertGeneralTabel(cabLibraryPath + cabName + "\\1.SLDDRW", contentList, 0.025, 0.25);
            //closeFile(cabLibraryPath + cabName + "\\1.SLDDRW");
            //killSW();




            //File.Delete(masterPath + "drawing\\", "1.SLDASM");
            //File.Delete(masterPath + "drawing\\", "2.SLDASM");

            return statuse;
        }
        public void packAndGo(string path, string fileName, string addSuffix, string addPrefix)
        {
            if (Directory.Exists(path))
            { }
            else
            {
                Directory.CreateDirectory(path);
            }
            //swApp.ActivateDoc2(filename, false, 0);
            swModel = ((ModelDoc2)(swApp.ActiveDoc));
            swModelExt = (ModelDocExtension)swModel.Extension;
            int[] statuses;
            Debug.Print("Pack and Go");
            swPackandGo = (PackAndGo)swModelExt.GetPackAndGo();
            int namesCount = swPackandGo.GetDocumentNamesCount();
            Debug.Print("    Number of model documents: " + namesCount);
            swPackandGo.IncludeDrawings = true;
            Debug.Print(" Include drawings: " + swPackandGo.IncludeDrawings);
            swPackandGo.IncludeSimulationResults = true;
            Debug.Print(" Include SOLIDWORKS Simulation results: " + swPackandGo.IncludeSimulationResults);
            swPackandGo.IncludeToolboxComponents = true;
            Debug.Print(" Include SOLIDWORKS Tollbox components: " + swPackandGo.IncludeToolboxComponents);
            object fileNames;
            object[] pgFileNames = new object[namesCount - 1];
            boolStatus = swPackandGo.GetDocumentNames(out fileNames);
            pgFileNames = (object[])fileNames;

            Debug.Print("  Current path and filenames: ");
            if ((pgFileNames != null))
            {
                for (int i = 0; i <= pgFileNames.GetUpperBound(0); i++)
                {
                    Debug.Print("    The path and filename is: " + pgFileNames[i]);
                }
            }
            object pgFileStatus;
            //pgFileNames[0] = path + fileName + ".SLDASM";
            boolStatus = swPackandGo.GetDocumentSaveToNames(out fileNames, out pgFileStatus);
            pgFileNames = (object[])fileNames;
            Debug.Print("");
            Debug.Print("  Current default save-to filenames: ");
            //pgFileNames[0] = path + fileName + ".SLDASM";
            if ((pgFileNames != null))
            {
                for (int i = 0; i <= pgFileNames.GetUpperBound(0); i++)
                {
                    Debug.Print("   The path and filename is: " + pgFileNames[i]);
                }
            }
            boolStatus = swPackandGo.SetSaveToName(true, path);
            swPackandGo.FlattenToSingleFolder = true;
            swPackandGo.AddPrefix = addPrefix;
            swPackandGo.AddSuffix = addSuffix;
            statuses = (int[])swModelExt.SavePackAndGo(swPackandGo);

        }

        public void exportDrawing_Shell(string model, string cstyle, string specialdesign, string metaltop, int w, int h, int d, string metalTop, string selfClose, string lockPro,
            List<int> door_h, List<int> door_w, List<int> door_confi, List<int> door_x, List<int> door_y, List<int> door_lock,
            List<int> drawer_w, List<int> drawer_h, List<int> drawer_x, List<int> drawer_y, List<int> drawer_lock, List<int> drawer_load,
            List<string> shellBom,
            string drawingPath)
        {
            List<string> partname2 = new List<string>() { "Document No." };
            List<string> des2 = new List<string>() { "Title" };
            List<string> mat2 = new List<string>() { "Material" };
            List<string> num2 = new List<string>() { "QTY." };

            string[] keyGroup = {"Base_PN_-","Base_SN_-","Base_PH_-","Base_WH_-","Mobile_PH_-","Mobile_SH_-","Mobile_WH_-","Glide_PH_-","Glide_SH_-","Glide_WH_-","Suspended_PH_-",
                "Suspended_SH_-","Suspended_WH_-","Wall_PN_-","Wall_PH_-","Wall_WH_-","Special Base_PN_A","Base_PN_H","Base_PN_S","Base_PH_H","Base_PH_S","Special Base_PN_F_SC",
                "Special Base_SN_A","Special Base_SN_F_SC","Special Base_PN_VP","Special Base_SN_VP","Storage_PN_-","Storage_SN_-","Storage_PH_-","Storage_WH_-","Special Base_PN_F_NSC",
                "Special Base_SN_F_NSC"};
            if (specialdesign == "")
            {
                specialdesign = "-";
            }
            if (keyGroup.Contains(model + "_" + cstyle + "_" + specialdesign))
            {
                Dictionary<int, string> dicMat = new Dictionary<int, string>() { { 1, "CRS" }, { 2, "SS" }, { 3, "GUSSET" } };
                Dictionary<double, string> dicThickness = new Dictionary<double, string>() { { 0.036, "20" }, { 0.048, "18" }, { 0.06, "16" }, { 0.074, "14" }, { 0.104, "12" } };
                int doorh = 0;
                int door_conf1 = 0;
                int door_conf2 = 0;
                if (door_h.Count > 0)
                {
                    doorh = door_h[0];
                    if (door_h.Count == 1)
                    {
                        door_conf1 = door_confi[0];
                    }
                    else if (door_h.Count == 2)
                    {
                        if (door_y[0] == door_y[1])
                        {
                            if (door_confi[0] > 9)
                            {
                                door_conf1 = 4;
                            }
                            else
                            {
                                door_conf1 = 3;
                            }
                        }
                        else
                        {
                            door_conf1 = door_confi[0];
                            door_conf2 = door_confi[1];
                        }
                    }
                    else
                    {
                        if (door_confi[0] > 9)
                        {
                            door_conf1 = 4;
                        }
                        else
                        {
                            door_conf1 = 3;
                        }
                        if (door_confi[2] > 9)
                        {
                            door_conf2 = 4;
                        }
                        else
                        {
                            door_conf2 = 3;
                        }
                    }
                }
                List<List<string>> shellpart = ShellParts.shellParts(model, cstyle, "-", d, h, w, doorh, door_conf1, door_conf2, metalTop, door_h, drawer_w, drawer_y, door_x, door_w, drawer_lock, drawer_x, drawer_h, "NSC", "");
                List<string> shellPartTag = new List<string>() { "sideleft","back", "top", "toe", "runner", "toespacebox", "gusset", "shelf", "removableback", "bottom", "stileside", "stiletop", "stilebottom",
            "front", "hiddenrail", "partition", "centerpost","securitypanel","selfclosepackage"};
                for (int i = 0; i < shellpart.Count - 1; i++)
                {
                    string tag = shellPartTag[i];
                    for (int j = 0; j < shellpart[i].Count; j++)
                    {
                        if ((tag != "centerpost") || (j == 0))
                        {

                            if (tag == "sideleft")
                            {
                                if (j != 0)
                                {
                                    tag = "sideright";
                                }
                            }
                            List<string> cat = Price.nametocathwd(shellpart[i][j]);
                            if (shellpart[i][j].Contains("SS"))
                            {
                                cat[0] += "-SS";
                            }
                            string partname = VisualInfo.getVisualName(tag, cat[0], Convert.ToInt16(cat[1]), Convert.ToInt16(cat[2]), Convert.ToInt16(cat[3]), shellpart[i][j]);
                            bool removable = VisualInfo.getRemovability(tag, cat[0], Convert.ToInt16(cat[1]), Convert.ToInt16(cat[2]), Convert.ToInt16(cat[3]));
                            if (removable == false)
                            {
                                int k = 0;
                                for (k = 0; k < partname2.Count; k++)
                                {
                                    if (partname == partname2[k])
                                    {
                                        break;
                                    }
                                }
                                if (k < partname2.Count)
                                {
                                    num2[k] = (Convert.ToInt16(num2[k]) + 1).ToString();
                                }
                                else
                                {
                                    string des = VisualInfo.getVisualDescription(tag, cat[0], Convert.ToInt16(cat[1]), Convert.ToInt16(cat[2]), Convert.ToInt16(cat[3]));
                                    string mat = dicMat[Price.matindexCSG(shellpart[i][j])] + dicThickness[Price.matgauge(cat[0])];
                                    string qt = "1";
                                    partname2.Add(partname);
                                    des2.Add(des);
                                    mat2.Add(mat);
                                    num2.Add(qt);
                                }
                            }
                            if ((tag == "centerpost"))
                            {
                                j = shellpart[i].Count;
                            }
                        }
                    }
                }
                partname2.Add("SBS43"); des2.Add("1/8 DIA. X 5/16 STEEL POP RIVET"); num2.Add("34"); mat2.Add("STEEL");
                partname2.Add("SBS64"); des2.Add("3/16 DIA.X7/16 STEEL POP RIVET"); num2.Add("2"); mat2.Add("STEEL");
            }
            else
            {

            }
            List<string> itemno2 = new List<string>() { "ITEM NO." };
            for (int i = 1; i < partname2.Count; i++)
            {
                itemno2.Add(i.ToString());
            }
            insertGeneralTabel(drawingPath, new List<List<string>>() { itemno2, partname2, des2, mat2, num2 }, 0.0068, 0.2735);

        }
        ExportPdfData swPDFData;
        string[] obj;
        Sheet swsheet;
        DispatchWrapper[] dispWrapArr = null;
        public void exportDrawingToPDF(string path, string name)
        {

            swPDFData = (ExportPdfData)swApp.GetExportFileData((int)swExportDataFileType_e.swExportPdfData);
            swDrawing = (DrawingDoc)swModel;
            obj = (string[])swDrawing.GetSheetNames();
            object[] objs = new object[obj.Count()];
            for (int i = 0; i < obj.Count(); i++)
            {
                boolStatus = swDrawing.ActivateSheet(obj[i]);
                swsheet = (Sheet)swDrawing.GetCurrentSheet();
                objs[i] = swsheet;
            }
            dispWrapArr = (DispatchWrapper[])ObjectArrayToDispatchWrapperArray(objs);
            boolStatus = swPDFData.SetSheets((int)swExportDataSheetsToExport_e.swExportData_ExportSpecifiedSheets, (dispWrapArr));
            swPDFData.ViewPdfAfterSaving = true;
            boolStatus = swModel.Extension.SaveAs(path + name + ".pdf", 0, 1, null, errors, warning);
            killSW();
            //Process.Start(path + name + ".pdf");
        }
        public DispatchWrapper[] ObjectArrayToDispatchWrapperArray(object[] Objects)
        {

            int ArraySize = 0;
            ArraySize = Objects.GetUpperBound(0);
            DispatchWrapper[] d = new DispatchWrapper[ArraySize + 1];
            int ArrayIndex = 0;
            for (ArrayIndex = 0; ArrayIndex <= ArraySize; ArrayIndex++)
            {
                d[ArrayIndex] = new DispatchWrapper(Objects[ArrayIndex]);
            }

            return d;
        }

        public double counterWeightInfo(string myname, string cabname)
        {
            Database database = new Database();
            string[] mynameArray = myname.Split('-');
            string[] mynameArray1 = mynameArray[0].Split('_');
            string[] mynameArray2 = mynameArray[1].Split('_');
            string[] mynameArray3 = mynameArray[2].Split('_');
            string[] mynameArray4 = mynameArray[3].Split('_');

            string model = mynameArray1[0];
            string specialDesign = mynameArray1[1];
            string cstyle = mynameArray1[2];
            string metalTop = mynameArray1[3];
            int w = Convert.ToInt16(mynameArray1[4]);
            int h = Convert.ToInt16(mynameArray1[5]);
            int d = Convert.ToInt16(mynameArray1[6]);
            string pullouts = mynameArray1[7];
            string secp = mynameArray1[8];
            string selfclo = mynameArray1[9];

            List<int> drawerx = new List<int>();
            List<int> drawery = new List<int>();
            List<int> drawerh = new List<int>();
            List<int> drawerw = new List<int>();
            List<int> drawerlock = new List<int>();
            List<int> drawerload = new List<int>();
            List<int> drawerch = new List<int>();
            List<int>[] drawerListArray = { drawerx, drawery, drawerw, drawerh, drawerlock, drawerch,  drawerload };
            for (int i = 0; i < mynameArray2.Count(); i++)
            {
                if (mynameArray2[i] != "")
                {
                    string[] drawerArray = mynameArray2[i].Split(',');
                    for (int j = 0; j < drawerListArray.Count(); j++)
                    {
                        drawerListArray[j].Add(Convert.ToInt16(drawerArray[j]));
                    }
                }
            }

            List<int> doorcon = new List<int>();
            List<int> doormat = new List<int>();
            List<int> doorx = new List<int>();
            List<int> doory = new List<int>();
            List<int> doorw = new List<int>();
            List<int> doorh = new List<int>();
            List<int> doorlock = new List<int>();
            List<int> doorch = new List<int>();
            List<int>[] doorListArray = { doorcon, doormat, doorx, doory, doorw, doorh, doorlock, doorch };
            for (int i = 0; i < mynameArray3.Count(); i++)
            {
                if (mynameArray3[i] != "")
                {
                    string[] doorArray = mynameArray3[i].Split(',');
                    for (int j = 0; j < doorListArray.Count(); j++)
                    {
                        doorListArray[j].Add(Convert.ToInt16(doorArray[j]));
                    }
                }
            }

            string pullStyle = mynameArray4[0];
            string hingeStyle = mynameArray4[1];
            string track = mynameArray4[2];
            string lockPro = mynameArray4[3];
            string cardPro = mynameArray4[4];
            string lockGroup = "";
            try
            {
                lockGroup = mynameArray4[5];
            }
            catch { }
            List<string> lockl = ShellParts.locknumList(lockPro, drawerlock);
            string lockList = "";
            for (int i = 0; i < lockl.Count; i++)
            {
                lockList += lockl[i];
                if (i != lockl.Count-1)
                {
                    lockList += "_";
                }
            }


            List<List<int>> hiddenrail = ShellParts.hidrailindex(doorh, doorx, doorw, h - 1, drawerw, drawery, model, w,
                                    drawerlock, drawerx, drawerh);//(y,x,l)
            List<List<int>> centerpost = ShellParts.centerpoindex(hiddenrail, h, drawery, drawerw, w, model);//(l, start_y)
            int partition = ShellParts.partitionindex(model, doorx, doory, w);//l
            List<List<int>> securitypanel = ShellParts.securitypanelindex(lockList, doorx, drawery, w, drawerx, drawerw);//(x, y, l)
            List<List<string>> drawerlist_Shell = new List<List<string>>();
            bool slfcl = false;
            if ((track == "T3") || (track == "T6") || (track == "T9"))
            {
                slfcl = true;
            }
            bool intl = false;
            if ((model != "M") && (model != "G"))
            {

            }
            else
            {
                if ((track == "T2L") || (track== "T5L") || (track == "T8L"))
                {
                    intl = true;
                }
            }
            
            List<int> dr_ext = index012_drawer(w, drawerh, drawerw);

            string cw_dr_center = "";
            string cw_dr_g = "";
            for (int j = 0; j < drawerx.Count; j++)
            {

                /// drawerlist[i]:
                /// 0:drawerheight
                /// 1:drawerwidth
                /// 2:lock(true/false)
                /// 3:cardholder(true/false)
                /// 4: drawerload
                /// 5: selfclose(t/f)
                /// 6: interlock(t/f)
                /// 7: extension(t/f)
                /// 8: drawerlocation_x
                /// 9: drawerlocation_y
                /// 10: drawername("DRA6-18SSPNFF100400K0")
                /// 11: drawerhead("DRH_IN6-18PN400K0")
                /// 12: drawerbody("DRBU6-18SS-5043")
                /// 13: pack an go drawername("6-18SS-5043")
                /// 14: track("DTF100-ZBFL18-STANDARD")
                /// 15: drawerhead without 'DRH'("_IN6-18PN400K0")
                /// 16: drawerhead startfrom height("6-18PN400K0")
                /// 17: wide(t/f)
                /// 18: extense or not(0: empty,inside/1: loaded, outside/2: loaded, inside)
                List<string> drawerlist = three_prep.drawerlist(intl, drawerlock[j], drawerch[j], model, drawerload[j], drawerh[j], drawerw[j], slfcl,
                    drawerx[j], drawery[j], cstyle, pullStyle, specialDesign, track, w, h, dr_ext[j]);
                drawerlist_Shell.Add(drawerlist);
                List<string> drawerCWList = Mdrawer(drawerlist, cstyle, cabname, 0, pullStyle, h, 0);
                cw_dr_center += drawerCWList[0];
                cw_dr_g += drawerCWList[1];
            }
            if (cw_dr_center != "")
            {
                cw_dr_center = cw_dr_center.Substring(0, cw_dr_center.Count() - 1);
                cw_dr_g = cw_dr_g.Substring(0, cw_dr_g.Count() - 1);
            }
            List<List<string>> doorlist = new List<List<string>>();
            for (int j = 0; j < doorx.Count; j++)
            {
                ///doorlis:
                ///0: h
                ///1: w
                ///2: lock(true/false)
                ///3: cardholder(true/false)
                ///4: doorconfiguration(1:left 2: right 41:sliding left 42:sliding right)
                ///5: doormaterial: {{"Solid", 1 },{"Framed Glass", 2 },{"Open Face", 0 },
                ///               { "Framed 1/8\" Glass", 3 }, { "Framed 1/4\" Glass", 4 }
                ///6: x
                ///7: y
                ///8: doorname
                ///9: innerdoorname
                ///10: outdoorname
                ///11：short doorname
                ///12: short outdoorname
                ///
                List<string> doorlist_in = three_prep.doorlist(doorh[j], doorw[j], cstyle, doorlock[j], doorch[j], doorcon[j], doormat[j], doorx[j],
                    doory[j], w, model, pullStyle, specialDesign, selfclo, hingeStyle);

                doorlist.Add(doorlist_in);
            }

            int pullOutShelfNum = Convert.ToInt16(pullouts.Split(',')[0]);
            List<int> pulloutshelfIndex = index012_pulloutshelf(pullOutShelfNum, w);
            for (int i = 0; i < pullOutShelfNum; i++)
            {
                List<string> shelfCWList = Mshelf(w, pulloutshelfIndex[i]);
                cw_dr_center += shelfCWList[0];
                cw_dr_g += shelfCWList[1];
            }
            if (cw_dr_center != "")
            {
                cw_dr_center = cw_dr_center.Substring(0, cw_dr_center.Count() - 1);
                cw_dr_g = cw_dr_g.Substring(0, cw_dr_g.Count() - 1);
            }
            string shelfTrackName = "";
            if (w >= 30)
            {
                shelfTrackName = "DTF200-ZBFL18-STANDARD";
            }
            else
            {
                shelfTrackName = "DTF100-ZBFL18-STANDARD";
            }
            List<string> shellCWList = Mshell(doorlist, pullStyle, cstyle, metalTop, hiddenrail, centerpost, partition, securitypanel, h, w, cabname, 0, drawerlist_Shell, shelfTrackName, pullOutShelfNum);




            //calculate counterweight
            List<double> drawer_grav_y = new List<double>();
            List<double> drawer_grav_z = new List<double>();
            double shell_center_y = 0;
            double shell_center_z = 0;
            List<double> drawer_g = new List<double>();
            double shell_g = 0;
            string[] cw_drawerstringarray = cw_dr_center.Split('_');
            string[] cw_drawergravityarray = cw_dr_g.Split('_');
            for (int j = 0; j < cw_drawerstringarray.Count(); j++)
            {
                if (cw_drawerstringarray[j] != "")
                {
                    string[] cw_dr_zy = cw_drawerstringarray[j].Split(',');
                    drawer_grav_z.Add(Convert.ToDouble(cw_dr_zy[0]));
                    drawer_grav_y.Add(Convert.ToDouble(cw_dr_zy[1]));
                    drawer_g.Add(Convert.ToDouble(cw_drawergravityarray[j]));
                }
            }
            string[] cw_shellcenterstringarray = shellCWList[0].Split(',');
            shell_center_z = Convert.ToDouble(cw_shellcenterstringarray[0]);
            shell_center_y = Convert.ToDouble(cw_shellcenterstringarray[1].Split('_')[0]);
            shell_g = Convert.ToDouble(shellCWList[1].Split('_')[0]);
            double counterbox_center_z = 17.5412;
            double counterbox_center_y = 19;

            double cw = 0;
            for (int j = 0; j < drawer_g.Count; j++)
            {
                cw -= drawer_g[j] * drawer_grav_z[j];
            }
            cw -= shell_g * shell_center_z;
            if (w >= 20)
            {
                cw += ((Convert.ToDouble(h)) * 31 / 32 + 0.505) * 44 / 4.45;
            }
            cw /= counterbox_center_z;

            string savenameforcw_ = myname.Split('+')[0];
            for (int j = savenameforcw_.Count() - 1; j >= 0; j--)
            {
                if (savenameforcw_.Substring(j, 1) == "_")
                {
                    break;
                }
                else
                {
                    savenameforcw_ = savenameforcw_.Substring(0, j);
                    //j--;
                }

            }



            string sql = "insert into counterweightinfo(cabname, dr_cent, dr_g, sh_cent, sh_g) values('"
                + savenameforcw_ + "', '" + cw_dr_center + "', '" + cw_dr_g + "', '" + shellCWList[0] + "', '" + shellCWList[1] + "')";
            database.mySqlExecuteQuery1(sql);
            database.CloseConnection1();
            return cw;
        }
        public static List<int> index012_pulloutshelf(int num, int full_w)
        {
            //index for pullout shelf pullout/load
            List<int> output = new List<int>();

            if (num > 0)
            {
                if ((full_w <20)||((full_w>=20)&&(num == 1)))
                {
                    output.Add(1);
                    for (int i = 1; i < num; i++)
                    {
                        output.Add(0);
                    }
                }
                else
                {
                    output.Add(1);
                    output.Add(2);
                    for (int i = 2; i < num; i++)
                    {
                        output.Add(0);
                    }
                }
            }

            return output;
        }
        public static List<int> index012_drawer(int full_w, List<int> drawer_h, List<int> drawer_w)
        {
            //index for drawer pullout/load
            List<int> dr_ext = new List<int>();
            if (drawer_h.Count > 0)
            {
                if ((full_w < 20) || ((full_w >= 20) && (drawer_h.Count == 1)))
                {
                    double big1 = drawer_h[0] * drawer_w[0];
                    dr_ext.Add(1);
                    int ind = 0;
                    for (int i = 1; i < drawer_h.Count; i++)
                    {
                        if (drawer_w[i] * drawer_h[i] > big1)
                        {
                            big1 = drawer_w[i] * drawer_h[i];
                            dr_ext[ind] = 0;
                            ind = i;
                            dr_ext.Add(1);
                        }
                        else
                        {
                            dr_ext.Add(0);
                        }
                    }
                }
                else
                {
                    double big1 = 0;
                    int ind1 = 0;
                    double big2 = 0;
                    int ind2 = 0;
                    if (drawer_h[0] * drawer_w[0] >= drawer_h[1] * drawer_w[1])
                    {
                        big1 = drawer_h[0] * drawer_w[0];
                        ind1 = 0;
                        dr_ext.Add(1);
                        big2 = drawer_h[1] * drawer_w[1];
                        ind2 = 1;
                        dr_ext.Add(2);
                    }
                    else
                    {
                        big1 = drawer_h[1] * drawer_w[1];
                        ind1 = 1;
                        dr_ext.Add(2);
                        big2 = drawer_h[0] * drawer_w[0];
                        ind2 = 0;
                        dr_ext.Add(1);
                    }
                    for (int i = 2; i < drawer_h.Count; i++)
                    {
                        if (drawer_w[i] * drawer_h[i] > big1)
                        {
                            dr_ext[ind2] = 0;
                            ind2 = ind1;
                            big2 = big1;
                            dr_ext[ind1] = 2;
                            ind1 = i;
                            big1 = drawer_w[i] * drawer_h[i];
                            dr_ext.Add(1);
                        }
                        else if ((drawer_w[i] * drawer_h[i] <= big1) && (drawer_w[i] * drawer_h[i] > big2))
                        {
                            dr_ext[ind2] = 0;
                            ind2 = i;
                            big2 = drawer_w[i] * drawer_h[i];
                            dr_ext.Add(2);
                        }
                        else
                        {
                            dr_ext.Add(0);
                        }
                    }
                }
            }
            return dr_ext;
        }
        public List<string> Mdrawer(List<string> drawerlist, string cstyle, string myname, int loc, string pullstyle, int h, int whichcab)
        {
            activeApp();
            //drawerbody
            
            int namesCount = 0;
            string myPath = null;
            int[] statuses = null;
            int longstatus = 0;
            int longwarnings = 0;

            ModelDoc2 swModel;
            ModelDocExtension swModelDocExt;
            PackAndGo swPackAndGo;
            bool boolstatus;
            int interrors = 0 ;
            int intwarnings = 0;
            int intstatus = 0;
            int swErrors = 0;
            int swWarnings = 0;
            PartDoc swPart;
            string sModelName;
            string sPathName;
            object varAlignment;
            double[] dataAlignment = new double[12];
            object varViews;
            string[] dataViews = new string[2];
            int options;
            ModelDoc2 swDoc;
            //pack and go body assembly
            //open body assembly
            //string unitpath = path + "\\" + projectname.value + "\\" + swfloor + "\\" + swroom + "\\" + swtag;
            string libpath = bifmapath + "\\BIFMA\\";
            Component2 swComp = default(Component2);
            if (File.Exists(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM"))
            {
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
            }
            else
            {

                if (Convert.ToInt32(drawerlist[0]) < 6)
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\DRBU\\3\\DRBU.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                }
                else
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\DRBU\\DRBU.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                }
                //pack and go
                swApp.ActivateDoc2("DRBU.SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swModelDocExt = (ModelDocExtension)swDoc.Extension;
                swPackAndGo = (PackAndGo)swModelDocExt.GetPackAndGo();
                namesCount = swPackAndGo.GetDocumentNamesCount();
                swPackAndGo.IncludeDrawings = true;
                swPackAndGo.IncludeSimulationResults = true;
                swPackAndGo.IncludeToolboxComponents = true;
                object fileNames;
                object[] pgFileNames = new object[namesCount];
                boolstatus = swPackAndGo.GetDocumentNames(out fileNames);
                pgFileNames = (object[])fileNames;
                object pgFileStatus;
                boolstatus = swPackAndGo.GetDocumentSaveToNames(out fileNames, out pgFileStatus);
                pgFileNames = (object[])fileNames;
                myPath = libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerbody" + "\\AC";
                boolstatus = swPackAndGo.SetSaveToName(true, myPath);
                swPackAndGo.FlattenToSingleFolder = false;
                swPackAndGo.AddPrefix = "";
                swPackAndGo.AddSuffix = drawerlist[13];
                statuses = (int[])swModelDocExt.SavePackAndGo(swPackAndGo);
                swApp.CloseDoc(bifmapath + "\\master\\DRAWER\\DRBU\\3\\DRBU.SLDASM");
                swApp.CloseDoc(bifmapath + "\\master\\DRAWER\\DRBU\\DRBU.SLDASM");
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerbody" + "\\AC" + "\\" + drawerlist[12] + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                boolstatus = swDoc.Extension.SelectByID2(drawerlist[14], "CONFIGURATIONS", 0, 0, 0, false, 0, null, 0);
                boolstatus = swDoc.ShowConfiguration2(drawerlist[14]);
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swApp.CloseDoc(libpath + "drawer" + "\\" + drawerlist[10] + "\\" + "drawerbody" + "\\AC" + "\\" + drawerlist[12] + ".SLDASM");
                string directoryPath = libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerbody" + "\\AC";
                string newDirectoryPath = libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerbody";
                string[] filename = {"\\" +drawerlist[12] + ".SLDASM",
                                 "\\" + drawerlist[12] + ".SLDPRT",
                                 "\\" + drawerlist[12] + ".SLDDRW"};

                try
                {
                    foreach (string filename1 in filename)
                    {
                        var ori = directoryPath + filename1;
                        var dest = newDirectoryPath + filename1;
                        File.Move(ori, dest);
                    }
                }
                catch
                {
                }


                //open drawer body          
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "\\drawer" + "\\" + drawerlist[10] + "\\drawerbody" + "\\" + drawerlist[12] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(drawerlist[10] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //change size
                boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSketch();
                swDoc.ClearSelection2(true);
                boolstatus = swDoc.Extension.SelectByID2("WIDTH@SIZE@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                boolstatus = swDoc.Extension.SelectByID2("HEIGHT@SIZE@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", -0.13259303986718474, -0.016468606438235153, -0.33084417233571484, false, 0, null, 0);
                Dimension myDimension = null;
                myDimension = ((Dimension)(swDoc.Parameter("WIDTH@SIZE")));
                myDimension.SystemValue = Convert.ToDouble(drawerlist[1]) * 25.4 / 1000;
                boolstatus = swDoc.Extension.SelectByID2("HEIGHT@SIZE@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("HEIGHT@SIZE")));
                myDimension.SystemValue = Convert.ToDouble(drawerlist[0]) * 31 / 32 * 25.4 / 1000;
                swDoc.ClearSelection2(true);
                swDoc.SketchManager.InsertSketch(true);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swModel = (ModelDoc2)swApp.ActiveDoc;

                swModelDocExt = (ModelDocExtension)swModel.Extension;



                bool bRebuild;
                int ret;
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelDocExt = (ModelDocExtension)swModel.Extension;

                ret = swModelDocExt.NeedsRebuild2;
                Debug.Print("Features need to be rebuilt (1 = needs rebuild)? " + ret);

                if (ret == 1)
                {
                    bRebuild = swModelDocExt.ForceRebuildAll();
                    Debug.Print("    All features rebuilt in all configurations without activating each configuration? " + bRebuild);
                }

                //select drawer head

                if (cstyle == "wh")
                {
                    boolstatus = swDoc.Extension.SelectByID2("H1", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    boolstatus = swDoc.Extension.SelectByID2("H2", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    boolstatus = swDoc.Extension.SelectByID2("H2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress2();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("HNM", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);
                }
                swApp.ActivateDoc2("DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", false, ref intstatus);
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swApp.CloseDoc(drawerlist[12] + ".SLDPRT");


                // open darwer head
                string drawerhead_type = "";
                if (cstyle == "PN")
                {
                    drawerhead_type = "_IN";
                }
                else if (cstyle == "SN")
                {
                    drawerhead_type = "_IN";
                }
                else if ((cstyle == "SH") || (cstyle == "PH"))
                {
                    drawerhead_type = "_FO";
                }
                else
                {
                    drawerhead_type = "_FO_HNM";
                }
                if (cstyle == "PH")
                {
                    if (Convert.ToDouble(drawerlist[0]) < 6)
                    {
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\DRH\\3\\" + "DRH" + drawerhead_type + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\DRH\\" + "DRH" + drawerhead_type + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));

                }
                else if (cstyle == "WH")
                {
                    if (Convert.ToDouble(drawerlist[0]) < 6)
                    {
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\DRH\\3\\" + "DRH" + drawerhead_type + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\DRH\\" + "DRH" + drawerhead_type + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));

                }
                else
                {
                    if (Convert.ToDouble(drawerlist[0]) < 6)
                    {
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\DRH\\3\\" + "DRH" + drawerhead_type + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\DRH\\" + "DRH" + drawerhead_type + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));

                }



                swApp.ActivateDoc2("DRH" + drawerhead_type + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swModelDocExt = (ModelDocExtension)swDoc.Extension;
                swPackAndGo = (PackAndGo)swModelDocExt.GetPackAndGo();
                namesCount = swPackAndGo.GetDocumentNamesCount();
                swPackAndGo.IncludeDrawings = true;
                swPackAndGo.IncludeSimulationResults = true;
                swPackAndGo.IncludeToolboxComponents = true;
                boolstatus = swPackAndGo.GetDocumentNames(out fileNames);
                pgFileNames = (object[])fileNames;
                boolstatus = swPackAndGo.GetDocumentSaveToNames(out fileNames, out pgFileStatus);
                pgFileNames = (object[])fileNames;
                myPath = libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerhead";
                boolstatus = swPackAndGo.SetSaveToName(true, myPath);
                swPackAndGo.FlattenToSingleFolder = false;
                swPackAndGo.AddPrefix = "";
                swPackAndGo.AddSuffix = drawerlist[16];
                statuses = (int[])swModelDocExt.SavePackAndGo(swPackAndGo);
                swApp.CloseDoc(bifmapath + "\\master\\DRAWER\\DRH\\3\\" + "DRH" + drawerhead_type + ".SLDPRT");
                swApp.CloseDoc(bifmapath + "\\master\\DRAWER\\DRH\\" + "DRH" + drawerhead_type + ".SLDPRT");

                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerhead" + "\\" + drawerlist[11] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(drawerlist[11] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //change size
                boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSketch();
                swDoc.ClearSelection2(true);
                boolstatus = swDoc.Extension.SelectByID2("WIDTH@SIZE@" + "DRH" + drawerlist[11] + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                boolstatus = swDoc.Extension.SelectByID2("HEIGHT@SIZE@" + "DRH" + drawerlist[11] + ".SLDPRT", "DIMENSION", -0.13259303986718474, -0.016468606438235153, -0.33084417233571484, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("WIDTH@SIZE")));
                myDimension.SystemValue = Convert.ToDouble(drawerlist[1]) * 25.4 / 1000;
                boolstatus = swDoc.Extension.SelectByID2("HEIGHT@SIZE@" + "DRBU_" + drawerlist[1] + drawerlist[0] + cstyle + ".SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("HEIGHT@SIZE")));
                myDimension.SystemValue = Convert.ToDouble(drawerlist[0]) * 31 / 32 * 25.4 / 1000;
                swDoc.ClearSelection2(true);
                swDoc.SketchManager.InsertSketch(true);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //LOCK
                if (Convert.ToBoolean(drawerlist[2]) == false)
                {
                }
                else
                {
                    boolstatus = swDoc.Extension.SelectByID2("K0", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);
                }
                if (Convert.ToBoolean(drawerlist[3]) == true)
                {
                    boolstatus = swDoc.Extension.SelectByID2("L", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);
                }

                //pull
                if (pullstyle == "P1")
                {
                    if (Convert.ToInt32(drawerlist[1]) >= 30)
                    {
                        boolstatus = swDoc.Extension.SelectByID2("SP2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                    }
                    else
                    {
                        boolstatus = swDoc.Extension.SelectByID2("SP1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                    }

                }
                if (pullstyle == "P2")
                {
                    if (Convert.ToInt32(drawerlist[1]) >= 30)
                    {
                        boolstatus = swDoc.Extension.SelectByID2("AP2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                    }
                    else
                    {
                        boolstatus = swDoc.Extension.SelectByID2("AP1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                    }

                }
                if (pullstyle == "P3")
                {
                    if (Convert.ToInt32(drawerlist[1]) >= 30)
                    {
                        boolstatus = swDoc.Extension.SelectByID2("WIRE2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("WIRE 2", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditSketch();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("D1@WIRE 2@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                        myDimension = null;
                        myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE 2")));
                        myDimension.SystemValue = 0.096;
                        swDoc.ClearSelection2(true);
                        swDoc.SketchManager.InsertSketch(true);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));

                    }
                    else
                    {
                        boolstatus = swDoc.Extension.SelectByID2("WIRE1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("WIRE 1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditSketch();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("D1@WIRE 1@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                        myDimension = null;
                        myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE 1")));
                        myDimension.SystemValue = 0.096;
                        swDoc.ClearSelection2(true);
                        swDoc.SketchManager.InsertSketch(true);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    }
                }
                if (pullstyle == "P4")
                {
                    if (Convert.ToInt32(drawerlist[1]) >= 30)
                    {
                        boolstatus = swDoc.Extension.SelectByID2("WIRE2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("WIRE 2", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditSketch();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("D1@WIRE 2@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                        myDimension = null;
                        myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE 2")));
                        myDimension.SystemValue = 0.1016;
                        swDoc.ClearSelection2(true);
                        swDoc.SketchManager.InsertSketch(true);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));

                    }
                    else
                    {
                        boolstatus = swDoc.Extension.SelectByID2("WIRE1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("WIRE 1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditSketch();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("D1@WIRE 1@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                        myDimension = null;
                        myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE 1")));
                        myDimension.SystemValue = 0.1016;
                        swDoc.ClearSelection2(true);
                        swDoc.SketchManager.InsertSketch(true);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    }


                }
                if (pullstyle == "P5")
                {
                    if (Convert.ToInt32(drawerlist[1]) >= 30)
                    {
                        boolstatus = swDoc.Extension.SelectByID2("WIRE2", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("WIRE 2", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditSketch();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("D1@WIRE 2@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                        myDimension = null;
                        myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE 2")));
                        myDimension.SystemValue = 0.128;
                        swDoc.ClearSelection2(true);
                        swDoc.SketchManager.InsertSketch(true);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));

                    }
                    else
                    {
                        boolstatus = swDoc.Extension.SelectByID2("WIRE1", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditUnsuppress2();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("WIRE 1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditSketch();
                        swDoc.ClearSelection2(true);
                        boolstatus = swDoc.Extension.SelectByID2("D1@WIRE 1@" + "DRBU_" + drawerlist[0] + drawerlist[1] + cstyle + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                        myDimension = null;
                        myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE 1")));
                        myDimension.SystemValue = 0.128;
                        swDoc.ClearSelection2(true);
                        swDoc.SketchManager.InsertSketch(true);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    }

                }


                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelDocExt = (ModelDocExtension)swModel.Extension;

                ret = swModelDocExt.NeedsRebuild2;
                Debug.Print("Features need to be rebuilt (1 = needs rebuild)? " + ret);

                if (ret == 1)
                {
                    bRebuild = swModelDocExt.ForceRebuildAll();
                    Debug.Print("    All features rebuilt in all configurations without activating each configuration? " + bRebuild);
                }


                swModel = (ModelDoc2)swApp.ActiveDoc;
                //save and close
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swApp.CloseDoc(drawerlist[11] + ".SLDPRT");

                //open assembly
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\" + "Assem1.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swAssembly = ((AssemblyDoc)(swDoc));
                swApp.ActivateDoc2("Assem1.SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));

                //save as
                longstatus = swDoc.SaveAs3(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM", 0, 2);
                swPart = null;
                swDoc = null;
                //close track profile
                swApp.CloseDoc("Assem1.SLDASM");
                //OPEN ASSEM
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swAssembly = ((AssemblyDoc)(swDoc));
                swApp.ActivateDoc2(drawerlist[10] + ".SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //OPEN AND INSERT PART
                //drawer body
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "drawer\\" + drawerlist[10] + "\\" + "drawerbody" + "\\" + drawerlist[12] + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(drawerlist[12] + ".SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(libpath + "drawer\\" + drawerlist[10] + "\\" + "drawerbody" + "\\" + drawerlist[12] + ".SLDASM", 0, 0, 0);
                swApp.ActivateDoc2(libpath + "drawer\\" + drawerlist[10] + "\\" + "drawerbody" + "\\" + drawerlist[12] + ".SLDASM", false, ref longstatus);
                swApp.CloseDoc(libpath + "drawer\\" + drawerlist[10] + "\\" + "drawerbody" + "\\" + drawerlist[12] + ".SLDASM");
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.ActivateDoc2(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Save3(1, swErrors, swWarnings);
                swApp.CloseDoc(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM");
                //drawer head
                swApp.CloseDoc(drawerlist[10] + ".SLDASM");
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "\\drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swAssembly = ((AssemblyDoc)(swDoc));
                swApp.ActivateDoc2(drawerlist[10] + ".SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerhead" + "\\" + drawerlist[11] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(drawerlist[11] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerhead" + "\\" + drawerlist[11] + ".SLDPRT", 0, 0, 0.22232620197);
                swApp.ActivateDoc2(drawerlist[11] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc(libpath + "drawer" + "\\" + drawerlist[10] + "\\drawerhead" + "\\" + drawerlist[11] + ".SLDPRT");
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //AC    

                if (pullstyle == "P1")
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "P2-51.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("P2-51.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    if (Convert.ToDouble(drawerlist[1]) < 30)
                    {
                        boolstatus = swAssembly.AddComponent("P2-51.SLDPRT", 0, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203);
                    }
                    else
                    {
                        boolstatus = swAssembly.AddComponent("P2-51.SLDPRT", -Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203);
                        boolstatus = swAssembly.AddComponent("P2-51.SLDPRT", Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203);
                    }

                    swApp.ActivateDoc2("P2-51.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("P2-51.SLDPRT");
                }
                if (pullstyle == "P2")
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "AEMC-S-22.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("AEMC-S-22.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    if (Convert.ToInt32(drawerlist[1]) < 30)
                    {
                        boolstatus = swAssembly.AddComponent("AEMC-S-22.SLDPRT", 0, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000 + 0.0032131, 0.22383920203 - 0.002148);
                    }
                    else
                    {
                        boolstatus = swAssembly.AddComponent("AEMC-S-22.SLDPRT", -Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000 + 0.0032131, 0.22383920203 - 0.002148);
                        boolstatus = swAssembly.AddComponent("AEMC-S-22.SLDPRT", Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000 + 0.0032131, 0.22383920203 - 0.002148);
                    }

                    swApp.ActivateDoc2("AEMC-S-22.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("AEMC-S-22.SLDPRT");
                }
                if (pullstyle == "P3")
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "RAISED WIRE 96MM.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("RAISED WIRE 96MM.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    if (Convert.ToDouble(drawerlist[1]) < 30)
                    {
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 96MM.SLDPRT", 0, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                    }
                    else
                    {
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 96MM.SLDPRT", -Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 96MM.SLDPRT", Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                    }

                    swApp.ActivateDoc2("RAISED WIRE 96MM.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("RAISED WIRE 96MM.SLDPRT");
                }
                if (pullstyle == "P5")
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "RAISED WIRE 128MM.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("RAISED WIRE 128MM.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    if (Convert.ToDouble(drawerlist[1]) < 30)
                    {
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 128MM.SLDPRT", 0, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                    }
                    else
                    {
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 128MM.SLDPRT", -Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 128MM.SLDPRT", Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                    }

                    swApp.ActivateDoc2("RAISED WIRE 128MM.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("RAISED WIRE 128MM.SLDPRT");
                }
                if (pullstyle == "P4")
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "RAISED WIRE 4IN.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("RAISED WIRE 4IN.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    if (Convert.ToDouble(drawerlist[1]) < 30)
                    {
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 4IN.SLDPRT", 0, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                    }
                    else
                    {
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 4IN.SLDPRT", -Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                        boolstatus = swAssembly.AddComponent("RAISED WIRE 4IN.SLDPRT", Convert.ToDouble(drawerlist[1]) / 4 * 25.4 / 1000, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.37 * 25.4 / 1000, 0.22383920203 + 0.0219185);
                    }

                    swApp.ActivateDoc2("RAISED WIRE 4IN.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("RAISED WIRE 4IN.SLDPRT");
                    swApp.CloseDoc("RAISED WIRE 4IN.SLDPRT");

                }
                if (Convert.ToBoolean(drawerlist[2]) == false)
                {

                }
                else
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "K-17.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("K-17.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("K-17.SLDPRT", 0.0917702, (Convert.ToDouble(drawerlist[0]) - 3) * 31 / 32 / 2 * 25.4 / 1000 + 0.28625 * 25.4 / 1000 + 0.01017187201, 0.21162010203);
                    swApp.ActivateDoc2("K-17.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("K-17.SLDPRT");

                }
                //cardholder
                if (Convert.ToBoolean(drawerlist[3]) == true)
                {
                    double cardhoderposition_X;
                    if (cstyle == "PN")
                    {
                        if (Convert.ToDouble(drawerlist[1]) >= 18)
                        {
                            cardhoderposition_X = 0.16805275 - 0.01914144 + (Convert.ToDouble(drawerlist[1]) - 18) / 2 * 25.4 / 1000;
                        }
                        else
                        {
                            cardhoderposition_X = -0.16805275 + 0.01914144 + (18 - Convert.ToDouble(drawerlist[1])) / 2 * 25.4 / 1000;
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(drawerlist[1]) >= 18)
                        {
                            cardhoderposition_X = 0.16805275 + (Convert.ToDouble(drawerlist[1]) - 18) / 2 * 25.4 / 1000;
                        }
                        else
                        {
                            cardhoderposition_X = -0.16805275 + (18 - Convert.ToDouble(drawerlist[1])) / 2 * 25.4 / 1000;
                        }
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "LH3.5X1.5NP.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("LH3.5X1.5NP.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("LH3.5X1.5NP.SLDPRT", cardhoderposition_X, Convert.ToDouble(drawerlist[0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.25 * 25.4 / 1000, 0.23183850203);
                    swApp.ActivateDoc2("LH3.5X1.5NP.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("LH3.5X1.5NP.SLDPRT");
                }
                //counter wight

                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\COUNTER WEIGHT\\" + "counter weight.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2("counter weight.SLDPRT", false, ref longstatus);
                boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSketch();
                swDoc.ClearSelection2(true);
                boolstatus = swDoc.Extension.SelectByID2("HEIGHT@SIZE@" + "counter weight.SLDPRT", "DIMENSION", -0.13259303986718474, -0.016468606438235153, -0.33084417233571484, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("WIDTH@SIZE")));
                myDimension.SystemValue = Convert.ToDouble(drawerlist[1]) * 25.4 / 1000;
                boolstatus = swDoc.Extension.SelectByID2("HEIGHT@SIZE@" + "counter weight.SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("HEIGHT@SIZE")));
                if (Convert.ToDouble(drawerlist[0]) > 6)
                {
                    myDimension.SystemValue = 6 * 25.4 / 1000 + 0.01895475468;
                }
                else
                {
                    myDimension.SystemValue = Convert.ToDouble(drawerlist[0]) * 25.4 / 1000;
                }
                swDoc.ClearSelection2(true);
                swDoc.SketchManager.InsertSketch(true);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelDocExt = (ModelDocExtension)swModel.Extension;
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelDocExt = (ModelDocExtension)swModel.Extension;

                ret = swModelDocExt.NeedsRebuild2;
                Debug.Print("Features need to be rebuilt (1 = needs rebuild)? " + ret);

                if (ret == 1)
                {
                    bRebuild = swModelDocExt.ForceRebuildAll();
                    Debug.Print("    All features rebuilt in all configurations without activating each configuration? " + bRebuild);
                }
                longstatus = swDoc.SaveAs3(libpath + "drawer\\" + drawerlist[10] + "\\" + "counter weight.SLDPRT", 0, 2);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc("counter weight.SLDPRT");
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "drawer\\" + drawerlist[10] + "\\" + "counter weight.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent("counter weight.SLDPRT", 0, 0.01685925011, 0);
                swApp.ActivateDoc2("counter weight.SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc("counter weight.SLDPRT");





                //save and close
                swApp.ActivateDoc2(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Save3(1, swErrors, swWarnings);
                //swApp.CloseDoc(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM");

            }
            //make weight correct

            if (Convert.ToInt32(drawerlist[18]) == 0)
            {
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Extension.SelectByID2("counter weight-1@" + drawerlist[10], "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress2();
                swDoc.ClearSelection2(true);
            }
            else
            {
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Extension.SelectByID2("counter weight-1@" + drawerlist[10], "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2();
                swDoc.ClearSelection2(true);
            }

            if (Convert.ToInt32(drawerlist[18]) == 2)
            {
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Extension.SelectByID2("counter weight-1@" + drawerlist[10], "COMPONENT", 0, 0, 0, false, 0, null, 0);
                // 
                // Component Configuration (Flyout Menu)
                //object swComp = null;
                swComp = swDoc.ISelectionManager.GetSelectedObjectsComponent4(1, -1);
                swComp.ReferencedConfiguration = "2";
                boolstatus = swDoc.EditRebuild3();
                boolstatus = swDoc.ShowConfiguration2("2");

            }
            if (Convert.ToInt32(drawerlist[18]) == 1)
            {
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Extension.SelectByID2("counter weight-1@" + drawerlist[10], "COMPONENT", 0, 0, 0, false, 0, null, 0);
                // 
                // Component Configuration (Flyout Menu)
                //object swComp = null;
                swComp = swDoc.ISelectionManager.GetSelectedObjectsComponent4(1, -1);
                swComp.ReferencedConfiguration = "1";
                boolstatus = swDoc.EditRebuild3();
                boolstatus = swDoc.ShowConfiguration2("1");

            }
            //get mass info

            ModelDocExtension swModelExt = default(ModelDocExtension);

            SelectionMgr swSelMgr = default(SelectionMgr);
            swComp = default(Component2);

            int nStatus = 0;
            double[] vMassProp = null;
            int i = 0;
            int nbrSelections = 0;

            swModel = (ModelDoc2)swApp.ActiveDoc;
            swModelExt = swModel.Extension;
            swSelMgr = (SelectionMgr)swModel.SelectionManager;
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Extension.SelectByID2(drawerlist[10], "COMPONENT", 0, 0, 0, false, 0, null, 0);

            ModelDocExtension Extn;
            ModelDoc2 swModelDoc;
            MassProperty MyMassProp;
            double[] pmoi;
            double[] vValue;
            object com;
            double[] value = new double[3];
            int errors;
            int warnings;
            double val;

            Extn = swModelExt;

            // Create mass properties
            MyMassProp = Extn.CreateMassProperty();

            // Use document property units (MKS)
            MyMassProp.UseSystemUnits = false;

            val = MyMassProp.Mass;
            double[] X = MyMassProp.CenterOfMass;
            double temp_z = X[2];
            double temp_y = X[1];

            temp_z *= -1;
            if (Convert.ToInt32(drawerlist[18]) == 1)
            {
                if (Convert.ToBoolean(drawerlist[7]) == true)
                {
                    temp_z += -12.08469283;
                }
                else
                {
                    temp_z += -7.4121;
                }
            }
            else
            {
                temp_z += 6.61;
            }

            temp_y += Convert.ToDouble(h - 1) - Convert.ToDouble(drawerlist[9]) - Convert.ToDouble(drawerlist[0]) / 2;




            string cw_dr_center = temp_z.ToString("0.00") + "," + temp_y.ToString("0.00") + "_";
            string cw_dr_g = MyMassProp.Mass.ToString("0.00") + "_";
            swApp.ActivateDoc2(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM", false, ref longstatus);
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Save3(1, swErrors, swWarnings);
            swApp.CloseDoc(libpath + "drawer\\" + drawerlist[10] + "\\" + drawerlist[10] + ".SLDASM");
            killSW();
            return new List<string>() { cw_dr_center, cw_dr_g };

        }
        public List<string> Mshell(List<List<string>> doorlist, string pullstyle, string cstyle, string metaltop, List<List<int>> hiddenrail, List<List<int>> centerpost, int partition, List<List<int>> securitypanel,
            int h, int w, string myname, int whichcab, List<List<string>> drawerlist_Shell, string shelfList_shell, int shelfnum)
        {

            // metaltop: M/N/MP/NP
            //hiddenrail: y, x, l
            //centerpost: l, y
            //partition: y
            //securitypanel: x,y,l
            ModelDoc2 swDoc;
            ModelDoc2 swModel;
            ModelDocExtension swModelDocExt;
            PackAndGo swPackAndGo;
            bool boolstatus;
            int interrors = 0;
            int intwarnings = 0;
            int intstatus = 0;
            int swErrors = 0;
            int swWarnings = 0;


            int namesCount = 0;
            string myPath = null;
            int[] statuses = null;
            int longstatus = 0;
            int longwarnings = 0;
            bool status = false;
            int i = 0;

            DrawingDoc swDrawing = null;
            AssemblyDoc swAssembly = null;

            PartDoc swPart;
            string sModelName;
            string sPathName;
            object varAlignment;
            double[] dataAlignment = new double[12];
            object varViews;
            string[] dataViews = new string[2];
            int options;
            //DOOR
            object fileNames;
            object pgFileStatus;
            object[] pgFileNames = new object[namesCount];
            string libpath = bifmapath + "\\BIFMA\\" + myname + "\\";
            Dimension myDimension = null;
            double doorwidth = 0;
            double doorheight = 0;
            for (i = 0; i < doorlist.Count; i++)
            {

                //string unitpath = path + "\\" + projectname.value + "\\" + swfloor + "\\" + swroom + "\\" + swtag;


                //pack and go in and out door

                string hinge = "R";
                string doordirection = "";

                if (doorlist[i][4] == "1")
                {
                    doordirection = "L";
                }
                else
                        if (doorlist[i][4] == "2")
                {
                    doordirection = "R";
                }

                //inner door
                activeApp();
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DOOR\\in\\" + doordirection + "\\" + "LC15.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2("LC15.SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swModelDocExt = (ModelDocExtension)swDoc.Extension;
                swPackAndGo = (PackAndGo)swModelDocExt.GetPackAndGo();
                namesCount = swPackAndGo.GetDocumentNamesCount();
                swPackAndGo.IncludeDrawings = true;
                swPackAndGo.IncludeSimulationResults = true;
                swPackAndGo.IncludeToolboxComponents = true;

                boolstatus = swPackAndGo.GetDocumentNames(out fileNames);
                pgFileNames = (object[])fileNames;

                boolstatus = swPackAndGo.GetDocumentSaveToNames(out fileNames, out pgFileStatus);

                myPath = libpath + "door" + "\\" + doorlist[i][8] + "\\inner door";
                boolstatus = swPackAndGo.SetSaveToName(true, myPath);
                swPackAndGo.FlattenToSingleFolder = false;
                swPackAndGo.AddPrefix = "";
                swPackAndGo.AddSuffix = doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge;
                statuses = (int[])swModelDocExt.SavePackAndGo(swPackAndGo);
                swApp.CloseDoc(bifmapath + "\\master\\DOOR\\in\\" + doordirection + "\\" + "LC15.SLDPRT");

                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "door" + "\\" + doorlist[i][8] + "\\inner door" + "\\" + "LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2("LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //change size
                //same in build cabinet change together

                if ((cstyle == "PN") || (cstyle == "SN"))
                {
                    if (doorlist.Count == 2)
                    {
                        doorwidth = (Convert.ToDouble(doorlist[i][1]) + 0.284) * 25.4 / 1000;
                        doorheight = Convert.ToDouble(doorlist[i][0]) * 31 / 32 * 25.4 / 1000;
                    }
                    else
                    {
                        doorwidth = (Convert.ToDouble(doorlist[i][1]) + 0.5) * 25.4 / 1000;
                        doorheight = Convert.ToDouble(doorlist[i][0]) * 31 / 32 * 25.4 / 1000;
                    }

                }
                else
                {
                    doorwidth = (Convert.ToDouble(doorlist[i][1])) * 25.4 / 1000;
                    doorheight = (Convert.ToDouble(doorlist[i][0])) * 31 / 32 * 25.4 / 1000;
                }
                boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSketch();
                swDoc.ClearSelection2(true);
                boolstatus = swDoc.Extension.SelectByID2("WIDTH@SIZE@" + "LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);

                myDimension = ((Dimension)(swDoc.Parameter("WIDTH@SIZE")));
                myDimension.SystemValue = doorwidth;
                boolstatus = swDoc.Extension.SelectByID2("HEIGHT@SIZE@" + "LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("HEIGHT@SIZE")));
                myDimension.SystemValue = doorheight;
                swDoc.ClearSelection2(true);
                swDoc.SketchManager.InsertSketch(true);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                bool bRebuild;
                int ret;
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelDocExt = (ModelDocExtension)swModel.Extension;

                ret = swModelDocExt.NeedsRebuild2;
                Debug.Print("Features need to be rebuilt (1 = needs rebuild)? " + ret);

                if (ret == 1)
                {
                    bRebuild = swModelDocExt.ForceRebuildAll();
                    Debug.Print("    All features rebuilt in all configurations without activating each configuration? " + bRebuild);
                }


                swModel = (ModelDoc2)swApp.ActiveDoc;

                sModelName = swModel.GetPathName();
                sPathName = swModel.GetPathName();
                sPathName = sPathName.Substring(0, sPathName.Length - 6);
                sPathName = sPathName + "dwg";

                swPart = (PartDoc)swModel;

                dataAlignment[0] = 0.0;
                dataAlignment[1] = 0.0;
                dataAlignment[2] = 0.0;
                dataAlignment[3] = 1.0;
                dataAlignment[4] = 0.0;
                dataAlignment[5] = 0.0;
                dataAlignment[6] = 0.0;
                dataAlignment[7] = 1.0;
                dataAlignment[8] = 0.0;
                dataAlignment[9] = 0.0;
                dataAlignment[10] = 0.0;
                dataAlignment[11] = 1.0;

                varAlignment = dataAlignment;

                dataViews[0] = "*Current";
                dataViews[1] = "*Front";

                varViews = dataViews;

                //Export each annotation view to a separate drawing file
                //swPart.ExportToDWG2(sPathName, sModelName, (int)swExportToDWG_e.swExportToDWG_ExportAnnotationViews, false, varAlignment, false, false, 0, varViews);

                //Export sheet metal to a single drawing file
                options = 33;  //include flat-pattern geometry and feature
                swPart.ExportToDWG2(sPathName, sModelName, (int)swExportToDWG_e.swExportToDWG_ExportSheetMetal, true, varAlignment, false, false, options, null);

                //save and close
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swApp.CloseDoc("LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT");

                //outdoor

                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DOOR\\out\\" + doordirection + "\\" + "LC15.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2("LC15.SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swModelDocExt = (ModelDocExtension)swDoc.Extension;
                swPackAndGo = (PackAndGo)swModelDocExt.GetPackAndGo();
                namesCount = swPackAndGo.GetDocumentNamesCount();
                swPackAndGo.IncludeDrawings = true;
                swPackAndGo.IncludeSimulationResults = true;
                swPackAndGo.IncludeToolboxComponents = true;
                boolstatus = swPackAndGo.GetDocumentNames(out fileNames);
                pgFileNames = (object[])fileNames;
                boolstatus = swPackAndGo.GetDocumentSaveToNames(out fileNames, out pgFileStatus);
                pgFileNames = (object[])fileNames;
                myPath = libpath + "door" + "\\" + doorlist[i][8] + "\\out door";
                boolstatus = swPackAndGo.SetSaveToName(true, myPath);
                swPackAndGo.FlattenToSingleFolder = false;
                swPackAndGo.AddPrefix = "";
                swPackAndGo.AddSuffix = doorlist[i][12];
                statuses = (int[])swModelDocExt.SavePackAndGo(swPackAndGo);
                swApp.CloseDoc(bifmapath + "\\master\\DOOR\\out\\" + doordirection + "\\" + "LC15.SLDPRT");

                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "door" + "\\" + doorlist[i][8] + "\\out door" + "\\" + doorlist[i][10] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(doorlist[i][10] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //change size

                boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSketch();
                swDoc.ClearSelection2(true);
                boolstatus = swDoc.Extension.SelectByID2("WIDTH@SIZE@" + doorlist[i][10] + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("WIDTH@SIZE")));
                myDimension.SystemValue = doorwidth;
                boolstatus = swDoc.Extension.SelectByID2("HEIGHT@SIZE@" + doorlist[i][10] + ".SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("HEIGHT@SIZE")));
                myDimension.SystemValue = doorheight;
                swDoc.ClearSelection2(true);
                swDoc.SketchManager.InsertSketch(true);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //LOCK
                if (Convert.ToBoolean(doorlist[i][2]) == false)
                {
                }
                else
                {
                    boolstatus = swDoc.Extension.SelectByID2("K0", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);
                }
                if (Convert.ToBoolean(doorlist[i][3]) == true)
                {
                    boolstatus = swDoc.Extension.SelectByID2("L", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);
                }

                //pull
                if (pullstyle == "P1")
                {

                    boolstatus = swDoc.Extension.SelectByID2("SP", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);

                }
                if (pullstyle == "P2")
                {
                    boolstatus = swDoc.Extension.SelectByID2("AP", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);

                }
                if (pullstyle == "P3")
                {
                    boolstatus = swDoc.Extension.SelectByID2("WIRE", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("WIRE1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSketch();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("D1@WIRE1@" + doorlist[i][10] + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                    myDimension = null;
                    myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE1")));
                    myDimension.SystemValue = 0.096;
                    swDoc.ClearSelection2(true);
                    swDoc.SketchManager.InsertSketch(true);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                }
                if (pullstyle == "P4")
                {
                    boolstatus = swDoc.Extension.SelectByID2("WIRE", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("WIRE1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSketch();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("D1@WIRE1@" + doorlist[i][10] + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                    myDimension = null;
                    myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE1")));
                    myDimension.SystemValue = 0.1016;
                    swDoc.ClearSelection2(true);
                    swDoc.SketchManager.InsertSketch(true);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));

                }
                if (pullstyle == "P5")
                {
                    boolstatus = swDoc.Extension.SelectByID2("WIRE", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditUnsuppress2();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("WIRE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSketch();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("D1@WIRE1@" + doorlist[i][10] + ".SLDPRT", "DIMENSION", 0.1045499975475318, -0.11075180249058744, -0.32608333935022532, false, 0, null, 0);
                    myDimension = null;
                    myDimension = ((Dimension)(swDoc.Parameter("D1@WIRE1")));
                    myDimension.SystemValue = 0.128;
                    swDoc.ClearSelection2(true);
                    swDoc.SketchManager.InsertSketch(true);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                }

                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelDocExt = (ModelDocExtension)swModel.Extension;

                ret = swModelDocExt.NeedsRebuild2;
                Debug.Print("Features need to be rebuilt (1 = needs rebuild)? " + ret);

                if (ret == 1)
                {
                    bRebuild = swModelDocExt.ForceRebuildAll();
                    Debug.Print("    All features rebuilt in all configurations without activating each configuration? " + bRebuild);
                }


                swModel = (ModelDoc2)swApp.ActiveDoc;

                sModelName = swModel.GetPathName();
                sPathName = swModel.GetPathName();
                sPathName = sPathName.Substring(0, sPathName.Length - 6);
                sPathName = sPathName + "dwg";

                swPart = (PartDoc)swModel;

                dataAlignment[0] = 0.0;
                dataAlignment[1] = 0.0;
                dataAlignment[2] = 0.0;
                dataAlignment[3] = 1.0;
                dataAlignment[4] = 0.0;
                dataAlignment[5] = 0.0;
                dataAlignment[6] = 0.0;
                dataAlignment[7] = 1.0;
                dataAlignment[8] = 0.0;
                dataAlignment[9] = 0.0;
                dataAlignment[10] = 0.0;
                dataAlignment[11] = 1.0;

                varAlignment = dataAlignment;

                dataViews[0] = "*Current";
                dataViews[1] = "*Front";

                varViews = dataViews;

                //Export each annotation view to a separate drawing file
                //swPart.ExportToDWG2(sPathName, sModelName, (int)swExportToDWG_e.swExportToDWG_ExportAnnotationViews, false, varAlignment, false, false, 0, varViews);

                //Export sheet metal to a single drawing file
                options = 33;  //include flat-pattern geometry and feature
                swPart.ExportToDWG2(sPathName, sModelName, (int)swExportToDWG_e.swExportToDWG_ExportSheetMetal, true, varAlignment, false, false, options, null);

                //save and close
                swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
                swApp.CloseDoc(doorlist[i][10] + ".SLDPRT");

                //open assembly
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\DRAWER\\" + "Assem1.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swAssembly = ((AssemblyDoc)(swDoc));
                swApp.ActivateDoc2("Assem1.SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //save as
                longstatus = swDoc.SaveAs3(libpath + "\\door\\" + doorlist[i][8] + "\\" + doorlist[i][8] + ".SLDASM", 0, 2);
                swPart = null;
                swDoc = null;
                //close track profile
                swApp.CloseDoc("Assem1.SLDASM");
                //OPEN ASSEM
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "\\door\\" + doorlist[i][8] + "\\" + doorlist[i][8] + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swAssembly = ((AssemblyDoc)(swDoc));
                swApp.ActivateDoc2(doorlist[i][8] + ".SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                //OPEN AND INSERT PART
                //door body in
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "door" + "\\" + doorlist[i][8] + "\\inner door" + "\\" + "LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(doorlist[i][8] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(libpath + "door" + "\\" + doorlist[i][8] + "\\inner door" + "\\" + "LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT", 0, 0, -(0.018 * 25.4 / 1000));
                swApp.ActivateDoc2(libpath + "door" + "\\" + doorlist[i][8] + "\\inner door" + "\\" + "LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc(libpath + "door" + "\\" + doorlist[i][8] + "\\inner door" + "\\" + "LC15" + doorlist[i][0] + doordirection + "I" + "-" + doorlist[i][1] + "Z-" + hinge + ".SLDPRT");
                //out
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "door" + "\\" + doorlist[i][8] + "\\out door" + "\\" + doorlist[i][10] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(doorlist[i][8] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(libpath + "door" + "\\" + doorlist[i][8] + "\\out door" + "\\" + doorlist[i][10] + ".SLDPRT", 0, 0, 0.018 * 25.4 / 1000);
                swApp.ActivateDoc2(libpath + "door" + "\\" + doorlist[i][8] + "\\out door" + "\\" + doorlist[i][10] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc(libpath + "door" + "\\" + doorlist[i][8] + "\\out door" + "\\" + doorlist[i][10] + ".SLDPRT");

                double rollerpositionX = 0;

                if (doorlist[i][4] == "1")
                {
                    rollerpositionX = doorwidth / 2 - 0.0613165;
                }
                else if (doorlist[i][4] == "2")
                {
                    rollerpositionX = 0.0613165 - doorwidth / 2;
                }
                // roller catch and bumper
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\AC\\Roller Catch.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(doorlist[i][8] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(bifmapath + "\\master\\AC\\Roller Catch.SLDPRT", rollerpositionX, Convert.ToDouble(doorlist[i][0]) * 31 / 32 * 25.4 / 1000 / 2 - 0.0254635 + 0.009367, -0.022);
                swApp.ActivateDoc2(bifmapath + "\\master\\AC\\Roller Catch.SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc(bifmapath + "\\master\\AC\\Roller Catch.SLDPRT");
                // CRL#D656(.125X.375)
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\AC\\CRL#D656(.125X.375).SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                swApp.ActivateDoc2(doorlist[i][8] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(bifmapath + "\\master\\AC\\CRL#D656(.125X.375).SLDPRT", rollerpositionX, Convert.ToDouble(doorlist[i][0]) * 31 / 32 * 25.4 / 1000 / 2 - 0.0192 + 0.002657475 + 0.009367, -0.02 + 0.00108687 + 0.00978183000);
                swApp.ActivateDoc2(bifmapath + "\\master\\AC\\CRL#D656(.125X.375).SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc(bifmapath + "\\master\\AC\\CRL#D656(.125X.375).SLDPRT");


                //AC    
                double handlepositionX = 0;
                double handlepositionY = 0;



                if (pullstyle == "P1")
                {
                    if (doorlist[i][4] == "1")
                    {
                        handlepositionX = doorwidth / 2 - 1.455 * 25.4 / 1000;
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000 - 0.0032385;
                    }
                    else if (doorlist[i][4] == "2")
                    {
                        handlepositionX = -(doorwidth / 2 - 1.455 * 25.4 / 1000);
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000 - 0.0032385;
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "P2-51-H.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("P2-51-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("P2-51-H.SLDPRT", handlepositionX, handlepositionY, 0.0027322);
                    swApp.ActivateDoc2("P2-51-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("P2-51-H.SLDPRT");
                }
                if (pullstyle == "P2")
                {
                    if (doorlist[i][4] == "1")
                    {
                        handlepositionX = doorwidth / 2 - 1.455 * 25.4 / 1000;
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    else if (doorlist[i][4] == "2")
                    {
                        handlepositionX = -(doorwidth / 2 - 1.455 * 25.4 / 1000);
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "AEMC-S-22-H.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("AEMC-S-22-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("AEMC-S-22-H.SLDPRT", handlepositionX, handlepositionY, 0);
                    swApp.ActivateDoc2("AEMC-S-22-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("AEMC-S-22-H.SLDPRT");
                }
                if (pullstyle == "P3")
                {
                    if (doorlist[i][4] == "1")
                    {
                        handlepositionX = doorwidth / 2 - 1.455 * 25.4 / 1000;
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    else if (doorlist[i][4] == "2")
                    {
                        handlepositionX = -(doorwidth / 2 - 1.455 * 25.4 / 1000);
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "RAISED WIRE 96MM-H.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("RAISED WIRE 96MM-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("RAISED WIRE 96MM-H.SLDPRT", handlepositionX, handlepositionY, 0.0246507);
                    swApp.ActivateDoc2("RAISED WIRE 96MM-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("RAISED WIRE 96MM-H.SLDPRT");
                }
                if (pullstyle == "P5")
                {
                    if (doorlist[i][4] == "1")
                    {
                        handlepositionX = doorwidth / 2 - 1.455 * 25.4 / 1000;
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    else if (doorlist[i][4] == "2")
                    {
                        handlepositionX = -(doorwidth / 2 - 1.455 * 25.4 / 1000);
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "RAISED WIRE 128MM-H.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("RAISED WIRE 128MM.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("RAISED WIRE 128MM-H.SLDPRT", handlepositionX, handlepositionY, 0.0246507);
                    swApp.ActivateDoc2("RAISED WIRE 128MM-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("RAISED WIRE 128MM-H.SLDPRT");
                }
                if (pullstyle == "P4")
                {
                    if (doorlist[i][4] == "1")
                    {
                        handlepositionX = doorwidth / 2 - 1.455 * 25.4 / 1000;
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    else if (doorlist[i][4] == "2")
                    {
                        handlepositionX = -(doorwidth / 2 - 1.455 * 25.4 / 1000);
                        handlepositionY = (Convert.ToDouble(doorlist[i][0]) / 2 * 31 / 32 - 5.4) * 25.4 / 1000;
                    }
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "RAISED WIRE 4IN-H.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("RAISED WIRE 4IN-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("RAISED WIRE 4IN-H.SLDPRT", handlepositionX, handlepositionY, 0.0246507);
                    swApp.ActivateDoc2("RAISED WIRE 4IN-H.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("RAISED WIRE 4IN-H.SLDPRT");
                    swApp.CloseDoc("RAISED WIRE 4IN-H.SLDPRT");

                }
                if (Convert.ToBoolean(doorlist[i][2]) == false)
                {
                }
                else
                {
                    double DLPX = 0;
                    double DLPY = 0;
                    double DLPZ = 0;
                    if (doorlist[i][4] == "1")
                    {
                        DLPX = 4.775 * 25.4 / 1000;
                    }
                    else if (doorlist[i][4] == "2")
                    {
                        DLPX = -4.775 * 25.4 / 1000;
                    }

                    DLPY = Convert.ToDouble(doorlist[i][0]) * 31 / 32 * 25.4 / 1000 / 2 - 0.01972070889;

                    DLPZ = -0.0094869;

                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "K-17.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("K-17.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("K-17.SLDPRT", DLPX, DLPY, DLPZ);
                    swApp.ActivateDoc2("K-17.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("K-17.SLDPRT");

                }
                if (Convert.ToBoolean(doorlist[i][3]) == true)
                {
                    double cardhoderposition_X;
                    if (pullstyle == "PN")
                    {
                        cardhoderposition_X = 0.16805275 - 0.01948119749 + (Convert.ToDouble(doorlist[i][1]) - 18) / 2 * 25.4 / 1000;

                    }
                    else
                    {
                        cardhoderposition_X = 0.16805275 + (Convert.ToDouble(doorlist[i][1]) - 18) / 2 * 25.4 / 1000;
                    }
                    if (doorlist[i][4] == "1")
                    {
                        cardhoderposition_X = -cardhoderposition_X;
                    }

                    swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\AC\\" + "LH3.5X1.5NP.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swApp.ActivateDoc2("LH3.5X1.5NP.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("LH3.5X1.5NP.SLDPRT", cardhoderposition_X, Convert.ToDouble(doorlist[i][0]) * 31 / 32 / 2 * 25.4 / 1000 - 1.25 * 25.4 / 1000, 0.0107315);
                    swApp.ActivateDoc2("LH3.5X1.5NP.SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("LH3.5X1.5NP.SLDPRT");
                }

                //insert hinge

                double hingepositionX = 0;
                double hingepositionY = 0;
                double hingepositionZ = 0;

                if ((cstyle == "PN") || (cstyle == "SN"))
                {
                    hingepositionX = doorwidth / 2 - 0.0313486018;
                    hingepositionY = Convert.ToDouble(doorlist[i][0]) * 31 / 32 / 2 * 25.4 / 1000 - 0.001216 - 0.01094422907;
                    hingepositionZ = 0.0038227;
                    if (doorlist[i][4] == "1")
                    {
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\DOOR\\" + "LC1101ZTL.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                        swApp.ActivateDoc2("LC1101ZTL.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        boolstatus = swAssembly.AddComponent("LC1101ZTL.SLDASM", -hingepositionX, hingepositionY, hingepositionZ);
                        swApp.ActivateDoc2("LC1101ZTL.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        swApp.CloseDoc("LC1101ZTL.SLDASM");
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\DOOR\\" + "LC1101ZBL.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                        swApp.ActivateDoc2("LC1101ZBL.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        boolstatus = swAssembly.AddComponent("LC1101ZBL.SLDASM", -hingepositionX, -hingepositionY, hingepositionZ);
                        swApp.ActivateDoc2("LC1101ZBL.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        swApp.CloseDoc("LC1101ZBL.SLDASM");
                    }
                    else if (doorlist[i][4] == "2")
                    {
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\DOOR\\" + "LC1101ZTR.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                        swApp.ActivateDoc2("LC1101ZTR.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        boolstatus = swAssembly.AddComponent("LC1101ZTR.SLDASM", hingepositionX, hingepositionY, hingepositionZ);
                        swApp.ActivateDoc2("LC1101ZTR.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        swApp.CloseDoc("LC1101ZTR.SLDASM");
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\DOOR\\" + "LC1101ZBR.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                        swApp.ActivateDoc2("LC1101ZBR.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        boolstatus = swAssembly.AddComponent("LC1101ZBR.SLDASM", hingepositionX, -hingepositionY, hingepositionZ);
                        swApp.ActivateDoc2("LC1101ZBR.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        swApp.CloseDoc("LC1101ZBR.SLDASM");
                    }
                }
                else if (cstyle == "PH")
                {
                    hingepositionX = doorwidth / 2 - 0.0338124018;
                    hingepositionY = Convert.ToDouble(doorlist[i][0]) * 31 / 32 / 2 * 25.4 / 1000 - 0.01212847907;
                    hingepositionZ = 0;
                    if (doorlist[i][4] == "1")
                    {
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\DOOR\\" + "LC1101HTL.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                        swApp.ActivateDoc2("LC1101HTL.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        boolstatus = swAssembly.AddComponent("LC1101HTL.SLDASM", -hingepositionX, hingepositionY, hingepositionZ);
                        swApp.ActivateDoc2("LC1101HTL.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        swApp.CloseDoc("LC1101HTL.SLDASM");
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\DOOR\\" + "LC1101HBL.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                        swApp.ActivateDoc2("LC1101HBL.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        boolstatus = swAssembly.AddComponent("LC1101HBL.SLDASM", -hingepositionX, -hingepositionY, hingepositionZ);
                        swApp.ActivateDoc2("LC1101HBL.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        swApp.CloseDoc("LC1101HBL.SLDASM");
                    }
                    else if (doorlist[i][4] == "2")
                    {
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\DOOR\\" + "LC1101HTR.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                        swApp.ActivateDoc2("LC1101HTR.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        boolstatus = swAssembly.AddComponent("LC1101HTR.SLDASM", hingepositionX, hingepositionY, hingepositionZ);
                        swApp.ActivateDoc2("LC1101HTR.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        swApp.CloseDoc("LC1101HTR.SLDASM");
                        swDoc = ((ModelDoc2)(swApp.OpenDoc6("C:\\Users\\ruoxu\\Desktop\\cabinet\\master\\DOOR\\" + "LC1101HBR.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                        swApp.ActivateDoc2("LC1101HBR.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        boolstatus = swAssembly.AddComponent("LC1101HBR.SLDASM", hingepositionX, -hingepositionY, hingepositionZ);
                        swApp.ActivateDoc2("LC1101HBR.SLDASM", false, ref longstatus);
                        swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                        swApp.CloseDoc("LC1101HBR.SLDASM");
                    }
                }



                //save and close
                swApp.ActivateDoc2(libpath + "door\\" + doorlist[i][8] + "\\" + doorlist[i][8] + ".SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Save3(1, swErrors, swWarnings);
                swApp.CloseDoc(libpath + "door\\" + doorlist[i][8] + "\\" + doorlist[i][8] + ".SLDASM");
            }


            /////////////////////////////////////////////////////////////////////
            activeApp();
            swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\SHELL\\" + "M_" + cstyle + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
            swAssembly = ((AssemblyDoc)(swDoc));
            swApp.ActivateDoc2("M_" + cstyle + ".SLDASM", false, ref longstatus);

            // pack and go to project libray

            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            swDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, ref interrors, ref intwarnings);
            swModelDocExt = (ModelDocExtension)swDoc.Extension;
            swPackAndGo = (PackAndGo)swModelDocExt.GetPackAndGo();
            namesCount = swPackAndGo.GetDocumentNamesCount();
            swPackAndGo.IncludeDrawings = true;
            swPackAndGo.IncludeSimulationResults = true;
            swPackAndGo.IncludeToolboxComponents = true;
            boolstatus = swPackAndGo.GetDocumentNames(out fileNames);
            pgFileNames = (object[])fileNames;
            boolstatus = swPackAndGo.GetDocumentSaveToNames(out fileNames, out pgFileStatus);
            pgFileNames = (object[])fileNames;
            myPath = bifmapath + "\\BIFMA\\" + myname + "\\shell";
            boolstatus = swPackAndGo.SetSaveToName(true, myPath);
            swPackAndGo.FlattenToSingleFolder = true;
            swPackAndGo.AddPrefix = "";
            swPackAndGo.AddSuffix = "";
            object getFileNames;
            object getDocumentStatus;
            string[] pgGetFileNames = new string[namesCount - 1];

            status = swPackAndGo.GetDocumentSaveToNames(out getFileNames, out getDocumentStatus);
            pgGetFileNames = (string[])getFileNames;
            Debug.Print("");
            Debug.Print("  My Pack and Go path and filenames after adding prefix and suffix: ");
            for (int j = 0; j <= namesCount - 1; j++)
            {
                Debug.Print("    My path and filename is: " + pgGetFileNames[j]);
            }
            statuses = (int[])swModelDocExt.SavePackAndGo(swPackAndGo);
            swApp.CloseDoc(bifmapath + "\\master\\SHELL\\" + "M_" + cstyle + ".SLDASM");

            //open shell in unit

            swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\BIFMA\\" + myname + "\\Shell\\" + "M_" + cstyle + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
            swAssembly = ((AssemblyDoc)(swDoc));
            swApp.ActivateDoc2("M_" + cstyle + ".SLDASM", false, ref longstatus);
            //edit shell size

            boolstatus = swDoc.Extension.SelectByID2("WIDTH@TOTAL SIZE@" + "M_" + cstyle, "DIMENSION", 0.77034484898079558, 1.5921172786793578, 1.07597489717334, false, 0, null, 0);
            myDimension = ((Dimension)(swDoc.Parameter("WIDTH@TOTAL SIZE")));
            myDimension.SystemValue = w * 25.4 / 1000;
            boolstatus = swDoc.Extension.SelectByID2("HEIGHT@TOTAL SIZE@" + "M_" + cstyle + ".SLDASM", "DIMENSION", 0.35098855312920829, 1.2720748544873577, 1.3738489226698953, false, 0, null, 0);
            myDimension = ((Dimension)(swDoc.Parameter("HEIGHT@TOTAL SIZE")));
            myDimension.SystemValue = h * 25.4 / 1000;
            swDoc.ClearSelection2(true);
            swDoc.SketchManager.InsertSketch(true);
            boolstatus = swDoc.EditRebuild3();
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Save3(1, ref interrors, ref intwarnings);
            //swApp.CloseDoc(unitpath + "\\" + swshellname + "_" + sww + "_" + swh + ".SLDASM");

            //EDIT BACK CHANNEL

            if (w >= 30)
            {
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Extension.SelectByID2("M-LC1172-1@M_PH", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress();
                swDoc.ClearSelection2(true);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Save3(1, ref interrors, ref intwarnings);
            }
            //edit top

            if ((metaltop == "M") || (metaltop == "MP"))
            {
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Extension.SelectByID2("M-LC1230-1@M_PH", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress();
                swDoc.ClearSelection2(true);
            }
            else
            {
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swDoc.Extension.SelectByID2("Phenolic-1@M_PH", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress();
                swDoc.ClearSelection2(true);
            }
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Save3(1, ref interrors, ref intwarnings);
            // ADD TRACK
            for (int j = 0; j < drawerlist_Shell.Count; j++)
            {
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\drawer\\drawertrack\\" + drawerlist_Shell[j][14] + "-L.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(drawerlist_Shell[j][14] + "-L.SLDASM", w / 2 * 25.4 / 1000, h / 2 * 25.4 / 1000, 0.27507142299);
                swApp.ActivateDoc2(drawerlist_Shell[j][14] + "-L.SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc(drawerlist_Shell[j][14] + "-L.SLDASM");
            }
            for (int j = 0; j < shelfnum; j++)
            {
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\master\\drawer\\drawertrack\\" + shelfList_shell + "-L.SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(shelfList_shell + "-L.SLDASM", w / 2 * 25.4 / 1000, h / 2 * 25.4 / 1000, 0.27507142299);
                swApp.ActivateDoc2(shelfList_shell + "-L.SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc(shelfList_shell + "-L.SLDASM");
            }
            // ADD DOOR 

            for (int k = 0; k < doorlist.Count; k++)
            {
                double DoorPX = 0;
                double DoorPY = 0;
                double DoorPZ = 0;
                double doordistance = 0.01729470180;

                //////////////////////////////////////////////////////////////////////////////////
                //same in door change together
                if ((cstyle == "PN") || (cstyle == "SN"))
                {
                    doorwidth = (Convert.ToDouble(doorlist[k][1]) + 0.284) * 25.4 / 1000;
                    doorheight = Convert.ToDouble(doorlist[k][0]) * 31 / 32 * 25.4 / 1000;
                }
                else
                {
                    doorwidth = (Convert.ToDouble(doorlist[k][1])) * 25.4 / 1000;
                    doorheight = (Convert.ToDouble(doorlist[k][0])) * 31 / 32 * 25.4 / 1000;
                }
                ///////////////////////////////////////////////////////////////////////////////////////



                if ((pullstyle == "P3") || (pullstyle == "P4") || (pullstyle == "P5"))
                {
                    DoorPZ = 0.2474 + 0.30380730403;
                }
                else
                {
                    DoorPZ = 0.2474 + 0.30380730403 - 0.00951850437;
                }

                DoorPY = Convert.ToDouble(h) * 25.4 / 1000 / 2 - Convert.ToDouble(doorlist[k][7]) * 31 / 32 * 25.4 / 1000 / 2; //- 0.0197 - (Convert.ToDouble(doorlist[k][0]) * 25.4 / 1000) / 2 - Convert.ToDouble(doorlist[k][7]) * 31 / 32 * 25.4 / 1000;
                if (doorlist[k][6] == "0")
                {
                    if (doorlist.Count == 2)
                    {
                        DoorPX = Convert.ToDouble(doorlist[k][6]) * 25.4 / 1000 - doorwidth / 2 + Convert.ToDouble(w) * 25.4 / 1000 / 2;
                    }
                    else
                    {
                        if ((cstyle == "PN") || (cstyle == "SN"))
                        {
                            DoorPX = Convert.ToDouble(doorlist[k][6]) * 25.4 / 1000 - doorwidth / 2 - 0.02322329817;
                        }
                        else
                        {
                            DoorPX = Convert.ToDouble(doorlist[k][6]) * 25.4 / 1000 - doorwidth / 2 + Convert.ToDouble(w) * 25.4 / 1000 / 2;
                        }

                    }

                }
                else
                {
                    if ((cstyle == "PN") || (cstyle == "SN"))
                    {
                        DoorPX = Convert.ToDouble(doorlist[k][6]) * 25.4 / 1000 - doorwidth / 2 - (doordistance - 0.125 * 2.54 / 1000);
                    }
                    else
                    {
                        DoorPX = Convert.ToDouble(doorlist[k][6]) * 25.4 / 1000 - doorwidth / 2 + Convert.ToDouble(w) * 25.4 / 1000 / 2;
                    }

                }
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "door\\" + doorlist[k][8] + "\\" + doorlist[k][8] + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent(doorlist[k][8] + ".SLDASM", DoorPX, DoorPY, DoorPZ);
                swApp.ActivateDoc2(doorlist[k][8] + ".SLDASM", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc(doorlist[k][8] + ".SLDASM");
            }
            ///////////////////////////////////////////////////////////////
            //hidden rail
            for (int j = 0; j < hiddenrail.Count; j++)
            {
                double hiddenX;
                if (hiddenrail[j][1] == 0)
                {
                    hiddenX = -Convert.ToDouble(hiddenrail[j][2]) * 25.4 / 1000 * 0.75 + w / 2 * 25.4 / 1000;// + 0.7795 * 25.4 / 1000
                }
                else
                {
                    hiddenX = Convert.ToDouble(hiddenrail[j][2]) * 25.4 / 1000 * 0.25 + w / 2 * 25.4 / 1000;
                }
                if (File.Exists(libpath + "SHELL\\" + "LC1280-" + hiddenrail[j][2] + ".SLDPRT"))
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "SHELL\\" + "LC1280-" + hiddenrail[j][2] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("LC1280-" + hiddenrail[j][2] + ".SLDPRT", hiddenX, (h - Convert.ToDouble(hiddenrail[j][0])) * 25.4 / 1000 - 0.013, 0.52260299998);
                    swApp.ActivateDoc2("LC1280-" + hiddenrail[j][0] + ".SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("LC1280-" + hiddenrail[j][2] + ".SLDPRT");
                }
                else
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\MASTER\\configerator part\\" + "LC1280.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    longstatus = swDoc.SaveAs3(libpath + "SHELL\\" + "LC1280-" + hiddenrail[j][2] + ".SLDPRT", 0, 1);
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "SHELL\\" + "LC1280-" + hiddenrail[j][2] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSketch();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("Width@SIZE@" + "LC1280.SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("Width@SIZE")));
                    myDimension.SystemValue = Convert.ToDouble(hiddenrail[j][2]) * 25.4 / 1000;
                    swDoc.ClearSelection2(true);
                    swDoc.SketchManager.InsertSketch(true);
                    boolstatus = swDoc.Save3(1, swErrors, swWarnings);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("LC1280-" + hiddenrail[j][2] + ".SLDPRT", hiddenX, (h - Convert.ToDouble(hiddenrail[j][0])) * 25.4 / 1000 - 0.013, 0.52260299998);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swModel = (ModelDoc2)swApp.ActiveDoc;
                    swApp.ActivateDoc2("LC1280-" + hiddenrail[j][2] + ".SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("LC1280-" + hiddenrail[j][2] + ".SLDPRT");
                }
            }

            //CENTERPOST
            for (int j = 0; j < centerpost.Count; j++)
            {
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\MASTER\\configerator part\\" + "H-LC13.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                longstatus = swDoc.SaveAs3(libpath + "SHELL\\" + "H-LC13-" + centerpost[j][0] + ".SLDPRT", 0, 1);
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "SHELL\\" + "H-LC13-" + centerpost[j][0] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSketch();
                swDoc.ClearSelection2(true);
                boolstatus = swDoc.Extension.SelectByID2("Height@SIZE@" + "H-LC13-" + centerpost[j][0] + ".SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("Height@SIZE")));
                myDimension.SystemValue = Convert.ToDouble(centerpost[j][0]) * 25.4 / 1000;
                swDoc.ClearSelection2(true);
                swDoc.SketchManager.InsertSketch(true);
                boolstatus = swDoc.Save3(1, swErrors, swWarnings);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent("H-LC13-" + centerpost[j][0] + ".SLDPRT", w / 2 * 25.4 / 1000, (h - Convert.ToDouble(centerpost[j][1]) - Convert.ToDouble(centerpost[j][0]) / 2) * 25.4 / 1000, 0.52260299998 + 0.00562378475);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swApp.ActivateDoc2("H-LC13-" + centerpost[j][0] + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc("H-LC13-" + centerpost[j][0] + ".SLDPRT");
            }
            //patition
            if (partition == -1)
            {
            }
            else
            {
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\MASTER\\configerator part\\" + "LC1400.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                longstatus = swDoc.SaveAs3(libpath + "SHELL\\" + "LC1400-" + partition + ".SLDPRT", 0, 1);
                swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "SHELL\\" + "LC1400-" + partition + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSketch();
                swDoc.ClearSelection2(true);
                boolstatus = swDoc.Extension.SelectByID2("Height@SIZE@" + "LC1400-" + partition + ".SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                myDimension = ((Dimension)(swDoc.Parameter("Height@SIZE")));
                myDimension.SystemValue = (h - partition) * 25.4 / 1000;
                swDoc.ClearSelection2(true);
                swDoc.SketchManager.InsertSketch(true);
                boolstatus = swDoc.Save3(1, swErrors, swWarnings);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                boolstatus = swAssembly.AddComponent("LC1400-" + partition + ".SLDPRT", w / 2 * 25.4 / 1000, 0.03105756966 + (h - partition) * 25.4 / 1000 / 2, 0.52260299998 - 0.23661169995);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swApp.ActivateDoc2("LC1400-" + partition + ".SLDPRT", false, ref longstatus);
                swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                swApp.CloseDoc("LC1400-" + partition + ".SLDPRT");
            }
            //security panel

            for (int j = 0; j < securitypanel.Count; j++)
            {
                double hiddenX;
                if (securitypanel[j][0] == 0)
                {
                    hiddenX = -Convert.ToDouble(securitypanel[j][2]) * 25.4 / 1000 * 0.5 + w / 2 * 25.4 / 1000;// + 0.7795 * 25.4 / 1000
                }
                else
                {
                    hiddenX = w / 2 * 25.4 / 1000;
                }
                if (File.Exists(libpath + "SHELL\\" + "FO-LC1460-" + securitypanel[j][2] + ".SLDPRT"))
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "SHELL\\" + "FO-LC1460-" + securitypanel[j][2] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("FO-LC1460-" + securitypanel[j][2] + ".SLDPRT", hiddenX, (h - Convert.ToDouble(securitypanel[j][1])) * 25.4 / 1000 - 0.013, 0.52260299998 - 0.27129637136);
                    swApp.ActivateDoc2("FO-LC1460-" + securitypanel[j][1] + ".SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("FO-LC1460-" + securitypanel[j][2] + ".SLDPRT");
                }
                else
                {
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(bifmapath + "\\MASTER\\configerator part\\" + "FO-LC1460.SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    longstatus = swDoc.SaveAs3(libpath + "SHELL\\" + "FO-LC1460-" + securitypanel[j][2] + ".SLDPRT", 0, 1);
                    swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "SHELL\\" + "FO-LC1460-" + securitypanel[j][2] + ".SLDPRT", 1, 0, "", ref longstatus, ref longwarnings)));
                    boolstatus = swDoc.Extension.SelectByID2("SIZE", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSketch();
                    swDoc.ClearSelection2(true);
                    boolstatus = swDoc.Extension.SelectByID2("Width@SIZE@" + "FO-LC1460.SLDPRT", "DIMENSION", -0.13157904031785439, -0.015785117377604879, -0.33106607200280841, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("Width@SIZE")));
                    myDimension.SystemValue = Convert.ToDouble(securitypanel[j][2]) * 25.4 / 1000;
                    swDoc.ClearSelection2(true);
                    swDoc.SketchManager.InsertSketch(true);
                    boolstatus = swDoc.Save3(1, swErrors, swWarnings);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    boolstatus = swAssembly.AddComponent("FO-LC1460-" + securitypanel[j][2] + ".SLDPRT", hiddenX, (h - Convert.ToDouble(securitypanel[j][1])) * 25.4 / 1000 - 0.013, 0.52260299998 - 0.27129637136);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swModel = (ModelDoc2)swApp.ActiveDoc;
                    swApp.ActivateDoc2("FO-LC1460-" + securitypanel[j][2] + ".SLDPRT", false, ref longstatus);
                    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
                    swApp.CloseDoc("FO-LC1460-" + securitypanel[j][2] + ".SLDPRT");
                }
            }

            //get mass info

            ModelDocExtension swModelExt = default(ModelDocExtension);

            SelectionMgr swSelMgr = default(SelectionMgr);
            Component2 swComp = default(Component2);

            int nStatus = 0;
            double[] vMassProp = null;
            int nbrSelections = 0;

            swModel = (ModelDoc2)swApp.ActiveDoc;
            swModelExt = swModel.Extension;
            swSelMgr = (SelectionMgr)swModel.SelectionManager;
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Extension.SelectByID2("m_" + cstyle, "COMPONENT", 0, 0, 0, false, 0, null, 0);

            ModelDocExtension Extn;
            ModelDoc2 swModelDoc;
            MassProperty MyMassProp;
            double[] pmoi;
            double[] vValue;
            object com;
            double[] value = new double[3];
            int errors;
            int warnings;
            double val;

            Extn = swModelExt;

            // Create mass properties
            MyMassProp = Extn.CreateMassProperty();

            // Use document property units (MKS)
            MyMassProp.UseSystemUnits = false;
            val = MyMassProp.Mass;
            double[] X = MyMassProp.CenterOfMass;

            double temp_z = X[2];
            double temp_y = X[1];
            temp_z *= -1;
            temp_z += 19.45227666;



            string cw_sh_center = temp_z.ToString("0.00") + "," + temp_y.ToString("0.00") + "_";
            string cw_sh_g = MyMassProp.Mass.ToString("0.00") + "_";



            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Save3(1, swErrors, swWarnings);
            swApp.CloseDoc("m_" + cstyle + ".SLDASM");
            killSW();
            return new List<string>() { cw_sh_center, cw_sh_g };
            //swApp.ExitApp();

        }
        public List<string> Mshelf(int w, int index)
        {
            List<string> output = new List<string>();
            activeApp();
            //drawerbody

            int namesCount = 0;
            string myPath = null;
            int[] statuses = null;
            int longstatus = 0;
            int longwarnings = 0;

            ModelDoc2 swModel;
            ModelDocExtension swModelDocExt;
            PackAndGo swPackAndGo;
            bool boolstatus;
            int interrors = 0;
            int intwarnings = 0;
            int intstatus = 0;
            int swErrors = 0;
            int swWarnings = 0;
            PartDoc swPart;
            string sModelName;
            string sPathName;
            object varAlignment;
            double[] dataAlignment = new double[12];
            object varViews;
            string[] dataViews = new string[2];
            int options;
            Dimension myDimension = null;
            ModelDoc2 swDoc;
            string libpath = bifmapath + "\\BIFMA\\";
            Component2 swComp = default(Component2);
            swDoc = ((ModelDoc2)(swApp.OpenDoc6(libpath + "shelf\\" + "BIFMA_PS" + ".SLDASM", 2, 0, "", ref longstatus, ref longwarnings)));
            swApp.ActivateDoc2("BIFMA_PS" + ".SLDASM", false, ref longstatus);
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            //change size

            boolstatus = swDoc.Extension.SelectByID2("width@width@BIFMA_PS.SLDASM", "DIMENSION", 0.35098855312920829, 1.2720748544873577, 1.3738489226698953, false, 0, null, 0);
            myDimension = ((Dimension)(swDoc.Parameter("width@width")));
            myDimension.SystemValue = w * 25.4 / 1000;
            swDoc.ClearSelection2(true);
            swDoc.SketchManager.InsertSketch(true);
            boolstatus = swDoc.EditRebuild3();
            //choose wight
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Extension.SelectByID2("1@BIFMA_PS.SLDASM", "COMPONENT", 0, 0, 0, false, 0, null, 0);
            // code comended below work on changing a part's config in a assembly
            // Component Configuration (Flyout Menu)
            //object swComp = null;
            //swComp = swDoc.ISelectionManager.GetSelectedObjectsComponent4(1, -1);
            //swComp.ReferencedConfiguration = Convert.ToString(index);
            //boolstatus = swDoc.EditRebuild3();
            boolstatus = swDoc.ShowConfiguration2(Convert.ToString(index));
            //get mass info
            ModelDocExtension swModelExt = default(ModelDocExtension);

            SelectionMgr swSelMgr = default(SelectionMgr);
            swComp = default(Component2);

            int nStatus = 0;
            double[] vMassProp = null;
            int i = 0;
            int nbrSelections = 0;

            swModel = (ModelDoc2)swApp.ActiveDoc;
            swModelExt = swModel.Extension;
            swSelMgr = (SelectionMgr)swModel.SelectionManager;
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Extension.SelectByID2("BIFMA_PS", "COMPONENT", 0, 0, 0, false, 0, null, 0);

            ModelDocExtension Extn;
            ModelDoc2 swModelDoc;
            MassProperty MyMassProp;
            double[] pmoi;
            double[] vValue;
            object com;
            double[] value = new double[3];
            int errors;
            int warnings;
            double val;

            Extn = swModelExt;

            // Create mass properties
            MyMassProp = Extn.CreateMassProperty();

            // Use document property units (MKS)
            MyMassProp.UseSystemUnits = false;

            val = MyMassProp.Mass;
            double[] X = MyMassProp.CenterOfMass;
            double temp_z = X[2];
            double temp_y = X[1];

            temp_z *= -1;
            if (index == 1)
            {

                temp_z += -10.17635949 - 9.95;

            }
            else
            {
                temp_z += 8.51834051 - 9.95;
            }






            string cw_dr_center = temp_z.ToString("0.00") + "," + temp_y.ToString("0.00") + "_";
            string cw_dr_g = MyMassProp.Mass.ToString("0.00") + "_";
            swApp.ActivateDoc2("BIFMA_PS.SLDASM", false, ref longstatus);
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Save3(1, swErrors, swWarnings);
            swApp.CloseDoc("BIFMA_PS.SLDASM");
            killSW();






            return new List<string>() { cw_dr_center, cw_dr_g };
        }
        public void activeCurrent(string name)
        {
            swApp.ActivateDoc2(name, false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
        }
        public void updatePart_Shell(string partclass, string partmaster)
        {
            Database database = new Database();
            DateTime today = DateTime.Today;
            string todayString = today.ToString("MMddyyyy");
            string variable = "";
            variable = CabinetOverview.getPartVarString(partmaster, partclass);
            //string sql = "select * from partclass where name = '" + partclass + "'";
            //MySqlDataReader rd = database.getMySqlReader_rd1(sql);
            //if (rd.Read())
            //{
            //    variable = rd.GetString("variables");
            //}
            //database.CloseConnection_rd1();
            Directory.Move(libraryPath + "part\\shell\\" + partclass + "\\" + partmaster, libraryPath + "part\\shell\\" + partclass + "\\" + partmaster + "_" + todayString);
            Directory.CreateDirectory(libraryPath + "part\\shell\\" + partclass + "\\" + partmaster + "\\");
            string[] partNames = Directory.GetDirectories(libraryPath + "part\\shell\\" + partclass + "\\" + partmaster + "_" + todayString + "\\", 
                "*", SearchOption.AllDirectories);
            
            for (int i = 0; i < partNames.Count(); i++)
            {
                string[] partnames = partNames[i].Split('\\');
                string partname = partnames[partnames.Count() - 1];
                List<int> whd = CabinetOverview.whd(partclass, partmaster, partname, variable);
                activeApp();
                BuildPart_Shell(partclass, whd[0], whd[1], whd[2], partmaster);
            }
        }
        public void updateDrawing_Shell(string partClass, string partMaster)
        {
            Database database = new Database();
            string num = "";
            string variable = "";
            string sql = "select * from masterpart_shell where parthead = '" + partMaster + "'";
            MySqlDataReader rd = database.getMySqlReader_rd1(sql);
            if (rd.Read())
            {
                num = rd.GetString("num");
            }
            database.CloseConnection_rd1();
            string keyvalue = "";
            sql = "select * from bom_shell where bom_shell." + partClass + " = '" + num + "'";
            rd = database.getMySqlReader_rd1(sql);
            if (rd.Read())
            {
                keyvalue = rd.GetString("keyvalue");
            }
            database.CloseConnection_rd1();
            variable = CabinetOverview.getPartVarString(partMaster, partClass);
            //sql = "select * from partclass where name = '" + partClass + "'";
            //rd = database.getMySqlReader_rd1(sql);
            //if (rd.Read())
            //{
            //    variable = rd.GetString("variables");
            //}
            //database.CloseConnection_rd1();
            string[] cabNames = Directory.GetDirectories(libraryPath + "assembly\\cabinet\\","*", SearchOption.AllDirectories);
            for (int i = 0; i < cabNames.Count(); i++)
            {
                string[] cabnames = cabNames[i].Split('\\');
                string cabname = cabnames[cabnames.Count() - 1];
                List<string> keylist = CabinetOverview.fromFormalNameToCabDetail(cabname);
                if (keyvalue == keylist[0] + "_" + keylist[1] + "_" + keylist[2])
                {
                    int index = CabinetOverview.fromVarToPartClassNameRegulation(variable);
                    string partname = CabinetOverview.genPartNameByMaster(Convert.ToInt16(keylist[3]), Convert.ToInt16(keylist[4]), Convert.ToInt16(keylist[5]), partMaster, index);
                    File.Delete(libraryPath + "assembly\\cabinet\\" + cabname + "\\" + cabname + "_" + partname + ".SLDPRT");
                    File.Delete(pdfPath + cabname + ".PDF");
                    File.Copy(libraryPath + "part\\shell\\" + partClass + "\\" + partMaster + "\\" + partname + "\\" + partname + ".SLDPRT", libraryPath + "assembly\\cabinet\\" + cabname + "\\" + cabname + "_" + partname + ".SLDPRT");
                    activeApp();
                    openDrawingFile(libraryPath + "assembly\\cabinet\\" + cabname + "\\" + cabname + "_1" + ".SLDDRW");
                    exportDrawingToPDF(pdfPath, cabname);
                    killSW();
                }
            }
        }

        public void test()
        {
            activeApp();
            openAssemblyFile(masterPath + "1.SLDASM");
            openPartFile(libraryPath + "part\\shell\\10\\LC10_06302020\\LC10-35\\LC10-35.SLDPRT");
            swApp.ActivateDoc2("1.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;
            Component2 swComponent = (Component2)swAssembly.AddComponent4(libraryPath + "part\\shell\\10\\LC10_06302020\\LC10-35\\LC10-35.SLDPRT", "0", 0, 0, 0);
            swApp.ActivateDoc2("1.SLDASM", false, 0);
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swAssembly = (AssemblyDoc)swModel;
            swComponent = (Component2)swAssembly.AddComponent4(libraryPath + "part\\shell\\10\\LC10_06302020\\LC10-35\\LC10-35.SLDPRT", "1", 0, 0, 0);

        }

        public void buildGlassDoor(List<string> doorList)
        {
            ///0:  material
            ///1:  confi
            ///2:  actualWidth.ToString("0.000000")
            ///3:  actualHeight.ToString("0.000000")
            ///4:  lockN.ToString()
            ///5:  chN.ToString()
            ///6:  pullPro
            ///7:  dic_hinge[hingePro]
            ///8:  d3Name
            ///9:  outDoorName
            ///10: innerDoorName
            ///11: doorName
            ///12: nameWidth
            ///13: nameHeight
            ///14: doorType
        }

        

        public bool buildSingle(string partType, string mainName, string configName, double w, double h, double d, string partName, string paintIndex)
        {
            string realName = mainName;
            int l = mainName.Count();
            if (realName.Substring(0, 1) == "-")
            {
                realName = mainName.Substring(1, l - 1);
            }
            else if (realName.Substring(l - 1, 1) == "-")
            {
                realName = mainName.Substring(0, l - 1);
            }
            realName += ".SLDPRT";

            if (fileName != realName)
            {
                activeApp();
                openPartFile(buildMasterPath + partType + "\\" + realName);
            }
            if (configName == "") { configName = "Default"; }
            boolStatus = swModel.ShowConfiguration2(configName);
            int featCount = swModel.GetFeatureCount();
            for (int i = featCount; i > 0; i--)
            {
                swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - i);
                if ((swFeature) != null)
                {
                    string temp_featName_ori = swFeature.Name;
                    string temp_featName = swFeature.Name;
                    if (temp_featName_ori == "width")
                    {
                        if (w != 0)
                        {
                            changeDimension("width@width", w / mToInch); reBuildThis();
                        }
                    }
                    else if (temp_featName_ori == "height")
                    {
                        if (h != 0)
                        {
                            changeDimension("height@height", h / mToInch); reBuildThis();
                        }
                    }
                    else if (temp_featName_ori == "depth")
                    {
                        if (d != 0)
                        {
                            changeDimension("depth@depth", d / mToInch); reBuildThis();
                        }
                    }
                    else if (temp_featName_ori == "widthHeight")
                    {
                        if (w != 0)
                        {
                            changeDimension("width@widthHeight", w / mToInch);
                        }
                        if (h != 0)
                        {
                            changeDimension("height@widthHeight", h / mToInch); 
                        }
                        reBuildThis();
                    }
                    else if (temp_featName_ori == "widthDepth")
                    {
                        if (w != 0)
                        {
                            changeDimension("width@widthDepth", w / mToInch);
                        }
                        if (d != 0)
                        {
                            changeDimension("depth@widthDepth", d / mToInch);
                        }
                        reBuildThis();
                    }
                    else if (temp_featName_ori == "heightDepth")
                    {
                        if (h != 0)
                        {
                            changeDimension("height@heightDepth", h / mToInch);
                        }
                        if (d != 0)
                        {
                            changeDimension("depth@heightDepth", d / mToInch); 
                        }
                        reBuildThis();
                    }
                }
            }

            List<string> readInfo = readPartIndex(configName, partName, partType);
            string sql = "";
            sql = "insert into partindexes(partname, materialType, materialThickness, surfaceArea, cubicBoundingBoxWHD, " +
            "accessory, stockQuant, cutProgram, punch, bendNum, salvagniniBool, type, description, flattenSheetInfo, cutLength, cutOut, paint, mainname) " +
            $"value('{partName}', '{readInfo[1]}', '{readInfo[2]}', '{readInfo[3]}', " +
            $"'{readInfo[4]}', '{readInfo[5]}', '{readInfo[6]}', '{readInfo[9]}', '{readInfo[10]}', '{readInfo[11]}', " +
            $"'{readInfo[12]}', '{readInfo[13]}', '{readInfo[14]}', '{readInfo[7]}', '{readInfo[8]}', '{readInfo[15]}', '{paintIndex}', '{mainName}')";
            database.mySqlExecuteQuery(sql);
            database.CloseConnection();
            return true;
        }
        public bool openFileEditSketchReadIndex(string fileMasterNameWithPath, List<string> configName, List<double> w, List<double> d, List<string> partName, string paintIndex, string partType)
        {
            activeApp();
            openPartFile(fileMasterNameWithPath);
            for (int i = 0; i < configName.Count; i++)
            {
                boolStatus = swModel.ShowConfiguration2(configName[i]);
                for (int j = 0; j < w.Count; j++)
                {
                    for (int k = 0; k < d.Count; k++)
                    {
                        int featCount = swModel.GetFeatureCount();
                        for (int l = featCount; l > 0; l--)
                        {
                            swFeature = (Feature)swModel.FeatureByPositionReverse(featCount - l);
                            if ((swFeature) != null)
                            {
                                string temp_featName_ori = swFeature.Name;
                                string temp_featName = swFeature.Name;
                                if (temp_featName_ori == "widthDepth")
                                {
                                    changeDimension("width@widthDepth", w[j]);
                                    changeDimension("depth@widthDepth", d[k]);
                                    reBuild();
                                }
                                
                            }
                        }
                        List<string> readInfo = readPartIndex(configName[i], partName[i]+w[j].ToString()+d[k].ToString(), partType);



                        string sql = "";

                        sql = "insert into partindexes(partname, materialType, materialThickness, surfaceArea, cubicBoundingBoxWHD, " +
                        "accessory, stockQuant, cutProgram, punch, bendNum, salvagniniBool, type, description, flattenSheetInfo, cutLength, cutOut, paint) " +
                        $"value('{partName + "-" + configName}', '{readInfo[1]}', '{readInfo[2]}', '{readInfo[3]}', " +
                        $"'{readInfo[4]}', '{readInfo[5]}', '{readInfo[6]}', '{readInfo[9]}', '{readInfo[10]}', '{readInfo[11]}', " +
                        $"'{readInfo[12]}', '{readInfo[13]}', '{readInfo[14]}', '{readInfo[7]}', '{readInfo[8]}', '{readInfo[15]}', '{paintIndex}')";

                        database.mySqlExecuteQuery(sql);
                        database.CloseConnection();

                    }
                }
            }
            


            return true;
        }
        CustomPropertyManager swCustMgr;
        SelectionMgr swSelMgr;
        object _boxIndex;
        public static Dictionary<string, string> boolToInt = new Dictionary<string, string>()
        {
            {"true", "1" }, {"false", "0"}, {"", "0"}
        };
        
        public List<string> readPartIndex(string configurationName, string partname, string partType)
        {
            bool status = false;
            string configValue;
            string stockQuant = "0";
            string Programs = null;
            string punch = "false";
            string salvagnini = "false";
            string description = "";
            string materialType = "";
            string materialThickness = "";
            string surfaceArea = "";
            string bendNum = "0";
            string flattenSheetSize_l = "";
            string flattenSheetSize_w = "";
            string cutLength_outer = "";
            string cutLength_inner = "";
            string flattenSheetSizeString = "";
            string cutLengthString = "";
            string cutOut = "";

            swModelExt = swModel.Extension;
            swCustMgr = swModelExt.CustomPropertyManager[configurationName];
            status = swCustMgr.Get4("Stock Quant", false, out configValue, out stockQuant);
            status = swCustMgr.Get4("Program", false, out configValue, out Programs);

            status = swCustMgr.Get4("Punch", false, out configValue, out punch);
            try
            {
                punch = Convert.ToBoolean(punch).ToString();
            }
            catch { }
            punch = boolToInt[punch];
            status = swCustMgr.Get4("Salvagnini", false, out configValue, out salvagnini);
            try
            {
                salvagnini = Convert.ToBoolean(salvagnini).ToString();
            }
            catch { }
            salvagnini = boolToInt[salvagnini];
            status = swCustMgr.Get4("Description", false, out configValue, out description);
            ////cool
            ////cutlist properties
            ///swModel = (ModelDoc2)swApp.ActiveDoc;
            swModelExt = swModel.Extension;
            swSelMgr = swModel.SelectionManager;
            status = false;
            while (status == false)
            {
                status = swModelExt.SelectByID2("Sheet<1>", "SUBWELDFOLDER", 0, 0, 0, true, 0, null, 0);
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
            status = swModelExt.SelectByID2("Bounding Box", "SKETCH", 0, 0, 0, false, 0, null, 0);
            swFeature = swSelMgr.GetSelectedObject6(1, -1);
            status = swFeature.GetBox(ref _boxIndex);
            double[] boxIndex = (double[])_boxIndex;
            List<double> boxWHD = new List<double>()
            {
                (boxIndex[3]-boxIndex[0])*mToInch, (boxIndex[4]-boxIndex[1])*mToInch, (boxIndex[5]-boxIndex[2])*mToInch
            };
            string cubicBoxString = boxWHD[0].ToString("0.0000") + "," + boxWHD[1].ToString("0.0000") + "," + boxWHD[2].ToString("0.0000");
            

            //killSW();
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
                salvagnini = "0";
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
                Programs, punch, bendNum, salvagnini, partType, description, cutOut};
        }
    }
}
