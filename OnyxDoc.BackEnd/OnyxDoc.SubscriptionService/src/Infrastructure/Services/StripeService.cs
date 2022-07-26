using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using ReventInject;
using ReventInject.Services;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Customer = Stripe.Customer;
using OnyxDoc.SubscriptionService.Domain.Common;

namespace OnyxDoc.SubscriptionService.Infrastructure.Utility
{
    public class StripeService : IStripeService, IStripeSubscriptionService, IStripeProductService, IStripePriceService, IStripeCustomerService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public StripeService(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        #region Stripe Payment Services
        public async Task<Result> GetPaymentStatus(PaymentIntentVm paymentIntentVm)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];
                var service = new SessionService();
                var session = await service.GetAsync(paymentIntentVm.SessionId);
                if (session == null)
                {
                    return Result.Failure(new string[] { $"Invalid checkout session id: {paymentIntentVm.SessionId} " });
                }
                return Result.Success(session);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Payment Status failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }

        public async Task<Result> InitiateCardPayment(PaymentIntentVm paymentIntentVm)
        {
            try
            {
                var metadata = new Dictionary<string, string>();
                metadata.Add("SubscriptionNo", paymentIntentVm.SubscriptionNo);
                string domain = _configuration["Domain"];
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];
                var options = new SessionCreateOptions
                {
                    ClientReferenceId = string.Format("{0:ddMMyyyyHHmmssfff}-{1}", DateTime.Now, paymentIntentVm.SubscriptionNo),
                    PaymentMethodTypes = new List<string>
                {
                  PaymentMethodType.Card.ToString().ToLower(),
                },
                    LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = paymentIntentVm.UnitAmountInt64,
                        Currency = paymentIntentVm.CurrencyCode,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                          Name = $"Subscription fulfilment for subscription number {paymentIntentVm.SubscriptionNo}",
                          Description = paymentIntentVm.Description,
                      },
                    },
                    Quantity = paymentIntentVm.Quantity,
                  },
                },
                    Mode = paymentIntentVm.Mode,
                    SuccessUrl = paymentIntentVm.CallBackUrl,
                    CancelUrl = paymentIntentVm.CallBackUrl,

                    // SuccessUrl = domain + "api/stripewebhooks",
                    // CancelUrl = domain + "api/stripewebhooks"
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                //create the payment record in the database here by calling the CreatePayment command
                paymentIntentVm.SessionId = session.Id;
                paymentIntentVm.PaymentMethodType = options.PaymentMethodTypes.FirstOrDefault();
                paymentIntentVm.SuccessUrl = options.SuccessUrl;
                paymentIntentVm.CancelUrl = options.CancelUrl;
                return Result.Success("Initiating card payment was successful", paymentIntentVm.SessionId);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Initiating card payment failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }

        #endregion

        #region Stripe Payout Services

        public async Task<Result> InitiateInstantPayout(PayoutVm payoutVm)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                //Get the Current Balance
                var balservice = new BalanceService();
                Balance balance = balservice.Get();


                var metadata = new Dictionary<string, string>();
                //metadata.Add("SubscriptionNo", payoutVm.SubscriptionNo);
                string domain = _configuration["Domain"];

                var options = new PayoutCreateOptions
                {
                    Amount = payoutVm.Amount == 0 && balance != null ? balance?.Available?.FirstOrDefault().Amount : payoutVm.Amount * 100,
                    Currency = payoutVm.CurrencyCode,
                    Description = payoutVm.Description,
                    Method = "instant",
                };

                var requestOptions = new RequestOptions();
                requestOptions.StripeAccount = "{{CONNECTED_STRIPE_ACCOUNT_ID}}";

                var service = new PayoutService();
                var payout = await service.CreateAsync(options, requestOptions);

                //create the payment record in the database here by calling the CreatePayment command
                payoutVm.PayoutId = payout.Id;
                payoutVm.Automatic = payout.Automatic;
                payoutVm.Object = payout.Object;
                payoutVm.ArrivalDate = payout.ArrivalDate;
                payoutVm.BalanceTransaction = payout.BalanceTransaction?.Id;
                payoutVm.Created = payout.Created;
                payoutVm.Description = payout.Description;
                payoutVm.Destination = payout.Destination?.Id;
                payoutVm.FailureBalanceTransaction = payout.FailureBalanceTransaction?.Id;
                payoutVm.FailureCode = payout.FailureCode;
                payoutVm.FailureMessage = payout.FailureMessage;
                payoutVm.LiveMode = payout.Livemode;
                payoutVm.Method = payout.Method;
                payoutVm.OriginalPayout = payout.OriginalPayout?.Id;
                payoutVm.Metadata = payout.Metadata;
                payoutVm.ReversedBy = payout.ReversedBy?.Id;
                payoutVm.SourceType = payout.SourceType;
                payoutVm.StatementDescriptor = payout.StatementDescriptor;
                payoutVm.Status = payout.Status;
                payoutVm.Type = payout.Type;

                return Result.Success(payoutVm);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Initiating card payment failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> InitiateManualPayout(PayoutVm payoutVm)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                //Get the Current Balance
                var balservice = new BalanceService();
                Balance balance = balservice.Get();


                var metadata = new Dictionary<string, string>();
                //metadata.Add("SubscriptionNo", payoutVm.SubscriptionNo);
                string domain = _configuration["Domain"];

                var options = new PayoutCreateOptions
                {
                    Amount = payoutVm.Amount == 0 && balance != null ? balance?.Available?.FirstOrDefault().Amount : payoutVm.Amount * 100,
                    Currency = payoutVm.CurrencyCode,
                    Description = payoutVm.Description
                };

                var requestOptions = new RequestOptions();
                requestOptions.StripeAccount = "{{CONNECTED_STRIPE_ACCOUNT_ID}}";

                var service = new PayoutService();
                var payout = await service.CreateAsync(options, requestOptions);

                //create the payment record in the database here by calling the CreatePayment command
                payoutVm.PayoutId = payout.Id;
                payoutVm.Automatic = payout.Automatic;
                payoutVm.Object = payout.Object;
                payoutVm.ArrivalDate = payout.ArrivalDate;
                payoutVm.BalanceTransaction = payout.BalanceTransaction?.Id;
                payoutVm.Created = payout.Created;
                payoutVm.Description = payout.Description;
                payoutVm.Destination = payout.Destination?.Id;
                payoutVm.FailureBalanceTransaction = payout.FailureBalanceTransaction?.Id;
                payoutVm.FailureCode = payout.FailureCode;
                payoutVm.FailureMessage = payout.FailureMessage;
                payoutVm.LiveMode = payout.Livemode;
                payoutVm.Method = payout.Method;
                payoutVm.OriginalPayout = payout.OriginalPayout?.Id;
                payoutVm.Metadata = payout.Metadata;
                payoutVm.ReversedBy = payout.ReversedBy?.Id;
                payoutVm.SourceType = payout.SourceType;
                payoutVm.StatementDescriptor = payout.StatementDescriptor;
                payoutVm.Status = payout.Status;
                payoutVm.Type = payout.Type;

                return Result.Success(payoutVm);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Initiating card payment failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion

        #region Stripe Account Services 

        public async Task<Result> GetBalance()
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];
                //Get the Current Balance
                var balservice = new BalanceService();
                Balance balance = await balservice.GetAsync();

                return Result.Success(balance);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion

        #region Connected Account Services


        public async Task<Result> CreateBankAccountToken(IConfiguration _configuration, StripeAccountVm stripeAccountVm)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new TokenCreateOptions
                {
                    BankAccount = new TokenBankAccountOptions
                    {
                        AccountHolderName = stripeAccountVm.AccountHolderName,
                        Country = stripeAccountVm.Country,
                        Currency = stripeAccountVm.Currency,
                        RoutingNumber = stripeAccountVm.RoutingNumber
                    },
                    Customer = stripeAccountVm.CustomerId,
                };
                var service = new TokenService();
                var token = await service.CreateAsync(options);

                return Result.Success(token);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Create Token failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<string> GetBankAccountToken(IConfiguration _configuration, StripeAccountVm stripeAccountVm)
        {
            try
            {
                var token = await CreateBankAccountToken(_configuration, stripeAccountVm);
                if (token == null)
                {
                    throw new Exception("Create token failed;");
                }
                return token.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Result> CreateConnectedAccount(IConfiguration _configuration, StripeConnectedAccountVm stripeConnectedAccountVm)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new AccountCreateOptions
                {
                    Type = stripeConnectedAccountVm.Type,
                    Country = stripeConnectedAccountVm.Country,
                    Email = stripeConnectedAccountVm.Email,
                    BusinessType = stripeConnectedAccountVm.BusinessType,
                    Individual = new AccountIndividualOptions
                    {
                        Address = new AddressOptions
                        {
                            City = stripeConnectedAccountVm.AddressCity,  //optional
                            Country = stripeConnectedAccountVm.AddressCountry,  //optional
                            Line1 = stripeConnectedAccountVm.AddressLine1,  //o //optionalptional
                            Line2 = stripeConnectedAccountVm.AddressLine2,
                            PostalCode = stripeConnectedAccountVm.AddressPostalCode,
                            State = stripeConnectedAccountVm.AddressState
                        },
                        Dob = new DobOptions { } //optional
                    },
                    Capabilities = new AccountCapabilitiesOptions
                    {
                        CardPayments = new AccountCapabilitiesCardPaymentsOptions
                        {
                            Requested = true,
                        },
                        Transfers = new AccountCapabilitiesTransfersOptions
                        {
                            Requested = true,
                        },
                    },
                    TosAcceptance = new AccountTosAcceptanceOptions
                    {
                        Date = DateTime.UtcNow,
                        Ip = new NetworkService().GetUser_IP() // "127.0.0.1", // provide request's IP address

                    },
                    Metadata = new Dictionary<string, string> { }
                };
                var service = new AccountService();
                var customer = await service.CreateAsync(options);

                //create the payment record in the database here by calling the CreatePayment command
                stripeConnectedAccountVm.CustomerId = customer.Id;
                stripeConnectedAccountVm.DetailsSubmitted = customer.DetailsSubmitted;
                stripeConnectedAccountVm.DefaultCurrency = customer.DefaultCurrency;
                stripeConnectedAccountVm.Deleted = customer.Deleted;
                stripeConnectedAccountVm.PayoutsEnabled = customer.PayoutsEnabled;

                if (customer.Individual != null)
                {
                    var acc = customer.Individual;
                    stripeConnectedAccountVm.Created = acc.Created;
                    stripeConnectedAccountVm.Gender = acc.Gender;
                    stripeConnectedAccountVm.IdNumberProvided = acc.IdNumberProvided;
                    stripeConnectedAccountVm.LastName = acc.LastName;
                    stripeConnectedAccountVm.MaidenName = acc.MaidenName;
                    stripeConnectedAccountVm.Phone = acc.Phone;
                    stripeConnectedAccountVm.PoliticalExposure = acc.PoliticalExposure;
                    stripeConnectedAccountVm.SsnLast4Provided = acc.SsnLast4Provided;

                    if (acc.Verification != null & acc.Verification.Document != null)
                    {
                        var doc = acc.Verification.Document;
                        stripeConnectedAccountVm.Back = doc.Back?.Url;
                        stripeConnectedAccountVm.Front = doc.Front?.Url;
                    }

                    if (acc.Address != null)
                    {
                        var address = acc.Address;
                        stripeConnectedAccountVm.AddressCity = address.City;
                        stripeConnectedAccountVm.AddressCountry = address.Country;
                        stripeConnectedAccountVm.AddressLine1 = address.Line1;
                        stripeConnectedAccountVm.AddressLine2 = address.Line2;
                        stripeConnectedAccountVm.AddressPostalCode = address.PostalCode;
                        stripeConnectedAccountVm.AddressState = address.State;
                    }
                }

                if (customer.Requirements != null)
                {
                    var x = customer.Requirements;

                    stripeConnectedAccountVm.DisabledReason = x.DisabledReason;
                    stripeConnectedAccountVm.CurrentDeadline = x.CurrentDeadline;
                    stripeConnectedAccountVm.CurrentlyDue = x.CurrentlyDue?.ToDelimitedString();
                    stripeConnectedAccountVm.PendingVerification = x.PendingVerification?.ToDelimitedString();
                    stripeConnectedAccountVm.PastDue = x.PastDue?.ToDelimitedString();
                    stripeConnectedAccountVm.EventuallyDue = x.EventuallyDue.ToDelimitedString();
                }

                if (customer.Capabilities != null)
                {
                    var x = customer.Capabilities;
                    stripeConnectedAccountVm.CardPayments = x.CardPayments;
                    stripeConnectedAccountVm.Transfers = x.Transfers;
                }

                return Result.Success(stripeConnectedAccountVm);

            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        /// <summary>
        /// Updates a connected Express or Custom account by setting the values of the parameters passed. 
        /// Any parameters not provided are left unchanged. Most parameters can be changed only for Custom accounts. 
        /// (These are marked Custom Only below.) Parameters marked Custom and Express are supported by both account types.
        /// </summary>
        /// <param name="_configuration"></param>
        /// <param name="stripeConnectedAccountID"></param>
        /// <param name="metadata">
        /// Set of key-value pairs that you can attach to an object. This can be useful for storing additional information about the object in a structured format. 
        /// Individual keys can be unset by posting an empty value to them. All keys can be unset by posting an empty value to metadata.
        /// </param>
        /// <returns></returns>
        public async Task<Result> UpdatConnectedAccountMetaData(IConfiguration _configuration, string stripeConnectedAccountID, Dictionary<string, string> metadata)
        {
            try
            {
                var options = new AccountUpdateOptions
                {
                    Metadata = metadata,
                };
                var service = new AccountService();
                var account = await service.UpdateAsync(stripeConnectedAccountID, options);

                return Result.Success(account);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Update connected account failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        /// <summary>
        /// With Connect, you may flag accounts as suspicious. 
        /// Test-mode Custom and Express accounts can be rejected at any time. 
        /// Accounts created using live-mode keys may only be rejected once all balances are zero.
        /// </summary>
        /// <param name="_configuration"></param>
        /// <param name="stripeConnectedAccountID">The stripe connected account id</param>
        /// <param name="reason">The reason for rejecting the account. Can be fraud, terms_of_service, or other.</param>
        /// <returns>
        /// Returns an account with payouts_enabled and charges_enabled set to false on success. If the account ID does not exist, this call throws an error.
        /// </returns>
        public async Task<Result> RejectConnectedAccount(IConfiguration _configuration, string stripeConnectedAccountID, string reason)
        {
            try
            {
                var options = new AccountRejectOptions
                {
                    Reason = reason,
                };
                var service = new AccountService();
                var response = await service.RejectAsync(stripeConnectedAccountID, options);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Reject account failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        /// <summary>
        /// With Connect, you can delete Custom or Express accounts you manage.
        /// Accounts created using test-mode keys can be deleted at any time. Accounts created using live-mode keys can only be deleted once all balances are zero.
        /// </summary>
        /// <param name="_configuration"></param>
        /// <param name="stripeConnectedAccountID">The stripe connected account id</param>
        /// <returns>Returns an object with a deleted parameter on success. If the account ID does not exist, this call throws an error.</returns>
        public async Task<Result> DeleteConnectedAccount(IConfiguration _configuration, string stripeConnectedAccountID)
        {
            try
            {
                var service = new AccountService();
                var response = await service.DeleteAsync(stripeConnectedAccountID);

                return Result.Success(response.Deleted);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete connected account failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        /// <summary>
        /// This method returns a list of accounts connected to your platform via Connect. If you’re not a platform, the list is empty.
        /// </summary>
        /// <param name="_configuration"></param>
        /// <param name="limit">A limit on the number of objects to be returned. Limit can range between 1 and 100, and the default is 10.</param>
        /// <param name="startingAfter">A cursor for use in pagination. starting_after is an object ID that defines your place in the list. 
        /// For instance, if you make a list request and receive 100 objects, ending with obj_foo, your subsequent call can include starting_after=obj_foo in subscription to fetch the next page of the list.</param>
        /// <param name="endingBefore">A cursor for use in pagination. ending_before is an object ID that defines your place in the list. 
        /// For instance, if you make a list request and receive 100 objects, starting with obj_bar, your subsequent call can include ending_before=obj_bar in subscription to fetch the previous page of the list.</param>
        /// <param name="created">optional Dictionary: A filter on the list based on the object created field. The value can be a System.DateTime or a Stripe.DateRangeOptions i.e. dictionary with the following options:
        /// created.gt: optional - Return results where the created field is greater than this value.
        /// created.gte: optional - Return results where the created field is greater than or equal to this value.
        /// created.lt: optional - Return results where the created field is less than this value.
        /// created.lte: optional - Return results where the created field is less than or equal to this value.
        /// </param>
        /// <returns>
        /// A Dictionary with a data property that contains an array of up to limit accounts, starting after account starting_after. 
        /// Each entry in the array is a separate Account object. 
        /// If no more accounts are available, the resulting array is empty.
        /// </returns>
        public async Task<Result> GetConnectedAccounts(IConfiguration _configuration, long? limit, string startingAfter, string endingBefore, AnyOf<DateTime?, DateRangeOptions> created)
        {
            try
            {
                var options = new AccountListOptions
                {
                    Limit = limit,
                    EndingBefore = endingBefore,
                    StartingAfter = startingAfter,
                    Created = created,
                };
                var service = new AccountService();
                StripeList<Account> accounts = await service.ListAsync(options);
                return Result.Success(accounts);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get connected account list failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }


        public async Task<Result> AcceptAccountAgreement(IConfiguration _configuration, string stripeAccountID)
        {
            try
            {
                var options = new AccountUpdateOptions
                {
                    TosAcceptance = new AccountTosAcceptanceOptions
                    {
                        ServiceAgreement = "full",
                        Date = DateTime.UtcNow,
                        Ip = new NetworkService().GetUser_IP() // "127.0.0.1", // provide request's IP address
                    },
                };

                var service = new AccountService();
                var account = service.Update("{{CONNECTED_STRIPE_ACCOUNT_ID}}", options);

                return Result.Success(account);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Accept account agreement Token failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> CreateVerificationDocument(IConfiguration _configuration, string filename)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                using (FileStream stream = System.IO.File.Open(filename, FileMode.Open))
                {
                    var options = new FileCreateOptions
                    {
                        File = stream,
                        Purpose = FilePurpose.IdentityDocument,
                    };
                    var service = new FileService();
                    var upload = await service.CreateAsync(options);
                    if (upload != null)
                    {

                    }
                }

                //create the payment record in the database here by calling the CreatePayment command
                //var stripeCustomerDocVm.CustomerId = customer.Id;
                //stripeConnectedAccountVm.DetailsSubmitted = customer.DetailsSubmitted;
                //stripeConnectedAccountVm.DefaultCurrency = customer.DefaultCurrency;
                //stripeConnectedAccountVm.Deleted = customer.Deleted;
                //stripeConnectedAccountVm.PayoutsEnabled = customer.PayoutsEnabled;

                //if (customer.Individual != null)
                //{
                //    var acc = customer.Individual;
                //    stripeConnectedAccountVm.Created = acc.Created;
                //    stripeConnectedAccountVm.Gender = acc.Gender;
                //    stripeConnectedAccountVm.IdNumberProvided = acc.IdNumberProvided;
                //    stripeConnectedAccountVm.LastName = acc.LastName;
                //    stripeConnectedAccountVm.MaidenName = acc.MaidenName;
                //    stripeConnectedAccountVm.Phone = acc.Phone;
                //    stripeConnectedAccountVm.PoliticalExposure = acc.PoliticalExposure;
                //    stripeConnectedAccountVm.SsnLast4Provided = acc.SsnLast4Provided;

                //    if (acc.Verification != null & acc.Verification.Document != null)
                //    {
                //        var doc = acc.Verification.Document;
                //        stripeConnectedAccountVm.Back = doc.Back?.Url;
                //        stripeConnectedAccountVm.Front = doc.Front?.Url;
                //    }

                //    if (acc.Address != null)
                //    {
                //        var address = acc.Address;
                //        stripeConnectedAccountVm.AddressCity = address.City;
                //        stripeConnectedAccountVm.AddressCountry = address.Country;
                //        stripeConnectedAccountVm.AddressLine1 = address.Line1;
                //        stripeConnectedAccountVm.AddressLine2 = address.Line2;
                //        stripeConnectedAccountVm.AddressPostalCode = address.PostalCode;
                //        stripeConnectedAccountVm.AddressState = address.State;
                //    }
                //}

                //if (customer.Requirements != null)
                //{
                //    var x = customer.Requirements;

                //    stripeConnectedAccountVm.DisabledReason = x.DisabledReason;
                //    stripeConnectedAccountVm.CurrentDeadline = x.CurrentDeadline;
                //    stripeConnectedAccountVm.CurrentlyDue = x.CurrentlyDue?.ToDelimitedString();
                //    stripeConnectedAccountVm.PendingVerification = x.PendingVerification?.ToDelimitedString();
                //    stripeConnectedAccountVm.PastDue = x.PastDue?.ToDelimitedString();
                //    stripeConnectedAccountVm.EventuallyDue = x.EventuallyDue.ToDelimitedString();
                //}

                //if (customer.Capabilities != null)
                //{
                //    var x = customer.Capabilities;
                //    stripeConnectedAccountVm.CardPayments = x.CardPayments;
                //    stripeConnectedAccountVm.Transfers = x.Transfers;
                //}

                return Result.Success("Hey");

            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }
        #endregion

        #region External Account Services

        public async Task<Result> CreateExternalBankAccount(StripeAccountVm stripeAccountVm)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new ExternalAccountCreateOptions
                {
                    // ExternalAccount = "btok_1IXjtlEHcxMTVy8nmUAiVuLA",                    
                    ExternalAccount = new AccountBankAccountOptions
                    {
                        AccountHolderName = stripeAccountVm.AccountHolderName,
                        AccountHolderType = stripeAccountVm.AccountHolderType,
                        AccountNumber = stripeAccountVm.AccountNumber,
                        Country = stripeAccountVm.Country,
                        Currency = stripeAccountVm.Currency,
                        RoutingNumber = stripeAccountVm.RoutingNumber
                    },
                    //DefaultForCurrency = false,
                    Expand = new List<string> { },
                    ExtraParams = new Dictionary<string, object> { },
                    Metadata = new Dictionary<string, string> { }
                };
                var service = new ExternalAccountService();
                IExternalAccount externalAccount = await service.CreateAsync("acct_1IAFmIEHcxMTVy8n", options);

                //create the payment record in the database here by calling the CreatePayment command
                stripeAccountVm.AccountId = externalAccount.Id;
                stripeAccountVm.AccountNo = externalAccount.AccountId;

                if (externalAccount.Account != null)
                {
                    var acc = externalAccount.Account;
                    stripeAccountVm.Created = acc.Created;
                    stripeAccountVm.DefaultCurrency = acc.DefaultCurrency;
                    // stripeAccountVm.FingerPrint =  
                }

                return Result.Success(stripeAccountVm);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion

        #region Stripe Customer Services

        public async Task<Result> CreateCustomer(CreateStripeCustomerRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new CustomerCreateOptions
                {
                    Name = request.Subscriber.Name,
                    Email = request.Subscriber.Email,
                    InvoicePrefix = request.Subscriber.SubscriberCode?.ToUpper(),
                    Description = request.Subscriber.Description,
                    Address = new AddressOptions
                    {
                        Line1 = request.Subscriber.Address,
                        City = request.Subscriber.City,
                        State = request.Subscriber.State,
                        Country = request.Subscriber.Country
                    }
                };

                var service = new CustomerService();
                var stripeCustomer = await service.CreateAsync(options);

                return Result.Success(stripeCustomer);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> UpdateCustomer(UpdateStripeCustomerRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new CustomerUpdateOptions
                {
                    Name = request.Subscriber.Name,
                    Email = request.Subscriber.Email,
                    InvoicePrefix = request.Subscriber.SubscriberCode?.ToUpper(),
                    Description = request.Subscriber.Description,
                    Address = new AddressOptions
                    {
                        Line1 = request.Subscriber.Address,
                        City = request.Subscriber.City,
                        State = request.Subscriber.State,
                        Country = request.Subscriber.Country
                    }
                };
                CustomerService service = new CustomerService();
                Customer stripeCustomer = await service.UpdateAsync(request.Subscriber.StripeCustomerId, options);

                return Result.Success(stripeCustomer);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DeleteCustomer(string stripeCustomerId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                CustomerService service = new CustomerService();
                Customer customer = await service.DeleteAsync("prod_KTwHpFlTzcLVnV");

                return Result.Success(customer);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetCustomer(string stripeCustomerId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                CustomerService service = new CustomerService();
                Customer stripeCustomer = await service.GetAsync(stripeCustomerId);

                return Result.Success(stripeCustomer);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetCustomers(string startingAfter = null, string endingBefore = null, long? limit = null)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                CustomerListOptions options = new CustomerListOptions
                {
                    Limit = limit,
                    StartingAfter = startingAfter,
                    EndingBefore = endingBefore
                };
                CustomerService service = new CustomerService();
                StripeList<Customer> products = await service.ListAsync(options);

                return Result.Success(products);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion

        #region Stripe Product Services

        public async Task<Result> CreateProduct(CreateStripeProductRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new ProductCreateOptions
                {
                    Name = request.Plan.Name,
                    Description = request.Plan.Description,
                    Active = true
                };

                var service = new ProductService();
                var stripeProduct = await service.CreateAsync(options);

                return Result.Success(stripeProduct);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> UpdateProduct(UpdateStripeProductRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new ProductUpdateOptions
                {
                    Name = request.Plan.Name,
                    Description = request.Plan.Description,
                    Active = request.Plan.Status == Status.Active,
                    //Metadata = new Dictionary<string, string>
                    //{
                    //    { "order_id", "6735" },
                    //},
                };
                ProductService service = new ProductService();
                Product stripeProduct = await service.UpdateAsync(request.Plan.StripeProductId, options);

                return Result.Success(stripeProduct);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DeleteProduct(string stripeProductId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                ProductService service = new ProductService();
                Product product = await service.DeleteAsync("prod_KTwHpFlTzcLVnV");

                return Result.Success(product);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetProduct(string stripeProductId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                ProductService service = new ProductService();
                Product stripeProduct = await service.GetAsync(stripeProductId);

                return Result.Success(stripeProduct);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetProducts(string startingAfter = null, string endingBefore = null, long? limit = null)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                ProductListOptions options = new ProductListOptions
                {
                    Limit = limit,
                    StartingAfter = startingAfter,
                    EndingBefore = endingBefore
                };
                ProductService service = new ProductService();
                StripeList<Product> products = await service.ListAsync(options);

                return Result.Success(products);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion

        #region Stripe Price Services

        public async Task<Result> CreatePrice(CreateStripePriceRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new PriceCreateOptions
                {
                    UnitAmount = request.Price.Amount.ToLong(),
                    UnitAmountDecimal = request.Price.Amount,
                    Currency = request.Price.CurrencyCode,
                    Recurring = new PriceRecurringOptions
                    {
                        Interval = request.Price.PricingPlanType == PricingPlanType.Monthly ? "month" : "year",
                        IntervalCount = 1,
                        UsageType = "licensed",
                        TrialPeriodDays = request.Price.SubscriptionPlan.FreeTrialDays
                    },
                    TaxBehavior = "inclusive",
                    Product = request.Price.SubscriptionPlan.StripeProductId,
                };

                var service = new PriceService();
                var stripePrice = await service.CreateAsync(options);

                return Result.Success(stripePrice);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> UpdatePrice(UpdateStripePriceRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new PriceUpdateOptions
                {
                    Active = (request.Price.Status == Status.Active),
                    Recurring = new PriceRecurringOptions
                    {
                        Interval = request.Price.PricingPlanType == PricingPlanType.Monthly ? "month" : "year",
                        TrialPeriodDays = request.Price.SubscriptionPlan.FreeTrialDays
                    }
                };

                PriceService service = new PriceService();
                Price stripePrice = await service.UpdateAsync(request.Price.StripePriceId, options);

                return Result.Success(stripePrice);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DeletePrice(string stripePriceId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new PriceUpdateOptions
                {
                    Active = false
                };

                PriceService service = new PriceService();
                Price stripePrice = await service.UpdateAsync(stripePriceId, options);

                return Result.Success(stripePrice);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetPrice(string stripePriceId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                PriceService service = new PriceService();
                Price stripePrice = await service.GetAsync(stripePriceId);

                return Result.Success(stripePrice);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetPrices(string startingAfter = null, string endingBefore = null, long? limit = null)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                PriceListOptions options = new PriceListOptions
                {
                    Limit = limit,
                    StartingAfter = startingAfter,
                    EndingBefore = endingBefore,

                };
                PriceService service = new PriceService();
                StripeList<Price> products = await service.ListAsync(options);

                return Result.Success(products);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion

        #region Stripe Subscription Services

        public async Task<Result> CreateSubscription(CreateStripeSubscriptionRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var subscriber = request.Subscriber;


                var options = new SubscriptionCreateOptions
                {
                    Customer = subscriber.StripeCustomerId,
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Price = request.Price.StripePriceId,
                        },
                    },

                    TrialEnd = request.Subscription.FreeTrialActivated && request.Subscriber.FreeTrialCompleted ? request.Subscription.EndDate : DateTime.Now.AddDays(30).ToUniversalTime(),
                    PaymentBehavior = "default_incomplete",
                    AddInvoiceItems = new List<SubscriptionAddInvoiceItemOptions>
                    {
                        new SubscriptionAddInvoiceItemOptions
                        {
                            Price = request.Price.StripePriceId,
                            Quantity =  request.Subscription.NumberOfUsers?.ToLong()
                        },
                    },
                };
                options.AddExpand("latest_invoice.payment_intent");
                var subscriptionService = new Stripe.SubscriptionService();

                var service = new Stripe.SubscriptionService();
                var stripeSubscription = await service.CreateAsync(options);

                return Result.Success(stripeSubscription);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Create subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> UpdateSubscription(UpdateStripeSubscriptionRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                var options = new SubscriptionUpdateOptions
                {
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Price = request.Price.StripePriceId,
                        },
                    },
                    AddInvoiceItems = new List<SubscriptionAddInvoiceItemOptions>
                    {
                        new SubscriptionAddInvoiceItemOptions
                        {
                            Price = request.Price.StripePriceId,
                            Quantity =  request.Subscription.NumberOfUsers?.ToLong()
                        },
                    },
                    CollectionMethod = "charge_automatically"
                };
                Stripe.SubscriptionService service = new Stripe.SubscriptionService();
                Subscription stripeSubscription = await service.UpdateAsync(request.Subscription.PaymentGatewaySubscriptionId, options);

                return Result.Success(stripeSubscription);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Update subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }


        public async Task<Result> ReactivateSubscription(UpdateStripeSubscriptionRequest request)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                Stripe.SubscriptionService service = new Stripe.SubscriptionService();

                Subscription subscription = service.Get(request.Subscription.PaymentGatewaySubscriptionId);

                var items = new List<SubscriptionItemOptions> {
                    new SubscriptionItemOptions {
                        Id = subscription.Items.Data[0].Id,
                        Price = request.Price.StripePriceId,
                    },
                };
                var options = new SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = false,
                    ProrationBehavior = "create_prorations",
                    Items = items,
                };
                subscription = await service.UpdateAsync(request.Price.StripePriceId, options);

                return Result.Success(subscription);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Reactivate subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> CancelSubscription(string stripeSubscriptionId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                Stripe.SubscriptionService service = new Stripe.SubscriptionService();
                var options = new SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = true,
                };
                await service.UpdateAsync(stripeSubscriptionId, options);

                var cancelOptions = new SubscriptionCancelOptions
                {
                    InvoiceNow = false,
                    Prorate = false,
                };
                Subscription subscription = await service.CancelAsync(stripeSubscriptionId, cancelOptions);

                return Result.Success(subscription);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Cancel subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DeleteSubscription(string stripeSubscriptionId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                Stripe.SubscriptionService service = new Stripe.SubscriptionService();
                var options = new SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = true,
                    PauseCollection = new SubscriptionPauseCollectionOptions()
                    {
                        Behavior = "void"
                    }

                };
                var cancelOptions = new SubscriptionCancelOptions
                {
                    InvoiceNow = false,
                    Prorate = false,
                };
                await service.CancelAsync(stripeSubscriptionId, cancelOptions);

                var subscription = await service.UpdateAsync(stripeSubscriptionId, options);
                return Result.Success(subscription);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetSubscription(string stripeSubscriptionId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                Stripe.SubscriptionService service = new Stripe.SubscriptionService();
                Subscription stripeSubscription = await service.GetAsync(stripeSubscriptionId);

                return Result.Success(stripeSubscription);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetSubscriptions(string startingAfter = null, string endingBefore = null, long? limit = null)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey:SecretKey"];

                SubscriptionListOptions options = new SubscriptionListOptions
                {
                    Limit = limit,
                    StartingAfter = startingAfter,
                    EndingBefore = endingBefore
                };
                Stripe.SubscriptionService service = new Stripe.SubscriptionService();
                StripeList<Subscription> products = await service.ListAsync(options);

                return Result.Success(products);
            }
            catch (Exception ex)
            {
                return Result.Failure($"GetGet subscriptions failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion
    }
}
