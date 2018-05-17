﻿using Dashboard_New.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using ClosedXML.Excel;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data;
using Dashboard_New.App_Start;
using System.Web.Security;
using Dashboard_New.Models.custom;

namespace Dashboard_New.Controllers
{
    [DashboardAutherisation]
    public class HomeController : Controller
    {
       // [DashboardAutherisation]
        public ActionResult Index()
        {
            return View();
        }

        [DashboardAutherisation("vc")]
        public ActionResult vc()
        {
            return View(new VModel());
        }      

        [HttpPost]
        public ActionResult vcpost(Dashboard_New.Models.custom.vcvendor v1, string grid, string export)
        {
            if (string.IsNullOrEmpty(v1.from_dt))
            {
                ModelState.AddModelError("from_dt", "Date Required");
            }
            if (string.IsNullOrEmpty(v1.to_dt))
            {
                ModelState.AddModelError("to_dt", "Date Required");
            }
            if (ModelState.IsValid)
            {
                if (string.Equals("Export To Excel", export))
                {
                    vcEntities entities = new vcEntities();
                    DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[18] { new DataColumn("DINP"),new DataColumn("VENDOR NAME"), new DataColumn("VCTRNO"), new DataColumn("AMOUNT"), new DataColumn("NPRNO")
                , new DataColumn("ICCNO"), new DataColumn("COMNO"), new DataColumn("VRNO"), new DataColumn("BRNO"), new DataColumn("VPartyCode"),
                 new DataColumn("ASSTCK"), new DataColumn("ACCTCK"), new DataColumn("ACCT1CK"), new DataColumn("SOCK"), new DataColumn("DRCK"), new DataColumn("CRDATE")
                , new DataColumn("VCTRBNO"), new DataColumn("LUSER")});
                    DateTime fromdt = DateTime.ParseExact(v1.from_dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime todt = DateTime.ParseExact(v1.to_dt, "dd/MM/yyyy", CultureInfo.InvariantCulture); var disp = entities.Database.SqlQuery<Dashboard_New.Models.custom.vcvendor>(string.Format("select DINP,VCTRNO,AMOUNT,NPRNO,ICCNO,COMNO,VRNO,BRNO,VPartyCode,ASSTCK,ACCTCK,ACCT1CK,SOCK,DRCK,CRDATE,VCTRBNO,LUSER, convert(decimal,dbo.getAmount(AMOUNT)), (select top 1 VName from vendormaster where VPartyCode = vvv.VPartyCode) VName from vcvouch vvv where DINP>= '{0}' AND DINP<= '{1}'", fromdt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), todt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
                    dt.Columns[3].DataType = typeof(decimal);
                    foreach (var x in disp)
                    {
                        dt.Rows.Add(x.DINP.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), x.vname, x.VCTRNO, x.AMOUNT, x.NPRNO, x.ICCNO, x.COMNO, x.VRNO, x.BRNO, x.VPartyCode, x.ASSTCK, x.ACCTCK, x.ACCT1CK, x.SOCK, x.DRCK, x.CRDATE, x.VCTRBNO, x.LUSER);
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add("VC");
                        wb.Worksheet(1).Cell(1, 7).Value = "Vendor Credit (VC) : " + v1.from_dt + " to " + v1.to_dt;
                        wb.Worksheet(1).Cell(1, 7).Style.Font.Bold=true;                
                        wb.Worksheet(1).Cell(3, 1).InsertTable(dt);
                        var wbs = wb.Worksheets.FirstOrDefault();
                        wbs.Tables.FirstOrDefault().ShowAutoFilter = false;
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VC.xlsx");
                        }
                    }
                }
                if (string.Equals("Submit", grid))
                {
                    string from_dt = v1.from_dt;
                    string to_dt = v1.to_dt;
                    DateTime fromdt = DateTime.ParseExact(v1.from_dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime todt = DateTime.ParseExact(v1.to_dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);                   
                    List<vcvendor> records = new List<vcvendor>();
                    try
                    {
                        vcEntities vcobj = new vcEntities();
                        records = vcobj.Database.SqlQuery<vcvendor>(string.Format("select *, (select top 1 VName from vendormaster where VPartyCode = vvv.VPartyCode) VName from vcvouch vvv where DINP>= '{0}' AND DINP<= '{1}'", fromdt.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture), todt.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture))).ToList();
                        Dashboard_New.Models.VModel hg = new VModel();
                        hg.vm = records;
                        return View("vc", hg);
                    }
                    catch (Exception e)
                    { Console.WriteLine("Erorrr : " + e); }
                    VModel vd = new VModel();              
                    vd.vm = records;
                    return View("vc", vd);
                }
            }
            else
            {
                return View("vc", v1);
            }
            return null;
        }
        [DashboardAutherisation("dc")]
        public ActionResult dc()
        {
            return View(new VModel());
        }
        [HttpPost]
         public ActionResult dcpost(Dashboard_New.Models.custom.dcvendor v2, string grid, string export)
         {
            if (string.IsNullOrEmpty(v2.from_dt1))
            {
                ModelState.AddModelError("from_dt1", "Date Required");
            }
            if (string.IsNullOrEmpty(v2.to_dt1))
            {
                ModelState.AddModelError("to_dt1", "Date Required");
            }
            if (ModelState.IsValid)
            {
                if (string.Equals("Export To Excel", export))
                {
                    vcEntities entities = new vcEntities();
                    DataTable dt = new DataTable("Grid");
                  dt.Columns.AddRange(new DataColumn[18] { new DataColumn("DINP"),new DataColumn("VENDOR NAME"), new DataColumn("DCTRNO"), new DataColumn("AMOUNT"), new DataColumn("NPRNO")
                , new DataColumn("ICCNO"), new DataColumn("COMNO"), new DataColumn("VRNO"), new DataColumn("BRNO"), new DataColumn("DCID"),
                 new DataColumn("ASSTCK"), new DataColumn("ACCTCK"), new DataColumn("ACCT1CK"), new DataColumn("SOCK"), new DataColumn("DRCK"), new DataColumn("CRDATE")
                , new DataColumn("DCTRBNO"), new DataColumn("LUSER")});
                     DateTime fromdt = DateTime.ParseExact(v2.from_dt1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime todt = DateTime.ParseExact(v2.to_dt1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var disp= entities.Database.SqlQuery<Dashboard_New.Models.custom.dcvendor>(string.Format("select DINP,DCTRNO,dbo.getAmount(AMOUNT) AS AMOUNT,NPRNO,ICCNO,COMNO,VRNO,BRNO,DCID,ASSTCK,ACCTCK,ACCT1CK,SOCK,DRCK,CRDATE,DCTRBNO,LUSER, (select top 1 COORNAME from DCMLST where DCID = vvv.DCID) VName from DVOUCH vvv where DINP>= '{0}' AND DINP<= '{1}'", fromdt.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture),todt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
                     dt.Columns[3].DataType = typeof(decimal);
                     foreach (var x in disp)
                    {
                        dt.Rows.Add(x.DINP.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), x.vname, x.DCTRNO, x.AMOUNT, x.NPRNO, x.ICCNO, x.COMNO, x.VRNO, x.BRNO, x.DCID, x.ASSTCK, x.ACCTCK, x.ACCT1CK, x.SOCK, x.DRCK, x.CRDATE, x.DCTRBNO, x.LUSER);
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add("DC");
                        wb.Worksheet(1).Cell(3, 1).InsertTable(dt);                       
                        wb.Worksheet(1).Cell(1, 7).Value = "Direct Credit (DC) : " + v2.from_dt1 + " to " + v2.to_dt1;
                        wb.Worksheet(1).Cell(1, 7).Style.Font.Bold = true;
                        var wbs = wb.Worksheets.FirstOrDefault();
                        wbs.Tables.FirstOrDefault().ShowAutoFilter = false;
                        wb.Properties.Title = "theTitle";
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);                            
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DC.xlsx");
                        }
                    }
                }
                if (string.Equals("Submit", grid))
                {
                    string from_dt = v2.from_dt1;
                    string to_dt = v2.to_dt1;
                    DateTime fromdt = DateTime.ParseExact(v2.from_dt1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime todt = DateTime.ParseExact(v2.to_dt1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    List<dcvendor> records = new List<dcvendor>();
                    try
                    {                        
                        vcEntities vcobj = new vcEntities();
                        records = vcobj.Database.SqlQuery<dcvendor>(string.Format("select *, (select top 1 COORNAME from DCMLST where DCID = vvv.DCID) VName from DVOUCH vvv where DINP>= '{0}' AND DINP<= '{1}'", fromdt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), todt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
                        Dashboard_New.Models.VModel dv = new VModel();                      
                        dv.dm = records;                        
                        return View("dc", dv);
                    }
                    catch (Exception e)
                    { Console.WriteLine("Erorrr : " + e); }
                    VModel vd = new VModel();
                    vd.dm = records;
                    return View("dc", vd);
                }
            }
            else
            {
                return View("dc", v2);
            }
            return null;
        }

        [DashboardAutherisation("nirf")]
        public ActionResult nirf()
        {
            return View(new VModel());
        }
        public ActionResult nirfpost(Dashboard_New.Models.custom.nirf v3,string grid, string export)
        {
            if (string.IsNullOrEmpty(v3.from_year))
            {
                ModelState.AddModelError("From year", "Year Required");
            }
            if (string.IsNullOrEmpty(v3.to_year))
            {
                ModelState.AddModelError("To year", "Year Required");
            }
            if (ModelState.IsValid)
            {
                if (string.Equals("Export To Excel", export))
                {
                    vcEntities entities = new vcEntities();
                    DataTable dt = new DataTable("Grid");
                    string from_dt = v3.from_year.Substring(2);
                    string to_dt = v3.to_year.Substring(2);
                    DateTime fromdt = DateTime.ParseExact(v3.from_year, "yyyy", CultureInfo.InvariantCulture);
                    DateTime todt = DateTime.ParseExact(v3.to_year, "yyyy", CultureInfo.InvariantCulture);
                    string tabname = "REC" + from_dt + to_dt;
                    dt.Columns.AddRange(new DataColumn[9] { new DataColumn("YEAR"),new DataColumn("D"),new DataColumn("PROJECT NUMBER"),new DataColumn("COORDINATOR NAME"), new DataColumn("AGENCY"), new DataColumn("TITLE"), new DataColumn("SANCTION NUMBER"), new DataColumn("SANCTION DATE"), new DataColumn("AMOUNT")});
                    var disp = entities.Database.SqlQuery<Dashboard_New.Models.custom.NIRF_RPT>(string.Format("select '"+ v3.from_year + " - "+ v3.to_year + "' as YEAR,datepart(month,(R.date)) as sdm,m.NPRNO,m.COOR_NAME,SUBSTRING(m.NPRNO,11,4)as AGENCY, REPLACE(M.TITLE, CHAR(13) + char(10), '') AS TITLE, m.SANCTNNO, m.SANCTDTE, sum(r.rt) as amount  from " + tabname + " r , mstlst m where m.NPRNO = r.NPRNO and((r.RTNO like '0%') or(r.RTNO like 'SO%') or(r.RTNO like 'MO%') or (r.RTNO like 'B%' and r.rt < 0)) and r.HEAD is null and substring(m.NPRNO, 11, 4) not  in (select researchCode from FoxOffice.dbo.InternalProjectCode where ResearchCode != 'IITM')and m.NPRNO not like 'FDR' and m.NPRNO not like 'ACC%' and m.NPRNO not like 'ICC' and m.NPRNO not like '%DEVP%' and m.NPRNO not like 'OAA'and m.NPRNO not like '%EQPT%' and m.NPRNO not like 'FDR' and m.NPRNO not like 'ICSROH' and m.NPRNO not like 'ACC%' and m.NPRNO not like 'OTHERS' and m.NPRNO not like '%BMF%' and m.NPRNO not like '%DADM%' and m.NPRNO not like '%DARE%' and m.NPRNO not like '%ACCT%' and m.NPRNO not like '%RMF%'and m.NPRNO not like '%DPLA%' and m.NPRNO not in('DIA1213001IITMDIAR','DIA1718005IITMDIAR','DIA17148007ALUMDIAR') AND M.NPRNO NOT LIKE 'RSI%' AND M.NPRNO not like '%IPRC%' and spons not like 'IIT A%' and m.NPRNO not like 'CCE0910008IITMSHAN' and m.NPRNO not like 'COM%' group by m.NPRNO,m.COOR_NAME,m.SANCTDTE,m.SANCTNNO,DATEPART(Month, (R.Date)),m.TITLE order by DATEPART(MONTH,(R.date)),SUBSTRING(M.nprno, 1, 3),m.NPRNO", fromdt.ToString("yy", CultureInfo.InvariantCulture), todt.ToString("yy", CultureInfo.InvariantCulture))).ToList();
                    dt.Columns[8].DataType = typeof(Int32);
                    dt.Columns[1].DataType = typeof(Int32);
                    foreach (var x in disp)
                    {
                        dt.Rows.Add(x.YEAR, x.sdm, x.NPRNO, x.COOR_NAME, x.AGENCY, x.TITLE, x.SANCTNNO, x.SANCTDTE,x.amount);
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add("NIRF");
                        wb.Worksheet(1).Cell(3, 1).InsertTable(dt);
                        wb.Worksheet(1).Cell(1, 7).Value = "NIRF : " + v3.from_year + " to " + v3.to_year;
                        wb.Worksheet(1).Cell(1, 7).Style.Font.Bold = true;
                        var wbs = wb.Worksheets.FirstOrDefault();
                        wbs.Tables.FirstOrDefault().ShowAutoFilter = false;
                        wb.Properties.Title = "NIRF REPORT";
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NIRF.xlsx");
                        }
                    }
                }
                if (string.Equals("Submit", grid))
                {
                    string from_dt = v3.from_year.Substring(2);
                    string to_dt = v3.to_year.Substring(2);
                    DateTime fromdt = DateTime.ParseExact(v3.from_year, "yyyy", CultureInfo.InvariantCulture);
                    DateTime todt = DateTime.ParseExact(v3.to_year, "yyyy", CultureInfo.InvariantCulture);
                    string tabname = "REC" + from_dt + to_dt;
                    List<NIRF_RPT> records = new List<NIRF_RPT>();
                    try
                    {
                        vcEntities vcobj = new vcEntities();
                        records = vcobj.Database.SqlQuery<NIRF_RPT>(string.Format("select '" + v3.from_year + " - " + v3.to_year + "' as YEAR,datepart(month,(R.date)) as sdm,m.NPRNO,m.COOR_NAME,SUBSTRING(m.NPRNO,11,4)as AGENCY, REPLACE(M.TITLE, CHAR(13) + char(10), '') AS TITLE, m.SANCTNNO, m.SANCTDTE, sum(r.rt) as amount  from " + tabname + " r , mstlst m where m.NPRNO = r.NPRNO and((r.RTNO like '0%') or(r.RTNO like 'SO%') or(r.RTNO like 'MO%') or (r.RTNO like 'B%' and r.rt < 0)) and r.HEAD is null and substring(m.NPRNO, 11, 4) not  in (select researchCode from FoxOffice.dbo.InternalProjectCode where ResearchCode != 'IITM')and m.NPRNO not like 'FDR' and m.NPRNO not like 'ACC%' and m.NPRNO not like 'ICC' and m.NPRNO not like '%DEVP%' and m.NPRNO not like 'OAA'and m.NPRNO not like '%EQPT%' and m.NPRNO not like 'FDR' and m.NPRNO not like 'ICSROH' and m.NPRNO not like 'ACC%' and m.NPRNO not like 'OTHERS' and m.NPRNO not like '%BMF%' and m.NPRNO not like '%DADM%' and m.NPRNO not like '%DARE%' and m.NPRNO not like '%ACCT%' and m.NPRNO not like '%RMF%'and m.NPRNO not like '%DPLA%' and m.NPRNO not in('DIA1213001IITMDIAR','DIA1718005IITMDIAR','DIA17148007ALUMDIAR') AND M.NPRNO NOT LIKE 'RSI%' AND M.NPRNO not like '%IPRC%' and spons not like 'IIT A%' and m.NPRNO not like 'CCE0910008IITMSHAN' and m.NPRNO not like 'COM%' group by m.NPRNO,m.COOR_NAME,m.SANCTDTE,m.SANCTNNO,DATEPART(Month, (R.Date)),m.TITLE order by DATEPART(MONTH,(R.date)),SUBSTRING(M.nprno, 1, 3),m.NPRNO", fromdt.ToString("yy", CultureInfo.InvariantCulture), todt.ToString("yy", CultureInfo.InvariantCulture))).ToList();
                        Dashboard_New.Models.VModel dv = new VModel();
                        dv.from_year = v3.from_year;
                        dv.to_year = v3.to_year;
                        dv.nirf = records;
                        return View("NIRF", dv);
                    }
                    catch (Exception e)
                    { Console.WriteLine("Erorrr : " + e); }
                    VModel vd = new VModel();
                    vd.nirf = records;
                    vd.from_year = v3.from_year;
                    vd.to_year = v3.to_year;
                    return View("NIRF", vd);
                }
            }
            else
            {
                return View("nirf", v3);
            }
            return null;
        }
        public ActionResult Contact()
        { return View(); }

        [HttpPost]
        public FileResult Export(Dashboard_New.Models.VModel mod)
        {
            return null;
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(login l)
        {
            login lg = new login();
            if (ModelState.IsValid) 
            {
                using (FACCTEntities lge = new FACCTEntities())
                {
                    var v = lge.useraccounts.Where(a => a.Name.Equals(l.UserName) && a.Password.Equals(l.Password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogedUserID"] = v.UserName.ToString();
                        Session["LogedUserFullname"] = v.Password.ToString();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View("login");
        }    
    }
}