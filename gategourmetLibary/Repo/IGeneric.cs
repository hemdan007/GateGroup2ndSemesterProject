using gategourmetLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gategourmetLibrary.Repo
{
    internal interface IGeneric<T>
    {
        void Add(T item);
        int Get();
        void Delete(int primryKey);
        void GetAll();
        void Update(T item);
        void Filter(string item);
    }
}

