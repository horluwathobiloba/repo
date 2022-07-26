using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;
using Onyx.AuthService.Application.IntegrationTests;
using Onyx.AuthService.Application.Organizations.Commands.ChangeOrganizationStatus;
using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Organizations.Commands.CreateOrganization;

namespace Onyx.LoanService.Application.IntegrationTests.Organizations.Commands
{
    using static Testing;

    public class ChangeOrganizationStatusTests : TestBase
    {
        [Test]
        public void ShouldRequireValidOrganizationId()
        {
            var command = new ChangeOrganizationStatusCommand { OrganizationId = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeactivateOrganization()
        {
            var listId = await SendAsync(new CreateOrganizationCommand
            {
                //Title = "New List"
            });

            var itemId = await SendAsync(new CreateOrganizationCommand
            {
                //ListId = listId,
                //Title = "New Item"
            });

            await SendAsync(new ChangeOrganizationStatusCommand
            {
               // Id = itemId
            });

           // var list = await FindAsync<Organization>(listId);

           // list.Should().BeNull();
        }
    }
}
