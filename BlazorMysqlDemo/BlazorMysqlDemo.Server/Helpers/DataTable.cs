using BlazorMysqlDemo.DataLibrary;
using BlazorMysqlDemo.DataLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace BlazorMysqlDemo.Server.Helpers
{
    public class DataTable
    {
        private readonly IDataAccess _data;
        private readonly IConfiguration _config;

        public DataTable(IDataAccess data, IConfiguration config)
        {
            _data = data;
            _config = config;
            string sql = "select * from People;";
            Persons.AddRange(_data.LoadData<PersonModel, dynamic>(sql, new { }, _config.GetConnectionString("default")).Result);
        }

        public List<PersonModel> Persons { get; set; } = new List<PersonModel>();
        public Action OnDataTable { get; set; }

        public async Task AddPerson(PersonModel person)
        {
            using (var trans = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
            {
                string sql = "insert into People (Name , Surname) values (@Name , @Surname);";

                await _data.SaveData(sql, new { person.Name, person.Surname }, _config.GetConnectionString("default"));

                Persons.Add(person);
                OnDataTable?.Invoke();

                trans.Complete();
            }
        }

        public async Task UpdatePerson(PersonModel person)
        {
            using (var trans = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
            {
                string sql = "update People set Name = @Name , Surname = @Surname where Id = @Id;";

                await _data.SaveData(sql, new { person.Name, person.Surname, person.Id }, _config.GetConnectionString("default"));

                OnDataTable?.Invoke();

                trans.Complete();
            }
        }

        public async Task DeletePerson(PersonModel person)
        {
            using (var trans = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled))
            {
                string sql = "delete from People where Id = @Id;";
                await _data.SaveData(sql, new { person.Id }, _config.GetConnectionString("default"));

                Persons.Remove(person);
                OnDataTable?.Invoke();

                trans.Complete();
            }
        }
    }
}
