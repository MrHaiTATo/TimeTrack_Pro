using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TimeTrack_Pro.Model
{
    //定义员工实体
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ShiftPreference> Preferences { get; set; }
    }

    //定义班次实体
    public class Shift
    {
        public int Id {  set; get; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    //定义员工偏好
    public class ShiftPreference
    {
        public int ShiftId { set; get; }
        public int PreferenceLevel { set; get; }
    }

    //定义排班系统类
    public class SchedulingSystem
    {
        private List<Employee> employees;
        private List<Shift> shifts;

        public SchedulingSystem(List<Employee> employees, List<Shift> shifts)
        {
            this.employees = employees;
            this.shifts = shifts;   
        }

        //智能化
        public async Task<List<(Employee, Shift)>> ScheduleAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 使用并行处理提高效率
            var schedules = await Task.Run(() =>
            {
                return employees.SelectMany(employee =>
                    shifts.OrderBy(shift =>
                        employee.Preferences.Any(p => p.ShiftId == shift.Id) ? 0 : 1)
                        .Select(shift => (employee, shift))).ToList();
            });

            stopwatch.Stop();
            Debug.WriteLine($"Scheduling took {stopwatch.ElapsedMilliseconds} ms");

            return schedules;
        }
        
    }

    public class Demo
    {
        // 智能排班示例使用
        public async static Task demo1()
        {
            // 初始化员工和班次数据
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Alice", Preferences = new List<ShiftPreference>{ new ShiftPreference { ShiftId = 1, PreferenceLevel = 1 } } },
                new Employee { Id = 2, Name = "Maike", Preferences = new List<ShiftPreference>{ new ShiftPreference { ShiftId = 2, PreferenceLevel = 2 } } },
                new Employee { Id = 3, Name = "Boj", Preferences = new List<ShiftPreference>{ new ShiftPreference { ShiftId = 3, PreferenceLevel = 3 } } },
                new Employee { Id = 4, Name = "ChenJie", Preferences = new List<ShiftPreference>{ new ShiftPreference { ShiftId = 1, PreferenceLevel = 1 } } },
                new Employee { Id = 5, Name = "LinDa", Preferences = new List<ShiftPreference>{ new ShiftPreference { ShiftId = 2, PreferenceLevel = 2 } } },
                new Employee { Id = 6, Name = "Bob", Preferences = new List<ShiftPreference>{ new ShiftPreference { ShiftId = 3, PreferenceLevel = 3 } } }
            };

            var shifts = new List<Shift>
            {
                new Shift{ Id = 1, Name = "Morning", StartTime = new TimeSpan(8,0,0), EndTime = new TimeSpan(16,0,0) },
                new Shift{ Id = 2, Name = "Afterning", StartTime = new TimeSpan(16,0,0), EndTime = new TimeSpan(24,0,0) },
                new Shift{ Id = 3, Name = "Evening", StartTime = new TimeSpan(0,0,0), EndTime = new TimeSpan(8,0,0) }
            };

            var system = new SchedulingSystem(employees, shifts);
            var schedules = await system.ScheduleAsync();

            foreach (var (employee, shift) in schedules)
            {
                Debug.WriteLine($"{employee.Name} is scheduled for {shift.Name} shift.");
            }
        }
    }

}
