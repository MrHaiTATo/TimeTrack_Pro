using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.ViewModel
{
    public class DataGridViewModel : ViewModelBase
    {
        private ObservableCollection<AttendanceRule> dataList;
        public ObservableCollection<AttendanceRule> DataList
        {
            get => dataList;
            set => Set(ref dataList, value);
        }

        public DataGridViewModel()
        {
            DataList = GetDataList();
        }

        private ObservableCollection<AttendanceRule> GetDataList()
        {
            return new ObservableCollection<AttendanceRule>() { };
        }
    }
}
