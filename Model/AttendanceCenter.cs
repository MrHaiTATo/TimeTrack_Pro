using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using HandyControl.Tools.Extension;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TimeTrack_Pro.Model
{
    public class AttendanceCenter
    {
        private List<AttendanceData> attendanceDatas;
        public List<AttendanceData> AttendanceDatas { get { return attendanceDatas; } }

        private List<BakUseData> employees;
        public List<BakUseData> Employees { get { return employees; } }
        

        public AttendanceCenter(string attendanceFile, string employeeFile) 
        {
            _init(attendanceFile, employeeFile);   
        }

        private void _init(string attendanceFile, string employeeFile)
        {
            string row;
            string[] cells;
            using (StreamReader reader = new StreamReader(attendanceFile))
            {
                attendanceDatas = new List<AttendanceData>();
                while (!reader.EndOfStream)
                {
                    row = reader.ReadLine();
                    if(string.IsNullOrEmpty(row) || row.Contains("NO") || row.Contains("YYYY/MM/DD"))
                        continue;
                    AttendanceData attendance = new AttendanceData();
                    cells = row.Split('|');
                    try
                    {
                        if(Regex.IsMatch(cells[0].Trim(), @"^[0-9]+$"))
                            attendance.Number = Convert.ToInt32(cells[0].Trim());
                        if(Regex.IsMatch(cells[1].Trim(), @"^[0-9]{4}(\-[0-9]{2}){2}$") && Regex.IsMatch(cells[2].Trim(), @"^[0-9]{2}:[0-9]{2}$"))
                            attendance.ClockTime = DateTime.ParseExact(cells[1].Trim() + " " + cells[2].Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        if (Regex.IsMatch(cells[3].Trim(), @"^[0-9]+$"))
                            attendance.UserIndex = Convert.ToInt32(cells[3].Trim());
                        if(Regex.IsMatch(cells[4].Trim(),@"^([0-9]{1,2})+\-[0-6]+$"))
                        {
                            attendance.Class = Convert.ToInt32(cells[4].Trim().Substring(0, 1));                          
                            attendance.ShiftClass = (ShiftClass)Convert.ToInt32(cells[4].Trim().Substring(2, 1));
                        }
                        else
                        {
                            attendance.Class = -1;
                            if (Regex.IsMatch(cells[4].Trim(), @"^\-[0-6]+$"))
                            {                                
                                attendance.ShiftClass = (ShiftClass)Convert.ToInt32(cells[4].Trim().Substring(1, 1));
                            }
                        }
                        if(Regex.IsMatch(cells[5].Trim(), @"^[1-5]{1}\s\-\s[0-9]{1}\s\-\s[0-1]{1}\s\-\s[0-9]{1}$"))
                        {
                            attendance.ClockMethod = (ClockMethod)Convert.ToInt32(cells[5].Trim().Substring(0, 1));
                            attendance.ClockState = (ClockState)Convert.ToInt32(cells[5].Trim().Substring(8, 1));
                        }      
                        attendanceDatas.Add(attendance);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }                     
                }
                reader.Close();
            }
            using (StreamReader reader = new StreamReader(employeeFile))
            {
                employees = new List<BakUseData>();
                while (!reader.EndOfStream)
                {
                    row = reader.ReadLine();
                    if (string.IsNullOrEmpty(row) || row.Contains("NO") || row.Contains("UserName"))
                        continue;
                    cells = row.Split('|');
                    try
                    {
                        BakUseData employee = new BakUseData();
                        if (Regex.IsMatch(cells[0].Trim(), @"^[0-9]+$"))
                            employee.Number = Convert.ToInt32(cells[0].Trim());
                        employee.Name = cells[1].Trim();
                        if (Regex.IsMatch(cells[2].Trim(), @"^[0-9]+$"))
                        {
                            employee.Index = Convert.ToInt32(cells[2].Trim());
                        }
                        else
                        {
                            employee.Index = -1;
                        }
                        if (Regex.IsMatch(cells[3].Trim(), @"^[0-9]+$"))
                        {
                            employee.Id = Convert.ToInt32(cells[3].Trim());
                        }
                        else
                        {
                            employee.Id = -1;
                        }
                        if (Regex.IsMatch(cells[4].Trim(), @"^[0-9]{4}(\-[0-9]{2}){2}$"))
                            employee.CreatedTime = DateTime.ParseExact(cells[4].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        employees.Add(employee);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
                employees.RemoveAll(e => 
                {   
                    if(string.IsNullOrEmpty(e.Name) || e.Id == -1 || e.Index == -1)
                        return true;
                    else
                        return false;
                });
                employees = employees.GroupBy(e => e.Id).Select(g => g.OrderByDescending(e => e.CreatedTime).First()).ToList();
                reader.Close();
            }
        }

        public List<AttendanceData> GetEmployeeAndAttendanceDataByDateTime(DateTime selectTime)
        {
            return attendanceDatas.Where(a => a.ClockTime.Year == selectTime.Year && a.ClockTime.Month == selectTime.Month).ToList();                                                            
        }

       
    }
}
