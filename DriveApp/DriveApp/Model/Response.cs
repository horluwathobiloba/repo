using System.Collections.Generic;

namespace DriveApp.Model
{
    public class Response<T>
    {
        public bool Success { get; set; } = false;
        public List<T> Data { get; set; }
        public T DataT { get; set; }
        public string Message { get; set; }
    }
}
