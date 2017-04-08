using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StagecraftOrganizingService.DataContracts
{
    [DataContract]
    public class CustomExpMsg
    {
        public CustomExpMsg()
        {
            this.ErrorMsg = "Service encountered an error";
        }
        public CustomExpMsg(string message)
        {
            this.ErrorMsg = message;
        }
        private int errorNumber;
        [DataMember(Order = 0)]
        public int ErrorNumber
        {
            get { return errorNumber; }
            set { errorNumber = value; }
        }
        private string errrorMsg;
        [DataMember(Order = 1)]
        public string ErrorMsg
        {
            get { return errrorMsg; }
            set { errrorMsg = value; }
        }
        private string description;
        [DataMember(Order = 2)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
