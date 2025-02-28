﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Common.Repository.Exceptions;
using Autofac;
using AmberEggApi.Domain.Commands;
using AmberEggApi.Domain.Tests.Factories;
using FluentAssertions;
using NUnit.Framework;

namespace AmberEggApi.Domain.Tests.UnitTests
{
    [TestFixture]
    public class CompanyDomainTests
    {
        private readonly CompanyFactoryTest factory;

        public CompanyDomainTests()
        {
            factory = SetupDomainTests.Container.Resolve<CompanyFactoryTest>();
        }

        [Test]
        public async Task WhenCreate_Then_ICanFindItById()
        {
            //act
            var responseCreate = await factory.Create();
            var responseFindById = await factory.Get(responseCreate.Id);

            //assert
            responseFindById.Id.Should().Be(responseCreate.Id);
            responseFindById.Name.Should().Be(responseCreate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdate_Then_ICanFindItById()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Company-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var responseCreate = await factory.Create();
            var commandUpdate = new UpdateCompanyCommand(
                responseCreate.Id,
                expectedNameAfterUpdate);

            var responseUpdate = await factory.Update(commandUpdate);
            var responseFindById = await factory.Get(responseCreate.Id);

            //assert
            responseFindById.Id.Should().Be(responseUpdate.Id);
            responseFindById.Name.Should().Be(responseUpdate.Name);
        }

        [Test]
        public async Task WhenCreateAndUpdateAndDelete_Then_Success()
        {
            //arrange
            var expectedNameAfterUpdate =
                $"AfterUpdate-Company-Test-{DateTime.UtcNow.ToLongTimeString()}";

            //act
            var responseCreate = await factory.Create();
            var commandUpdate = new UpdateCompanyCommand(
                responseCreate.Id,
                expectedNameAfterUpdate);

            await factory.Update(commandUpdate);
            await factory.Delete(responseCreate.Id);

            var responseFindById = await factory.Get(responseCreate.Id);

            //assert
            responseFindById.Should().BeNull();
        }

        [Test]
        public void WhenCreateNotValidEntity_Then_Error()
        {
            //arrange
            var expectedNumberOfErrors = 1;

            var name = string.Empty;
            var command = new CreateCompanyCommand(name);

            //act
            Func<Task> action = async () => { await factory.Create(command); };

            //assert
            action.Should()
                .Throw<ModelException>()
                .Where(x => x.Errors.Count() == expectedNumberOfErrors);

            action.Should()
                .Throw<ModelException>()
                .WithMessage(
                    "This object instance is not valid based on DataAnnotation definitions. See more details on Errors list.");
        }
    }
}