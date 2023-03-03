using WebAPIClone.Model;

namespace WebAPIClone.Commom.Result
{
    public class ApiResultList
    {
        public Code code { get; set; }
        public string Message { get; set; }
        public ResponsePaging Data { get; set; }
    }

}
