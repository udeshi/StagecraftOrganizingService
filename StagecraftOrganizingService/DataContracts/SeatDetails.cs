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
        private Int32 seatNo;
        private Int32 rowNo;
        private Int32 blockNo;
        private Int32 floor;
        private String bgColour;
        private TicketDetails ticketDetails;
        private Boolean isAvailableSeat;

        [DataMember]
        public Int32 SeatNo
        {
            get
            {
                return this.seatNo;
            }

            set
            {
                this.seatNo = value;
            }
        }

        [DataMember]
        public Int32 RowNo
        {
            get
            {
                return this.rowNo;
            }

            set
            {
                this.rowNo = value;
            }
        }

        [DataMember]
        public Int32 BlockNo
        {
            get
            {
                return this.blockNo;
            }

            set
            {
                this.blockNo = value;
            }
        }

        [DataMember]
        public Int32 Floor
        {
            get
            {
                return this.floor;
            }

            set
            {
                this.floor = value;
            }
        }

        [DataMember]
        public String BgColour
        {
            get
            {
                return this.bgColour;
            }

            set
            {
                this.bgColour = value;
            }
        }

        [DataMember]
        public TicketDetails TicketDetails
        {
            get
            {
                return this.ticketDetails;
            }

            set
            {
                this.ticketDetails = value;
            }
        }

        [DataMember]
        public bool IsAvailableSeat
        {
            get
            {
                return this.isAvailableSeat;
            }

            set
            {
                this.isAvailableSeat = value;
            }
        }
    }
}
