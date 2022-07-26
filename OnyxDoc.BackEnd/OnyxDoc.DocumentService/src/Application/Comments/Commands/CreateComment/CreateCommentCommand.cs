using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Comments.Queries.GetComments;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentId { get; set; }
        public CommentType CommentType { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public CoordinateVm Coordinates { get; set; }
    }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateCommentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                

                var entity = new Domain.Entities.Comment
                {
                    DocumentId = request.DocumentId,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = request.SubscriberName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    Text = request.Comment,
                    CommentType = request.CommentType
                };
                entity.Coordinate = new Domain.Entities.Coordinate
                {
                    CoordinateType = CoordinateType.Comment,
                    Position = request.Coordinates.Position,
                    Transform = request.Coordinates.Transform,
                    Name = request.Coordinates.Name,
                    Width = request.Coordinates.Width,
                    Height = request.Coordinates.Height,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                };
                await _context.Comments.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<CommentDto>(entity);
                return Result.Success("Comment created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Comment creation failed. Error: { ex?.Message +" "+ ex?.InnerException.Message}");
            }
        }
    }
}
