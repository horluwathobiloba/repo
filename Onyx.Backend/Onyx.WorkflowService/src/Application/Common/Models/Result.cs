using System.Collections.Generic;
using System.Linq;

namespace Onyx.WorkFlowService.Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> messages)
        {
            Succeeded = succeeded;
            Messages = messages.ToArray();
        }

        internal Result(bool succeeded, object result)
        {
            Succeeded = succeeded;
            Entity = result;
        }

        public bool Succeeded { get; set; }

        public object Entity { get; set; }

        public string[] Messages { get; set; }

        public static Result Success()
        {
            return new Result(true, new string[] { });
        }

        public static Result Success(string message)
        {
            return new Result(true, new string[] { message });
        }

        public static Result Success(string message, object entity)
        {
            return new Result(true,  entity);
        }
        public static Result Success(object entity)
        {
            return new Result(true, entity);
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }
}
