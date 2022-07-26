using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Tags.Commands.CreateTagsCommand;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.FAQQuestion.Commands.CreateFAQQuestions
{
    
    public class CreateFAQQuestionsCommand:IRequest<Result>
    {
        public string Question { get; set; }
        public List<TagVm> TagVms { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class CreateFAQQuestionsCommandHandler : IRequestHandler<CreateFAQQuestionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        public CreateFAQQuestionsCommandHandler(IApplicationDbContext context,IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Result> Handle(CreateFAQQuestionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                //Insert into FAQQuestions
                var questionExists = await _context.FAQQuestions.AnyAsync(a => (a.Question.ToUpper() == request.Answer.ToUpper())
                   && a.Status == Status.Active);

                if (questionExists)
                {
                    return Result.Failure(new string[] { "Create new FAQ Category failed because a FAQ category name already exists. Please enter a new FAQ category name to continue." });
                }

                var entity = new Domain.Entities.FAQQuestion
                {
                   Answer=request.Answer,
                   Question=request.Question,
                   FAQCategoryId=request.CategoryId
                };
                await _context.FAQQuestions.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                //Insert into Tags
                var tagsToCreate=await CreateTags(request.TagVms,cancellationToken);

                //insert into TagFAQ
                var tagFAQs = new List<TagFAQ>();
                foreach (var item in tagsToCreate)
                {
                    var tagFAQ = new TagFAQ
                    {
                        FAQQuestion = entity,
                        FAQQuestionId = entity.Id,
                        Tag = item,
                        TagId = item.Id
                    };
                    tagFAQs.Add(tagFAQ);
                }
                await _context.TagFAQs.AddRangeAsync(tagFAQs);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Creating FAQ questions failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        internal async Task<IEnumerable<Tag>> CreateTags(List<TagVm> TagVms,CancellationToken cancellationToken)
        {
            try
            {
              //  await _context.BeginTransactionAsync();
                var getAll = await _context.Tags.ToListAsync();
                var tags = new List<Domain.Entities.Tag>();
                foreach (var tagVm in TagVms)
                {
                    var inbox = new Domain.Entities.Tag
                    {
                        Name = tagVm.Name
                    };
                    tags.Add(inbox);
                }
                //  var tagsToCreate = getAll.Where(x => tags.All(a => a.Name != x.Name));
                var tagsToCreate = getAll.Where(x => tags.Any(a => a.Name == x.Name));
                await _context.Tags.AddRangeAsync(tagsToCreate);
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
    }
}
