using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EAGO.DLL;
using System.Data;

namespace EAGO.BLL
{
    public partial class DropDrownList
    {

        private readonly EAGO.DLL.DropDrownList ddl = new EAGO.DLL.DropDrownList();

        public List<string> getValueList(string type)
        {
            return ddl.getValueList(type);
        }


        public List<EAGO.Models.DropDrownList> getDropDrownList(string type)
        {
            return ddl.getDropDrownList(type);
        }

    }
}
