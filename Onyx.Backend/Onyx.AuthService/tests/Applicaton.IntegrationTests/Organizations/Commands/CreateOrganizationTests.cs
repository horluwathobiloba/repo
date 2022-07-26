using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Organizations.Commands.CreateOrganization;
using Onyx.AuthService.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.IntegrationTests.Organizations.Commands
{
    using static Testing;

    public class CreateOrganizationTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateOrganizationCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateOrganization()
        {
            var userId = await RunAsDefaultUserAsync();

            var listId = await SendAsync(new CreateOrganizationCommand
            {
                Name = "New List"
            });

            var command = new CreateOrganizationCommand
            {
                //ListId = listId,
                //Title = "Tasks"
            };

            //var itemId = await SendAsync(command);

            //var item = await FindAsync<Organization>(itemId);

            //item.Should().NotBeNull();
            ////item.ListId.Should().Be(command.ListId);
            ////item.Title.Should().Be(command.Title);
            //item.CreatedBy.Should().Be(userId);
            //item.CreatedDate.Should().BeCloseTo(DateTime.Now, 10000);
            //item.LastModifiedBy.Should().BeNull();
            //item.LastModifiedDate.Should().BeNull();
        }
    }
}
