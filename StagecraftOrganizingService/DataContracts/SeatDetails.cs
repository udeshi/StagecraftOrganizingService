using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StagecraftOrganizingService.DataContracts
{
    [DataContract]
    public class SeatDetails
    {
        private Int32 _seatNo;
        private Int32 _rowNo;
        private Int32 _blockNo;
        private Int32 _floor;
        private String _bgColour;
        private TicketDetails _ticketDetails;

        [DataMember]
        public Int32 SeatNo
        {
            get
            {
                return _seatNo;
            }

            set
            {
                _seatNo = value;
            }
        }

        [DataMember]
        public Int32 RowNo
        {
            get
            {
                return _rowNo;
            }

            set
            {
                _rowNo = value;
            }
        }

        [DataMember]
        public Int32 BlockNo
        {
            get
            {
                return _blockNo;
            }

            set
            {
                _blockNo = value;
            }
        }

        [DataMember]
        public Int32 Floor
        {
            get
            {
                return _floor;
            }

            set
            {
                _floor = value;
            }
        }

        [DataMember]
        public String BgColour
        {
            get
            {
                return _bgColour;
            }

            set
            {
                _bgColour = value;
            }
        }
        [DataMember]
        public TicketDetails TicketDetails
        {
            get
            {
                return _ticketDetails;
            }

            set
            {
                _ticketDetails = value;
            }
        }
    }
}
