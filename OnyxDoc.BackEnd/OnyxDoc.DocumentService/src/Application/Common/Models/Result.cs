﻿using System.Collections.Generic;
using System.Linq;

namespace OnyxDoc.DocumentService.Application.Common.Models
{
    public class Result
    {
        public Result()
        {
        }

        internal Result(bool succeeded, string message, object result = null)
        {
            Succeeded = succeeded;
            Message = message;
            Entity = result;
        }

        internal Result(bool succeeded, object result)
        {
            Succeeded = succeeded;
            Entity = result;
        }
    
        public bool Succeeded { get; set; }

        public object Entity { get; set; }

        public string Error { get; set; }

        public string Message { get; set; }
        public string[] Messages { get; set; }

        public static Result Success()
        {
            return new Result(true, new string[] { });
        }

        public static Result Success(string message)
        {
            return new Result(true, message);
        }

        public static Result Success(string message, object entity)
        {
            return new Result(true, message, entity);
        }

        public static Result Success(object entity)
        {
            return new Result(true, entity);
        }

        public static Result Failure(string error)
        {
            return new Result(false, error);
        }
        public static Result Failure(object entity)
        {
            return new Result(false, entity);
        }
        //public static Result Failure(IEnumerable<string> errors)
        //{
        //    return new Result(false, errors);
        //}
    }
}

