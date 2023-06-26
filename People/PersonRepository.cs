using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using People.Models;
using SQLite;

namespace People
{
    public class PersonRepository
    {
        string _dbPath;

        public string StatusMessage { get; set; }



        // TODO: Add variable for the SQLite connection
        private SQLiteAsyncConnection conn;

        private void Init()
        {
            // TODO: Add code to initialize the repository
            if (conn != null)
            return;

            conn = new SQLiteAsyncConnection(_dbPath);
            conn.CreateTableAsync<Person_ER>();
        }

        public PersonRepository(string dbPath)
        {
            _dbPath = dbPath;                        
        }

        public async Task AddNewPerson(string name)
        {
            int result = 0;
            try
            {
                // Call Init()
                Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                result = await conn.InsertAsync(new Person_ER { Name = name });

                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }

        public async Task<List<Person_ER>> GetAllPeople()
        {
            try
            {
                Init();
                return await conn.Table<Person_ER>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Person_ER>();
        }
    }
}
