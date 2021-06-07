using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_App.Models
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }

        public ErrorType ErrorType { get; set; }
    }

    public enum ErrorType
    {
        None, Fatal, Connectivity, Business
    }
}
