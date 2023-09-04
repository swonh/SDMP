using Nodez.Data.Interface;
using SDMP.General.CRP.MyMethods;
using Nodez.Sdmp.General.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.Controls
{
    public class UserDataControl : DataControl
    {
        private static readonly Lazy<UserDataControl> lazy = new Lazy<UserDataControl>(() => new UserDataControl());

        public static new UserDataControl Instance { get { return lazy.Value; } }

        public override IData GetData(dynamic[] args)
        {
            IData data = DataHelper.CreateData();

            return data;
        }
    }
}
