using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class OrdinaryRequestNotification:ISerializable
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Count { get; set; }

        public OrdinaryRequestNotification(int requestId)
        {
            RequestId = requestId;
            Count = 0;
        }
        public OrdinaryRequestNotification()
        {
            Count = 0;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), RequestId.ToString(), Count.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            RequestId = Convert.ToInt32((string)values[1]);
            Count = Convert.ToInt32((string)values[2]);
        }
    }
}
