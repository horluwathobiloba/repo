using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Organizations.Commands.CreateOrganization;
using Onyx.WorkFlowService.Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;
using System;
using Onyx.WorkFlowService.Application.Organizations.Commands.UpdateOrganization;

namespace Onyx.WorkFlowService.Application.IntegrationTests.Organizations.Commands
{
    using static Testing;

    public class UpdateOrganizationTests : TestBase
    {
        [Test]
        public void ShouldRequireValidOrganizationId()
        {
            var command = new UpdateOrganizationCommand
            {
                //Id = 99,
                //Title = "New Title"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldUpdateOrganization()
        {
            var userId = await RunAsDefaultUserAsync();

            var listId = await SendAsync(new CreateOrganizationCommand
            {
                Name = "Kuda"
            });

            var result = await SendAsync(new CreateOrganizationCommand
            {
                //ListId = listId,
                //Title = "New Item"
            });

            var command = new UpdateOrganizationCommand
            {
                //Id = itemId,

                //Title = "Updated Item Title"
            };

            await SendAsync(command);

            //var item = await FindAsync<Organization>(result.);

            //item.Name.Should().Be(command.Name);
            //item.LastModifiedBy.Should().NotBeNull();
            //item.LastModifiedBy.Should().Be(userId);
            //item.LastModifiedDate.Should().NotBeNull();
            //item.LastModifiedDate.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}
