using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simply_database_test
{
    internal class testallergi
    {
        public int ID { set; get; }
        public string Name { set; get; }

        public testallergi()
        {

        }


    public testallergi(int id,string name):this()
        {
            ID = id;
            Name = name;
        }
    }
}
