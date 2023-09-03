using Nodez.Data.Interface;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.General.Controls
{
    public class DataControl
    {
        private static readonly Lazy<DataControl> lazy = new Lazy<DataControl>(() => new DataControl());

        public static DataControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.DataControl.ToString(), out object control))
                {
                    return (DataControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual IData GetData(dynamic[] args) 
        {
            return null;
        }
    }
}
