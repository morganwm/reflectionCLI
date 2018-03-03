using System;
using System.Collections.Generic;
using System.Text;
using ReflectionCli.Lib.Enums;

namespace ReflectionCli.Lib.Models
{
    public class Record
    {
        public DateTime Written;
        public string Message;
        public RecordType RecordType;

        public Record(string message = "", RecordType recordType = RecordType.Output)
        {
            Message = message;
            Written = DateTime.Now;
            RecordType = recordType;
        }

    }
}
