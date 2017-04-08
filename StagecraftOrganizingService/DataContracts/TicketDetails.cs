using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StagecraftOrganizingService.DataContracts
{
    [DataContract]
    public class TicketDetails
    {
        private String _ticketType;
        private Int32 _ticketNo;
        private Decimal _ticketPrice;

        [DataMember]
        public String TicketType
        {
            get
            {
                return _ticketType;
            }

            set
            {
                _ticketType = value;
            }
        }

        [DataMember]
        public Int32 TicketNo
        {
            get
            {
                return _ticketNo;
            }

            set
            {
                _ticketNo = value;
            }
        }

        [DataMember]
        public Decimal TicketPrice
        {
            get
            {
                return _ticketPrice;
            }

            set
            {
                _ticketPrice = value;
            }
        }
    }
}
