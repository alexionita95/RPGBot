using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    public interface IDataManager
    {
        string GetDataFrom(string name);
        void SaveDataTo(string name, string data);
    }
}
