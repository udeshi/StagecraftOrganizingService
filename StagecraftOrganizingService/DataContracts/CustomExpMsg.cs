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
        public CustomExpMsg(String message)
        {
            this.ErrorMsg = message;
        }
        private Int32 errorNumber;
        [DataMember(Order = 0)]
        public Int32 ErrorNumber
        {
            get { return errorNumber; }
            set { errorNumber = value; }
        }
        private String errrorMsg;
        [DataMember(Order = 1)]
        public String ErrorMsg
        {
            get { return errrorMsg; }
            set { errrorMsg = value; }
        }
        private String description;
        [DataMember(Order = 2)]
        public String Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
