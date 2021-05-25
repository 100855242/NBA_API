namespace DotNetAuthentication.Models
{
    public class Response<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public int Pages { get; set; }
        public Response()
        {

        }

        public Response(T data)
        {
            Data = data;
        }

        public Response(T data, int pagesCount)
        {
            Data = data;
            Succeeded = true;
            Message = string.Empty;
            Pages = pagesCount;
        }
    }
}