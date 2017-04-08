using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StagecraftOrganizingService.DataContracts
{
    public class SeatBookingDetails
    {
        private Int32 _customerId;
        private String _customerEmail;
        private Decimal _pendingAmount;
        private List<SeatDetails> _bookedSeatDetails;
        public int CustomerId
        {
            get
            {
                return _customerId;
            }

            set
            {
                _customerId = value;
            }
        }

        public string CustomerEmail
        {
            get
            {
                return _customerEmail;
            }

            set
            {
                _customerEmail = value;
            }
        }

        public decimal PendingAmount
        {
            get
            {
                return _pendingAmount;
            }

            set
            {
                _pendingAmount = value;
            }
        }

        public List<SeatDetails> BookedSeatDetails
        {
            get
            {
                return _bookedSeatDetails;
            }

            set
            {
                _bookedSeatDetails = value;
            }
        }
    }
}
