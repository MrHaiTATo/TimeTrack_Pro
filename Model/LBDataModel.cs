using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class LBDataModel : ViewModelBase
    {
        public int Index { get; set; }

        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        public bool IsSelected { get; set; }

        public string ImgPath { get; set; }

        public ObservableCollection<LBDataModel> DataList { get; set; }
    }
}
