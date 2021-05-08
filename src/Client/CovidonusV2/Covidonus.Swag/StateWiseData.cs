using System.Collections.Generic;

namespace CovidonusV2.Swag
{
    public partial class StateWiseData : System.ComponentModel.INotifyPropertyChanged
    {
        public StateWiseData()
        {
            DistrictData = new List<DistrictWiseData>();
        }
    }
}
