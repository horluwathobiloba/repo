using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Onyx.ContractService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();      
        protected string accessToken;
      
    }
}
