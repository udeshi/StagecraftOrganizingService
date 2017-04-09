using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StagecraftOrganizingService.DataContracts
{
    [DataContract]
    public class SeatBookingDetails
    {
        private Int32 customerId;
        private String bookingDate;
        private List<TicketDetails> bookedTicketDetails;
        private List<SeatDetails> bookedSeatDetails;
        private String bookingStatus;

        [DataMember]
        public int CustomerId
        {
            get
            {
                return this.customerId;
            }

            set
            {
                this.customerId = value;
            }
        }

        [DataMember]
        public string BookingDate
        {
            get
            {
                return this.bookingDate;
            }

            set
            {
                this.bookingDate = value;
            }
        }

        [DataMember]
        public List<TicketDetails> BookedTicketDetails
        {
            get
            {
                return this.bookedTicketDetails;
            }

            set
            {
                this.bookedTicketDetails = value;
            }
        }

        [DataMember]
        public List<SeatDetails> BookedSeatDetails
        {
            get
            {
                return this.bookedSeatDetails;
            }

            set
            {
                this.bookedSeatDetails = value;
            }
        }

        [DataMember]
        public string BookingStatus
        {
            get
            {
                return this.bookingStatus;
            }

            set
            {
                this.bookingStatus = value;
            }
        }
    }
}
