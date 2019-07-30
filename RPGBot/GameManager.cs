using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    public class GameManager
    {
        private static GameManager instance;
        IDataManager dataManager;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public IDataManager DataManager { get { return dataManager; } }

        private GameManager()
        {

        }

        public void Init(Type type)
        {
            dataManager = (IDataManager)Activator.CreateInstance(type);
        }
    }
}
