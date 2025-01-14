﻿using System.Threading.Tasks;
using Api.Common.Repository.Repositories;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Models;

namespace AmberEggApi.Database.Migrations
{
    public class V20191001001InitialState : BaseDatabaseMigration
    {
        private readonly IRepository<Company> repository;

        public V20191001001InitialState(IRepository<Company> repository)
        {
            this.repository = repository;
        }

        public override async Task Up()
        {
            var company = new Company();
            company.Create(new CreateCompanyCommand("Initial Company"));

            await repository.Insert(company);
        }
    }
}
