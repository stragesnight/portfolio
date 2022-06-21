using System;

namespace NP_Exam_Library
{
    [Serializable]
    public class ServerResponse
    {
        public ResponseStatus Status { get; set; }
        public object ResponseObject { get; set; }

        public ServerResponse(object responseObject)
        {
            ResponseObject = responseObject;
            Status = responseObject == null ? ResponseStatus.Failure : ResponseStatus.Success;
        }
    }

    public enum ResponseStatus
    {
        Success,
        Failure
    }
}
