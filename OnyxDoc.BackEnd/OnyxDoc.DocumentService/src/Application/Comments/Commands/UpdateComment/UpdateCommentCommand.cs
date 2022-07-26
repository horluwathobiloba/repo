using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Comments.Queries.GetComments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Domain.Entities;

namespace OnyxDoc.DocumentService.Application.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public CoordinateVm Coordinates { get; set; }
    }

    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateCommentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
               // var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.Comments.Include(a=>a.Coordinate).FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid contract comment specified.");
                }

                entity.Text = request.Comment;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;
                entity.Coordinate = new Domain.Entities.Coordinate
                {
                    CoordinateType = CoordinateType.Comment,
                    Position = request.Coordinates.Position,
                    Transform = request.Coordinates.Transform,
                    Name = request.Coordinates.Name,
                    Width = request.Coordinates.Width,
                    Height = request.Coordinates.Height,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                };
                _context.Comments.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<CommentDto>(entity);
                return Result.Success(" comment update was successful!", result);
            }
            catch (Exception ex)
            {
               return Result.Failure($" comment update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
