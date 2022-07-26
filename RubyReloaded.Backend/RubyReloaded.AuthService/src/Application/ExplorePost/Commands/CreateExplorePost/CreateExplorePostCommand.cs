using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExplorePost.Commands.CreateExplorePost
{
    public class CreateExplorePostCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string Body { get; set; }
        public int CategoryId { get; set; }
        public List<TagVm> TagVms { get; set; }
        public List<byte[]> UploadData { get; set; }
        public string UserId { get;  set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string ExplorePostFileURL { get; set; }
        public ExploreImageType ExploreImageType { get;  set; }
       
    }



    public class CreateExplorePostCommandHandler : IRequestHandler<CreateExplorePostCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        public CreateExplorePostCommandHandler(IApplicationDbContext context, IMediator mediator,IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _mediator = mediator;
            _base64ToFileConverter = base64ToFileConverter;
        }

        public async Task<Result> Handle(CreateExplorePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                //Insert into ExplorePost
                // List<string> imageBlobs = await NewMethod(request);
                var uploads = await StoreImages(request.UploadData, request.Name);
                var fileUpload = new ExplorePostFile
                {
                    Extension = request.Extension,
                    Name = request.Name,
                    ExplorePostFileURL = uploads,
                    ExploreImageType = request.ExploreImageType,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    MimeType = request.MimeType
                };
                await _context.ExplorePostFiles.AddAsync(fileUpload);
                await _context.SaveChangesAsync(cancellationToken);
                var entity = new Domain.Entities.ExplorePost
                {
                    ExploreCategoryId = request.CategoryId,
                    Body = request.Body,
                    Status = Domain.Enums.Status.Active,
                    SubHeader = request.SubHeader,
                    Header = request.Header,
                    CreatedDate = DateTime.Now,
                    StatusDesc = Domain.Enums.Status.Active.ToString(),
                    CreatedBy = request.UserId,
                    ExplorePostFileId = fileUpload.Id,
                    ExplorePostFile = fileUpload
                };
                await _context.ExplorePosts.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                //Insert into Tags
                var tagsToCreate = await CreateTags(request.TagVms, cancellationToken);

                //insert into TagFAQ
                var exploreTagPosts = new List<ExploreTagPost>();
                foreach (var item in tagsToCreate)
                {
                    var exploreTagPost = new ExploreTagPost
                    {
                        ExplorePost = entity,
                        ExplorePostId = entity.Id,
                        Tag = item,
                        ExploreTagId = item.Id
                    };
                    exploreTagPosts.Add(exploreTagPost);
                }
                await _context.ExploreTagPosts.AddRangeAsync(exploreTagPosts);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Creating Posts failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

      

        internal async Task<IEnumerable<ExploreTag>> CreateTags(List<TagVm> TagVms, CancellationToken cancellationToken)
        {
            try
            {
                //  await _context.BeginTransactionAsync();
                var getAll = await _context.ExploreTags.ToListAsync();
                var tags = new List<Domain.Entities.ExploreTag>();
                foreach (var tagVm in TagVms)
                {
                    var inbox = new Domain.Entities.ExploreTag
                    {
                        Name = tagVm.Name
                    };
                    tags.Add(inbox);
                }
                //var tagsToCreatetest = getAll.Where(x => tags.All(a => a.Name != x.Name));
                var tagsToCreate =  tags.Except(getAll).ToList();
               
             //   var tagsToCreate = getAll.Where(x => tags.Any(a => a.Name == x.Name)).ToList();
                await _context.ExploreTags.AddRangeAsync(tagsToCreate);
                await _context.SaveChangesAsync(cancellationToken);
                //  await _context.CommitTransactionAsync();
                return tagsToCreate;
            }
            catch (Exception ex)
            {
                // _context.RollbackTransaction();
                return null;
            }
        }
        internal async Task<List<string>> StoreImages(List<byte[]> uploadData, string fileName)
        {
            int count = 0;
            var blobs = new List<string>();
            foreach (var file in uploadData)
            {
                count = +count;
                var blob = await _base64ToFileConverter.ConvertByteImageToFile(file, string.Concat(fileName, count.ToString()));
                blobs.Add(blob);

            }
            return blobs;
        }
        internal async Task<List<string>> StoreVideos(List<byte[]> uploadData, string fileName,string mimeType)
        {
            int count = 0;
            var blobs = new List<string>();
            foreach (var file in uploadData)
            {
                count = +count;
                var blob = await _base64ToFileConverter.ConvertByteToFile(file, string.Concat(fileName, count.ToString()),mimeType);
                blobs.Add(blob);

            }
            return blobs;
        }
        internal async Task<List<string>> StoreBlob(List<byte[]> uploadData, string fileName, string mimeType, ExploreImageType exploreImageType)
        {
            if (exploreImageType==ExploreImageType.video)
            {
                var videoBlobs=await StoreVideos(uploadData, fileName, mimeType);
                return videoBlobs;
            }
            var imageBlobs = await StoreImages(uploadData, fileName);
            return imageBlobs;
        }
    }



}
