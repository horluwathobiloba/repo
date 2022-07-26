using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes;
using Onyx.ContractService.Application.Inboxs.Commands.CreateInbox; 
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipientActions.Commands.ApproveAction
{
    public class SubmitForApprovalActionCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int ContractId { get; set; }
        public string UserId { get; set; }
    }


    public class SubmitForApprovalActionCommandHandler : IRequestHandler<SubmitForApprovalActionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        public SubmitForApprovalActionCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService,
            IConfiguration configuration, IEmailService emailService, IAuthService authService,IMediator mediator,INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
            _emailService = emailService;
            _authService = authService;
            _mediator = mediator;
            _notificationService = notificationService;
            
        }
        public async Task<Result> Handle(SubmitForApprovalActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //get contract recipients
                //get contract
                var contract = await _context.Contracts.Where(a => a.Id == request.ContractId).FirstOrDefaultAsync();
                if (contract == null)
                {
                    return Result.Failure("Invalid Contract Specified");
                }
                string loginPage = _configuration["LoginPage"];
                //get contract recipient with first rank
                var nextRecipient = await _context.ContractRecipients.Where(a => a.ContractId == contract.Id
                                    && a.Status == Status.Active && a.Rank == 1).FirstOrDefaultAsync();
                if (nextRecipient == null)
                {
                    return Result.Failure("No Valid Recipient found");
                }

                //string updateQuery = "UPDATE Contracts";

                // string command = "UPDATE [dbo].[Contracts] SET " +
                //    "NextActorEmail = @a_nextActorEmail, NextActorAction = @a_nextActorAction, NextActorRank = @a_nextActorRank," +
                //    " ContractStatus = @a_contractStatus, ContractStatusDesc = @a_contractStatusDesc where id =@a_contractId";

                // //InsertUpdate_Data(updateQuery, CommandType.Text,
                //SqlParameter[] sqlParameters = new SqlParameter[]
                //{
                // new SqlParameter("@a_nextActorEmail", nextRecipient.Email),
                // new SqlParameter("@a_nextActorAction", nextRecipient.RecipientCategory.ToString()),
                // new SqlParameter("@a_nextActorRank", nextRecipient.Rank),
                // new SqlParameter("@a_contractId", contract.Id),
                // new SqlParameter("@a_contractStatus", ContractStatus.PendingApproval),
                // new SqlParameter("@a_contractStatusDesc", ContractStatus.PendingApproval.ToString()),

                //};
                // var updateCheck = ExecuteNonQuery(command, CommandType.Text, sqlParameters);


                contract.NextActorEmail = nextRecipient.Email;
                contract.NextActorAction = nextRecipient.RecipientCategory.ToString();
                contract.NextActorRank = nextRecipient.Rank;
                contract.ContractStatus = ContractStatus.PendingApproval;
                contract.ContractStatusDesc = ContractStatus.PendingApproval.ToString();
                _context.Contracts.Update(contract);
                await _context.SaveChangesAsync(cancellationToken);
                if (contract.NextActorEmail == null)
                {
                    return Result.Failure("Invalid Next Actor Email Specified");
                }

                var emailResult = string.Empty;
                //var emailResult; ;
                if (contract.NextActorEmail != null)
                {
                    var email = new EmailVm
                    {
                        Subject = "New Contract - Approval Request",
                        Text = $"You have received a document initiation request for you to approve.",
                        Body = $"You have received a document initiation request for you to approve.",
                        RecipientEmail = contract.NextActorEmail,
                        ButtonText = "Approve Request Now!",
                        ButtonLink = loginPage
                    };
                    // var emailResponse =
                      emailResult = await _emailService.SendEmail(email);
                        
                     this.CreateInbox(request, email);
                }
                return Result.Success($"Submit for approval was successful" + emailResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Submit for approval was unsuccessful. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

        internal async Task CreateInbox(SubmitForApprovalActionCommand request, EmailVm email)
        {
            try
            {
                var command = new CreateInboxCommand
                {
                    AccessToken = request.AccessToken,
                    UserId = request.UserId,
                    OrganisationId = request.OrganisationId,
                    Body = email.Text,
                    Name = email.Subject,
                    Delivered = false,
                    RecipeintEmail = email.RecipientEmail,
                    EmailAction = EmailAction.Received,
                    
                };
                await new CreateInboxCommandHandler(_context, _mapper, _authService).Handle(command, new CancellationToken());
            }
            catch (Exception ex)
            {

            }
        }
        //internal async Task CreateInboxes(SubmitForApprovalActionCommand request, List<EmailVm> emails)
        //{
        //    try
        //    {
        //        var inboxes = new List<CreateInboxRequest>();
        //        foreach (var email in emails) //Send email to all the contract generators
        //        {
        //            inboxes.Add(new CreateInboxRequest
        //            {
        //                Name = email.Subject,
        //                ReciepientEmail = email.RecipientEmail,
        //                Body = email.Body,
        //                EmailAction = EmailAction.Received,
        //                Delivered = true,
        //                OrganizationId = request.OrganisationId,
        //                UserId = request.UserId
        //            });
        //        }
        //        var command = new CreateInboxesCommand
        //        {
        //            AccessToken = request.AccessToken,
                  
        //            OrganisationId = request.OrganisationId,
        //            Inboxes = inboxes,
        //            UserId = request.UserId
        //        };
        //        await new CreateInboxesCommandHandler(_context, _mapper, _authService).Handle(command, new CancellationToken());
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public int InsertUpdate_Data(string sql, CommandType cmdType, params SqlParameter[] parameters)
        //{
        //    try
        //    {
        //        var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            using (var tran = conn.BeginTransaction())
        //            using (SqlCommand cmd = new SqlCommand(sql, conn, tran))
        //            {
        //                cmd.CommandType = cmdType;
        //                cmd.Parameters.AddRange(parameters);
        //                var result = (int)cmd.ExecuteScalar();
        //                tran.Commit();
        //                return result;
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        //log to a file or Throw a message ex.Message;
        //        throw;
        //    }


            
        //}

       

        //public bool ExecuteNonQuery(string commandName, CommandType commandType, SqlParameter[] sqlParameters)
        //{
        //    var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        //    int result = 0;
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
        //        {
        //            sqlCommand.CommandType = commandType;
        //            sqlCommand.CommandText = commandName;
        //            sqlCommand.Parameters.AddRange(sqlParameters);


        //            try
        //            {
        //                if (sqlConnection.State != ConnectionState.Open)
        //                {
        //                    sqlConnection.Open();
        //                }
        //                result = sqlCommand.ExecuteNonQuery();
        //            }
        //            catch
        //            {

        //                throw;
        //            }
        //        }
        //    }
        //    return (result > 0);
        //}
    }
}

