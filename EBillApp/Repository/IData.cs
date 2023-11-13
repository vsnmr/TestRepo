using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBillApp.Models;

namespace EBillApp.Repository
{
    interface IData
    {
        void SaveBillDetails(BillDetail detail);
        void SaveBillitems(List<Items> items,SqlConnection con,int id);
        List<BillDetail> GetAllDetails();
        BillDetail GetDetail(int id);
    }
}
