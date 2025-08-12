using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthcare.Core.Entities
{
    public class QueueEntry
    {
        public int QueueEntryId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int Position { get; set; } // Position in queue
        public string Status { get; set; } // Waiting, InProgress, Done
    }
}

